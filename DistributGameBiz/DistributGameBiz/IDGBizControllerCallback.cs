using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DistributGameBiz
{
    /**
     * the remote callback function of Biz Tier
     * **/
    [ServiceContract]
    public interface IDGBizControllerCallback
    {
        [OperationContract(IsOneWay = true)]
        void BroadcastToClient(FriendState friendState);

        [OperationContract(IsOneWay = true)]
        void BroadcastServerState(ServerState serverState);
    }
}
