using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using P2pModels.model;

namespace P2pModels.managers
{
    public class PeerServer
    {
        protected bool serverstop = false;
        protected List<String> supportedFiles = new List<string>();
        protected FileEndPoint fileEP = new FileEndPoint();

        public PeerServer()
        {
            // fill my files
            if (Directory.Exists(Configurations.FileServerPath))
            {
                foreach (string filename in Directory.GetFiles(Configurations.FileServerPath))
                {
                    // do something
                    string[] filedetails = filename.Split(@"\");
                    supportedFiles.Add(filedetails[filedetails.Length-1]);
                }
            }

            Console.WriteLine("Server Started ");
            Console.WriteLine("with files supported");
            Console.WriteLine(String.Join("\n", supportedFiles));
        }

        private void StartUp()
        {
            Console.WriteLine("Peer Server started");

            HttpClient restClient = new HttpClient();
            fileEP.Port = Configurations.PORT;
            
            string hostName = Dns.GetHostName();
            IPAddress[] adrs = Dns.GetHostAddresses(hostName);
            foreach (IPAddress adr in adrs)
            {
                byte[] bytes = adr.GetAddressBytes();
                if (bytes.Length == 4)
                    fileEP.IpAddress = adr.ToString();
            }
            Console.WriteLine("file ep=" + fileEP);

            String jstr = JsonConvert.SerializeObject(fileEP);
            StringContent content = new StringContent(jstr, Encoding.UTF8, "application/json");
            foreach (string file in supportedFiles)
            {
                HttpResponseMessage postAsync = restClient.PostAsync(Configurations.URI + file, content).Result;

                if (postAsync.IsSuccessStatusCode)
                {
                    string str = postAsync.Content.ReadAsStringAsync().Result;
                    int res = JsonConvert.DeserializeObject<int>(str);
                    Console.WriteLine($"File {file} is registered with result {res}");
                }
            }
        }

        private void TearDown()
        {
            HttpClient restClient = new HttpClient();
            String jstr = JsonConvert.SerializeObject(fileEP);
            StringContent content = new StringContent(jstr, Encoding.UTF8, "application/json");

            foreach (string file in supportedFiles)
            {
                
                HttpResponseMessage delAsync = restClient.PutAsync(Configurations.URI + file, content).Result;

                if (delAsync.IsSuccessStatusCode)
                {
                    string str = delAsync.Content.ReadAsStringAsync().Result;
                    int res = JsonConvert.DeserializeObject<int>(str);
                    Console.WriteLine($"File {file} is DE-registered with result {res}");
                }
            }
            Console.WriteLine("Peer Server closed");

        }


        public void Start()
        {
            Console.WriteLine("Starting start up ...");
            StartUp();

            Task.Run(() => GraceFullShutdown());

            TcpListener server = new TcpListener(IPAddress.Any, Configurations.PORT);
            server.Start();
            

            while (!serverstop)
            {
                if (server.Pending())
                {
                    TcpClient sock = server.AcceptTcpClient();
                    Task.Run(() =>
                    {
                        TcpClient tmpSock = sock;
                        DoOneClient(tmpSock);
                    });
                }
                else
                {
                    Thread.Sleep(1000);
                }
            }

            Console.WriteLine("Closing started ...");
            TearDown();
            server.Stop();
        }

        private void DoOneClient(TcpClient tmpSock)
        {
            using (StreamReader sr = new StreamReader(tmpSock.GetStream()))
            using (BinaryWriter bw = new BinaryWriter(tmpSock.GetStream()) )
            {
                String reqLine = sr.ReadLine();
                String[] reqs = reqLine.Split(" ");
                if (reqs[0].ToUpper() == "GET")
                {
                    if (supportedFiles.Contains(reqs[1]))
                    {
                        String filename = Configurations.FileServerPath + reqs[1];
                        Console.WriteLine(File.Exists(filename));
                        var bytes = File.ReadAllBytes(Configurations.FileServerPath + reqs[1]);
                        foreach (byte b in bytes)
                        {
                            bw.Write(b);
                        }
                        bw.Flush();
                    }
                    bw?.Close();
                }
            }

            tmpSock?.Close();

        }


        public void GraceFullShutdown()
        {
            TcpListener shutDownServer = new TcpListener(IPAddress.Any, Configurations.PORT+1);
            shutDownServer.Start();

            bool closing = false;

            while (!closing)
            {
                TcpClient sock = shutDownServer.AcceptTcpClient();
                StreamReader sr = new StreamReader(sock.GetStream());

                String str = sr.ReadLine();

                if (str == "isaystop")
                {
                    closing = true;
                    serverstop = true;
                }

                sock?.Close();
            }

            shutDownServer.Stop();
        }





    }
}
