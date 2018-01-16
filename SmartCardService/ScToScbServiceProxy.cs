using System;
using System.Collections.Generic;
using System.ServiceModel;
using ServiceContracts;
using Modeli;

namespace SmartCardService
{
    public class ScToScbServiceProxy : ChannelFactory<ISmartCardBackupService>, ISmartCardBackupService, IDisposable
    {
        private ISmartCardBackupService factory;

        public ScToScbServiceProxy(NetTcpBinding binding, string address) : base(binding, address)
        {
            factory = this.CreateChannel();
        }

        public bool RevocationListBackup(List<string> revocationList)
        {
            try
            {
                return factory.RevocationListBackup(revocationList);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error message: {0}", e.Message);
                return false;
            }
        }

        public bool UserListBackup(List<User> userList)
        {
            try
            {
                return factory.UserListBackup(userList);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
                return false;
            }
        }

        public void Dispose()
        {
            if (factory != null)
            {
                factory = null;
            }
            this.Close();
        }
    }
}