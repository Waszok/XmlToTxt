using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Xml.Linq;

namespace XmlToTxt
{
    /// <summary>
    /// Helper static class with needed methods
    /// </summary>
    static class Utilities
    {
        #region Read and parse Xml

        /// <summary>
        /// The method loads and parses a xml file (the file name is given as an argument).
        /// To deal with the file uses XDocument class and Linq syntax.
        /// Method returns a list of strings, each of them represents a properly formated line
        /// (those lines will be write in a .txt file later).
        /// </summary>
        /// <param name="fileName">name of xml file</param>
        /// <returns></returns>
        public static List<string> XmlParser(string fileName)
        {
            List<string> lines = new List<string>();
            try
            {
                XDocument doc = XDocument.Load(fileName);
                var nodes = from el in doc.Descendants("SprzedazWiersz")
                            select new
                            {
                                LpSprzedazy = el.Descendants("LpSprzedazy").First().Value,
                                NrKontrahenta = el.Descendants("NrKontrahenta").First().Value,
                                NazwaKontrahenta = el.Descendants("NazwaKontrahenta").First().Value,
                                DowodSprzedazy = el.Descendants("DowodSprzedazy").First().Value,
                                DataWystawienia = el.Descendants("DataWystawienia").First().Value,
                                DataSprzedazy = el.Descendants("DataSprzedazy").First().Value,
                                K_14 = el.Descendants("K_14").First().Value,
                                K_19 = el.Descendants("K_19").First().Value,
                                K_20 = el.Descendants("K_20").First().Value
                            };

                foreach (var node in nodes)
                {
                    float K_14 = float.Parse(node.K_14);
                    float K_19 = float.Parse(node.K_19);
                    float K_20 = float.Parse(node.K_20);

                    float K_14_19_20 = K_14 + K_19 + K_20;
                    float K_19_20 = K_19 + K_20;

                    string line1 = $"\"KON\";\"{node.LpSprzedazy}\";\"{node.NrKontrahenta}\";\"{node.NazwaKontrahenta}\"";
                    string line2 = $"\"FS\";\"SPT/P\";\"{node.DataWystawienia}\";\"{node.DowodSprzedazy}\";\"{node.LpSprzedazy}\";\"\";{K_14_19_20.ToString("0.00")}";
                    string line3 = $"\"VAT\";23;0;{node.K_19};{node.K_20};{K_19_20.ToString("0.00")}";
                    string line4 = $"\"PLA\";\"{node.LpSprzedazy}\";\"gotówka\";\"{node.DataWystawienia}\";{K_19_20.ToString("0.00")}";
                    string line5 = $"";
                    lines.AddRange(new List<string> { line1, line2, line3, line4, line5 });
                }

            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine($"ArgumentNullException: {e.Message}.");
            }
            catch (SecurityException e)
            {
                Console.WriteLine($"SecurityException: {e.Message}.");
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine($"FileNotFoundException: {e.Message}.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message} Exception caught.");
            }
            return lines;
        }

        #endregion

        #region Write list of strings to the txt file

        /// <summary>
        /// The method writes elements of given list in a .txt file (every single list's element as a separated line).
        /// </summary>
        /// <param name="list"></param>
        public static void WriteToTxtFile(List<string> list)
        {
            using (System.IO.StreamWriter file =
                   new System.IO.StreamWriter(@"C:\Users\Kamil\source\repos\XmlToTxt\XmlToTxt\jpk.anon.txt")) //To append new text to an existing file just provide a second parameter as "true".
            {
                foreach (string line in list)
                {
                    file.WriteLine(line);
                }
            }
        }

        #endregion
    }
}
