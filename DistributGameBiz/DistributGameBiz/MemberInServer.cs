using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DistributGameBiz
{
    //list of member inforamtion
    [DataContract]
    public class MemberInServer
    {
        [DataMember]
        public List<Member> memberList { get; set; }

        public MemberInServer()
        {
            memberList = new List<Member>();
        }
    }
    //the detail of every member
    [DataContract]
    public class Member
    {
        [DataMember]
        public String name { get; set; }

        [DataMember]
        public String heroName { get; set; }

        [DataMember]
        public String hp { get; set; }

        public Member(String Name, String HeroName, String HP)
        {
            name = Name;
            heroName = HeroName;
            hp = HP;
        }
    }
}
