using System.Text.RegularExpressions;

namespace Väderdata
{
    internal class ReadFile
    {
        public static string path = "../../../Files/";        

        public static void AvgTempAndHumidity(string position, string fileName)
        {
            using (StreamReader reader = new StreamReader(path + fileName))
            {
                double totalTemp = 0;
                int amountOfDataInputs = 0;
                int totalHumidity = 0;
                Regex regex = new Regex("^(?<Year>2016)-(?<Month>[0-1][0-9])-(?<Day>[0-3][0-9]) (?<TimeOfDay>[0-2][0-9]:[0-5][0-9]:[0-5][0-9]),(?<Location>Inne|Ute),(?<Temp>-?\\d{1,2}.[0-9]),(?<Humidity>[0,1]?[0-9]{1,2})$");
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

                        if (month == inputMonth && year == 2016 && day == inputDay && location == position)
                        {
                            double Temp = double.Parse(match.Groups["Temp"].Value.Replace('.', ','));
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
                    Console.WriteLine("Data inputs: " + amountOfDataInputs);
                }
                else
                {
                    Console.WriteLine("Inga matchande data finns");
                }
            }
        }

        public static void TemperatureData(int keyPress, string position, string fileName)
        {
            using (StreamReader reader = new StreamReader(path + fileName))
            {
                int oldDay = 0;
                double totalTemp = 0;
                double totalHumidity = 0;
                double avgTemp = 0;
                double avgHumidity = 0;
                int amountOfDataInput = 0;
                int checkIfAutumn = 0;
                bool isAutumn = false;
                int checkIfWinter = 0;
                bool isWinter = false;

                var highestTemp = new List<(string Date, double Temp)>();
                var highestHumidity = new List<(string Date, double Humidity)>();
                var highestMoldRisk = new List<(string Date, double moldRisk)>();
                var autumnCountDay = new List<(string Date, double Temp)>();
                var winterCountDay = new List<(string Date, double Temp)>();

                string line = reader.ReadLine();
                Regex regex = new Regex("^(?<Year>2016)-(?<Month>[0-1][0-9])-(?<Day>[0-3][0-9]) (?<TimeOfDay>[0-2][0-9]:[0-5][0-9]:[0-5][0-9]),(?<Location>Inne|Ute),(?<Temp>-?\\d{1,2}.[0-9]),(?<Humidity>[0,1]?[0-9]{1,2})$");
                
                while (line != null)
                {
                    Match match = regex.Match(line);
                    if (match.Success)
                    {
                        int day = int.Parse(match.Groups["Day"].Value);
                        int year = int.Parse(match.Groups["Year"].Value);
                        int month = int.Parse(match.Groups["Month"].Value);
                        string location = match.Groups["Location"].Value;

                        if (year == 2016 && month >= 6 && month <= 12 && location == position && day < 32)
                        {
                            string date = $"{day} : {month} : {year}";
                            double temp = double.Parse(match.Groups["Temp"].Value.Replace('.', ','));
                            int humidity = int.Parse(match.Groups["Humidity"].Value);

                            if (day == oldDay || oldDay == 0)
                            {
                                oldDay = day;
                                totalTemp += temp;
                                totalHumidity += humidity;
                                amountOfDataInput++;
                                
                            }
                            else
                            {
                                //((luftfuktighet -78) * (Temp/15))/0,22
                                avgTemp = totalTemp / amountOfDataInput;
                                avgHumidity = totalHumidity / amountOfDataInput;

                                highestHumidity.Add((date, avgHumidity));
                                highestTemp.Add((date, avgTemp));

                                var calculateMold = (avgHumidity - 78) * (avgTemp / 15)/0.22;
                                highestMoldRisk.Add((date, calculateMold));

                                if (avgTemp <= 10 && isAutumn == false)
                                {
                                    checkIfAutumn++;
                                    autumnCountDay.Add((date, avgTemp));

                                    if(checkIfAutumn == 5)
                                    {
                                        isAutumn = true;
                                    }
                                }
                                else if(isAutumn == false)
                                {
                                    checkIfAutumn = 0;
                                    autumnCountDay.Clear();
                                }
                                if (avgTemp <= 0 && isWinter == false)
                                {
                                    checkIfWinter++;
                                    winterCountDay.Add((date, avgTemp));

                                    if (checkIfWinter == 5)
                                    {
                                        isWinter = true;
                                    }
                                }
                                else if (isWinter == false)
                                {
                                    checkIfWinter = 0;
                                    winterCountDay.Clear();
                                }

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
                int i = 1;

                if(keyPress == 1)
                {
                    foreach (var entry in sortedByTemp)

                    {

                        Console.WriteLine($"Day {i} {entry.Date} - Temp: {entry.Temp}");
                        i++;
                    }
                    
                }
                else if(keyPress == 2)
                {
                    var sortedByHumidity = highestHumidity.OrderBy(entry => entry.Humidity).ToList();
                    foreach (var entry in sortedByHumidity)
                    {
                        Console.WriteLine($"Day {i} {entry.Date} - Humidity: {entry.Humidity} %");
                        i++;
                    }
                }

                else if(keyPress == 3)
                {
                    var sortedByMoldRisk = highestMoldRisk.OrderByDescending(entry => entry.moldRisk).ToList();
                    foreach( var entry in sortedByMoldRisk)
                    {
                        Console.WriteLine($"Day {i} {entry.Date} - Moldrisk: {entry.moldRisk} %");
                        i++;
                    }
                }

                else if(keyPress == 4)
                {
                    foreach(var entry in autumnCountDay)
                    {
                        Console.WriteLine($"Index {i} {entry.Date} - Temp: {entry.Temp}");
                        i++;
                    }
                }
                else if (keyPress == 5 )
                {
                    if(winterCountDay.Count == 0)
                    {
                        Console.WriteLine("No official winter during 2016");
                    }
                    else
                    {
                        foreach (var entry in winterCountDay)
                        {
                            Console.WriteLine($"Index {i} {entry.Date} - Temp: {entry.Temp}");
                            i++;
                        }
                    }
                    
                }

            }
        }

        
    }
}
