using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace RVP_3_Yurlova
{
    public class CPassengerCar:CBrandAvto
    {
        
        public CPassengerCar()
        {
            BrandName = string.Empty;
            ModelName = string.Empty;
            Horsepower = 0;
            MaxSpeed = 0;

        }
        public CPassengerCar(string brand, string model, int horsepower, int maxSpeed)
        {
            BrandName = brand;
            ModelName = model;
            Horsepower = horsepower;
            MaxSpeed = maxSpeed;   
        }
    }
}
