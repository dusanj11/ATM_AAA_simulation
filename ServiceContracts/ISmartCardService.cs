using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Modeli;

namespace ServiceContracts
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ISmartCardService" in both code and config file together.
    [ServiceContract]
    public interface ISmartCardService
    {
        [OperationContract]
        void CreateNewSmartCard(string username, string pin);

        [OperationContract]
        bool ResetPinCode(string username, string oldPin, string newPin);

        [OperationContract]
        bool InvalidateSmartCard(string username, string pin, string cert_thumb);

        [OperationContract]
        bool UserCheck(string username);

        [OperationContract]
        List<KeyValuePair<string, User>> GetExistingUsers();

        [OperationContract]
        List<string> GetSmartCardRevocationList();

        [OperationContract]
        void SerializingChangedData(User user);

    }
}
