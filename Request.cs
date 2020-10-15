using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleServer
{
    class Request
    {
        public string type { get; set; }
        public string URL { get; set; }
        public string Host { get; set; }
        private Request(string type, string URL, string host) 
        {
            this.type = type;
            this.URL = URL;
            this.Host = host;
        }
        public static Request GetRequest(string msg) 
        {
            if (String.IsNullOrEmpty(msg))
            {
                return null;
            }
            string[] tokens = msg.Split(' ');
            tokens[3] = tokens[3].Split(new string[]{ "\n" },StringSplitOptions.RemoveEmptyEntries)[0];
            Console.WriteLine($"TYPE:{tokens[0]}, URL:{tokens[1]}, HOST:{tokens[3]}");
            return new Request(tokens[0], tokens[1], tokens[3]);

            
        }
    }
}
