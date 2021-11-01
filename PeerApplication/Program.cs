using System;

namespace PeerApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            PeerWorker worker = new PeerWorker();
            worker.Start();

            Console.ReadLine();
        }
    }
}
