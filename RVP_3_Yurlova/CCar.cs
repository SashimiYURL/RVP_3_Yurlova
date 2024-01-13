using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RVP_3_Yurlova
{
    public class CCar:Transport
    {
        public string MultiMedia { get; set; }
        public int AirbagCount { get; set; }
        public CCar()
        {
            this.RegistNum = string.Empty;
            this.MultiMedia = string.Empty;
            this.AirbagCount = 0;
        }
        public CCar(string registNum, string multimedia, int airbagCount)
        {
            this.RegistNum = registNum;
            this.MultiMedia = multimedia;
            this.AirbagCount = airbagCount;
        }
    }
}
