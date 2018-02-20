using System;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;

namespace DataCollector.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            
            string uri = @"http://+:8080/";


            HttpServer httpServer = new HttpServer();

            httpServer.StartServer(uri);
        }
    }


}



