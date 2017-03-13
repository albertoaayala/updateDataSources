using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

namespace updateDataSources
{
    class Program
    {
        static void Main(string[] args)
        {
            string xmlPath = args[0];
            //string filePath = args[1];
            //string fileName = filePath.Substring(filePath.LastIndexOf('\\')+1, (filePath.Length - filePath.LastIndexOf('\\'))-1);
            string mnemonic = args[1];
            //bool failed = false;
            bool presentMoreThanOnce = false;

            string line = string.Empty;

            XmlDocument transactionTemplate = new XmlDocument();
            try
            {
                transactionTemplate.Load(xmlPath);
            }
            catch
            {
                CustomLogger.Trace("Error - XML does not exist");
                Console.WriteLine("Make sure you have the correct path for DataSources.xml");
                return;
            }

            XmlNode dataSource = transactionTemplate.SelectSingleNode("/transactionTemplate/dataSources/dataSource[@mnemonic='" + mnemonic + "']");

            foreach(XmlNode entry in dataSource.ChildNodes)
            {
                string code = entry.ChildNodes[0].InnerText;
                string literal = entry.ChildNodes[1].InnerText;
                string[] splitLiteral = literal.Split(' ');
                for(int i = 2; i < splitLiteral.Length-1; i++)
                {
                    if(splitLiteral[0]+splitLiteral[1] == splitLiteral[i]+splitLiteral[i+1])
                    {
                        CustomLogger.Trace(string.Format("There is a statute literal duplicated:\nCode - {0}\nLiteral - {1}\n", code, literal));
                        presentMoreThanOnce = true;
                    }
                }
            }

            //StreamReader file = null;
            //try
            //{
            //    file = new StreamReader(filePath);
            //}
            //catch
            //{
            //    CustomLogger.Trace("Error - File does not exist");
            //    Console.WriteLine("Make sure you have given the correct path for the file");
            //    return;
            //}

            //file.ReadLine();
            //file.ReadLine();
            //while ((line = file.ReadLine()) != null)
            //{
            //    string txtCode = line.Substring(0, line.IndexOf('\t'));
            //    dataSource = transactionTemplate.SelectNodes("/transactionTemplate/dataSources/dataSource[@mnemonic='" + mnemonic + "']/entry/property[@name='Code'][text()='" + txtCode + "']");
            //    if (dataSource.Count < 1)
            //    {
            //        CustomLogger.Trace(string.Format("Could not find entry with properties: \"{0}\" in DataSources.xml", line));
            //        failed = true;
            //    }
            //    if(dataSource.Count > 1)
            //    {
            //        CustomLogger.Trace(string.Format("The statute with properties: \"{0}\" is present more than once", line));
            //        presentMoreThanOnce = true;
            //    }

            //    file.ReadLine();
            //}
            //file.Close();

            //if (failed)
            //{
            //    Console.WriteLine("\nCould not find all entries in DataSources.xml.\nLook at logs to see which entries are missing");
            //}
            //else 
            if (presentMoreThanOnce)
            {
                Console.WriteLine("\nThere are entries that are present more than once.\nLook at logs to see which entries are affected");
            }
            else
            {
                CustomLogger.Trace(string.Format("Success there are no duplicate entries in {0} from DataSources.xml", mnemonic));
                Console.WriteLine(string.Format("\nSuccess there are no duplicate entries in {0} from DataSources.xml", mnemonic));
            }
        }
    }
}
