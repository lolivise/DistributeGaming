using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DistributGameServer1
{
     //List of allies status
    [DataContract]
    public class Allies
    {
        [DataMember]
        public List<Ally> alliesList { get; set; }

        public Allies()
        {
            alliesList = new List<Ally>();
        }
    }

    //the detail of a ally
    [DataContract]
    public class Ally
    {
        [DataMember]
        public String name { get; set; }
        [DataMember]
        public String hero { get; set; }
        [DataMember]
        public String hp { get; set; }

        public Ally(String Name, String Hero, String HP)
        {
            name = Name;
            hero = Hero;
            hp = HP;
        }
    }
}
