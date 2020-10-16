using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
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
        public override string ToString()
        {
            return $"{type}, {URL}, {Host}";
        }
        public static Request GetRequest(HttpListenerRequest request) 
        {
            if (request == null)
            {
                return null;
            }
            return new Request(request.HttpMethod,request.RawUrl,request.UserHostAddress);
        }
    }
}
