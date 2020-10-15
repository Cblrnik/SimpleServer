using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SimpleServer
{
    class Response
    {
        Byte[] data;
        string status;
        string mime;
        private Response(string status, string mime, Byte[] data) 
        {
            this.status = status;
            this.mime = mime;
            this.data = data;
        }
        public static Response From (Request request)  
        {
            if (request == null)
            {
                return NotWork("400.html","400 Bad Request");
            }
            if (request.type == "GET")
            {
                string file = Environment.CurrentDirectory + HttpServer.WEB_DIR + request.URL;
                FileInfo fi = new FileInfo(file);
                if (fi.Exists && fi.Extension.Contains("."))
                {
                    return MakeFromFile(fi);
                }
                else 
                {
                    DirectoryInfo di = new DirectoryInfo(fi.FullName );
                    if (!di.Exists)
                    {
                        return NotWork("404.html","404 Page not Found");
                    }
                    FileInfo[] files = di.GetFiles();
                    foreach (FileInfo item in files)
                    {
                        if (item.Name.Contains("First.html"))
                        {
                            return MakeFromFile(item);
                        }
                    }
                }
            }
            else
            {
                return NotWork("405.html", "405 Method not Allowed");
            }
            return NotWork("404.html", "404 Page not Found");
        }
        private static Response MakeFromFile(FileInfo fi) 
        {
            FileStream fs = fi.OpenRead();
            BinaryReader reader = new BinaryReader(fs);
            Byte[] d1 = new Byte[fs.Length];
            reader.Read(d1, 0, d1.Length);
            return new Response("200 OK", "text/html", d1);
        } 
        private static Response NotWork(string filename, string status) 
        {
            string file = Environment.CurrentDirectory + HttpServer.MSG_DIR + "/" + filename;
            FileInfo fi = new FileInfo(file);
            FileStream fs = fi.OpenRead();
            BinaryReader reader = new BinaryReader(fs);
            Byte[] d1 = new Byte[fs.Length];
            reader.Read(d1, 0, d1.Length);
            return new Response(status,"text/html",d1);
        }
        public void Post(NetworkStream stream) 
        {
            StreamWriter writer = new StreamWriter(stream);
            Console.WriteLine(String.Format($"RESPONSE: {HttpServer.VERSION} {status} \r\nServer: {HttpServer.SERVERNAME} \r\nContent-Language: ru \r\nContent-type:{mime}\r\nAccept Ranges: bytes \r\nContent-Length: {data.Length}\r\nConnection: close\r\n"));
            writer.WriteLine(String.Format($"{HttpServer.VERSION} {status} \r\nServer: {HttpServer.SERVERNAME} \r\nContent-Language: ru \r\nContent-type:{mime}\r\nAccept Ranges: bytes \r\nContent-Length: {data.Length}\r\nConnection: close\r\n"));
            writer.Flush();
            stream.Write(data, 0, data.Length);
        }
    }
}
