using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DistributGameData
{
    /**
     * interface 
     * the function for connected client
     * **/
    [ServiceContract]
    public interface IDGDataController
    {
        [OperationContract]
        UserInfo GetUserInfo();

        [OperationContract]
        HeroInfo GetHeroInfo();

        [OperationContract]
        BossInfo GetBossInfo();

    }
}
