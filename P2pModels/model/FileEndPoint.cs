using System;
using System.Collections.Generic;
using System.Text;

namespace P2pModels.model
{
    public class FileEndPoint
    {
        public String IpAddress { get; set; }
        public int Port { get; set; }

        public FileEndPoint()
        {
        }

        public FileEndPoint(string ipAddress, int port)
        {
            IpAddress = ipAddress;
            Port = port;
        }

        public override string ToString()
        {
            return $"{nameof(IpAddress)}: {IpAddress}, {nameof(Port)}: {Port}";
        }

        protected bool Equals(FileEndPoint other)
        {
            return IpAddress == other.IpAddress && Port == other.Port;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((FileEndPoint) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((IpAddress != null ? IpAddress.GetHashCode() : 0) * 397) ^ Port;
            }
        }
    }
}
