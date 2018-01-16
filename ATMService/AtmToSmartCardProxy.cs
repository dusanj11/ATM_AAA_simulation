using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using ServiceContracts;
using Modeli;

namespace ATMService
{
    public class AtmToSmartCardProxy : ChannelFactory<ISmartCardService>, ISmartCardService, IDisposable
    {
        ISmartCardService factory;

        public AtmToSmartCardProxy(NetTcpBinding binding, string address) : base(binding, address)
        {
            factory = this.CreateChannel();
        }

        public void CreateNewSmartCard(string username, string pin)
        {
            throw new NotImplementedException();
        }

        public List<KeyValuePair<string, User>> GetExistingUsers()
        {
            try
            {
                return factory.GetExistingUsers();
            }
            catch(Exception e)
            {
                Console.WriteLine("Error {GetExistignUsers - AtmToSmartCardProxy} {0}", e.Message);
                return null;
            }

        }

        public List<string> GetSmartCardRevocationList()
        {
            try
            {
                return factory.GetSmartCardRevocationList();
            }
            catch(Exception e)
            {
                Console.WriteLine("Error {GetExistignUsers - AtmToSmartCardProxy} {0}", e.Message);
                return null;
            }
        }

        public bool InvalidateSmartCard(string username, string pin, string cert_thumb)
        {
            throw new NotImplementedException();
        }

        public bool ResetPinCode(string username, string oldPin, string newPin)
        {
            throw new NotImplementedException();
        }

        public void SerializingChangedData(User user)
        {
            try
            {
                factory.SerializingChangedData(user);
            }
            catch(Exception e)
            {
                Console.WriteLine("Error {TransactionExecuted - AtmToSmartCardProxy} {0}", e.Message);
            }

        }

        public bool UserCheck(string username)
        {
            throw new NotImplementedException();
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
