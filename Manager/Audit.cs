using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
    public class Audit : IDisposable
    {
        private static EventLog customLog = null;
        private static EventLog applicationLog = null;
        
        const string SourceName = "Manager.Audit";
        const string SourceName2 = "ApplicationSourceName";
        const string LogName = "CustomLog"; 
        const string LogName2 = "Application";
        
        static Audit()
        {
            try
            {
                if (!EventLog.SourceExists(SourceName))
                {
                    EventLog.CreateEventSource(SourceName, LogName);
                }

                customLog = new EventLog(LogName, Environment.MachineName, SourceName);

                //applicationLog = new EventLog(LogName2, Environment.MachineName, SourceName2);
                applicationLog = new EventLog("Application");
                applicationLog.Source = "Application";
            }
            catch(Exception e)
            {
                customLog = null;
                Console.WriteLine("Error while trying to create log handle. Error = {0}", e.Message);
            }
        }

        public static void NewSmartCardCreatedSuccess(string username)
        {
            if(customLog != null)
            {
                string msg = String.Format(AuditEvents.UserNewSmartCardCreatedSuccess, username);
                customLog.WriteEntry(msg, EventLogEntryType.SuccessAudit);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.UserNewSmartCardCreatedSuccess));
            }
        }

        public static void NewSmartCardCreatedFail(string username)
        {
            if (customLog != null)
            {
                string msg = String.Format(AuditEvents.UserNewSmartCardCreatedFail, username);
                customLog.WriteEntry(msg, EventLogEntryType.FailureAudit);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.UserNewSmartCardCreatedFail));
            }
        }

        public static void PinResetSuccess(string username)
        {
            if (customLog != null)
            {
                string msg = String.Format(AuditEvents.UserPinResetSuccess, username);
                customLog.WriteEntry(msg, EventLogEntryType.SuccessAudit);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.UserPinResetSuccess));
            }
        }

        public static void PinResetFail(string username)
        {
            if (customLog != null)
            {
                string msg = String.Format(AuditEvents.UserPinResetFail, username);
                customLog.WriteEntry(msg, EventLogEntryType.FailureAudit);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.UserPinResetFail));
            }
        }

        public static void RejectingOldSmartCardSuccess(string username)
        {
            if (customLog != null)
            {
                string msg = String.Format(AuditEvents.UserRejectingOldSmartCardSuccess, username);
                customLog.WriteEntry(msg, EventLogEntryType.SuccessAudit);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.UserRejectingOldSmartCardSuccess));
            }
        }

        public static void RejectingOldSmartCardFail(string username)
        {
            if (customLog != null)
            {
                string msg = String.Format(AuditEvents.UserRejectingOldSmartCardFail, username);
                customLog.WriteEntry(msg, EventLogEntryType.FailureAudit);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.UserRejectingOldSmartCardFail));
            }
        }

        public static void BackupCreatedSuccess()
        {
            if (customLog != null)
            {
                string msg = String.Format(AuditEvents.DataBackupCreatedSuccess);
                customLog.WriteEntry(msg, EventLogEntryType.SuccessAudit);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.DataBackupCreatedSuccess));
            }
        }

        public static void BackupCreatedFail()
        {
            if (customLog != null)
            {
                string msg = String.Format(AuditEvents.DataBackupCreatedFail);
                customLog.WriteEntry(msg, EventLogEntryType.FailureAudit);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.DataBackupCreatedFail));
            }
        }

        public static void PushMoneySuccess(string username, string amount)
        {
            if (applicationLog != null)
            {
                string msg = String.Format(AuditEvents.UserPushMoneySuccess, username, amount);
                applicationLog.WriteEntry(msg, EventLogEntryType.SuccessAudit);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.UserPushMoneySuccess));
            }
        }

        public static void PushMoneyFail(string username, string amount)
        {
            if (applicationLog != null)
            {
                string msg = String.Format(AuditEvents.UserPushMoneyFail, username, amount);
                applicationLog.WriteEntry(msg, EventLogEntryType.FailureAudit);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.UserPushMoneyFail));
            }
        }

        public static void PullMoneySuccess(string username, string amount)
        {
            if (applicationLog != null)
            {
                string msg = String.Format(AuditEvents.UserPullMoneySuccess, username, amount);
                applicationLog.WriteEntry(msg, EventLogEntryType.SuccessAudit);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.UserPullMoneySuccess));
            }
        }

        public static void PullMoneyFail(string username, string amount)
        {
            if (applicationLog != null)
            {
                string msg = String.Format(AuditEvents.UserPullMoneyFail, username, amount);
                applicationLog.WriteEntry(msg, EventLogEntryType.FailureAudit);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.UserPullMoneyFail));
            }
        }


        public void Dispose()
        {
            if (customLog != null)
            {
                customLog.Dispose();
                customLog = null;
            }
        }
    }
}
