using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using ServiceContracts;
using Modeli;

namespace Client
{
    public class ClientToSmartCardProxy : ChannelFactory<ISmartCardService>, ISmartCardService, IDisposable
    {
        ISmartCardService factory;

        public ClientToSmartCardProxy(NetTcpBinding binding, string address) : base(binding, address)
        {
            factory = this.CreateChannel();
           
        }

        public void CreateNewSmartCard(string username, string pin)
        {
            try
            {
                //Console.WriteLine("Klijent pokrenuo pravljene nove kartice.");
                factory.CreateNewSmartCard(username, pin);
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR {0}", e.Message);
               
            }
        }

        public bool InvalidateSmartCard(string username, string pin, string cert_thumb)
        {
            try
            {
                //Console.WriteLine("Klijent pokrenuo povlacenje kartice.");
                return factory.InvalidateSmartCard(username, pin, cert_thumb);
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR {0}", e.Message);
                return false;
            }
        }

        public bool ResetPinCode(string username, string oldPin, string newPin)
        {
            try
            {
               // Console.WriteLine("Klijent pokrenuo resetovanje PINa.");
                return factory.ResetPinCode(username, oldPin, newPin);
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR {0}", e.Message);
                return false;
            }
        }

        public bool UserCheck(string username)
        {
            try
            {
                return factory.UserCheck(username);
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR {0}", e.Message);
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

        public List<KeyValuePair<string, User>> GetExistingUsers()
        {
            try
            {
                return factory.GetExistingUsers();    
            }
            catch(Exception e)
            {
                Console.WriteLine("Error {GetExistignUsers - ClientToSmartCardProxy} {0}", e.Message);
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
                Console.WriteLine("Error {GetSmartCardRevocationList - ClientToSmartCardProxy} {0}", e.Message);
                return null;
            }
        }

        public void SerializingChangedData(User user)
        {
            throw new NotImplementedException();
        }
    }
}
