using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DistributGameServer1
{
    //the List of ability information
    [DataContract]
    public class Abilities
    {
        [DataMember]
        public List<AbilityInfo> abilitesList { get; set; }

        public Abilities()
        {
            abilitesList = new List<AbilityInfo>();
        }

    }

    //the detail of a ability information
    [DataContract]
    public class AbilityInfo
    {
        [DataMember]
        public String abilityDetail { get; set; }

        public AbilityInfo(String AbilityDetail)
        {
            abilityDetail = AbilityDetail;
        }
    }
}
