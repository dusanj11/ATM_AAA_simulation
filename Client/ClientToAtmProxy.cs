using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceContracts;
using System.ServiceModel;
using Common;
using System.Security.Principal;
using Manager;
using System.Security.Cryptography.X509Certificates;

namespace Client
{
    public class ClientToAtmProxy : ChannelFactory<IATMService>, IATMService, IDisposable
    {
        IATMService factory;

        public ClientToAtmProxy(NetTcpBinding binding, EndpointAddress address) : base(binding, address)
        {
            string username = Authentication.getUserName(WindowsIdentity.GetCurrent().Name);
            //Console.WriteLine("Username: {0}", username);
            this.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.ChainTrust;
            //this.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.Custom;
            //this.Credentials.ServiceCertificate.Authentication.CustomCertificateValidator = new ClientCertValidator();
            this.Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

            /// Set appropriate client's certificate on the channel. Use CertManager class to obtain the certificate based on the "cltCertCN"
            this.Credentials.ClientCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, username);


            factory = this.CreateChannel();
        }

        public bool Authentificate(string username, string pin, string cert_thumb)
        {
            try
            {
                //Console.WriteLine("Klijent pokrenuo autentifikaciju.");
                return factory.Authentificate(username, pin, cert_thumb);
            }
            catch(Exception e)
            {
                Console.WriteLine("Error (ClientToAtm - Authentificate) : {0} " + e.Message);
                return false;
            }
        }

        public bool PullMoney(string username, string amount)
        {
            try
            {
                //Console.WriteLine("Klijent pokrenuo isplatu.");
                return factory.PullMoney(username, amount);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error (ClientToAtm - PullMoney) : {0} " + e.Message);
                return false;
            }
        }

        public bool PushMoney(string username, string amount)
        {
            try
            {
                //Console.WriteLine("Klijent pokrenuo uplatu.");
                return factory.PushMoney(username, amount);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error (ClientToAtm - PushMoney): {0} !!!", e.Message);
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
