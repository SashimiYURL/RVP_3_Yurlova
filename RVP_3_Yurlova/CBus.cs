using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RVP_3_Yurlova
{
    public class CBus:Transport
    {
        public int CountSeats { get; set; }
        public int MaxPeoples { get; set; }
        public CBus()
        {
            this.RegistNum = string.Empty;
            this.CountSeats = 0;
            this.MaxPeoples = 0;
        }
        public CBus(string registNum, int seats, int maxPeoples)
        {
            this.RegistNum = registNum;
            this.CountSeats = seats;
            this.MaxPeoples = maxPeoples;
        }
    }
}
