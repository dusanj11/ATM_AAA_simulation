﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Manager {
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
    internal class AuditEventsFile {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal AuditEventsFile() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Manager.AuditEventsFile", typeof(AuditEventsFile).Assembly);
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
        ///   Looks up a localized string similar to Failed backing up data..
        /// </summary>
        internal static string DataBackupCreatedFail {
            get {
                return ResourceManager.GetString("DataBackupCreatedFail", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Backup data is up to date..
        /// </summary>
        internal static string DataBackupCreatedSuccess {
            get {
                return ResourceManager.GetString("DataBackupCreatedSuccess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed creating new smart card for user {0}..
        /// </summary>
        internal static string UserNewSmartCardCreatedFail {
            get {
                return ResourceManager.GetString("UserNewSmartCardCreatedFail", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Successfully created new smart card for user {0}..
        /// </summary>
        internal static string UserNewSmartCardCreatedSuccess {
            get {
                return ResourceManager.GetString("UserNewSmartCardCreatedSuccess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to User {0} failed reseting pin..
        /// </summary>
        internal static string UserPinResetFail {
            get {
                return ResourceManager.GetString("UserPinResetFail", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to User {0} successfully reseted pin..
        /// </summary>
        internal static string UserPinResetSuccess {
            get {
                return ResourceManager.GetString("UserPinResetSuccess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to User {0} failed taking {1} from their account..
        /// </summary>
        internal static string UserPullMoneyFail {
            get {
                return ResourceManager.GetString("UserPullMoneyFail", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to User {0} successfully took {1} from their account..
        /// </summary>
        internal static string UserPullMoneySuccess {
            get {
                return ResourceManager.GetString("UserPullMoneySuccess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to User {0} failed puting {1} on their account..
        /// </summary>
        internal static string UserPushMoneyFail {
            get {
                return ResourceManager.GetString("UserPushMoneyFail", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to User {0} successfully put {1} on their account..
        /// </summary>
        internal static string UserPushMoneySuccess {
            get {
                return ResourceManager.GetString("UserPushMoneySuccess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to User {0} rejected old smart card, and requested new one. Failed creating new one..
        /// </summary>
        internal static string UserRejectingOldSmartCardFail {
            get {
                return ResourceManager.GetString("UserRejectingOldSmartCardFail", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to User {0} rejected old smart card, and requested new one. Successfully created new one..
        /// </summary>
        internal static string UserRejectingOldSmartCardSuccess {
            get {
                return ResourceManager.GetString("UserRejectingOldSmartCardSuccess", resourceCulture);
            }
        }
    }
}
