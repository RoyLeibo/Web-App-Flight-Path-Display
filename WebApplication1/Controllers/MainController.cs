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

        public MainController()
        {
            this.myModel = new MyModel();
            this.myModel.ioFromSimulator.IoEvent += getLonAndLat;
            this.myModel.ioFromFile.IoEvent += getLonAndLat;
            this.IsRunning = false;
        }

        public ActionResult Index()
        {
            return View();
        }

        public void CheckURL(String ip, int port)
        {
            try
            {
                IPAddress address = IPAddress.Parse(ip);
                this.DisplayLocation(ip, port);
            }
            catch (Exception e)
            {
                this.DisplayPath(ip, port);
            }
        }

        public ActionResult DisplayLocation(String ip, int port)
        {

            this.myModel.ioFromSimulator.ConnectInOtherThread(ip, port);
            this.myModel.ioFromSimulator.getPoint();
            return View();
        }

        public ActionResult DisplayAnimation(String ip, int port, int freq)
        {
            Session["freq"] = freq;
            this.myModel.ioFromSimulator.ConnectInOtherThread(ip, port);
            this.myModel.ioFromSimulator.ReadDataFromSimulator();
            return View();
        }

        public ActionResult Save(String ip, int port, int freq, int sec, String fileName)
        {
            Session["sec"] = sec;
            Session["freq"] = freq;
            this.myModel.ioFromSimulator.FileName = fileName;
            this.myModel.ioFromSimulator.isWriteToFile = true;
            this.myModel.ioFromSimulator.ConnectInOtherThread(ip, port);
            this.myModel.ioFromSimulator.ReadDataFromSimulator();
            return View();

        }

        public ActionResult DisplayPath(String fileName, int freq)
        {
            Session["freq"] = freq;
            this.myModel.ioFromFile.loadData(fileName);
            return View();
        }

        public void getLonAndLat()
        {
            this.LonAndLat = this.myModel.ioFromSimulator.LonAndLat;
        }

        [HttpPost]
        public String GetPoint()
        {
            this.myModel.ioFromSimulator.ReadDataFromSimulator();
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            XmlWriter writer = XmlWriter.Create(sb, settings);
            writer.WriteStartDocument();
            writer.WriteStartElement("Point");
            writer.WriteElementString("Lon", Convert.ToString(this.LonAndLat.getX()));
            writer.WriteElementString("Lat", Convert.ToString(this.LonAndLat.getY()));
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            return sb.ToString();
        }

        [HttpPost]
        public void SaveData()
        {
            this.myModel.ioFromFile.SendDataToFile();
        }
        [HttpPost]
        public String GetPointFromFile()
        {
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            XmlWriter writer = XmlWriter.Create(sb, settings);
            writer.WriteStartDocument();
            writer.WriteStartElement("Point");
            writer.WriteElementString("Lon", Convert.ToString(this.LonAndLat.getX()));
            writer.WriteElementString("Lat", Convert.ToString(this.LonAndLat.getY()));
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            return sb.ToString();
        }
    }
}