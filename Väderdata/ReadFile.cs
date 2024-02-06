using System.Text.RegularExpressions;

namespace Väderdata
{
    internal class ReadFile
    {
        public static string path = "../../../Files/";

        public static void ReadAll(string fileName)
        {
            using (StreamReader reader = new StreamReader(path + fileName))
            {
                Regex regex = new Regex("^(?<Year>2016)-(?<Month>[0-1][1-9])-(?<Day>[0-3][0-9]) (?<TimeOfDay>[0-2][0-9]:[0-5][0-9]:[0-5][0-9]),(?<Location>Inne|Ute),(?<Temp>-?\\d{1,2}.[0-9]),(?<Humidity>[0,1]?[0-9]{1,2})$");
                string line = reader.ReadLine();
                int rowCount = 0;

                while (line != null)
                {
                    Match match = regex.Match(line);
                    if (match.Success)
                    {
                        int month = int.Parse(match.Groups["Month"].Value);
                        int year = int.Parse(match.Groups["Year"].Value);

                        if (month >= 6 && month <= 12 && year == 2016)
                        {
                            Console.WriteLine(rowCount + " " + line);
                        }
                    }

                    rowCount++;
                    line = reader.ReadLine();
                }
            }
        }

        public static void AvgTempAndHumidity(string fileName)
        {
            using (StreamReader reader = new StreamReader(path + fileName))
            {
                double totalTemp = 0;
                int amountOfDataInputs = 0;
                int totalHumidity = 0;
                Regex regex = new Regex("^(?<Year>2016)-(?<Month>[0-1][1-9])-(?<Day>[0-3][0-9]) (?<TimeOfDay>[0-2][0-9]:[0-5][0-9]:[0-5][0-9]),(?<Location>Inne|Ute),(?<Temp>-?\\d{1,2}.[0-9]),(?<Humidity>[0,1]?[0-9]{1,2})$");
                string line = reader.ReadLine();

                Console.WriteLine("vissa mellan värden");
                Console.WriteLine("Skriv in månanden: ");
                double inputMonth = int.Parse(Console.ReadLine());
                Console.WriteLine("Skriv in Dag: ");
                double inputDay = int.Parse(Console.ReadLine());


                while (line != null)
                {
                    Match match = regex.Match(line);
                    if (match.Success)
                    {
                        int month = int.Parse(match.Groups["Month"].Value);
                        int day = int.Parse(match.Groups["Day"].Value);
                        int year = int.Parse(match.Groups["Year"].Value);
                        string location = match.Groups["Location"].Value;

                        if (month == inputMonth && year == 2016 && day == inputDay && location == "Ute")
                        {
                            double Temp = double.Parse(match.Groups["Temp"].Value);
                            int humidity = int.Parse(match.Groups["Humidity"].Value);
                            totalHumidity += humidity;
                            totalTemp += Temp;
                            amountOfDataInputs++;
                        }
                    }

                    line = reader.ReadLine();
                }

                if (amountOfDataInputs > 0)
                {
                    double avgTemp = totalTemp / amountOfDataInputs;
                    int avgHumidity = totalHumidity / amountOfDataInputs;
                    Console.WriteLine("Temp: " + avgTemp);
                    Console.WriteLine("Humidity: " + avgHumidity);
                    Console.WriteLine(amountOfDataInputs);
                }
                else
                {
                    Console.WriteLine("Inga matchande data finns");
                }
            }
        }

        public static void HighestTemp(string fileName)
        {
            using (StreamReader reader = new StreamReader(path + fileName))
            {
                double totalTemp = 0;
                int oldDay = 0;
                double dayTemp = 0;
                int dayHumidity = 0;
                int amountOfDataInputs = 0;
                List <double> amountOfDayInputs = new List<double>();
                List<double> highestTemp = new List<double>();
                List<int> highestHumidity = new List<int>();
                int totalHumidity = 0;
                Regex regex = new Regex("^(?<Year>2016)-(?<Month>[0-1][1-9])-(?<Day>[0-3][0-9]) (?<TimeOfDay>[0-2][0-9]:[0-5][0-9]:[0-5][0-9]),(?<Location>Inne|Ute),(?<Temp>-?\\d{1,2}.[0-9]),(?<Humidity>[0,1]?[0-9]{1,2})$");
                string line = reader.ReadLine();
                while (line != null)
                {
                    Match match = regex.Match(line);
                    if (match.Success)
                    {
                        int month = int.Parse(match.Groups["Month"].Value);
                        int day = int.Parse(match.Groups["Day"].Value);
                        int year = int.Parse(match.Groups["Year"].Value);
                        string location = match.Groups["Location"].Value;

                        if (month >= 6 && month <= 12 && year == 2016 && location == "Ute")
                        {
                            double Temp = double.Parse(match.Groups["Temp"].Value);
                            int humidity = int.Parse(match.Groups["Humidity"].Value);
                            totalHumidity += humidity;
                            totalTemp += Temp;


                            if (day == oldDay)
                            {
                                dayTemp += Temp;
                                dayHumidity += humidity;
                                amountOfDataInputs++;
                            }
                            else
                            {
                                
                                oldDay = day;
                                highestTemp.Add(dayTemp);
                                highestHumidity.Add(dayHumidity);
                                dayTemp = 0;
                                dayHumidity = 0;
                            }
                        }
                    }
                    line = reader.ReadLine();
                }

                if (amountOfDataInputs > 0)
                {
                    var test = from c in highestTemp select c;
                    var test1 = highestTemp.OrderByDescending(temp => temp);

                    foreach (var temp in test1)
                    {
                        Console.WriteLine(temp);
                    }
                }
            }
        }
    }
}
