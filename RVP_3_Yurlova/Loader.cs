using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RVP_3_Yurlova
{
    public class Loader
    {
        private static Dictionary<string, List<Transport>> modelCarData = new Dictionary<string, List<Transport>>();
        private static Dictionary<string, int> countCarInData = new Dictionary<string, int>();
        private static Random random = new Random();
        private static List<string> RegistNum = new List<string>() { "M280PY52", "A222AA11", "M908YT48", "C676HE36",
        "B344AT66", "K999KK12", "B456CX22", "M578MM34", "K894BE", "P098PO56", "H435KK45", "H899CM34", "K637HY52", "L935OO11",
            "A111AA11", "O000OO11", "X989OP78", "H777EE32", "B735AC44", "H100KK96"};
        private static List<string> MultiMedia = new List<string>() { "HGKFKDK", "URHHFCN", "FDFDFDDF0", "LDJBZ888ZJKX",
            "AURA900", "JBL"};
        private static List<int> AirbagCount = new List<int>() { 1, 3, 5, 4, 2 };
        private static List<int> WheelCount = new List<int>() { 4, 6, 8, 12, 14 };
        private static List<int> BodyVolume = new List<int>() { 16, 15, 25, 18, 30, 32, 28, 10, 26 };
        private static List<int> MaxPeoples = new List<int>() { 12, 20, 15, 35, 8, 10, 40 };
        private static List<int> CountSeats = new List<int>() { 12, 15, 6, 8, 20 };

        public static async Task Load(string brandName, string type)
        {
            string regNum, multimedia;
            int airbagCount, wheelCount, volume,seats,maxPeoples;
            if (!modelCarData.ContainsKey(brandName))
            {
                await Task.Run(() =>
                {
                    modelCarData[brandName] = new List<Transport>();
                    int numbersOfCars = random.Next(10, 21);
                    countCarInData.Add(brandName, numbersOfCars);
                    for (int i = 0; i < numbersOfCars; i++)
                    {
                        if (type == "Passenger")
                        {
                            regNum = RegistNum[random.Next(0, RegistNum.Count)];
                            multimedia = MultiMedia[random.Next(0, MultiMedia.Count)];
                            airbagCount = AirbagCount[random.Next(0, AirbagCount.Count)];
                            modelCarData[brandName].Add(new CCar(regNum, multimedia, airbagCount));
                        }
                        if (type == "Truck")
                        {
                            regNum = RegistNum[random.Next(0, RegistNum.Count)];
                            wheelCount = WheelCount[random.Next(0, WheelCount.Count)];
                            volume = BodyVolume[random.Next(0, BodyVolume.Count)];
                            modelCarData[brandName].Add(new CTruck(regNum, wheelCount, volume));
                        }
                        if (type == "Bus")
                        {
                            regNum = RegistNum[random.Next(0, RegistNum.Count)];
                            seats = CountSeats[random.Next(0, CountSeats.Count)];
                            maxPeoples = MaxPeoples[random.Next(0, MaxPeoples.Count)];
                            modelCarData[brandName].Add(new CBus(regNum, seats, maxPeoples));
                        }
                        Thread.Sleep(random.Next(0, 501));
                    }
                });

            }

        }

        public static List<Transport> GetData(string brandName)
        {
            List<Transport> data = new List<Transport>();
            for (int i = 0; i < modelCarData[brandName].Count; i++)
            {
                data.Add(modelCarData[brandName][i]);
            }
            return data;

        }

        public static int GetProgress(string brandName)
        {
            if (modelCarData.ContainsKey(brandName))
            {
                int loadedCars = modelCarData[brandName].Count;
                int totalCars = countCarInData[brandName];
                return (int)(((double)loadedCars / totalCars) * 100);

            }
            else return 0;
        }
        public static void RemoveBrand(string brandName)
        {
            modelCarData.Remove(brandName);
            countCarInData.Remove(brandName);
        }

    }
}
