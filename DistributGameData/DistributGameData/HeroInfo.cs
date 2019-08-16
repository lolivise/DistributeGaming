using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DistributGameData
{
    //List of hero detail
    [DataContract]
    public class HeroInfo
    {
        [DataMember]
        public List<Hero> Heroes { get; set; }

        public HeroInfo()
        {
            Heroes = new List<Hero>();
        }

    }

    //the detail of hero
    [DataContract]
    public class Hero
    {
        [DataMember]
        public String heroName { get; set; }
        [DataMember]
        public int healthPoints { get; set; }
        [DataMember]
        public int defence { get; set; }
        [DataMember]
        public List<Ability> abilityList { get; set; }

        public Hero(String HeroName, int HealthPoints, int Defence, List<Ability> AbilityList)
        {
            abilityList = AbilityList;
            heroName = HeroName;
            healthPoints = HealthPoints;
            defence = Defence;
        }
    }

    
}
