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
                int oldDay = 0;
                double totalTemp = 0;
                double totalHumidity = 0;
                double avgTemp = 0;
                double avgHumidity = 0;
                int amountOfDataInput = 0;
                var highestTemp = new List<(string Date, double Temp)>();
                var highestHumidity = new List<(string Date, double Humidity)>();
                string line = reader.ReadLine();
                Regex regex = new Regex("^(?<Year>2016)-(?<Month>[0-1][1-9])-(?<Day>[0-3][0-9]) (?<TimeOfDay>[0-2][0-9]:[0-5][0-9]:[0-5][0-9]),(?<Location>Inne|Ute),(?<Temp>-?\\d{1,2}.[0-9]),(?<Humidity>[0,1]?[0-9]{1,2})$");

                while (line != null)
                {
                    Match match = regex.Match(line);
                    if (match.Success)
                    {
                        int day = int.Parse(match.Groups["Day"].Value);
                        int year = int.Parse(match.Groups["Year"].Value);
                        int month = int.Parse(match.Groups["Month"].Value);
                        string location = match.Groups["Location"].Value;
                        if (year == 2016 && month >= 6 && month <= 12 && location == "Ute")
                        {
                            string date = $"{day} : {month} : {year}";
                            double temp = double.Parse(match.Groups["Temp"].Value);
                            int humidity = int.Parse(match.Groups["Humidity"].Value);

                            if(day == oldDay)
                            {
                                totalTemp += temp;
                                totalHumidity += humidity;
                                amountOfDataInput++;
                            }
                            else
                            {


                                avgTemp = totalTemp / amountOfDataInput;
                                avgHumidity = totalHumidity / amountOfDataInput;
                                highestHumidity.Add((date, avgHumidity));
                                highestTemp.Add((date, avgTemp));

                                oldDay = day;
                                totalHumidity = 0;
                                totalTemp = 0;
                                avgHumidity = 0;
                                avgTemp = 0;
                                amountOfDataInput = 0;
                            }
                        }
                    }
                    line = reader.ReadLine();
                }

                var sortedByTemp = highestTemp.OrderByDescending(entry => entry.Temp).ToList();

                foreach (var entry in sortedByTemp)
                {
                    Console.WriteLine($"{entry.Date} - Temp: {entry.Temp}");
                    Thread.Sleep(1000);
                }
            }
        }
    }
}
