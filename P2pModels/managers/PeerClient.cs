using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;
using P2pModels.model;

namespace P2pModels.managers
{
    public class PeerClient
    {
        public List<FileEndPoint> Search(String filename)
        {
            List<FileEndPoint> liste = new List<FileEndPoint>();

            HttpClient restClient = new HttpClient();
            String jstr = restClient.GetStringAsync(Configurations.URI + filename).Result;
            ICollection<FileEndPoint> files = JsonConvert.DeserializeObject<ICollection<FileEndPoint>>(jstr);

            liste.AddRange(files);


            return liste;
        }

        public void Download(String filename, FileEndPoint ep, String outFileName)
        {

            TcpClient client = new TcpClient(ep.IpAddress, ep.Port);
            StreamWriter sw = new StreamWriter(client.GetStream());
            sw.WriteLine("GET " + filename);
            sw.Flush();

            NetworkStream fromStream = client.GetStream();
            FileStream toStream = File.Create(Configurations.FileClientPath + outFileName);
            
            fromStream.CopyTo(toStream);
            
            toStream?.Close();
            client?.Close();
        }
    }
}
