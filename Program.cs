﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleServer
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpServer server = new HttpServer(8080);
            server.Start();
            Console.ReadLine();
        }
    }
}
