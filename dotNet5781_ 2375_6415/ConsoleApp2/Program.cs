using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DS;
using DalApi;
using DO;
using System.IO;
using System.Xml.Serialization;

namespace ConsoleApp2
{
    class Program
    {

        public static void SaveListToXMLSerializer<T>(List<T> list, string filePath)
        {
            try
            {
                FileStream file = new FileStream(@"C:\Users\Yona et Audelia\Desktop\Mini Project .NET\dotNet5781_ 2375_6415\bin\Xml\" + filePath, FileMode.Create);
                XmlSerializer x = new XmlSerializer(list.GetType());
                x.Serialize(file, list);
                file.Close();
            }
            catch (Exception ex)
            {
                throw new DO.XMLFileLoadCreateException(filePath, $"fail to create xml file: {filePath}", ex);
            }
        }
        static void Main(string[] args)
        {
            SaveListToXMLSerializer<LineDeparting>(DataSource.lineDepartingList, "LineDeparting.xml");
        }
    }
}
