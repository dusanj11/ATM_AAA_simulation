using Modeli;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ServiceContracts
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ISmartCardBackupService" in both code and config file together.
    [ServiceContract]
    public interface ISmartCardBackupService
    {
        [OperationContract]
        bool UserListBackup(List<User> userList);

        [OperationContract]
        bool RevocationListBackup(List<string> revocationList);
    }
}
