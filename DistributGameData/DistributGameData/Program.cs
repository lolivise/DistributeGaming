using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DistributGameData
{
    class Program
    {
        static void Main(string[] args)
        {
            
            ServiceHost host;
            NetTcpBinding tcpBinding = new NetTcpBinding();
            tcpBinding.MaxReceivedMessageSize = System.Int32.MaxValue;
            tcpBinding.ReaderQuotas.MaxArrayLength = System.Int32.MaxValue;
            //Host the implementation class access via interface class
            host = new ServiceHost(typeof(DGDataControllerImpl));

            try
            {
                host.AddServiceEndpoint(typeof(IDGDataController), tcpBinding, "net.tcp://localhost:50001/DGData");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            //Listen the request from client
            host.Open();
            Console.WriteLine("Press Enter to Exit");
            Console.ReadLine(); //block the program while waiting for clients' requests
            host.Close();
            
        }
    }
}
