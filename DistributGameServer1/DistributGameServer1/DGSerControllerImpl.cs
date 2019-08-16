using DistributGameData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DistributGameServer1
{
    /**
    * Implimentation of interface IDGSerController.
    * provide function for connected client.
    * **/
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    internal class DGSerControllerImpl : IDGSerController
    {
        private Dictionary<String, IDGSerControllerCallback> clients;
        private Dictionary<String, IDGSerControllerCallback> players;
        private object clientLock;
        private object playerLock;
        private object abilityLock;

        private Dictionary<String, int> selectedHeroHP;
        private Dictionary<String, int> playerSelectedHero;
        private Dictionary<String, int> playerSelectedAbility;
        private Dictionary<String, int> playerSelectedTarget;
        private Dictionary<String, String> playersHeroName;
        private Dictionary<String, String> playersHeroHPInfo;


        private int decisionTime;
        private int numberOfPlayer;
        private int number;
        private List<int> heroSelectTimes;
        private List<String> heroInfoList;
        private List<String> heroNameList;

        //Boss detail
        private String selectedBossInfo;
        private int bossCurrentHP;
        private String lastTarget;

        //Combat Info
        private String combatResult;

        IDGDataController m_DGData;
        HeroInfo heroInfo;
        Hero hero;
        Ability ability;
        Allies allies;
        Abilities selection;
        BossInfo bossInfo;
        Boss boss;
        Calculation calculation;

        public DGSerControllerImpl()
        {
            ChannelFactory<IDGDataController> DGData;
            NetTcpBinding tcpBinding = new NetTcpBinding();

            //release the limitation of the size of message which can be sent
            tcpBinding.MaxReceivedMessageSize = System.Int32.MaxValue;
            tcpBinding.ReaderQuotas.MaxArrayLength = System.Int32.MaxValue;

            try
            {
                //connect to Data Tier
                DGData = new ChannelFactory<IDGDataController>(tcpBinding, "net.tcp://localhost:50001/DGData");
                m_DGData = DGData.CreateChannel();
            }
            catch (CommunicationObjectFaultedException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("From DGSerControllerImpl.cs > DGSerControllerImpl\n" + e.Message);
            }

            calculation = new Calculation();
            heroInfoList = new List<String>();
            heroNameList = new List<String>();
            try
            {
                heroInfo = m_DGData.GetHeroInfo();
                bossInfo = m_DGData.GetBossInfo();
                buildHeroInfoList();
                initServer();
            }
            catch (Exception e)
            {
                Console.WriteLine("From DGSerControllerImpl.cs > DGSerControllerImpl\n" + e.Message);
            }
            
            Console.WriteLine("Server1 Object Created");
        }

        //convert hero information to list of string for character selection iterface to show
        private void buildHeroInfoList()
        {
            for (int index = 0; index < heroInfo.Heroes.Count; index++)
            {
                hero = heroInfo.Heroes[index];
                String heroMessage = hero.heroName + ", HP: " + hero.healthPoints + ", DEF: " + hero.defence
                                + "\nAbility:";
                for (int j = 0; j < hero.abilityList.Count; j++)
                {
                    ability = hero.abilityList[j];
                    heroMessage = heroMessage + "\n" + ability.description
                                              + "\nValue: " + ability.value
                                              + ", Type: " + ability.type
                                              + ", Target: " + ability.target;
                }
                heroInfoList.Add(heroMessage);
                heroNameList.Add(hero.heroName);
            }
        }

        //reset the server for next combat
        private void initServer()
        {
            clients = new Dictionary<String, IDGSerControllerCallback>();
            players = new Dictionary<String, IDGSerControllerCallback>();
            selectedHeroHP = new Dictionary<String, int>();
            playerSelectedHero = new Dictionary<String, int>();
            playerSelectedAbility = new Dictionary<String, int>();
            playerSelectedTarget = new Dictionary<String, int>();
            playersHeroName = new Dictionary<String, String>();
            playersHeroHPInfo = new Dictionary<String, String>();
            clientLock = new object();
            playerLock = new object();
            abilityLock = new object();
            numberOfPlayer = 0;
            number = 0;
            combatResult = "";
            initBossInfo();
            heroSelectTimes = new List<int>();
            for (int i = 0; i < heroInfo.Heroes.Count; i++)
            {
                heroSelectTimes.Add(0);
            }
        }

        //Randomly select a new boss for current combat
        private void initBossInfo()
        {
            Random seed = new Random();
            int indexOfBoss = seed.Next(System.Int32.MaxValue) % bossInfo.Bosses.Count;
            boss = bossInfo.Bosses[indexOfBoss];
            selectedBossInfo = boss.bossName + "\n"
                              + "\nHP  : " + boss.healthPoints
                              + "\nATK : " + boss.damage
                              + "\nDEF : " + boss.defence
                              + "\nTarget Strategy : " + boss.targetStrategy;
            bossCurrentHP = boss.healthPoints;
            lastTarget = "";
        }

        /*******************Function For Character Selection UI*******************/

        //check connected client
        public void RegisterClient(String username)
        {
            if (username != null && username != "")
            {
                try
                {
                    IDGSerControllerCallback callback = OperationContext.Current.GetCallbackChannel<IDGSerControllerCallback>();
                    lock (clientLock)
                    {
                        //remove the old client
                        if (clients.Keys.Contains(username))
                        {
                            clients.Remove(username);
                        }
                        clients.Add(username, callback);
                        setPlayersData();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        //broadcast number of clients and how many time each hero has been select to all the client
        public void NotifyServer()
        {
            lock (clientLock)
            {
                var inactiveClients = new List<String>();
                foreach (var client in clients)
                {
                    try
                    {
                        client.Value.BroadcastToClient(numberOfPlayer, heroSelectTimes);
                    }
                    catch (Exception)
                    {
                        inactiveClients.Add(client.Key);
                    }

                }
                //remove the client who is already disconnect
                if (inactiveClients.Count > 0)
                {
                    foreach (var client in inactiveClients)
                    {
                        clients.Remove(client);
                    }
                }
            }
        } 

        public List<string> GetHeroInfoList()
        {
            return heroInfoList;
        }

        public List<string> GetHeroNameList()
        {
            return heroNameList;
        }

        public string GetSelectedBossInfo()
        {
            return selectedBossInfo;
        }

        public void AddNumberOfPlayer()
        {
            numberOfPlayer++;
        }

        public void AddHeroSelectTimes(int heroID)
        {
            heroSelectTimes[heroID]++;
        }

        /*******************Function For Combat Interface UI*******************/

        //Register Connected Players
        public void RegisterPlayer(String username, int selectedHeroID)
        {
            if (username != null && username != "")
            {
                try
                {
                    IDGSerControllerCallback callback = OperationContext.Current.GetCallbackChannel<IDGSerControllerCallback>();
                    int playerHP = heroInfo.Heroes[selectedHeroID].healthPoints;
                    lock (playerLock)
                    {
                        //remove the old Player
                        if (players.Keys.Contains(username))
                        {
                            number--;
                            players.Remove(username);
                            selectedHeroHP.Remove(username);
                            playerSelectedHero.Remove(username);
                            playerSelectedAbility.Remove(username);
                            playerSelectedTarget.Remove(username);

                        }
                        number++;
                        players.Add(username, callback);
                        selectedHeroHP.Add(username, playerHP);
                        playerSelectedHero.Add(username, selectedHeroID);
                        playerSelectedAbility.Add(username, -1);
                        playerSelectedTarget.Add(username, -1);               

                        //if the number of players is five start combat
                        if (number == 5)
                        {
                            startCount();
                        }
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        //Broadcast information to All Players
        public void NotifyAllPlayer()
        {
            lock (playerLock)
            {
                var inactivePlayers = new List<String>();
                foreach (var player in players)
                {
                    setPlayersData();
                    try
                    {
                        player.Value.BroadcastToPlayer(allies, selectedHeroHP[player.Key], bossCurrentHP, lastTarget, combatResult);
                    }
                    catch (Exception)
                    {
                        inactivePlayers.Add(player.Key);
                    }

                }

                //remove the player who is already disconnect
                if (inactivePlayers.Count > 0)
                {
                    foreach (var player in inactivePlayers)
                    {
                        players.Remove(player);
                        selectedHeroHP.Remove(player);
                        playerSelectedHero.Remove(player);
                        playerSelectedAbility.Remove(player);
                        playerSelectedTarget.Remove(player);
                    }

                }
            }
                
        }

        //renew the detail of the allies
        private void setPlayersData()
        {
            allies = new Allies();
            playersHeroName = new Dictionary<String, String>();
            playersHeroHPInfo = new Dictionary<String, String>();
            int index = 0;
            foreach (var client in clients)
            {
                String name = client.Key;
                int heroID;
                String heroName = "Selecting Hero...";
                String heroHPInfo = "";
                if (players.Keys.Contains(client.Key))
                {
                    heroID = playerSelectedHero[name];
                    heroName = heroInfo.Heroes[heroID].heroName;
                    heroHPInfo = selectedHeroHP[name] + " / " + heroInfo.Heroes[heroID].healthPoints;
                    
                }
                Ally ally = new Ally(name, heroName, heroHPInfo);
                allies.alliesList.Insert(index, ally);
                playersHeroName.Add(name,heroName);
                playersHeroHPInfo.Add(name, heroHPInfo);
                index++;

            }

        }

        //return the name of hero that players have select
        public String GetHeroName(int heroID)
        {
            return heroInfo.Heroes[heroID].heroName;
        }

        //return the HP of hero that players have select
        public int GetSelectedHeroHP(String username)
        {
            int hp = 0;
            try
            {
                hp = selectedHeroHP[username];
            }
            catch (Exception)
            {
            }
            
            return hp;
        }

        //return name of current boss
        public string GetBossName()
        {
            return boss.bossName;
        }

        //return Max HP of current boss
        public int GetBossMaxHP()
        {
            return boss.healthPoints;
        }

        //return the list of abilities that players can select
        public Abilities GetAbilityInfo(int heroID)
        {
            lock (abilityLock)
            {
                selection = new Abilities();
                hero = heroInfo.Heroes[heroID];
                for (int i = 0; i < hero.abilityList.Count; i++)
                {
                    String abilityInfo = hero.abilityList[i].description
                                     + "\nValue: " + hero.abilityList[i].value
                                     + ", Type: " + hero.abilityList[i].type
                                     + ", Target: " + hero.abilityList[i].target;
                    AbilityInfo infoList = new AbilityInfo(abilityInfo);
                    selection.abilitesList.Insert(i, infoList);
                }
                return selection;
            } 
        }

        //Set the ability that the player has decide
        public void SetPlayerAbility(String username, int abilityID, int targetMember)
        {
            playerSelectedAbility[username] = abilityID;
            playerSelectedTarget[username] = targetMember;
        }

        //return the notification of what alility players have decide
        public String ShowSelection(int heroID, int abilityID, int targetMember)
        {
            String selection = "";
            try
            {
                String abilityName = heroInfo.Heroes[heroID].abilityList[abilityID].description;
                abilityName = abilityName.Substring(0, abilityName.IndexOf(":"));
                String abilityType = heroInfo.Heroes[heroID].abilityList[abilityID].type;
                String abilityTarget = heroInfo.Heroes[heroID].abilityList[abilityID].target;
                if (abilityType == "D")
                {
                    selection = "You select " + abilityName;
                }
                else
                {
                    if (abilityTarget == "S" && targetMember == -1)
                    {
                        selection = "Please select a ally to heal";
                    }
                    else if (abilityTarget == "S" && targetMember != -1)
                    {
                        String target = allies.alliesList[targetMember].name;
                        if (selectedHeroHP.Keys.Contains(target) && selectedHeroHP[target] > 0)
                        {
                            selection = "You select " + abilityName + " to Heal " + allies.alliesList[targetMember].name;
                        }
                        else
                        {
                            selection = "The ally you select cannot be healed.";
                        }

                    }
                    else
                    {
                        selection = "You select " + abilityName + " to Heal all the players";
                    }
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                selection = "";
            }
            
            return selection;
        }

        //return allies information
        public Allies GetAlliesInfo()
        {
            return allies;
        }

        //delegate of Countdoun function
        private delegate Boolean DecisionCountDown();

        private void startCount()
        {
            DecisionCountDown decisionCountDown;
            decisionCountDown = this.timeout;
            
            AsyncCallback asyncCallback;
            asyncCallback = this.battleEnd;
            //fork a thread
            decisionCountDown.BeginInvoke(asyncCallback, OperationContext.Current.GetCallbackChannel<IDGSerControllerCallback>());
        }

        //remote callback the final battle result
        private void battleEnd(IAsyncResult iAsyncResult)
        {
            Boolean result;
            DecisionCountDown decisionCountDown;
            AsyncResult asyncResultObj = (AsyncResult)iAsyncResult;

            if (asyncResultObj.EndInvokeCalled == false)
            {
                decisionCountDown = (DecisionCountDown)asyncResultObj.AsyncDelegate;
                result = decisionCountDown.EndInvoke(asyncResultObj);
                if (result == true)
                {
                    combatResult = "WIN";
                }
                else
                {
                    combatResult = "LOSS";
                }
                //broadcast the result to all the players
                NotifyAllPlayer();
                //reset the Srever
                initServer();
            }
        }

        //Function of thirty seconds countdown 
        private Boolean timeout()
        {
            decisionTime = 30;
            while (true)
            {
                Thread.Sleep(1000);
                decisionTime--;
                if (players.Count == 0)
                {
                    return false;
                }
                if (decisionTime == 0)
                {

                    //Initiate Combat info
                    calculation = new Calculation();
                    calculation.setCombatInfo(playerSelectedHero,playerSelectedAbility,playerSelectedTarget,selectedHeroHP,bossCurrentHP);

                    //Players Round and check whether players WIN
                    if (calculation.calPlayerRound(heroInfo, allies, boss))
                    {
                        return true;
                    }

                    //Boss Round
                    calculation.calBossRound(boss,heroInfo,lastTarget);

                    //Check whether all players are died
                    if (calculation.isAllDead())
                    {
                        return false;
                    }

                    //Update boss current HP
                    bossCurrentHP = calculation.getBossCurrentHP();
                    //Update the player that boss targeted
                    lastTarget = calculation.getBossTarget();

                    //Update every player's HP
                    Dictionary<String, int> newPlayersHP = new Dictionary<String, int>();
                    newPlayersHP = calculation.getPlayersHP();
                    int idd = 0;
                    idd++;
                    foreach (var plyHP in newPlayersHP.ToArray())
                    {
                        if (selectedHeroHP.Keys.Contains(plyHP.Key))
                        {
                            selectedHeroHP[plyHP.Key] = plyHP.Value;
                        }
                    }
                    
                    restPlayerAbility();
                    decisionTime = 30;
                    //broadcast the information to players
                    NotifyAllPlayer();
                }
                //broadcast remain decition time and allies information
                NotifyDecisionTime();

            }

            
        }

        //reset the selection of ability of player
        private void restPlayerAbility()
        {
            foreach (var player in players)
            {
                playerSelectedAbility[player.Key] = -1;
                playerSelectedTarget[player.Key] = -1;
            }
        }

        //broadcast remain decition time and allies information
        public void NotifyDecisionTime()
        {
            var inactivePlayers = new List<String>();
            setPlayersData();
            //Callback decision time and allies list to each player
            foreach (var player in players)
            {
                try
                {
                    player.Value.BroadcastDecisionTime(decisionTime, allies);
                }
                catch (Exception)
                {
                    inactivePlayers.Add(player.Key);
                }

            }
            //remove the player who is disconnect
            if (inactivePlayers.Count > 0)
            {
                foreach (var player in inactivePlayers)
                {
                    clients.Remove(player);
                    players.Remove(player);
                    selectedHeroHP.Remove(player);
                    playerSelectedHero.Remove(player);
                    playerSelectedAbility.Remove(player);
                    playerSelectedTarget.Remove(player);
                }
                
            }
        }

        public Dictionary<String, String> GetPlayersHeroName()
        {
            return playersHeroName;
        }

        public Dictionary<String, String> GetPlayersHeroHPInfo()
        {
            return playersHeroHPInfo;
        }
    }
}
