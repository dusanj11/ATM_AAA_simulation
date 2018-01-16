using Common;
using Manager;
using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;

namespace Client
{
    public class Program
    {
        private static void Main(string[] args)
        {
            NetTcpBinding binding = new NetTcpBinding();
            binding.OpenTimeout = new TimeSpan(0, 60, 0);
            binding.CloseTimeout = new TimeSpan(0, 60, 0);
            binding.SendTimeout = new TimeSpan(0, 60, 0);
            binding.ReceiveTimeout = new TimeSpan(0, 60, 0);
            ///192.168.100.103 
            string addressToSmartCard = "net.tcp://localhost:7000/SmartCardService";

            /// ime korisnika koji je inicirao klijenta
            string name = Authentication.getUserName(WindowsIdentity.GetCurrent().Name);
            Console.WriteLine("Client name: {0}", name);

            using (ClientToSmartCardProxy cToSCProxy = new ClientToSmartCardProxy(binding, addressToSmartCard))
            {
                int opt;
                string pin, newPin, oldPin;
                
                do
                {

                    if (cToSCProxy.UserCheck(name))
                    {
                        /// Korisnik postoji u listi korisnika - ima sertifikat, moguca konekcija sa AtmService
                        NetTcpBinding bindingToAtm = new NetTcpBinding();
                        bindingToAtm.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

                        string addressToAtm = "net.tcp://localhost:6000/AtmService";

                        X509Certificate2 atmCert = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, "atmservice");

                        EndpointAddress address = new EndpointAddress(new Uri(addressToAtm),
                                                  new X509CertificateEndpointIdentity(atmCert));

                        X509Certificate2 smartCard = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, name);

                        using (ClientToAtmProxy cToAtmProxy = new ClientToAtmProxy(bindingToAtm, address))
                        {
                            Console.Clear();
                            Console.WriteLine("Autentifikacija [1]");
                            Console.WriteLine("Reset PIN koda [2]");
                            Console.WriteLine("Povlacenje smart kartice [3]");

                            try
                            {
                                opt = int.Parse(Console.ReadLine());
                            }
                            catch (Exception)
                            {

                                Console.WriteLine("Unesite broj [1-3]\nEnter za nastavak...");
                                Console.ReadKey();
                                continue;
                            }
                           

                            switch (opt)
                            {
                                case 1:
                                    {
                                        Console.Clear();
                                        Console.WriteLine("Unesite PIN kod:");
                                        pin = Console.ReadLine();
                                        SHA256 mySHA256 = SHA256Managed.Create();
                                        byte[] hashPin;
                                        string hashPinString;
                                        hashPin = mySHA256.ComputeHash(Encoding.UTF8.GetBytes(pin));
                                        hashPinString = System.Text.Encoding.UTF8.GetString(hashPin);
                                        Console.WriteLine("Hash pina u string interpretaciji koju je korisnik uneo izgleda ovako: " + hashPinString);
                                        if (cToAtmProxy.Authentificate(name, hashPinString, smartCard.Thumbprint.ToString()))
                                        {
                                            Console.Clear();
                                            Console.WriteLine("Uplata [1]");
                                            Console.WriteLine("Isplata [2]");

                                            opt = int.Parse(Console.ReadLine());

                                            switch(opt)
                                            {
                                                case 1:
                                                    {
                                                        Console.Clear();
                                                        Console.WriteLine("Unesite iznos: ");
                                                        string amout = Console.ReadLine();
                                                        if (cToAtmProxy.PushMoney(name, amout))
                                                            Console.WriteLine("Uspesno izvrsena transakcija");
                                                        else
                                                            Console.WriteLine("Neuspesna transakcija");
                                                        Console.WriteLine("<Enter> za nastavak...");
                                                        Console.ReadKey();
                                                        break;
                                                    }
                                                case 2:
                                                    {
                                                        Console.Clear();
                                                        Console.WriteLine("Unesite iznos: ");
                                                        string amout = Console.ReadLine();
                                                        if (cToAtmProxy.PullMoney(name, amout))
                                                            Console.WriteLine("Uspesno izvrsena transakcija");
                                                        else
                                                            Console.WriteLine("Neuspesna transakcija");
                                                        Console.WriteLine("<Enter> za nastavak...");
                                                        Console.ReadKey();
                                                        break;
                                                    }
                                            }
                                        }
                                        else
                                        {
                                            Console.Clear();
                                            Console.WriteLine("Nevalidna kartica ili pogresan pin");
                                            Console.WriteLine("<Enter> za nastavak...");
                                            Console.ReadKey();
                                        }

                                        break;
                                    }
                                case 2:
                                    {
                                        Console.Clear();
                                        Console.WriteLine("Unesite PIN kod:");
                                        oldPin = Console.ReadLine();
                                        SHA256 mySHA256 = SHA256Managed.Create();
                                        byte[] hashOldPin;
                                        string hashOldPinString;
                                        hashOldPin = mySHA256.ComputeHash(Encoding.UTF8.GetBytes(oldPin));
                                        hashOldPinString = Encoding.UTF8.GetString(hashOldPin);

                                        Console.WriteLine("Unesite novi PIN kod:");
                                        newPin = Console.ReadLine();
                                        byte[] hashNewPin;
                                        string hashNewPinString;
                                        hashNewPin = mySHA256.ComputeHash(Encoding.UTF8.GetBytes(newPin));
                                        hashNewPinString = Encoding.UTF8.GetString(hashNewPin);

                                        if (cToSCProxy.ResetPinCode(name, hashOldPinString, hashNewPinString))
                                            Console.WriteLine("Pin kod uspesno izmenjen");
                                        else
                                            Console.WriteLine("Neuspesna izmena pin koda");
                                        Console.WriteLine("<Enter> za nastavak...");
                                        Console.ReadKey();
                                        break;
                                    }
                                case 3:
                                    {
                                        Console.Clear();
                                        Console.WriteLine("Unesite PIN kod:");
                                        pin = Console.ReadLine();
                                        SHA256 mySHA256 = SHA256Managed.Create();
                                        byte[] hashPin;
                                        string hashPinString;
                                        hashPin = mySHA256.ComputeHash(Encoding.UTF8.GetBytes(pin));
                                        hashPinString = Encoding.UTF8.GetString(hashPin);

                                        if (cToSCProxy.InvalidateSmartCard(name, hashPinString, smartCard.Thumbprint.ToString()))
                                        {
                                            /// povlacenje uspesno - kreirati novu karticu
                                            Console.Clear();
                                            Console.WriteLine("Izdavanje nove smart kartice <ENTER>");
                                            Console.ReadKey();
                                            Console.WriteLine("Unesite zeljeni PIN: ");
                                            pin = Console.ReadLine();

                                            hashPin = mySHA256.ComputeHash(Encoding.UTF8.GetBytes(pin));
                                            hashPinString = Encoding.UTF8.GetString(hashPin);

                                            cToSCProxy.CreateNewSmartCard(name, hashPinString);

                                        }
                                        else
                                        {
                                            Console.Clear();
                                            Console.WriteLine("Pogresan pin kod");
                                            Console.WriteLine("<Enter> za nastavak...");
                                            Console.ReadKey();
                                        }
                                        break;
                                    }
                            }
                        }
                    }
                    else
                    {
                        /// Korisnik ne postoji u listi korisnika - nema sertifikat, moguce izdavanje smart kartice
                        
                        Console.Clear();
                        Console.WriteLine("Izdavanje nove smart kartice <ENTER>");
                        Console.ReadKey();
                        Console.WriteLine("Unesite zeljeni PIN: ");
                        pin = Console.ReadLine();
                        SHA256 mySHA256 = SHA256Managed.Create();
                        byte[] hashPin;
                        string hashPinString;
                        hashPin = mySHA256.ComputeHash(Encoding.UTF8.GetBytes(pin));
                        hashPinString = Encoding.UTF8.GetString(hashPin);

                        cToSCProxy.CreateNewSmartCard(name, hashPinString);
                    }
                }
                while (true);
            }
        }
    }
}