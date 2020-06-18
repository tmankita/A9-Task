using System.Collections.Generic;
using System.Text.RegularExpressions;
using IpStack;
using IpStack.Models;


namespace ex1
{
    public class Content
    {
        private Dictionary<string, List<string>> res;
        private IpStackClient client;
        private IpAddressDetails respone;

        public Content()
        {
            res = new Dictionary<string, List<string>>();
            res["ID"] = new List<string>();
            res["Date"] = new List<string>();
            res["Location_Name"] = new List<string>();
            res["Longitude"] = new List<string>();
            res["Latitude"] = new List<string>();
            res["Day"] = new List<string>();
            res["Month"] = new List<string>();
            res["Year"] = new List<string>();
            res["Hour"] = new List<string>();
            res["Minute"] = new List<string>();
            res["DOW"] = new List<string>();
            res["IP"] = new List<string>();

            client = new IpStackClient("a571627c11be913524c15d06579b1814");
        }


        private void getDetailsOnIp(string ip)
        {
            respone = client.GetIpAddressDetails(ip);
        }

        public void setLongitudeAndLatitude(string ip)
        {
            res["Latitude"].Add(""+respone.Latitude);
            res["Longitude"].Add("" + respone.Longitude);
        }

        private void getDateInfo(ref Dictionary<string, List<string>> res, string date)
        {
            Regex Date = new Regex("(?<y>[1-9]\\d{3})-(?<m>(?:0[1-9])|(?:1[0-2]))-(?<d>(?:[1-2][0-9])|(3[01]))(?<dow>(?:[Ss]u)|(?:[Mm]o)|(?:[Tt]u)|(?:[Ww]e)|(?:[Tt]h)|(?:[Ff]r)|(?:[Ss]a))(?<hour>(?:[01][0-9])|(?:2[0-3])):(?<minute>[0-5][0-9])");
            res["Date"].Add(date);
            res["Day"].Add(Date.Match(date).Groups["d"].Value);
            res["Month"].Add(Date.Match(date).Groups["m"].Value);
            res["Year"].Add(Date.Match(date).Groups["y"].Value);
            res["Hour"].Add(Date.Match(date).Groups["hour"].Value);
            res["Minute"].Add(Date.Match(date).Groups["minute"].Value);
            res["DOW"].Add(Date.Match(date).Groups["dow"].Value);

        }
        private bool validateIP(string ip)
        {
            string numPattern = "((?:[0-9])|(?:[1-9][0-9])|(?:[1][0-9][0-9])|(?:[2][0-4][0-9])|(?:[2][5][0-5]))";
            Regex IP = new Regex("^(?:"+ numPattern + ")\\.(?:"+ numPattern + ")\\.(?:"+ numPattern + ")\\.(?:"+ numPattern + ")$");
            getDetailsOnIp(ip);
            bool match = IP.IsMatch(ip);
            return IP.IsMatch(ip) && respone.Latitude != 0 && respone.Longitude != 0;
        }

        public void createDB(string path)
        {
            string[] lines = System.IO.File.ReadAllLines(path);
            for (int j=1; j< lines.Length; j++)
            {
                string[] parts = lines[j].Split('\t');
                res["ID"].Add("" + j);

                for (int i = 0; i < parts.Length; i++)
                {
                    switch (i)
                    {
                        case 0:
                            getDateInfo(ref res, parts[i].Trim());
                            break;
                        case 1:
                            res["Location_Name"].Add(parts[i].Trim());
                            break;
                        case 2:
                            string ip = parts[i].Trim();
                            if (validateIP(ip))
                            {
                                res["IP"].Add(ip);
                                setLongitudeAndLatitude(ip);
                            }
                            else
                            {
                                res["ID"].RemoveAt(res["ID"].Count-1);
                                res["Date"].RemoveAt(res["Date"].Count - 1);
                                res["Location_Name"].RemoveAt(res["Location_Name"].Count - 1);
                                res["Day"].RemoveAt(res["Day"].Count - 1);
                                res["Month"].RemoveAt(res["Month"].Count - 1);
                                res["Year"].RemoveAt(res["Year"].Count - 1);
                                res["Hour"].RemoveAt(res["Hour"].Count - 1);
                                res["Minute"].RemoveAt(res["Minute"].Count - 1);
                                res["DOW"].RemoveAt(res["DOW"].Count - 1);
                            }
                            break;
                    }

                }
            }
        }

        public void writeToFile(string path)
        {
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(path))
            {
                for (int i=0; i< res["ID"].Count; i++)
                {
                    string line = res["ID"][i] + "\t" +
                    res["Date"][i] + "\t" +
                    res["Location_Name"][i] + "\t" +
                    res["Longitude"][i] + "\t" +
                    res["Latitude"][i] + "\t" +
                    res["Day"][i] + "\t" +
                    res["Month"][i] + "\t" +
                    res["Year"][i] + "\t" +
                    res["Hour"][i] + "\t" +
                    res["Minute"][i] + "\t" +
                    res["DOW"][i] + "\t" +
                    res["IP"][i];

                    file.WriteLine(line);
                    
                }
            }
        }
    }
}
