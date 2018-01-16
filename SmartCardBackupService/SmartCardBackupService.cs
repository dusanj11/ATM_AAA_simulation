using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using ServiceContracts;
using Modeli;
using Common;
using Manager;

namespace SmartCardBackupService
{
    public class SmartCardBackupService : ISmartCardBackupService
    {
        DataIO serializer = new DataIO();

        public bool RevocationListBackup(List<string> revocationList)
        {
            ///impelmentacija serijalizacije liste
            try
            {
                Console.WriteLine("Revocation list backup...");
                serializer.SerializeObject(revocationList, "RevocationListBackup.xml");
                Audit.BackupCreatedSuccess();
                return true;
            }
            catch(Exception e)
            {
                Console.WriteLine("Revocation list backup failed...");
                Console.WriteLine("Error: {0}", e.Message);
                Audit.BackupCreatedFail();
                return false;
            }   
        }

        public bool UserListBackup(List<User> userList)
        {
            ///implementacija serijalizacije liste
            try
            {
                Console.WriteLine("User list backup...");
                serializer.SerializeObject(userList, "UserListBackup.xml");
                Audit.BackupCreatedSuccess();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("User list backup failed...");
                Console.WriteLine("Error: {0}", e.Message);
                Audit.BackupCreatedFail();
                return false;
            }
        }
    }
}
