using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DistributGameData
{
    //List of boss detail
    [DataContract]
    public class BossInfo
    {
        [DataMember]
        public List<Boss> Bosses { get; set; }

        public BossInfo()
        {
            Bosses = new List<Boss>();
        }

    }

    //the detail of boss
    [DataContract]
    public class Boss
    {
        [DataMember]
        public String bossName { get; set; }
        [DataMember]
        public int healthPoints { get; set; }
        [DataMember]
        public int defence { get; set; }
        [DataMember]
        public int damage { get; set; }
        [DataMember]
        public String targetStrategy { get; set; }

        public Boss(String BossName, int HealthPoints, int Defence, int Damage, String TargetStrategy)
        {
            bossName = BossName;
            healthPoints = HealthPoints;
            defence = Defence;
            damage = Damage;
            targetStrategy = TargetStrategy;
        }
    }
}
