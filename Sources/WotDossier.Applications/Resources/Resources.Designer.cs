﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18034
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WotDossier.Applications.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("WotDossier.Applications.Resources.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Avg damage.
        /// </summary>
        internal static string ChartLegend_AvgDamage {
            get {
                return ResourceManager.GetString("ChartLegend_AvgDamage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Effectivity Rating.
        /// </summary>
        internal static string ChartLegend_ER {
            get {
                return ResourceManager.GetString("ChartLegend_ER", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Win %.
        /// </summary>
        internal static string ChartLegend_WinPercent {
            get {
                return ResourceManager.GetString("ChartLegend_WinPercent", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to WN6.
        /// </summary>
        internal static string ChartLegend_WN6Rating {
            get {
                return ResourceManager.GetString("ChartLegend_WN6Rating", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Battles: {0}
        ///Avg Damage: {1:0.00}.
        /// </summary>
        internal static string ChartTooltipFormat_AvgDamage {
            get {
                return ResourceManager.GetString("ChartTooltipFormat_AvgDamage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Battles: {0}
        ///Rating: {1:0.00}.
        /// </summary>
        internal static string ChartTooltipFormat_Rating {
            get {
                return ResourceManager.GetString("ChartTooltipFormat_Rating", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Battles: {0}
        ///Win percent: {1:0.00}.
        /// </summary>
        internal static string ChartTooltipFormat_WinPercent {
            get {
                return ResourceManager.GetString("ChartTooltipFormat_WinPercent", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error on getting player data from server.
        /// </summary>
        internal static string ErrorMsg_GetPlayerData {
            get {
                return ResourceManager.GetString("ErrorMsg_GetPlayerData", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Can&apos;t get player info from server.
        /// </summary>
        internal static string ErrorMsg_GetPlayerInfo {
            get {
                return ResourceManager.GetString("ErrorMsg_GetPlayerInfo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error.
        /// </summary>
        internal static string WindowCaption_Error {
            get {
                return ResourceManager.GetString("WindowCaption_Error", resourceCulture);
            }
        }
    }
}
