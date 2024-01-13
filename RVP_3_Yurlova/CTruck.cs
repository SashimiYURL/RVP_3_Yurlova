using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RVP_3_Yurlova
{
    public class CTruck:Transport
    {
        public int WheelCount { get; set; }
        public int BodyVolume { get; set; }
        public CTruck()
        {
            this.RegistNum=string.Empty;
            this.WheelCount = 0;
            this.BodyVolume = 0;
        }
        public CTruck(string registNum, int wheelCount, int bodyVolume)
        {
            this.RegistNum = registNum;
            this.WheelCount = wheelCount;
            this.BodyVolume = bodyVolume;
        }
    }
}
