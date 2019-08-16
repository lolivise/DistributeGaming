using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DistributGameData
{
    //List of ability detail
    [DataContract]
    public class AbilityInfo
    {

        public List<Ability> AbilityList { get; set; }

        public AbilityInfo()
        {
            AbilityList = new List<Ability>();
        }
    }
    //The detail of ability
    [DataContract]
    public class Ability
    {
        [DataMember]
        public String description { get; set; }
        [DataMember]
        public int value { get; set; }
        [DataMember]
        public String type { get; set; }
        [DataMember]
        public String target { get; set; }

        public Ability(String Description, int Value, String Type, String Target)
        {
            description = Description;
            value = Value;
            type = Type;
            target = Target;
        }

    }
}
