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
        public event handler IoEvent;
        private Point lonAndLat;
        public Point LonAndLat
        {
            get
            {
                return this.lonAndLat;
            }
            set
            {
                this.lonAndLat = value;
                IoEvent?.Invoke();
            }
        }
        private String DataToSave;
        public void saveData(String FileName, double[] dataToSave)
        {
            String FilePath = @"C:\Desktop\" + FileName;
            if (!File.Exists(FileName))
            {
                File.Create(FileName);
            }
            StreamWriter SW = new StreamWriter(FileName);
            SW.WriteLine(this.createDataString(dataToSave));
        }

        public void loadData(String fileName)
        {
            List<String> fileData = new List<String>();
            try
            {
                StreamReader SR = new StreamReader(fileName);
                String line = "";
                do
                {
                    line = SR.ReadLine();
                    fileData.Add(line);
                } while (line != null);
            }
            catch (Exception e)
            {

            }
            this.parseFileData(fileData);
        }

        public String createDataString(double[] dataToSave)
        {
            String data = "";
            for (int i = 0; i < dataToSave.Length; i++)
            {
                data += dataToSave[i].ToString() + "$";
            }
            return data;
        }

        public void parseFileData(List<String> fileData)
        {
            foreach (String line in fileData)
            {
                int StartOfLon = 0;
                int EndOfLon = line.IndexOf('$', StartOfLon);
                // Extract the Lon property from the data string by finding the closest
                // ',' to it from start.
                double Lon = Double.Parse(line.Substring(StartOfLon, EndOfLon - StartOfLon));
                int StartOfLat = EndOfLon + 1;
                int EndOfLat = line.IndexOf('$', StartOfLat);
                // Extract the Lat property from the data string by finding the closest
                // ',' to it after the Lon.
                double Lat = Double.Parse(line.Substring(StartOfLat, EndOfLat - StartOfLat));
                this.LonAndLat = new Point(Lat, Lon);
                //int StartOfThrottle = EndOfLat + 1;
                //int EndOfThrottle = line.IndexOf('$', StartOfThrottle);
                //this.Throttle = Double.Parse(line.Substring(StartOfThrottle, EndOfThrottle - StartOfThrottle));
                //int StartOfRudder = EndOfThrottle + 1;
                //int EndOfRudder = line.IndexOf(',', StartOfRudder);
                //this.Rudder = Double.Parse(line.Substring(StartOfRudder, EndOfRudder - StartOfRudder));
            }
        }
    }
}