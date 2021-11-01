using System;
using System.Collections.Generic;
using System.Text;

namespace P2pModels.managers
{
    public static class Configurations
    {
        private const String FilePath = @"M:\intpub\P2P\";


        public const String URI = "http://localhost:60341/api/Registers/";
        public const String FileServerPath = FilePath+@"uploads\";
        public const String FileClientPath = FilePath + @"downloads\";
        public const int PORT = 45000;
    }
}
