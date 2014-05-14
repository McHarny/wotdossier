﻿using System;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Common.Logging;
using SimpleInjector;
using WotDossier.Applications;
using WotDossier.Applications.Update;
using WotDossier.Applications.View;
using WotDossier.Applications.ViewModel;
using WotDossier.Applications.ViewModel.Replay;
using WotDossier.Dal;
using WotDossier.Dal.NHibernate;
using WotDossier.Framework;
using WotDossier.Framework.Presentation.Services;
using WotDossier.ReplaysManager;
using WotDossier.Views;

namespace WotDossier
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly ILog _log = LogManager.GetLogger("App");

        private ReplaysManagerController _controller;

        [Import]
        public ReplaysManagerController Controller
        {
            get { return _controller; }
            set { _controller = value; }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Application.Startup" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.StartupEventArgs" /> that contains the event data.</param>
        protected override void OnStartup(StartupEventArgs e)
        {
#if !DEBUG
            // Don't handle the exceptions in Debug mode because otherwise the Debugger wouldn't
            // jump into the code when an exception occurs.
            DispatcherUnhandledException += AppDispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += AppDomainUnhandledException;
#endif

            // start application
            try
            {
                _log.Trace("OnStartup start");

                DatabaseManager manager = new DatabaseManager();
                manager.InitDatabase();

                //set app lang
                var culture = new CultureInfo(SettingsReader.Get().Language);
                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = culture;

                // Registrations here
                CompositionContainerFactory.Instance.Register<ReplaysManagerController, ReplaysManagerController>(Lifestyle.Singleton);
                CompositionContainerFactory.Instance.Register<ReplayManagerShellViewModel, ReplayManagerShellViewModel>(Lifestyle.Singleton);
                CompositionContainerFactory.Instance.Register<AddReplayFolderViewModel, AddReplayFolderViewModel>(Lifestyle.Transient);
                CompositionContainerFactory.Instance.Register<UploadReplayViewModel, UploadReplayViewModel>(Lifestyle.Transient);
                CompositionContainerFactory.Instance.Register<ReplayViewModel, ReplayViewModel>(Lifestyle.Transient);

                CompositionContainerFactory.Instance.Register<IDataProvider, DataProvider>(Lifestyle.Singleton);
                CompositionContainerFactory.Instance.Register<DossierRepository, DossierRepository>(Lifestyle.Singleton);
                CompositionContainerFactory.Instance.Register<Applications.Logic.ReplaysManager, Applications.Logic.ReplaysManager>(Lifestyle.Singleton);
                CompositionContainerFactory.Instance.Register<ISessionStorage, NHibernateSessionStorage>(Lifestyle.Singleton);

                CompositionContainerFactory.Instance.Register<IAddReplayFolderView, AddReplayFolderWindow>(Lifestyle.Transient);
                CompositionContainerFactory.Instance.Register<IShellView, MainWindow>(Lifestyle.Singleton);
                CompositionContainerFactory.Instance.Register<IReplayView, ReplayWindow>(Lifestyle.Transient);
                //CompositionContainerFactory.Instance.Register<IFileDialogService, FileDialogService>(Lifestyle.Transient);
                //CompositionContainerFactory.Instance.Register<IMessageService, MessageService>(Lifestyle.Transient);
                CompositionContainerFactory.Instance.Register<IUploadReplayView, UploadReplayWindow>(Lifestyle.Transient);

                Controller = CompositionContainerFactory.Instance.GetExport<ReplaysManagerController>();

                Controller.Run();

                _log.Trace("OnStartup end");
            }
            catch (Exception exception)
            {
                HandleException(exception, false);
                Shutdown();
            }

            base.OnStartup(e);
        }

        private void AppDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            HandleException(e.Exception, false);
            e.Handled = true;
        }

        private static void AppDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            HandleException(e.ExceptionObject as Exception, e.IsTerminating);
        }

        private static void HandleException(Exception e, bool isTerminating)
        {
            if (e == null)
            {
                return;
            }

            //Trace.TraceError(e.ToString());
            _log.Error(e);
            if (!isTerminating)
            {
                MessageBox.Show(e.ToString(), ApplicationInfo.ProductName, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Application.Exit" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.Windows.ExitEventArgs" /> that contains the event data.</param>
        protected override void OnExit(ExitEventArgs e)
        {
            if (Controller != null)
            {
                CompositionContainerFactory.Instance.Dispose();
            }
            base.OnExit(e);
        }
    }
}