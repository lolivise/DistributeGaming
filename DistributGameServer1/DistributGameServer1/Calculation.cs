using DistributGameData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributGameServer1
{
    /**
     * calculate the result of combat of each round
     * **/
    class Calculation
    {
        private Dictionary<String, int> playerSelectedHero;
        private Dictionary<String, int> playerSelectedAbility;
        private Dictionary<String, int> playerSelectedTarget;
        private Dictionary<String, int> playersHP;
        private Dictionary<String, int> playersDamage;
        private int bossCurrentHP;

        private String bossTarget;
        private List<String> alivesList;

        public Calculation()
        {
            playerSelectedHero = new Dictionary<String, int>();
            playerSelectedAbility = new Dictionary<String, int>();
            playerSelectedTarget = new Dictionary<String, int>();
            playersHP = new Dictionary<String, int>();
            playersDamage = new Dictionary<string, int>();
            alivesList = new List<String>();
        }

        //Set the information of combat
        public void setCombatInfo(Dictionary<String, int> playerSelectedHero, Dictionary<String, int> playerSelectedAbility, Dictionary<String, int> playerSelectedTarget, Dictionary<String, int> playersHP, int bossCurrentHP)
        {
            this.playerSelectedHero = playerSelectedHero;
            this.playerSelectedAbility = playerSelectedAbility;
            this.playerSelectedTarget = playerSelectedTarget;
            this.bossCurrentHP = bossCurrentHP;
            this.playersHP = playersHP;
            foreach (var playerHP in playersHP)
            {
                if (playerHP.Value > 0)
                {
                    alivesList.Add(playerHP.Key);
                }
            }
            bossTarget = "";
        }

        //calculate player round
        public Boolean calPlayerRound(HeroInfo heroInfo, Allies allies, Boss boss)
        {
            int playersTotalDamage = 0;
            foreach (var player in playerSelectedHero)
            {
                String name = player.Key;
                int heroID = playerSelectedHero[name];
                int heroMaxHP = heroInfo.Heroes[heroID].healthPoints;
                int abilityID = playerSelectedAbility[name];
                int selectTargetID = playerSelectedTarget[name];
                if (abilityID != -1)
                {
                    int abilityValue = heroInfo.Heroes[heroID].abilityList[abilityID].value;
                    int abilityActualValue = calHeroActualValue(abilityValue);
                    
                    String abilityType = heroInfo.Heroes[heroID].abilityList[abilityID].type;
                    String abilityTarget = heroInfo.Heroes[heroID].abilityList[abilityID].target;

                    //if Damage boss
                    if (abilityType == "D")
                    {
                        playersTotalDamage += abilityActualValue;
                        playersDamage.Add(player.Key,abilityActualValue);
                    }
                    else
                    {
                        //if Heal a single ally
                        if (abilityTarget == "S")
                        {
                            if (selectTargetID != -1)
                            {
                                String target = allies.alliesList[selectTargetID].name;
                                if (alivesList.Contains(target))
                                {
                                    heal(target, heroInfo, abilityActualValue);
                                }
                            }
                            
                        }
                        else//if  heal all allies
                        {
                            foreach (var alive in alivesList)
                            {
                                heal(alive, heroInfo, abilityActualValue);
                            }
                        }
                    }
                }
                    
            }
            //calculate current HP of boss
            if (playersTotalDamage > boss.defence)
            {
                bossCurrentHP -= (playersTotalDamage - boss.defence);
            }

            //if boss is died
            if (bossCurrentHP <= 0)
            {
                return true;
            }

            return false;
        }

        //function of healing ability
        private void heal(String playerName, HeroInfo heroInfo, int healPoints)
        {
            int heroID = playerSelectedHero[playerName];
            int heroMaxHP = heroInfo.Heroes[heroID].healthPoints;
            playersHP[playerName] += healPoints;

            if (playersHP[playerName] > heroMaxHP)
            {
                playersHP[playerName] = heroMaxHP;
            }
        }

        //calcualte the result of boss round
        public void calBossRound(Boss boss, HeroInfo heroInfo, String lastTarget)
        {
            int bossActualDamageValue = calBossActualValue(boss.damage);

            bossDecideTarget(boss.targetStrategy);
            //decide the target player for this round
            if (bossTarget == "")
            {;
                if (lastTarget == "" || !alivesList.Contains(lastTarget))
                {
                    bossDecideTarget("R");
                }
                else
                {
                    bossTarget = lastTarget;
                }
            }

            int targetID = playerSelectedHero[bossTarget];
            int targetHP = playersHP[bossTarget];
            int targetDef = heroInfo.Heroes[targetID].defence;
            bossActualDamageValue = bossActualDamageValue - targetDef;
            //calculate the damage for player
            if (bossActualDamageValue > 0)
            {
                targetHP = targetHP - bossActualDamageValue;
                if (targetHP <= 0)
                {
                    playersHP[bossTarget] = 0;
                }
                else
                {
                    playersHP[bossTarget] = targetHP;
                }
            }
            
        }

        //function for decide target
        private void bossDecideTarget(String targetStrategy)
        {
            //if target strategy is random
            if (targetStrategy == "R")
            {
                Random rnd = new Random();
                int index = rnd.Next(0, alivesList.Count);
                bossTarget = alivesList[index];

            }
            else//if target strategy if highest damage
            {
                findHightesDamagePlayer();
            }

        }

        //find the player who cause highest damage in current round
        private void findHightesDamagePlayer()
        {
            int value = 0;
            foreach (var playerDamage in playersDamage)
            {
                if (value <= playerDamage.Value)
                {
                    value = playerDamage.Value;
                    bossTarget = playerDamage.Key;
                }
            }
        }

        //calculate the actual value of hero ability
        private int calHeroActualValue(int value)
        {
            int returnValue = 0;
            Random rnd = new Random();
            returnValue = rnd.Next(value / 4, value + 1);
            return returnValue;
        }

        //calculate the actual value of boss damage
        private int calBossActualValue(int value)
        {
            int returnValue = 0;
            Random rnd = new Random();
            returnValue = rnd.Next(value / 2, value + 1);
            return returnValue;
        }

        //return boss current HP
        public int getBossCurrentHP()
        {
            return bossCurrentHP;
        }

        //return the target that boss has selected
        public String getBossTarget()
        {
            return bossTarget;
        }

        //retrun whether all players are died
        public Boolean isAllDead()
        {
            foreach (var playerHP in playersHP)
            {
                //if any player is still alive
                if (playerHP.Value > 0)
                {
                    return false;
                }
            }

            return true;
        }

        //retrun players HP
        public Dictionary<String, int> getPlayersHP ()
        {
            return playersHP;
        }
    }
}
