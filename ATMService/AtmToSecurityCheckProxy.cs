using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using ServiceContracts;

namespace ATMService
{
    public class AtmToSecurityCheckProxy : ChannelFactory<ISecurityChecks>, ISecurityChecks, IDisposable
    {
        ISecurityChecks factory;

        public AtmToSecurityCheckProxy(NetTcpBinding binding, string address) : base(binding, address)
        {
            factory = this.CreateChannel();
        }

        public bool CheckPin(string username, string pin)
        {
            return factory.CheckPin(username, pin);
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
