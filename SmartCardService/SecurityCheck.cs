using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceContracts;
using System.ServiceModel;
using System.Security.Cryptography;
using Modeli;
using Common;

namespace SmartCardService
{
    public class SecurityCheck : ISecurityChecks
    {
        public bool CheckPin(string username, string pin)
        {
            Console.WriteLine("CheckPin u SecurityCheck  username: {0}  pin: {1}", username, pin);

            DataIO serializer = new DataIO();
            ///SmartCardService scs = new SmartCardService(); // DA LI OVO TREBA
            
            User tempUser = new User();
            if(SmartCardService.GetExistingUsersS().TryGetValue(username, out tempUser))
            {
                Console.WriteLine("Hash pina u string interpretaciji koju korisnik ima u fajl sistemu izgleda ovako: " + tempUser.Pin);
                if (tempUser.Pin.Equals(pin))
                {
                    Console.WriteLine("Uneli ste tacan pin. Dobrodosli!");
                    return true;
                }
                else
                {
                    Console.WriteLine("Uneli ste NETACAN pin.");
                    return false;
                }
            }
            else
            {
                Console.WriteLine("User: {0} nije pronadjen u sistemu.", username);
                return false;
            }
        }
    }
}
