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
        TcpListener listener;

        bool running = false;
        public HttpServer(int port)
        {
            listener = new TcpListener(IPAddress.Any, port);
        }
        public void Start() 
        {
            Thread thread = new Thread(new ThreadStart(Run));
            thread.Start();
        }
        void Run() 
        {
            listener.Start();
            running = true;
            Console.WriteLine("Server is running");
            while (running)
            {
                Console.WriteLine("Waiting for connection...");
                TcpClient client = listener.AcceptTcpClient();
                Console.WriteLine("Client connected");
                HandleClient(client);
            }
        }
        void HandleClient(TcpClient client) 
        {
            var ClientStream = client.GetStream();
            StreamReader reader = new StreamReader(ClientStream);
            string massage = "";
            while (reader.Peek() != -1) 
            {
                massage += reader.ReadLine().Trim() + "\n";
            }
            Console.WriteLine($"REQUEST: \n\r {massage}");
            Request request = Request.GetRequest(massage);
            Response response = Response.From(request);
            response.Post(ClientStream);
            reader.Close();
        }
    }
}
