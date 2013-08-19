﻿using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Security.Authentication;
using System.Windows;
using Common.Logging;
using WotDossier.Applications.View;
using WotDossier.Framework.Applications;
using WotDossier.Framework.Forms.Commands;

namespace WotDossier.Applications.ViewModel
{
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Export(typeof (UploadReplayViewModel))]
    public class UploadReplayViewModel : ViewModel<IUploadReplayView>
    {
        private static readonly ILog _log = LogManager.GetLogger("UploadReplayViewModel");

        public DelegateCommand OnReplayUploadCommand { get; set; }

        public ReplayFile ReplayFile { get; set; }

        public string ReplayDescription { get; set; }

        public string ReplayName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModel&lt;TView&gt;"/> class and
        /// attaches itself as <c>DataContext</c> to the view.
        /// </summary>
        /// <param name="view">The view.</param>
        [ImportingConstructor]
        public UploadReplayViewModel([Import(typeof(IUploadReplayView))]IUploadReplayView view)
            : base(view)
        {
            OnReplayUploadCommand = new DelegateCommand(OnReplayUpload);
        }

        private void OnReplayUpload()
        {
            if (ReplayFile != null)
            {
                ReplayUploader replayUploader = new ReplayUploader();
                try
                {
                    replayUploader.Upload(ReplayFile.FileInfo, ReplayName, ReplayDescription, SettingsReader.Get().ReplaysUploadServerPath);
                    ViewTyped.Close();
                }
                catch (AuthenticationException e)
                {
                    _log.Error("Authentication error", e);
                    MessageBoxResult result = MessageBox.Show(Resources.Resources.Msg_ReplayUpload_AuthentificationFailure, Resources.Resources.WindowCaption_AuthFailure, MessageBoxButton.YesNo);

                    if (result == MessageBoxResult.Yes)
                    {
                        Process proc = new Process();
                        proc.EnableRaisingEvents = false;
                        proc.StartInfo.FileName = @"C:\Program Files\Internet Explorer\iexplore.exe";
                        proc.StartInfo.Arguments = "http://wotreplays.ru";
                        proc.Start();
                    }
                }
            }
        }

        public void Show()
        {
            ViewTyped.ShowDialog();
        }
    }
}
