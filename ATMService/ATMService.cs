using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using ServiceContracts;
using SmartCardService;
using Modeli;
using Common;
using System.Security.Cryptography.X509Certificates;
using Manager;

namespace ATMService
{
    public class ATMService : IATMService
    {
        public static NetTcpBinding binding = new NetTcpBinding();
        
        public static  string address = "net.tcp://localhost:8000/SmartCardService";
        
       

        public bool Authentificate(string username, string pin, string cert_thumb)
        {
            Console.WriteLine("Authentificate started  pin: {0}  username: {1}", pin, username);
           
            string addressToSc = "net.tcp://localhost:9090/SmartCardService";
            Console.WriteLine("Test");
            using (AtmToSmartCardProxy atmToSCSProxy = new AtmToSmartCardProxy(binding, addressToSc))
            {
                Console.WriteLine("Test2");
                User tempUser = null;
                Console.WriteLine("Revocation List, Count: {0}", atmToSCSProxy.GetSmartCardRevocationList().Count);
                if (atmToSCSProxy.GetSmartCardRevocationList().Contains(cert_thumb))
                {
                    Console.WriteLine("Nevalidna smartKartica!");
                    return false; //da ostane return ili nesto drugo, jer ne sme dalje da krene da izvrsava
                }
                else
                {
                    Console.WriteLine("Smart kartica je validna!");
                    List<KeyValuePair<string, User>> ExistingUserList = atmToSCSProxy.GetExistingUsers();
                    foreach (KeyValuePair<string, User> kvp in ExistingUserList)
                    {
                        if (kvp.Key.Equals(username))
                        {
                            tempUser = kvp.Value;
                            break;
                        }
                    }
                    if (tempUser != null)
                    {
                        binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
                        using(AtmToSecurityCheckProxy atmToSCProxy = new AtmToSecurityCheckProxy(binding, address))
                        {
                            Console.WriteLine("Check pin: pin = {0}", pin);
                            if (atmToSCProxy.CheckPin(username, pin))
                            {

                                tempUser.IsAuthenticated = true;
                                atmToSCSProxy.SerializingChangedData(tempUser);

                                return true;
                            }
                            else
                                return false;
                        }
                        
                    }
                    else
                    {
                        Console.WriteLine("Korisnik sa datim usernamom ne postoji!");
                        return false;
                    }
                }
            }
        }

        public bool PullMoney(string username, string amount)
        {
            Console.WriteLine("PullMoney started + amount {0} + username {1} ", amount, username);
            ///SmartCardService.SmartCardService scs = new SmartCardService.SmartCardService();
            ///localhost:9090/SmartCardService
            string addressToSc = "net.tcp://localhost:9090/SmartCardService";
            using (AtmToSmartCardProxy atmToSCProxy = new AtmToSmartCardProxy(binding, addressToSc))
            {
                User tempUser = null;
                List<KeyValuePair<string, User>> ExistingUserList = atmToSCProxy.GetExistingUsers();
                foreach (KeyValuePair<string, User> kvp in ExistingUserList)
                {
                    if (kvp.Key.Equals(username))
                    {
                        tempUser = kvp.Value;
                        break;
                    }
                }
                if(!tempUser.IsAuthenticated)
                {
                    Audit.PullMoneyFail(username, amount);
                    return false;
                }
                if (tempUser!=null)
                {
                    if ((tempUser.Balance) - Double.Parse(amount) >= 0)
                    {
                        tempUser.Balance = tempUser.Balance - Double.Parse(amount);
                        tempUser.IsAuthenticated = false;
                        Console.WriteLine("Uspesno ste podigili {0} dinara sa vaseg racuna!", amount);
                        atmToSCProxy.SerializingChangedData(tempUser);
                        Audit.PullMoneySuccess(username, amount);
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Nemate dovoljno novca na racunu.");
                        tempUser.IsAuthenticated = false;
                        atmToSCProxy.SerializingChangedData(tempUser);
                        Audit.PullMoneyFail(username, amount);
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Trazeni korisnik ne postoji u sistemu.");
                    tempUser.IsAuthenticated = false;
                    atmToSCProxy.SerializingChangedData(tempUser);
                    Audit.PullMoneyFail(username, amount);
                    return false; 
                        
                }
            }

            

        }

        public bool PushMoney(string username, string amount)
        {
            string addressToSc = "net.tcp://localhost:9090/SmartCardService";
            using (AtmToSmartCardProxy atmToSCProxy = new AtmToSmartCardProxy(binding, addressToSc))
            {

                List<KeyValuePair<string, User>> ExistingUserList = atmToSCProxy.GetExistingUsers();

                Console.WriteLine("PushMoney started + amount {0} + username {1} ", amount, username);
                SmartCardService.SmartCardService scs = new SmartCardService.SmartCardService();
                Console.WriteLine("ExistingUsers count {0}", ExistingUserList.Count);
                User tempUser = null;

                foreach(KeyValuePair<string, User> kvp in ExistingUserList)
                {
                    if(kvp.Key.Equals(username))
                    {
                        tempUser = kvp.Value;
                        break;
                    }
                }

                if (!tempUser.IsAuthenticated)
                {
                    Audit.PushMoneyFail(username, amount);
                    return false;
                }

                if (tempUser != null)
                {
                    tempUser.Balance = tempUser.Balance + Double.Parse(amount);
                    tempUser.IsAuthenticated = false;
                    Console.WriteLine("Uspesno ste uplatili {0} dinara na vas racun!", amount);
                    atmToSCProxy.SerializingChangedData(tempUser);
                    Audit.PushMoneySuccess(username, amount);
                    return true;
                }
                else
                {
                    Console.WriteLine("Trazeni korisnik ne postoji u sistemu.");
                    tempUser.IsAuthenticated = false;
                    atmToSCProxy.SerializingChangedData(tempUser);
                    Audit.PushMoneyFail(username, amount);
                    return false;
                }
            }

           
        }
    }
}
