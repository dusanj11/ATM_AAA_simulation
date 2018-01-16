using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ServiceContracts
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IATMService" in both code and config file together.
    [ServiceContract]
    public interface IATMService
    {
        [OperationContract]
        bool Authentificate(string username, string pin, string cert_thumb); 

        [OperationContract]
        bool PushMoney(string username, string amount);

        [OperationContract]
        bool PullMoney(string username, string amount);
    }
}
