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
            string filePath = args[1];
            string fileName = filePath.Substring(filePath.LastIndexOf('\\')+1, (filePath.Length - filePath.LastIndexOf('\\'))-1);
            string mnemonic = args[2];
            bool failed = false;

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

            XmlNode dataSource;

            StreamReader file = null;
            try
            {
                file = new StreamReader(filePath);
            }
            catch
            {
                CustomLogger.Trace("Error - File does not exist");
                Console.WriteLine("Make sure you have given the correct path for the file");
                return;
            }

            file.ReadLine();
            file.ReadLine();
            while ((line = file.ReadLine()) != null)
            {
                string txtCode = line.Substring(0, line.IndexOf('\t'));
                dataSource = transactionTemplate.SelectSingleNode("/transactionTemplate/dataSources/dataSource[@mnemonic='" + mnemonic + "']/entry/property[@name='Value'][text()='"+txtCode+"']");
                if (dataSource==null)
                {
                    CustomLogger.Trace(string.Format("Could not find entry with properties: \"{0}\" in DataSources.xml", line));
                    failed = true;
                }

                file.ReadLine();
            }
            file.Close();

            if (failed)
            {
                Console.WriteLine("\nCould not find all entries in DataSources.xml.\nLook at logs to see which entries are missing");
            }
            else
            {
                CustomLogger.Trace("Success all entries are in DataSources.xml from " + fileName);
                Console.WriteLine("\nSuccess all entries are in DataSources.xml from " + fileName);
            }
        }
    }
}
