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
using System.Xml.Linq;

namespace ConsoleApp2
{
    class Program
    {

        public static void SaveListToXMLSerializer<T>(List<T> list, string filePath)
        {
            try
            {
                FileStream file = new FileStream(@"C:\Users\אליסף\source\repos\dotNet5781_2375_6415\dotNet5781_ 2375_6415\bin\Xml\" + filePath, FileMode.Create);
                XmlSerializer x = new XmlSerializer(list.GetType());
                x.Serialize(file, list);
                file.Close();
            }
            catch (Exception ex)
            {
                throw new DO.XMLFileLoadCreateException(filePath, $"fail to create xml file: {filePath}", ex);
            }
        }

        public static void SaveListToXMLElement(XElement rootElem, string filePath)
        {
            try
            {
                rootElem.Save(@"C:\Users\אליסף\source\repos\dotNet5781_2375_6415\dotNet5781_ 2375_6415\bin\Xml\" + filePath);
            }
            catch (Exception ex)
            {
                throw new DO.XMLFileLoadCreateException(filePath, $"fail to create xml file: {filePath}", ex);
            }
        }

        static void Main(string[] args)
        {
           SaveListToXMLSerializer<Station>(DataSource.stationList, "Station.xml");

           /* XElement pairStations = new XElement("ArrayOfPairStations");
            foreach (var item in DataSource.PairStationList)
            {
                XElement pairStationToAdd = new XElement("PairStation",
                    new XElement("FirstStationNumber", item.FirstStationNumber),
                     new XElement("LastStationNumber", item.LastStationNumber),
                        new XElement("Distance", item.Distance),
                     new XElement("Time", item.Time),
                    new XElement("MyActivity", item.MyActivity)
                    );
                pairStations.Add(pairStationToAdd);
            }
            SaveListToXMLElement(pairStations, "PairStations.xml");

            XElement lineDeparting = new XElement("ArrayOfLineDeparting");
            foreach (var item in DataSource.lineDepartingList)
            {
                XElement lineDepartingToAdd = new XElement("LineDeparting",
                    new XElement("LineNumber", item.LineNumber),
                     new XElement("StartTime", item.StartTime),
                        new XElement("Frequency", item.Frequency),
                     new XElement("StopTime", item.StopTime),
                    new XElement("MyActivity", item.MyActivity)
                    );
                lineDeparting.Add(lineDepartingToAdd);
            }
            SaveListToXMLElement(lineDeparting, "LineDeparting.xml");*/
        }
    }
}
