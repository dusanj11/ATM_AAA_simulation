using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Security.Cryptography.X509Certificates;
using ServiceContracts;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using Manager;

namespace ATMService
{
    public class Program
    {
        static void Main(string[] args)
        {
            string srvCertCN = "atmservice";

            NetTcpBinding binding = new NetTcpBinding();
            binding.OpenTimeout = new TimeSpan(0, 60, 0);
            binding.CloseTimeout = new TimeSpan(0, 60, 0);
            binding.SendTimeout = new TimeSpan(0, 60, 0);
            binding.ReceiveTimeout = new TimeSpan(0, 60, 0);

            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;


            string address = "net.tcp://localhost:6000/AtmService";

            

            ServiceHost atmServiceHost = new ServiceHost(typeof(ATMService));
            atmServiceHost.AddServiceEndpoint(typeof(IATMService), binding, address);

            atmServiceHost.Description.Behaviors.Remove(typeof(ServiceDebugBehavior));
            atmServiceHost.Description.Behaviors.Add(new ServiceDebugBehavior() { IncludeExceptionDetailInFaults = true });

            atmServiceHost.Credentials.ClientCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.ChainTrust;
            ///Custom validation mode enables creation of a custom validator - CustomCertificateValidator
	//***   //atmServiceHost.Credentials.ClientCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.Custom;
            //atmServiceHost.Credentials.ClientCertificate.Authentication.CustomCertificateValidator = new ServiceCertValidator();

            ///If CA doesn't have a CRL associated, WCF blocks every client because it cannot be validated
            atmServiceHost.Credentials.ClientCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

            ///Set appropriate service's certificate on the host. Use CertManager class to obtain the certificate based on the "srvCertCN"
			atmServiceHost.Credentials.ServiceCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, srvCertCN);

            try
            {
                atmServiceHost.Open();

                Console.WriteLine("ATMService service is started.");
                Console.WriteLine("Press <enter> to stop services...");

                Console.ReadLine();
            }
            catch(Exception e)
            {
                Console.WriteLine("[ERROR] {0}", e.Message);
                Console.WriteLine("[StackTrace] {0}", e.StackTrace);
            }
            finally
            {
                atmServiceHost.Close();
            }
            
            
           
        }
    }
}
