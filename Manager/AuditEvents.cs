using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Resources;

namespace Manager
{
    public enum AuditEventTypes
    {
        UserNewSmartCardCreatedSuccess = 0,
        UserNewSmartCardCreatedFail = 1,
        UserPinResetSuccess = 2,
        UserPinResetFail = 3,
        UserRejectingOldSmartCardSuccess = 4,
        UserRejectingOldSmartCardFail = 5,
        DataBackupCreatedSuccess = 6,
        DataBackupCreatedFail = 7,
        UserPushMoneySuccess = 8,
        UserPushMoneyFail = 9,
        UserPullMoneySuccess = 10,
        UserPullMoneyFail = 11,
    }
    public class AuditEvents
    {
        private static ResourceManager resourceManager = null;
        private static object resourceLock = new object();

        private static ResourceManager ResourceMgr
        {
            get
            {
                lock(resourceLock)
                {
                    if(resourceManager == null)
                    {
                        resourceManager = new ResourceManager(typeof(AuditEventsFile).FullName, Assembly.GetExecutingAssembly());
                    }
                }
                return resourceManager;
            }
        }

        public static string UserNewSmartCardCreatedSuccess
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.UserNewSmartCardCreatedSuccess.ToString());
            }
        }

        public static string UserNewSmartCardCreatedFail
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.UserNewSmartCardCreatedFail.ToString());
            }
        }

        public static string UserPinResetSuccess
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.UserPinResetSuccess.ToString());
            }
        }

        public static string UserPinResetFail
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.UserPinResetFail.ToString());
            }
        }

        public static string UserRejectingOldSmartCardSuccess
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.UserRejectingOldSmartCardSuccess.ToString());
            }
        }

        public static string UserRejectingOldSmartCardFail
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.UserRejectingOldSmartCardFail.ToString());
            }
        }

        public static string DataBackupCreatedSuccess
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.DataBackupCreatedSuccess.ToString());
            }
        }

        public static string DataBackupCreatedFail
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.DataBackupCreatedFail.ToString());
            }
        }

        public static string UserPushMoneySuccess
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.UserPushMoneySuccess.ToString());
            }
        }

        public static string UserPushMoneyFail
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.UserPushMoneyFail.ToString());
            }
        }

        public static string UserPullMoneySuccess
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.UserPullMoneySuccess.ToString());
            }
        }

        public static string UserPullMoneyFail
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.UserPullMoneyFail.ToString());
            }
        }
    }
}
