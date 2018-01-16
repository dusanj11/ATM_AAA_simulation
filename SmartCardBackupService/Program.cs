using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using ServiceContracts;
using System.ServiceModel.Description;

namespace SmartCardBackupService
{
    public class Program
    {
        static void Main(string[] args)
        {
            NetTcpBinding binding = new NetTcpBinding();
            binding.OpenTimeout = new TimeSpan(0, 60, 0);
            binding.CloseTimeout = new TimeSpan(0, 60, 0);
            binding.SendTimeout = new TimeSpan(0, 60, 0);
            binding.ReceiveTimeout = new TimeSpan(0, 60, 0);

            string address = "net.tcp://localhost:9099/SmartCardBackupService";

            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            ServiceHost scbsHost = new ServiceHost(typeof(SmartCardBackupService));
            scbsHost.AddServiceEndpoint(typeof(ISmartCardBackupService), binding, address);

            scbsHost.Description.Behaviors.Remove(typeof(ServiceDebugBehavior));
            scbsHost.Description.Behaviors.Add(new ServiceDebugBehavior() { IncludeExceptionDetailInFaults = true });

            try
            {
                scbsHost.Open();
                Console.WriteLine("SmartCardBackup service is started.");
                Console.WriteLine("Press <enter> to exit");
                Console.ReadKey();
            }catch(Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
            }
            finally
            {
                scbsHost.Close();
            }

          

        }
    }
}
