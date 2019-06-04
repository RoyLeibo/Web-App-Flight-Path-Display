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
        public const string SCENARIO_FILE = "~/App_Data/{0}.txt";
        public List<String> LinesToSave { get; set; }
        public List<String> DataFromFile { get; set; }
        public String FileName { get; set; }
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
        private static IOFromFile instance = null;

        private IOFromFile()
        {
            this.LinesToSave = new List<String>();
        }

        public static IOFromFile Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new IOFromFile();
                }
                return instance;
            }
        }

        public void saveData(String FileName, double[] dataToSave)
        {
            this.FileName = FileName;
            this.LinesToSave.Add(this.createDataString(dataToSave));
        }

        public void SendDataToFile()
        {
            String FilePath = HttpContext.Current.Server.MapPath(String.Format(SCENARIO_FILE, this.FileName));
            StreamWriter SW = new StreamWriter(FilePath);
            foreach (String line in this.LinesToSave)
            {
                SW.WriteLine(line);
            }
            SW.Flush();
            SW.Close();
        }

        public void loadData(String fileName)
        {
            List<String> fileData = new List<String>();
            try
            {
                String FilePath = HttpContext.Current.Server.MapPath(String.Format(SCENARIO_FILE, fileName));
                if (File.Exists(FilePath))
                {
                    StreamReader SR = new StreamReader(FilePath);
                    String line = "";
                    while ((line = SR.ReadLine()) != null)
                    {
                        ;
                        fileData.Add(line);
                    }
                    SR.Close();
                }
            }
            catch (Exception e)
            {

            }
            this.DataFromFile = fileData;
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

        public bool parseFileData()
        {
            if (this.DataFromFile.Count == 0)
            {
                return false;
            }
            else
            {
                String line = this.DataFromFile[0];
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
                this.LonAndLat = new Point(Lon, Lat);
                this.DataFromFile.RemoveAt(0);
                return true;
            }
        }
    }
}