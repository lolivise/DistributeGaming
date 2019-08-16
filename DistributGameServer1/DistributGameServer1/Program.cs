using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DistributGameServer1
{
    class Program
    {
        static void Main(string[] args)
        {
            //sets up the server connection
            NetTcpBinding tcpBinding = new NetTcpBinding();
            //release the limitation of the size of message which can be sent
            tcpBinding.MaxReceivedMessageSize = System.Int32.MaxValue;
            tcpBinding.ReaderQuotas.MaxArrayLength = System.Int32.MaxValue;
            ServiceHost host = new ServiceHost(typeof(DGSerControllerImpl));

            try
            {
                host.AddServiceEndpoint(typeof(IDGSerController), tcpBinding, "net.tcp://localhost:50003/DGServer1");
            }
            catch (Exception e)
            {
                Console.WriteLine("From Program.cs > Main\n" + e.Message);
            }

            //opens connection
            host.Open();
            System.Console.WriteLine("Press Enter to Exit");

            //block the program while waiting for clients' requests
            System.Console.ReadLine();

            //closes connection
            host.Close();
        }
    }
}
