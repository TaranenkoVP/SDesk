using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Owin.Hosting;
using SDesk.Web.WebApi;


namespace Sdesk.SelfHost
{
    class Program
    {
        static void Main(string[] args)
        {
            string baseAddress = "http://localhost:5555/";

            // Start OWIN host 
            using (WebApp.Start<SelfHostConfig>(url: baseAddress))
            {
                Console.WriteLine("Server is running...");
                Console.WriteLine("Press Enter to shut down.");
                Console.ReadLine();
            }
        }
    }
}
