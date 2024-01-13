using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace RVP_3_Yurlova
{
    public interface IBrandAvto
    {
        string BrandName { get; set; }
        string ModelName { get; set; }
        int Horsepower { get; set; }
        int MaxSpeed { get; set; }
        
    }
    
}
