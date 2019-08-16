using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DistributGameData
{
    //List of user detail
    [DataContract]
    public class UserInfo
    {
        [DataMember]
        public List<User> Users { get; set; }

        public UserInfo()
        {
            Users = new List<User>();
        }

    }

    //the detail of user
    [DataContract]
    public class User
    {
        [DataMember]
        public String userName { get; set; }
        [DataMember]
        public String password { get; set; }
        [DataMember]
        public List<int> friendList { get; set; }

        public User(String UserName, String Password, List<int> FriendList)
        {
            friendList = FriendList;
            userName = UserName;
            password = Password;
        }
    }
}
