using Common;
using Manager;
using Modeli;
using ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace SmartCardService
{
    public class SmartCardService : ISmartCardService
    {
        /// <summary>
        /// u listi ce biti smesteni thumbprintovi certifikata koji su povuceni od strane klijenata
        /// </summary>
        private static List<string> SmartCardRevocationList = new List<string>(20);

        /// <summary>
        /// u lsiti ce biti smesteni svi postojeci korisnici
        /// </summary>
        private static Dictionary<string, User> ExistingUsers = new Dictionary<string, User>(10);

        /// <summary>
        /// sluzi nam za serijalizovanje/deserijalizovanje podataka
        /// </summary>
        private DataIO serializer = new DataIO();

        /// <summary>
        ///     binding i address koji se koriste za komunikacija SmartCardService sa SmartCardBackupService
        /// </summary>
        private NetTcpBinding binding = new NetTcpBinding();

        private string address = "net.tcp://localhost:9099/SmartCardBackupService";

        /// <summary>
        /// konstruktor koji ce uvek kada se kreira objekat SmarCardServic-a da osvezi podatke
        /// </summary>
        public SmartCardService()
        {
            DataIO serializer = new DataIO();

            List<User> existingUsersDe = serializer.DeSerializeObject<List<User>>("existingUsers.xml");
            if (existingUsersDe != null)
                ExistingUsers = existingUsersDe.ToDictionary(x => x.Username, x => x);
        }

        public void CreateNewSmartCard(string username, string pin)
        {
            List<User> existingUsersDe = serializer.DeSerializeObject<List<User>>("existingUsers.xml");
            if (existingUsersDe != null)
                ExistingUsers = existingUsersDe.ToDictionary(x => x.Username, x => x);

            Console.WriteLine("CreateNewSmartcard started + username: {0} +  pin: {1} ", username, pin);

            //Common.Certificate.ExecuteCommandSync("cd " + AppDomain.CurrentDomain.BaseDirectory);
            Common.Certificate.ExecuteCommandSync("makecert -sv " + username + ".pvk -iv TestCA.pvk -n \"CN = " + username + "\" -pe -ic TestCA.cer " + username + ".cer -sr localmachine -ss My -sky exchange");
            Common.Certificate.ExecuteCommandSync("pvk2pfx.exe /pvk " + username + ".pvk /pi ftn /spc " + username + ".cer /pfx " + username + ".pfx");

            Console.WriteLine("Instalirati pfx fajl u cmm <enter>");
            Console.ReadKey();
            //Common.Certificate.ExecuteCommandSync("cd  C:\\Program Files (x86)\\Windows Resource Kits\\Tools");
            Common.Certificate.ExecuteCommandSync(@"winhttpcertcfg - g - c LOCAL_MACHINE\My - s " + username + " - a " + username);
            Console.WriteLine("winhttpcertcfg uspesno izvrsen");

            ///kriptovanje pina pre serijalizacije u fajl na sistemu
            //SHA256 mySHA256 = SHA256Managed.Create();
            //byte[] hashPin;
            //string hashPinString;
            //hashPin = mySHA256.ComputeHash(Encoding.UTF8.GetBytes(pin));
            //hashPinString = System.Text.Encoding.UTF8.GetString(hashPin);
            //Console.WriteLine("Hash pina u string interpretaciji izgleda ovako: " + hashPinString);
            //nakon kriptovanja dodajemo na stticnu listu svih usera
            //ExistingUsers.Add(username, new User(username, hashPinString , 0));

            /// korisnik vec postoji u listi - pokrenuto je generisanje novog sertifikata nakon invalidacije prethodnog
            if (ExistingUsers.ContainsKey(username))
            {
                ExistingUsers[username].Pin = pin;
            }
            else
                ExistingUsers.Add(username, new User(username, pin, 0));

            //preuzimamo usere iz recnika kako bi mogli da serijalizujemo
            List<User> existingUsersSe = ExistingUsers.Values.ToList();

            //serijalizacija nakon dodavanja novog usera
            serializer.SerializeObject(existingUsersSe, "existingUsers.xml");

            // salje podatke na SmartCardBackupService
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            using (ScToScbServiceProxy scToScbProxy = new ScToScbServiceProxy(binding, address))
            {
                scToScbProxy.UserListBackup(existingUsersSe);
            }

            Audit.NewSmartCardCreatedSuccess(username);
        }

        public bool InvalidateSmartCard(string username, string pin, string cert_thumb)
        {
            Console.WriteLine("InvalidateSmartCard started + username: {0} +  pin: {1} ", username, pin);

            /// provera ispravnosti pin koda
            if (ExistingUsers.ContainsKey(username))
            {
                if (!ExistingUsers[username].Pin.Equals(pin))
                {
                    Audit.RejectingOldSmartCardFail(username);
                    return false;
                }
            }
            else
            {
                Audit.RejectingOldSmartCardFail(username);
                return false;
            }

            /// Smestanje sertifikata (thumbprint) u revocation listu

            /// preuzimanje sertifikata sa storage
            //X509Certificate2 cert =  CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, username);

            /// uspesno smestanje sertifikata u listu zahteva generisanje novog seritifikata - metoda vraca true
            if (cert_thumb != null)
            {
                List<string> SmartCardRevocationList = GetSmartCardRevocationList();

                SmartCardRevocationList.Add(cert_thumb);
                serializer.SerializeObject(SmartCardRevocationList, "smartCardRevocationList.xml");

                binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
                using (ScToScbServiceProxy scToScbProxy = new ScToScbServiceProxy(binding, address))
                {
                    scToScbProxy.RevocationListBackup(SmartCardRevocationList);
                }

                Audit.RejectingOldSmartCardSuccess(username);
                return true;
            }
            else
            {
                Console.WriteLine("Smart kartica nije ispravna");
                Audit.RejectingOldSmartCardFail(username);
                return false;
            }
        }

        public bool ResetPinCode(string username, string oldPin, string newPin)
        {
            Console.WriteLine("ResetPinCode started + username: {0} +  oldpin: {1} +  newpin: {2} ", username, oldPin, newPin);
            User tempUser = new User();
            if (ExistingUsers.TryGetValue(username, out tempUser))
            {
                //SHA256 mySHA256 = SHA256Managed.Create();
                //byte[] hashPin;
                //string hashPinString;
                //hashPin = mySHA256.ComputeHash(Encoding.UTF8.GetBytes(oldPin));
                //hashPinString = System.Text.Encoding.UTF8.GetString(hashPin);
                //Console.WriteLine("Hash pina u string interpretaciji  koji je korisnik uneo kao oldPIn izgleda ovako: " + hashPinString);
                //Console.WriteLine("Hash pina u string interpretaciji koji korisnik zapravo ima u bazi podataka je: " + tempUser.Pin);
                if (tempUser.Pin.Equals(oldPin))
                {
                    Console.WriteLine("Korisnik uneo dobar pin, odobrena promena starog pina");
                    //byte[] hashPin2;
                    //string hashPinString2;
                    //hashPin2 = mySHA256.ComputeHash(Encoding.UTF8.GetBytes(newPin));
                    //hashPinString2 = System.Text.Encoding.UTF8.GetString(hashPin2);
                    //tempUser.Pin = hashPinString2;
                    tempUser.Pin = newPin;
                    ExistingUsers[username] = tempUser;

                    //serijalizacija nakon menjanja pina
                    List<User> existingUsersSe = ExistingUsers.Values.ToList();
                    serializer.SerializeObject(existingUsersSe, "existingUsers.xml");
                    Console.WriteLine("Podaci azurirani, pin promenjen, novi podaci upisani u fajl sistem");

                    // backup

                    Console.WriteLine("Backup: User list - ResetPinCode ...");
                    binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
                    using (ScToScbServiceProxy scToScbProxy = new ScToScbServiceProxy(binding, address))
                    {
                        scToScbProxy.UserListBackup(existingUsersSe);
                    }

                    Audit.PinResetSuccess(username);

                    return true;
                }
                else
                {
                    Console.WriteLine("Niste uneli dobar stari pin. Prekida se akcija menjana pina.");
                    Audit.PinResetFail(username);

                    return false;
                }
            }
            else
            {
                Console.WriteLine("Korisnik ne postoji u fajl sistemu.");
                Audit.PinResetFail(username);

                return false;
            }
        }

        public bool UserCheck(string username)
        {
            List<User> existingUsersDe = serializer.DeSerializeObject<List<User>>("existingUsers.xml");
            if (existingUsersDe != null)
                ExistingUsers = existingUsersDe.ToDictionary(x => x.Username, x => x);

            User user = null;
            ExistingUsers.TryGetValue(username, out user);
            if (user != null)
                return true;
            else
                return false;
        }

        internal static Dictionary<string, User> GetExistingUsersS()
        {
            return ExistingUsers;
        }

        List<KeyValuePair<string, User>> ISmartCardService.GetExistingUsers()
        {
            return ExistingUsers.ToList();
        }

        public List<string> GetSmartCardRevocationList()
        {
            SmartCardRevocationList = serializer.DeSerializeObject<List<string>>("smartCardRevocationList.xml");
            if (SmartCardRevocationList == null)
                return new List<string>(20);
            else
                return SmartCardRevocationList;
        }

        public void SerializingChangedData(User user)
        {
            /// setovanje novih podataka za usera

            if (ExistingUsers.ContainsKey(user.Username))
            {
                ExistingUsers[user.Username] = user;

                ///preuzimamo usere iz recnika kako bi mogli da serijalizujemo
                List<User> existingUsersSe = ExistingUsers.Values.ToList();

                ///serijalizacija nakon dodavanja novog usera
                serializer.SerializeObject(existingUsersSe, "existingUsers.xml");

                ///
                Console.WriteLine("Backup: User list - User Changed...");
                binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
                using (ScToScbServiceProxy scToScbProxy = new ScToScbServiceProxy(binding, address))
                {
                    scToScbProxy.UserListBackup(existingUsersSe);
                }
            }
        }
    }
}