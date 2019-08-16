using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DistributGameBiz
{
    //the list of friends' status
    [DataContract]
    public class FriendState
    {
        [DataMember]
        public List<Friend> FriendList { get; set; }

        public FriendState()
        {
            FriendList = new List<Friend>();
        }
    }

    //the detail of friend status
    [DataContract]
    public class Friend
    {
        [DataMember]
        public String name { get; set; }

        [DataMember]
        public String state { get; set; }

        public Friend(String Name, String State)
        {
            name = Name;
            state = State;
        }
    }
}
