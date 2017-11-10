using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VodafoneBITti
{
    public class Token
    {
        public string username { get; set; }
        public string tokenId { get; set; }
        public int tokenTimeout { get; set; }
    }

    public class Response
    {
        public string result { get; set; }
        public string resultCode { get; set; }
        public string resultDescription { get; set; }
    }

    public class TokenRoot
    {
        public Token token { get; set; }
        public Response response { get; set; }
    }

    public class BitPoint
    {
        public int bitsHunted { get; set; }
        public long bitFlockId { get; set; }
    }

    public class Flocks
    {
        public long flockId { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
    }
}
