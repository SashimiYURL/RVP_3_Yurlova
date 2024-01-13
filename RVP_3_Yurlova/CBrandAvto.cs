using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RVP_3_Yurlova
{
    public class CBrandAvto:IBrandAvto
    {
        public string BrandName { get; set; }
        public string ModelName { get; set; }
        public int Horsepower { get; set; }
        public int MaxSpeed { get; set; }

        public CBrandAvto()
        {
            BrandName = string.Empty;
            ModelName = string.Empty;
            Horsepower = 0;
            MaxSpeed = 0;
        }
        public CBrandAvto(string brand, string model, int horsepower, int maxSpeed)
        {
            BrandName = brand;
            ModelName = model;
            Horsepower = horsepower;
            MaxSpeed = maxSpeed;
        }
    }
}
