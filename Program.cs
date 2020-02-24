using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceChecker
{
    class Program
    {
        static void Main(string[] args)
        {
            var setup = new JsonModel();
            if (args != null && args.Length > 0)
            {

                if (args.Any(e => e == "dev"))
                {
                    setup = JsonModel.FromJson(LoadJson("dev.json"));
                }
                if (args.Any(e => e == "internal"))
                {
                    setup = JsonModel.FromJson(LoadJson("internal.stage.json"));
                    
                }
                if (args.Any(e => e == "external"))
                {
                    setup = JsonModel.FromJson(LoadJson("external.stage.json"));
                }
                if (args.Any(e => e == "db"))
                {
                    setup = JsonModel.FromJson(LoadJson("db.stage.json"));
                }
                if (args.Any(e => e == "internalprod"))
                {
                    setup = JsonModel.FromJson(LoadJson("internal.prod.json"));

                }
                if (args.Any(e => e == "externalprod"))
                {
                    setup = JsonModel.FromJson(LoadJson("external.prod.json"));
                }
                if (args.Any(e => e == "dbprod"))
                {
                    setup = JsonModel.FromJson(LoadJson("db.prod.json"));
                }
            }
            else
            {
                setup = JsonModel.FromJson(LoadJson("local.json"));
            }
            
            List<Task> tasks = new List<Task>();

            foreach (var VARIABLE in setup.ListenPorts)
            {
                try
                {
                    tasks.Add(new Server(VARIABLE).StartListenerAsync());
                }
                catch (Exception e)
                {
                    Console.WriteLine(VARIABLE + e.Message);
                }
                
            }

            foreach (var fqdn in setup.PingList)
            {
                foreach (var VARIABLE in fqdn.Value)
                {
                    tasks.Add(new TestConnection().PingPortAsync(fqdn.Key, VARIABLE));
                }
            }

            Task.WaitAll(tasks.ToArray());
        }

        public static string LoadJson(string fileName)
        {
            using StreamReader r = new StreamReader(fileName);
            string json = r.ReadToEnd();
            return json;
        }
    }
}
