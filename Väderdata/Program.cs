using System.Security.Cryptography.X509Certificates;

namespace Väderdata
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.ReadKey(true);
                Console.Clear();
                Console.WriteLine("Choose which data you'd like to see\n1.Indoor\n2.Outdoor");
                {

                    var key = Console.ReadKey(true);

                    switch (key.KeyChar)
                    {
                        case '1':
                            IndoorMenu();
                            break;
                        case '2':
                            OutDoorMenu();
                            break;                        
                    }
                }
            }
            
        }

        internal static void IndoorMenu()
        {
            Console.Clear();

            Console.WriteLine("1.HighestTemp\n2.HighestHumidity\n3.MoldRisk\n4.AverageTempAndHumidity");
            var key = Console.ReadKey(true);
            int keyPress = 0;
            string location = "Inne";

            switch (key.KeyChar)
            {
                case '1':
                    keyPress = 1;

                    break;
                case '2':
                    keyPress = 2;
                    break;
                case '3':
                    keyPress = 3;
                    break;
                case '4':
                    ReadFile.AvgTempAndHumidity(location, "tempdata5-med fel.txt");
                    break;
            }
            ReadFile.TemperatureData(keyPress, location, "tempdata5-med fel.txt");
            
        }

        internal static void OutDoorMenu()
        {
            Console.Clear();

            Console.WriteLine("1.HighestTemp\n2.HighestHumidity\n3.MoldRisk\n4.Autumn\n5.Winter\n6.AverageTempAndHumidity");
            var key = Console.ReadKey(true);
            int keyPress = 0;
            string location = "Ute";
            switch (key.KeyChar)
            {
                case '1':
                    keyPress = 1;
                    break;
                case '2':
                    keyPress = 2;
                    break;
                case '3':
                    keyPress = 3;
                    break;
                case '4':
                    keyPress = 4;
                    break;
                case '5':
                    keyPress = 5;
                    break;
                case '6':
                    ReadFile.AvgTempAndHumidity(location, "tempdata5-med fel.txt");
                    break;
            }
            ReadFile.TemperatureData(keyPress, location, "tempdata5-med fel.txt");

        }
    }
}
