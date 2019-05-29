using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace WebApplication1.Models
{
    public class IOFromFile
    {
        private String DataToSave;
        public void saveData(String fileName, String data)
        {
            StreamWriter SW = new StreamWriter(fileName);
            SW.WriteLine(data);
        }

        public String loadData(String fileName, String data)
        {
            try
            {
                StreamReader SR = new StreamReader(fileName)
            }
            catch (ArgumentNullException e)
            {

            }

            FileStream F = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            List<int> list = new List<int>();
            int tempRead;
            do
            {
                tempRead = F.ReadByte();
                list.Add(tempRead);
            } while (tempRead != -1);
            int[] intArray = list.ToArray();
            byte[] byteArray = intArray.SelectMany(BitConverter.GetBytes).ToArray();
            return Encoding.ASCII.GetString(byteArray);
        }

        public void createDataString(Point LonAndLat, double Throttle, double Rudder)
        {

        }
    }
}