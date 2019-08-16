using DistributGameData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DistributGameBiz
{
    /**
     * the function for the connect clients
     * **/
    [ServiceContract(CallbackContract = typeof(IDGBizControllerCallback))]
    public interface IDGBizController
    {
        [OperationContract]
        List<int> GetFriendID(int userID);

        [OperationContract]
        void ChangeUserState(int userID, String state);

        [OperationContract(IsOneWay = true)]
        void RegisterClient(String username);

        [OperationContract(IsOneWay = true)]
        void NotifyServer();

        [OperationContract(IsOneWay = true)]
        void NotifyServerState();

        //Function for Login UI
        [OperationContract]
        int CheckAuthority(String username, String password);

        [OperationContract]
        Boolean CheckDuplicateLogin(String username);

        //Function for Portal Selection UI
        [OperationContract]
        void AddClientInServer(String serverX);
        
        [OperationContract]
        int GetClientInServer(String serverX);

        [OperationContract]
        Dictionary<String, String> GetServerList();

        [OperationContract]
        ServerInfo GetServerInfo();

        [OperationContract]
        List<String> GetBossInfoList();

        [OperationContract]
        Boolean CheckServerAvailaility(int serverIndex);

    }
}
