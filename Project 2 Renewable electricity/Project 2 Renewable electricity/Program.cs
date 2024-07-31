using System.ComponentModel.Design.Serialization;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.XPath;

namespace Project_2_Renewable_electricity
{
    internal class Program
    {
        const string XmlFile = @"..\..\..\renewable-electricity.xml";
        private static readonly List<string> _data = new();

        static void Main(string[] args)
        {
            
            String option = null!;
            bool wrong = false;

            XmlDocument xmlDocument = new XmlDocument();
            XmlElement root = xmlDocument.CreateElement("RenewableElectricity");
            xmlDocument.AppendChild(root);
            XmlElement c = xmlDocument.CreateElement("Country");
            root.AppendChild(c);
            XmlElement s = xmlDocument.CreateElement("Source");
            root.AppendChild(s);
            XmlElement p = xmlDocument.CreateElement("Percentages");
            root.AppendChild(p);
            do
            {
                do
                {
                    if (wrong)
                    {
                        Console.Write("\nPlease write one of the available options");
                        Thread.Sleep(400);
                        Console.Write(".");
                        Thread.Sleep(200);
                        Console.Write(".");
                        Thread.Sleep(200);
                        Console.Write(".");
                        Thread.Sleep(200);
                    }
                    Console.Clear();
                    Console.WriteLine("Renewable Electricity Production in 2021\n" + "========================================\n");
                    Console.Write("Enter 'C' to Select a country, 'S' to select a specific source, 'P' to select\na % range of renewable production or 'X' to quit: ");
                    option = Console.ReadLine()!.ToUpper();
                    wrong = true;
                } while (!option.Equals("C") & !option.Equals("X") & !option.Equals("S") & !option.Equals("P"));

                XmlDocument doc = new();

                try
                {
                    doc.Load(XmlFile);
                }
                catch (XmlException err)
                {
                    Console.WriteLine($"\nXML ERROR: {err.Message}");
                    return;
                }
                catch (XPathException err)
                {
                    Console.WriteLine($"\nXPATH ERROR: {err.Message}");
                    return;
                }
                catch (Exception err)
                {
                    Console.WriteLine($"\nERROR: {err.Message}");
                    return;
                }
                if (option.Equals("C"))
                {

                    getParameters(doc, "//country", "name");

                    for (int i = 0; i < _data.Count; i++)
                    {
                        String c1 = $"{i + 1} " + _data[i++].PadRight(40);
                        String c2 = $"{i + 1} " + _data[i++].PadRight(40);
                        String c3 = $"{i + 1} " + _data[i];

                        Console.WriteLine($"{c1}{c2}{c3}");
                    }
                    Console.Write("Enter a country #: ");
                    bool errorHandlr = true;
                    int countryNumber = 0;
                    while (errorHandlr)
                    {
                        try
                        {
                            countryNumber = Int32.Parse(Console.ReadLine()!) - 1;
                            errorHandlr = false;
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Must input an integer");
                        }
                    }
                    String countryName = _data[countryNumber];
                    c.SetAttribute("name", countryName);

                    getParameters(doc, $"//country[@name='{countryName}']/source", "type");
                    List<String> type = new List<String>(_data);
                    getParameters(doc, $"//country[@name='{countryName}']/source", "amount");
                    List<String> amount = new List<String>(_data);
                    getParameters(doc, $"//country[@name='{countryName}']/source", "percent-of-all");
                    List<String> percentofAll = new List<String>(_data);
                    getParameters(doc, $"//country[@name='{countryName}']/source", "percent-of-renewables");

                    Console.WriteLine("Renewable Electricity Production in " + countryName);
                    Console.WriteLine("-----------------------------------------------------");
                    int matches;
                    for (matches = 0; matches < _data.Count; matches++)
                    {
                        String c1 = $"{type[matches].PadRight(20)}";
                        String c2 = $"{double.Parse(amount[matches]):N2}".PadRight(20);
                        String c3 = $"{double.Parse(percentofAll[matches]):N2}".PadRight(20);
                        String c4 = $"{double.Parse(_data[matches]):N2}".PadRight(20);
                        Console.WriteLine($"{c1}{c2}{c3}{c4}");
                    }
                    Console.WriteLine($"\n{matches} match(es) found.");
                    Thread.Sleep(5000);

                }

                else if (option.Equals("S"))
                {
                    getParameters(doc, $"//country[@name='Honduras']/source", "type");
                    Console.WriteLine("\nSelect a renewable by number as shown below...");
                    for(int i = 0; i < _data.Count; i++)
                    {
                        Console.WriteLine($"\t{i + 1}. {_data[i]}");
                    }

                    String type = null;
                    while(type == null)
                    try
                    {
                        Console.Write("Enter a renewable #: ");
                        int renewableOption = Int32.Parse(Console.ReadLine()!);
                        type = _data[renewableOption-1];
                    }
                    catch {
                        Console.WriteLine("Type a valid number");
                    }
                    s.SetAttribute("Type", type);

                    getParameters(doc, "//country", "name");
                    List<String> countries = new List<String>(_data);
                    getParameters(doc, $"//country/source[@type='{type}']", "amount");
                    List<String> amount = new List<String>(_data);
                    getParameters(doc, $"//country/source[@type='{type}']", "percent-of-all");
                    List<String> percentOfAll = new List<String>(_data);
                    getParameters(doc, $"//country/source[@type='{type}']", "percent-of-renewables");

                    Console.WriteLine($"{type} Electricity Production\n" + "----------------------------");

                    Console.WriteLine($"{"Country".PadLeft(45)}{"Amount".PadLeft(20)}{"Percent Of All".PadLeft(20)}{"Percent of Renewables".PadLeft(20)}");
                    int matches;
                    for (matches = 0; matches < _data.Count; matches++)
                    {
                        String c1 = $"{countries[matches].PadLeft(45)}";
                        String c2 = $"{double.Parse(amount[matches]):N2}".PadLeft(20);
                        String c3 = $"{double.Parse(percentOfAll[matches]):N2}".PadLeft(20);
                        String c4 = $"{double.Parse(_data[matches]):N2}".PadLeft(20);
                        Console.WriteLine($"{c1}{c2}{c3}{c4}");

                    }

                    Console.WriteLine($"\n{matches} match(es) found.");
                    Thread.Sleep(5000);

                }

                else if (option.Equals("P"))
                {
                    int maxPercentage = 100;
                    int minPercentage = 0;
                    bool done = false;

                    while (!done)
                    {

                        try
                        {
                            Console.Write("Enter the minimum % of renewables produced or press Enter for no minimum:");
                            string temp = Console.ReadLine()!;
                            if (!string.IsNullOrEmpty(temp))
                                minPercentage = Int32.Parse(temp);

                            Console.Write("Enter the maximum % of renewables produced or press Enter for no maximum:");
                            temp = Console.ReadLine()!;
                            if (!string.IsNullOrEmpty(temp))
                                maxPercentage = Int32.Parse(temp);

                            done = true;
                        }
                        catch
                        {
                            Console.WriteLine("Invalid input. Please enter a valid percentage or press Enter to skip.");
                        }
                    }

                    p.SetAttribute("Min", $"{minPercentage}");
                    p.SetAttribute("Max", $"{maxPercentage}");

                    getParameters(doc, $"//country[totals[number(@renewable-percent) > {minPercentage} and number(@renewable-percent) < {maxPercentage}]]", "name");
                    List<string> countries = new List<string>(_data);
                    getParameters(doc, $"//country[totals[number(@renewable-percent) > {minPercentage} and number(@renewable-percent) < {maxPercentage}]]/totals", "renewable-percent");
                    List<string> percentOfRenewables = new List<string>(_data);
                    getParameters(doc, $"//country[totals[number(@renewable-percent) > {minPercentage} and number(@renewable-percent) < {maxPercentage}]]/totals", "all-renewables");
                    List<string> allRenewables = new List<string>(_data);
                    getParameters(doc, $"//country[totals[number(@renewable-percent) > {minPercentage} and number(@renewable-percent) < {maxPercentage}]]/totals", "all-sources");
                    List<string> allSources = new List<string>(_data);

                    Console.WriteLine($"{"Country".PadLeft(45)}{"All Elec. (GWh)".PadLeft(20)}{"Renewable (GWh)".PadLeft(20)}{"% Renewable".PadLeft(20)}");
                    int matches;
                    for (matches = 0; matches < _data.Count; matches++)
                    {
                        String c1 = $"{countries[matches].PadLeft(45)}";
                        String c2 = $"{double.Parse(allSources[matches]):N2}".PadLeft(20);
                        String c3 = $"{double.Parse(allRenewables[matches]):N2}".PadLeft(20);
                        String c4 = $"{double.Parse(percentOfRenewables[matches]):N2}".PadLeft(20);
                        Console.WriteLine($"{c1}{c2}{c3}{c4}");

                    }

                    Console.WriteLine($"\n{matches} match(es) found.");
                    Thread.Sleep(5000);
                }
                wrong = false;

            } while (!option.Equals("X"));

            xmlDocument.Save("Electricity.xml");

        }
        static void getParameters(XmlDocument doc, string path, string attribute)
        {
            _data.Clear();
            XmlNodeList nodes = doc.SelectNodes(path)!;
            foreach (XmlElement paramElem in nodes)
            {
                _data.Add(paramElem.GetAttribute(attribute));
            }
        }



    }
}
