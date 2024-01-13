using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RVP_3_Yurlova
{
    public class CTruckCar:CBrandAvto
    {
        public CTruckCar()
        {
            BrandName = string.Empty;
            ModelName = string.Empty;
            Horsepower = 0;
            MaxSpeed = 0;
        }
        public CTruckCar(string brand, string model, int horsepower, int maxSpeed)
        {
            BrandName = brand;
            ModelName = model;
            Horsepower = horsepower;
            MaxSpeed = maxSpeed;
        }
    }
}
