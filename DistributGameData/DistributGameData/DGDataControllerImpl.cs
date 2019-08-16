using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DistributGameData
{

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    internal class DGDataControllerImpl : IDGDataController
    {
        UserInfo userInfo;
        HeroInfo heroInfo;
        BossInfo bossInfo;

        public DGDataControllerImpl()
        {
            userInfo = new UserInfo();
            heroInfo = new HeroInfo();
            bossInfo = new BossInfo();
            Console.WriteLine("A new client has connected!");
        }

        ~DGDataControllerImpl()
        {
            Console.WriteLine("A client is no longer serviced!");
        }

        //read user detail
        public UserInfo GetUserInfo()
        {
            String filePath = "User.xml";
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            DataContractSerializer serializer = new DataContractSerializer(typeof(UserInfo));
            userInfo = (UserInfo)serializer.ReadObject(fs);

            return userInfo;
        }

        //read hero detail
        public HeroInfo GetHeroInfo()
        {
            String filePath = "Hero.xml";
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            DataContractSerializer serializer = new DataContractSerializer(typeof(HeroInfo));
            heroInfo = (HeroInfo)serializer.ReadObject(fs);
            fs.Close();
            return heroInfo;
        }

        //read boss detail
        public BossInfo GetBossInfo()
        {
            String filePath = "Boss.xml";
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            DataContractSerializer serializer = new DataContractSerializer(typeof(BossInfo));
            bossInfo = (BossInfo)serializer.ReadObject(fs);
            fs.Close();
            return bossInfo;
        }
       
    }
}
