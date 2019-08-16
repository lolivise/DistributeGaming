using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DistributGameServer1
{
    /**
     * remote callback function for connected client
     * **/
    [ServiceContract]
    public interface IDGSerControllerCallback
    {
        [OperationContract(IsOneWay = true)]
        void BroadcastToClient(int numOfPlayer, List<int> heroSelectTimes);

        [OperationContract(IsOneWay = true)]
        void BroadcastToPlayer(Allies allies, int selectedHeroHP, int bossCurrentHP, String lastTarget, String combatResult);

        [OperationContract(IsOneWay = true)]
        void BroadcastDecisionTime(int decisionTime, Allies allies);
    }
}
