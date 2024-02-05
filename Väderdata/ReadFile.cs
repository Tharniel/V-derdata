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
                Regex regex = new Regex("^2016-(?<Month>[0-1][1-9])-(?<Day>[0-3][0-9]) (?<TimeOfDay>[0-2][0-9]:[0-5][0-9]:[0-5][0-9]),(?<Location>Inne|Ute),(?<Temp>-?\\d{1,2}.[0-9]),(?<Humidity>[0,1]?[0-9]{1,2})$");
                string line = reader.ReadLine();
                Match match = regex.Match(line);
                int rowCount = 0;
                while (line != null)
                {
                    int month = int.Parse(match.Groups["Month"].Value);
                    if (month > 5 && month != 1)
                    {
                        Console.WriteLine(rowCount + " " + line);
                        rowCount++;
                        line = reader.ReadLine();
                    }
                    else
                    {
                        rowCount++;
                    }
                }
            }
        }
        //regex för att ta ut all data
        //^2016-(?<Month>[0-1][1-9])-(?<Day>[0-3][0-9]) (?<TimeOfDay>[0-2][0-9]:[0-5][0-9]:[0-5][0-9]),(?<Location>Inne|Ute),(?<Temp>-?\d{1,2}.[0-9]),(?<Humidity>[0,1]?[0-9]{1,2})$
    }
}
