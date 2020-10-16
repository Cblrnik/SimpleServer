using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace SimpleServer
{
    class Program
    {
        static void Main(string[] args)
        {
            //ReadHtml();
            HttpServer server = new HttpServer(8080);
            server.Start();
            Console.ReadLine();
        }
        static void ReadHtml() 
        {
            HttpListener listener = new HttpListener();
            // установка адресов прослушки
            listener.Prefixes.Add("http://localhost:8888/connection/");
            listener.Start();
            Console.WriteLine("Ожидание подключений...");
            // метод GetContext блокирует текущий поток, ожидая получение запроса 
            HttpListenerContext context = listener.GetContext();
            HttpListenerRequest request = context.Request;
            // получаем объект ответа
            HttpListenerResponse response = context.Response;
            // создаем ответ в виде кода html
            FileInfo fi = new FileInfo(Environment.CurrentDirectory + "/Root/web/" + "First.html");
            FileStream fs = fi.OpenRead();
            BinaryReader reader = new BinaryReader(fs);
            Byte[] buffer = new Byte[fs.Length];
            reader.Read(buffer, 0, buffer.Length);
            // получаем поток ответа и пишем в него ответ
            response.ContentLength64 = buffer.Length;
            Stream output = response.OutputStream;
            Console.WriteLine(output);
            output.Write(buffer, 0, buffer.Length);
            //закрываем поток
            output.Close();
            // останавливаем прослушивание подключений
            listener.Stop();
            Console.WriteLine("Обработка подключений завершена");
        }
    }
}
