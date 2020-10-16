using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
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
            return new Response("200 OK", "text/html", ReadHtmlFile(fi));
        }
        
        private static Response NotWork(string filename, string status) 
        {
            string file = Environment.CurrentDirectory + HttpServer.MSG_DIR + "/" + filename;
            FileInfo fi = new FileInfo(file);
            return new Response(status,"text/html",ReadHtmlFile(fi));
        }

        static Byte[] ReadHtmlFile(FileInfo fi)
        {
            FileStream fs = fi.OpenRead();
            BinaryReader reader = new BinaryReader(fs);
            Byte[] buffer = new Byte[fs.Length];
            reader.Read(buffer, 0, buffer.Length);
            return buffer;
        }
        
        public void Post(HttpListenerResponse response) 
        {
            Console.WriteLine(String.Format($"RESPONSE: {HttpServer.VERSION} {status} \r\nServer: {HttpServer.SERVERNAME} \r\nContent-Language: ru \r\nContent-type:{mime}\r\nAccept Ranges: bytes \r\nContent-Length: {data.Length}\r\nConnection: close\r\n"));
            Console.WriteLine(Encoding.UTF8.GetString(data));
            Stream output = response.OutputStream;
            output.Write(data, 0, data.Length);
            output.Close();
        }
    }
}
