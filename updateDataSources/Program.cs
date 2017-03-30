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
        /// <summary>
        /// call with arguments "path\to\file" "Mnemonic"
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            string xmlPath = args[0];
            string mnemonic = args[1];
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
                literal = literal.Replace(",", "");
                string[] splitLiteral = literal.Split(' ');
                var duplicate = false;
                for(int i = 0; i < splitLiteral.Length-2; i++)
                {
                    for(int x = i+2; x<splitLiteral.Length-2; x++)
                    {
                        if (splitLiteral[i] + splitLiteral[i+1] + splitLiteral[i+2] == splitLiteral[x] + splitLiteral[x + 1] + splitLiteral[x+2])
                        {
                            duplicate = true;
                        }
                    }
                }
                if (duplicate)
                {
                    CustomLogger.Trace(string.Format("There is a statute literal duplicated:\nCode - {0}\nLiteral - {1}\n", code, literal));
                    presentMoreThanOnce = true;
                }
            }

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
