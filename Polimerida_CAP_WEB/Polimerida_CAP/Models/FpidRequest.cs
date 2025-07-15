using System.Collections.Generic;

namespace Polimerida_CAP.Models
{
    public class FpidRequest
    {
        public List<Fpid> FPID { get; set; }
    }

    public class Fpid
    {
        public string value { get; set; }
    }
} 