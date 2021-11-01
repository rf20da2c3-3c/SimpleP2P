using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using P2pModels.managers;
using P2pModels.model;

namespace PeerApplication
{
    internal class PeerWorker
    {
        public void Start()
        {
            Task.Run(() =>
            {
                PeerServer pserver = new PeerServer();
                pserver.Start();
            });

            PeerClient pclient = new PeerClient();

            Console.WriteLine("Skriv ønskede fil");
            String fileName = Console.ReadLine();
            List<FileEndPoint> files = pclient.Search(fileName);

            if (files.Count > 0)
            {
                // take the first
                pclient.Download(fileName, files[0], fileName);
            }
        }
    }
}