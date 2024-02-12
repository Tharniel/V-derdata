using static Väderdata.ReadFile;

namespace Väderdata
{
    internal class Program
    {
        public static string path = "../../../Files/";
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();

                MyDelegate del = DisplayMessage;
                del("Choose which data you'd like to see:");
                del("1. Indoor");
                del("2. Outdoor");
                del("3. Save summary to file");

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
                        case '3':
                            var location = "Ute";
                            int keyPress = 6;
                            string tempDataFile = "tempDataFile.txt";
                            using (StreamWriter writer = new StreamWriter(path + tempDataFile, false))
                            {
                                writer.WriteLine("");
                            }
                                TemperatureData(keyPress, location, "tempdata5-med fel.txt");
                            break;
                    }
                }
            }
        }

        internal static void IndoorMenu()
        {
            Console.Clear();

            MyDelegate del = DisplayMessage;
            del("1. Order by highest temperature");
            del("2. Order by lowest humidity");
            del("3. Order by highest risk for mold");
            del("4. Check temp/humidity for specific date");

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
                    AvgTempAndHumidity(location, "tempdata5-med fel.txt");
                    break;
            }
            TemperatureData(keyPress, location, "tempdata5-med fel.txt");
        }

        internal static void OutDoorMenu()
        {
            Console.Clear();

            MyDelegate del = DisplayMessage;
            del("1. Order by highest temperature");
            del("2. Order by lowest humidity");
            del("3. Order by highest risk for mold");
            del("4. Check when autumn officially occured");
            del("5. Check when winter officially occured");
            del("6. Check temp/humidity for specific date");

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
                    AvgTempAndHumidity(location, "tempdata5-med fel.txt");
                    break;
            }
            TemperatureData(keyPress, location, "tempdata5-med fel.txt");
        }
    }
}
