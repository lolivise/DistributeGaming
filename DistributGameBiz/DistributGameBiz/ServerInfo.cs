using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DistributGameBiz
{
    //list of server information
    [DataContract]
    public class ServerInfo
    {
        [DataMember]
        public List<MemberInServer> serverInfoList { get; set; }

        public ServerInfo()
        {
            serverInfoList = new List<MemberInServer>();
        }
    }

}
