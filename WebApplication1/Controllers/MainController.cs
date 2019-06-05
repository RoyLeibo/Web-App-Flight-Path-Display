using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class MainController : Controller
    {
        private MyModel myModel;
        private bool IsRunning { get; set; }
        private Point lonAndLat;
        public Point LonAndLat
        {
            get
            {
                return this.lonAndLat;
            }
            set
            {
                this.IsRunning = true;
                this.lonAndLat = value;
                ViewBag.lon = this.lonAndLat.getX();
                ViewBag.lat = this.lonAndLat.getY();
                ViewBag.LonAndLat = this.lonAndLat;
            }
        }

        /*
         * This function is the constructor
         **/
        public MainController()
        {
            this.myModel = new MyModel();
            this.myModel.ioFromSimulator.IoEvent += getLonAndLat;
            this.myModel.ioFromFile.IoEvent += getLonAndLat;
            this.IsRunning = false;
        }
         
        /*
         * Ths defult action of the defult route
         */
        public ActionResult Index()
        {
            return View();
        }

        //public void CheckURL(String ip, int port)
        //{
        //    try
        //    {
        //        IPAddress address = IPAddress.Parse(ip);
        //        this.DisplayLocation(ip, port);
        //    }
        //    catch (Exception e)
        //    {
        //        this.DisplayPath(ip, port);
        //    }
        //}

        /*
         * This function return the action of the DisplayLocation.
         * connect to the simulator, get one point and show the point on the map
         **/
        public ActionResult DisplayLocation(String ip, int port)
        {
            this.myModel.ioFromSimulator.ConnectInOtherThread(ip, port);
            this.myModel.ioFromSimulator.getPoint();
            return View();
        }

        /*
         * This function retune the action of DisplayAnimation.
         * connect to the simulator, and read points from the simulator
         * according the number of the freq that we get in the url.
         * the points we show on the map the draw the route the plane
         **/
        public ActionResult DisplayAnimation(String ip, int port, int freq)
        {
            Session["freq"] = freq;
            this.myModel.ioFromSimulator.ConnectInOtherThread(ip, port);
            this.myModel.ioFromSimulator.ReadDataFromSimulator();
            return View();
        }

        /*
         * This function return the action of Save.
         * connect to the simulator, and read points from the simulator
         * according the number of the freq that we get in the url.
         * in addtion to show the points on the map we save the points 
         * in file.
         **/
        public ActionResult Save(String ip, int port, int freq, int sec, String fileName)
        {
            Session["sec"] = sec;
            Session["freq"] = freq;
            this.myModel.ioFromSimulator.FileName = fileName;
            this.myModel.ioFromSimulator.isWriteToFile = true;
            this.myModel.ioFromSimulator.ConnectInOtherThread(ip, port);
            return View();
        }

        /*
         * This function return the action of DisplayPath.
         * read the point from the file and shoe the route that 
         * they make on the map. the points show accordind the
         * number of freq.
         **/
        public ActionResult DisplayPath(String fileName, int freq)
        {
            Session["freq"] = freq;
            this.myModel.ioFromFile.loadData(fileName);
            return View();
        }

        /*
         * This function get the value of lon and lat from
         * simulator and save them in the controller
         **/
        public void getLonAndLat()
        {
            this.LonAndLat = this.myModel.ioFromSimulator.LonAndLat;
        }

        /*
         * This function called by the veiw and return Xml with the value 
         * of lon amd lat.
         **/
        [HttpPost]
        public String GetPoint()
        {
            this.myModel.ioFromSimulator.ReadDataFromSimulator();
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            XmlWriter writer = XmlWriter.Create(sb, settings);
            writer.WriteStartDocument();
            //write the value into the xml
            writer.WriteStartElement("Point");
            writer.WriteElementString("Lon", Convert.ToString(this.LonAndLat.getX()));
            writer.WriteElementString("Lat", Convert.ToString(this.LonAndLat.getY()));
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            return sb.ToString();
        }

        /*
         * This functiom calles by the veiw and save all the data 
         * that we read from the simulatot in file and close the 
         * connection to the simulator.
         **/
        [HttpPost]
        public void SaveData()
        {
            this.myModel.ioFromFile.SendDataToFile();
            this.myModel.CloseConnection();
        }

        /*
         * This function called by the view in case 4 
         * when we read from file.
         * and return one point each time. 
         **/
        [HttpPost]
        public String GetPointFromFile()
        {
            if (this.myModel.ioFromFile.parseFileData())
            {
                StringBuilder sb = new StringBuilder();
                XmlWriterSettings settings = new XmlWriterSettings();
                XmlWriter writer = XmlWriter.Create(sb, settings);
                writer.WriteStartDocument();
                //write the value of the lon and lat in XML
                writer.WriteStartElement("Point");
                writer.WriteElementString("Lon", Convert.ToString(this.LonAndLat.getX()));
                writer.WriteElementString("Lat", Convert.ToString(this.LonAndLat.getY()));
                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Flush();
                return sb.ToString();
            }
            else
            {
                return "Done";
            }
        }   

        [HttpPost]
        public void CloseFileRead()
        {
            this.myModel.CloseConnection();
        }

        [HttpPost]
        public void ReloadPage()
        {
            this.myModel.CloseConnection();
            this.myModel.ioFromSimulator.ConnectInOtherThread(null, 0);
        }
    }
}