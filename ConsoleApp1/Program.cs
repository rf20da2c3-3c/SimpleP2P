using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            HashSet<String> myset = new HashSet<string>();

            myset.Add("peter");

            Console.WriteLine("peter " + myset.Add("peter"));
            Console.WriteLine("Peter " + myset.Add("Peter"));


            Console.ReadLine();
        }
    }
}
