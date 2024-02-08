using System.Security.Cryptography.X509Certificates;

namespace Väderdata
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                
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

            Console.WriteLine("1.Order by highest temperature\n2.Order by lowest humidity\n3.Order by highest risk for mold\n" +
                "4.Check temp/humidity for specific date\n5.MonthTemp");
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
                case '5':
                    location = "Ute";
                    keyPress = 6;
                    break;
            }
            ReadFile.TemperatureData(keyPress, location, "tempdata5-med fel.txt");
            
        }

        internal static void OutDoorMenu()
        {
            Console.Clear();

            Console.WriteLine("1.Order by highest temperature\n2.Order by lowest humidity\n3.Order by highest risk for mold\n" +
                "4.Check when autumn officially occured\n5.Check when winter officially occured\n6.Check temp/humidity for specific date\n7.MonthTemp");
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
                case '7':
                    
                    keyPress = 6;
                    break;
            }
            ReadFile.TemperatureData(keyPress, location, "tempdata5-med fel.txt");

        }
    }
}
