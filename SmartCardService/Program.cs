using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceContracts;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace SmartCardService
{
    public class Program
    {
        static void Main(string[] args)
        {
            /// SmartCardService host za komunikaciju sa ATM - security checks
            NetTcpBinding binding = new NetTcpBinding();
            binding.OpenTimeout = new TimeSpan(0, 60, 0);
            binding.CloseTimeout = new TimeSpan(0, 60, 0);
            binding.SendTimeout = new TimeSpan(0, 60, 0);
            binding.ReceiveTimeout = new TimeSpan(0, 60, 0);

            string address = "net.tcp://localhost:8000/SmartCardService";

            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            ServiceHost sCSAtmHost = new ServiceHost(typeof(SecurityCheck));
            sCSAtmHost.AddServiceEndpoint(typeof(ISecurityChecks), binding, address);

            sCSAtmHost.Description.Behaviors.Remove(typeof(ServiceDebugBehavior));
            sCSAtmHost.Description.Behaviors.Add(new ServiceDebugBehavior() { IncludeExceptionDetailInFaults = true });

            sCSAtmHost.Open();
            Console.WriteLine("SecurityCheck service is started.");


            /// SmartCardService host za komunikaciju sa klijentom
            NetTcpBinding binding2 = new NetTcpBinding();
            binding2.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            string address2 = "net.tcp://locahost:7000/SmartCardService";

            ServiceHost sCSClientHost = new ServiceHost(typeof(SmartCardService));
            sCSClientHost.AddServiceEndpoint(typeof(ISmartCardService), binding2, address2);

            sCSClientHost.Description.Behaviors.Remove(typeof(ServiceDebugBehavior));
            sCSClientHost.Description.Behaviors.Add(new ServiceDebugBehavior() { IncludeExceptionDetailInFaults = true });
   
            sCSClientHost.Open();


            /// SmartCardService host za komunikaciju sa atm - data
            NetTcpBinding binding3 = new NetTcpBinding();
            binding3.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            string address3 = "net.tcp://localhost:9090/SmartCardService";

            ServiceHost sCSAtmDataHost = new ServiceHost(typeof(SmartCardService));
            sCSAtmDataHost.AddServiceEndpoint(typeof(ISmartCardService), binding3, address3);
            sCSAtmDataHost.Description.Behaviors.Remove(typeof(ServiceDebugBehavior));
            sCSAtmDataHost.Description.Behaviors.Add(new ServiceDebugBehavior() { IncludeExceptionDetailInFaults = true });

            sCSAtmDataHost.Open();

        

            Console.WriteLine("SmartCardService service is started.");
            Console.WriteLine("Press <enter> to stop services...");

            Console.ReadKey();
            Console.ReadKey();
            sCSAtmHost.Close();
            sCSClientHost.Close();
        }
    }
}
