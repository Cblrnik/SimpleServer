using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleServer
{
    class HttpServer
    {
        public const string MSG_DIR = "/Root/massage";
        public const string WEB_DIR = "/Root/web";
        public const string VERSION = "v 1.0";
        public const string SERVERNAME = " MEMES ";
        HttpListener listener;

        bool running = false;
        public HttpServer(int port)
        {
            listener = new HttpListener();
            listener.Prefixes.Add($"http://localhost:{port}/");
        }
        public void Start() 
        {
            listener.Start();
            Run();
        }
        void Run() 
        {
            Console.WriteLine("Server is running");
            Console.WriteLine("Waiting for connection...");
            running = true;
            while (running)
            {
                HandleClientFlow();
                Console.WriteLine("Обработка подключения завершена");
            }
            listener.Stop();
        }
        Request InformationAboutConnection(HttpListenerRequest request) 
        {
            Request req = Request.GetRequest(request);
            Console.WriteLine($"REQUEST: \n\r {req}");
            return req;
        }
        
        
        void HandleClientFlow() 
        {
            HttpListenerContext context = listener.GetContext();
            Request request = InformationAboutConnection(context.Request);
            Response response = Response.From(request);
            response.Post(context.Response);
        }
    }
}
