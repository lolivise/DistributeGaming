using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DistributGameServer1
{
    /*
     *Function of the server that connected client can use 
     **/
    [ServiceContract(CallbackContract = typeof(IDGSerControllerCallback))]
    public interface IDGSerController
    {
        //for Charater Selcetion UI 
        [OperationContract(IsOneWay = true)]
        void RegisterClient(String username);

        [OperationContract(IsOneWay = true)]
        void NotifyServer();

        [OperationContract]
        List<String> GetHeroInfoList();

        [OperationContract]
        List<String> GetHeroNameList();

        [OperationContract]
        String GetSelectedBossInfo();

        [OperationContract]
        void AddNumberOfPlayer();

        [OperationContract]
        void AddHeroSelectTimes(int heroID);

        //Callback Function for Combat Interface UI
        [OperationContract(IsOneWay = true)]
        void RegisterPlayer(String username, int selectedHeroID);

        [OperationContract(IsOneWay = true)]
        void NotifyAllPlayer();

        [OperationContract(IsOneWay = true)]
        void NotifyDecisionTime();

        //Function For Initiating Combat Interface
        [OperationContract]
        String GetHeroName(int heroID);

        [OperationContract]
        int GetSelectedHeroHP(String username);

        [OperationContract]
        String GetBossName();

        [OperationContract]
        int GetBossMaxHP();

        [OperationContract]
        Abilities GetAbilityInfo(int heroID);

        //Function for Combat
        [OperationContract]
        void SetPlayerAbility(String username, int abilityID, int targetMember);

        [OperationContract]
        String ShowSelection(int heroID, int abilityID, int targetMember);

        [OperationContract]
        Allies GetAlliesInfo();

        //Function for Biz Server Update Server Information
        [OperationContract]
        Dictionary<String, String> GetPlayersHeroName();

        [OperationContract]
        Dictionary<String, String> GetPlayersHeroHPInfo();
    }
}
