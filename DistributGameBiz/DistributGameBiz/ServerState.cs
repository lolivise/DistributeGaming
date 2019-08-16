using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DistributGameBiz
{
    //list of server status
    [DataContract]
    public class ServerState
    {
        [DataMember]
        public List<Server> ServerStateList { get; set; }

        public ServerState()
        {
            ServerStateList = new List<Server>();
        }
    }

    //the detail of server status
    [DataContract]
    public class Server
    {
        [DataMember]
        public String name { get; set; }

        [DataMember]
        public String state { get; set; }

        [DataMember]
        public int numberClient { get; set; }

        public Server(String Name, String State, int NumberClient)
        {
            name = Name;
            state = State;
            numberClient = NumberClient;
        }
    }
}
