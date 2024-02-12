using System;
using System.Text.RegularExpressions;
using System.Xml;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Väderdata
{
    public delegate void MyDelegate(string msg);
    internal class ReadFile
    {
        public static void DisplayMessage(string message)
        {
            Console.WriteLine(message);
        }

        public static string path = "../../../Files/";

        public static void AvgTempAndHumidity(string position, string fileName)
        {
            Console.Clear();
            using (StreamReader reader = new StreamReader(path + fileName))
            {
                double totalTemp = 0;
                int amountOfDataInputs = 0;
                int totalHumidity = 0;
                Regex regex = new Regex("^(?<Year>2016)-(?<Month>[0-1][0-9])-(?<Day>[0-3][0-9]) (?<TimeOfDay>[0-2][0-9]:[0-5][0-9]:[0-5][0-9]),(?<Location>Inne|Ute),(?<Temp>-?\\d{1,2}.[0-9]),(?<Humidity>[0,1]?[0-9]{1,2})$");
                string line = reader.ReadLine();

                Console.WriteLine("Mean Value\n=========");
                Console.WriteLine("Input month in numbers: ");
                double inputMonth = int.Parse(Console.ReadLine());
                Console.WriteLine("Input day in numbers: ");
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
                    Console.WriteLine("Temp: " + Math.Round(avgTemp, 1) + "°C");
                    Console.WriteLine("Humidity: " + avgHumidity + "%");
                    var datainput = "Data inputs: " + amountOfDataInputs;
                    datainput.Cw();
                }
                else
                {
                    Console.WriteLine("No matching data was found.");
                }
                Console.ReadKey(true);
            }
        }

        public static void TemperatureData(int keyPress, string position, string fileName)
        {
            Console.Clear();
            using (StreamReader reader = new StreamReader(path + fileName))
            {
                int oldMonth = 0;
                int oldDay = 0;
                double totalTempMonth = 0;
                double totalTemp = 0;
                double totalHumidity = 0;
                double totalHumidityMonth = 0;
                double avgTemp = 0;
                double avgHumidity = 0;
                double avgTempMonth = 0;
                double avgHumidityMonth = 0;
                double calculateMold = 0;
                int amountOfDataInput = 0;
                int amountOfDataInputMonth = 0;
                int checkIfAutumn = 0;
                bool isAutumn = false;
                int checkIfWinter = 0;
                int closestToWinter = 0;
                bool isWinter = false;
                string date = "";
                string oldDate = "";

                var highestTemp = new List<(string Date, double Temp)>();
                var highestHumidity = new List<(string Date, double Humidity)>();
                var highestMoldRisk = new List<(string Date, double moldRisk)>();
                var autumnCountDay = new List<(string Date, double Temp)>();
                var winterCountDay = new List<(string Date, double Temp)>();
                var closestToWinterList = new List<(string Date, double Temp)>();
                var monthTempData = new List<(int Date, double Temp)>();
                var monthHumidityData = new List<(int Date, double Humidity)>();
                var monthMoldRiskData = new List<(int Date, double moldRisk)>();

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
                            date = $"{day + "/", -3}{month, -2} - {year}";
                            double temp = double.Parse(match.Groups["Temp"].Value.Replace('.', ','));
                            int humidity = int.Parse(match.Groups["Humidity"].Value);

                            if (day == oldDay || oldDay == 0)
                            {
                                oldDay = day;
                                oldDate = date; 
                                totalTemp += temp;
                                totalHumidity += humidity;
                                amountOfDataInput++;

                                if (month == oldMonth || oldMonth == 0)
                                {
                                    oldMonth = month;
                                    totalTempMonth += temp;
                                    totalHumidityMonth += humidity;
                                    amountOfDataInputMonth++;
                                }
                                else
                                {
                                    
                                    avgTempMonth = totalTempMonth / amountOfDataInputMonth;
                                    monthTempData.Add((oldMonth, avgTempMonth));

                                    avgHumidityMonth = totalHumidityMonth / amountOfDataInputMonth;
                                    monthHumidityData.Add((oldMonth, avgHumidityMonth));

                                    calculateMold = (avgHumidityMonth - 78) * (avgTempMonth / 15) / 0.22;
                                    monthMoldRiskData.Add((oldMonth, calculateMold));

                                    oldMonth = month;
                                    totalTempMonth = 0;
                                    avgTempMonth = 0;
                                    totalHumidityMonth = 0;
                                    avgHumidityMonth = 0;
                                    amountOfDataInputMonth = 0;
                                }
                            }
                            else
                            {
                                //((luftfuktighet -78) * (Temp/15))/0,22
                                avgTemp = totalTemp / amountOfDataInput;
                                avgHumidity = totalHumidity / amountOfDataInput;

                                highestHumidity.Add((date, avgHumidity));
                                highestTemp.Add((oldDate, avgTemp));

                                calculateMold = (avgHumidity - 78) * (avgTemp / 15)/0.22;
                                highestMoldRisk.Add((oldDate, calculateMold));

                                if (avgTemp <= 10 && isAutumn == false)
                                {
                                    checkIfAutumn++;
                                    autumnCountDay.Add((oldDate, avgTemp));

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
                                    winterCountDay.Add((oldDate, avgTemp));

                                    if(closestToWinter == checkIfWinter)
                                    {
                                        closestToWinter++;
                                        closestToWinterList.Add((oldDate, avgTemp));
                                    }
                                    checkIfWinter++;

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
                                totalHumidity = humidity;
                                totalTemp = temp;
                                avgHumidity = 0;
                                avgTemp = 0;
                                amountOfDataInput = 1;
                                oldDate = date;
                            }
                        }
                    }
                    line = reader.ReadLine();
                }

                avgTempMonth = totalTempMonth / amountOfDataInputMonth;
                monthTempData.Add((oldMonth, avgTempMonth));

                avgHumidityMonth = totalHumidityMonth / amountOfDataInputMonth;
                monthHumidityData.Add((oldMonth, avgHumidityMonth));

                calculateMold = (avgHumidityMonth - 78) * (avgTempMonth / 15) / 0.22;
                monthMoldRiskData.Add((oldMonth, calculateMold));

                avgTemp = totalTemp / amountOfDataInput;
                avgHumidity = totalHumidity / amountOfDataInput;

                highestHumidity.Add((oldDate, avgHumidity));
                highestTemp.Add((oldDate, avgTemp));

                calculateMold = (avgHumidity - 78) * (avgTemp / 15) / 0.22;
                highestMoldRisk.Add((oldDate, calculateMold));

                var sortedByTemp = highestTemp.OrderByDescending(entry => entry.Temp).ToList();
                int i = 1;

                if(keyPress == 1)
                {
                    foreach (var entry in sortedByTemp)
                    {
                        Console.WriteLine($"Day {i + ":", -4} {entry.Date} | Temp: {Math.Round(entry.Temp, 1)}°C");
                        i++;
                    }
                    Console.ReadKey(true);

                }
                else if(keyPress == 2)
                {
                    var sortedByHumidity = highestHumidity.OrderBy(entry => entry.Humidity).ToList();
                    foreach (var entry in sortedByHumidity)
                    {
                        Console.WriteLine($"Day {i, 3} {entry.Date} | Humidity: {Math.Round(entry.Humidity, 2)} %");
                        i++;
                    }
                    Console.ReadKey(true);
                }

                else if(keyPress == 3)
                {
                    var sortedByMoldRisk = highestMoldRisk.OrderByDescending(entry => entry.moldRisk).ToList();
                    foreach( var entry in sortedByMoldRisk)
                    {
                        if(entry.moldRisk < 0)
                        {
                            Console.WriteLine($"Day {i,3} {entry.Date} | Moldrisk: 0 %");
                        }
                        else
                        {
                            Console.WriteLine($"Day {i,3} {entry.Date} | Moldrisk: {Math.Round(entry.moldRisk, 2)} %");
                        }
                        
                        i++;
                    }
                    Console.ReadKey(true);
                }

                else if(keyPress == 4)
                {
                    foreach(var entry in autumnCountDay)
                    {
                        Console.WriteLine($"Index {i, 3} {entry.Date} | Temp: {Math.Round(entry.Temp, 1)}°C");
                        i++;
                    }
                    Console.ReadKey(true);
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
                            Console.WriteLine($"Index {i, 3} {entry.Date} | Temp: {Math.Round(entry.Temp, 1)}°C");
                            i++;
                        }
                    }
                    Console.ReadKey(true);
                }
                else if (keyPress == 6)
                {
                    var sortedByTempMonth = monthTempData.ToList();
                    var sortedByHumidityMonth = monthHumidityData.ToList();
                    var sortedByMoldRiskMonth = monthMoldRiskData.ToList();
                    
                    string tempDataFile = "tempDataFile.txt";
                    using (StreamWriter writer = new StreamWriter(path + tempDataFile, true))
                    {
                        writer.WriteLine("====" + position + "====");
                        writer.WriteLine("====Temperature====");
                        foreach (var entry in sortedByTempMonth)
                        {
                            writer.WriteLine($"Month: {entry.Date}: {Math.Round(entry.Temp, 1)}°C");
                                                       
                        }
                        writer.WriteLine("====Humidity====");
                        foreach (var entry in sortedByHumidityMonth)
                        {
                            writer.WriteLine($"Month: {entry.Date}: {Math.Round(entry.Humidity, 2)}%");

                        }
                        writer.WriteLine("====Moldrisk====\ncalculateMold = (avgHumidity - 78) * (avgTemp / 15)/0.22;");
                        foreach (var entry in sortedByMoldRiskMonth)
                        {
                            if (entry.moldRisk <= 0)
                            {
                                writer.WriteLine($"Month: {entry.Date}: 0%");
                            }
                            else
                            {
                                writer.WriteLine($"Month: {entry.Date}: {Math.Round(entry.moldRisk, 2)}%");
                            }
                        }
                        if(position == "Ute")
                        {
                            writer.WriteLine("====When Autumn started====\n" + autumnCountDay.First().Date);
                            if (closestToWinterList.Count >= 5)
                            {
                                writer.WriteLine("====When Winter started====\n" + closestToWinterList.First().Date);
                                foreach (var entry in closestToWinterList)
                                {
                                    writer.WriteLine($"Month: {entry.Date} Temp: {Math.Round(entry.Temp, 2)}");
                                }
                            }
                            else
                            {
                                writer.WriteLine("====When Winter was closest to start====\n" + closestToWinterList.First().Date);
                                foreach (var entry in closestToWinterList)
                                {
                                    writer.WriteLine($"Month: {entry.Date} Temp: {Math.Round(entry.Temp, 2)}");
                                }
                            }
                        }
                    } 
                    if(position == "Ute")
                    {
                        TemperatureData(6, "Inne", fileName);
                    }
                    
                    Console.WriteLine("Data successfully saved!");
                    Console.ReadKey(true);
                }
            }
        }
    }
}
