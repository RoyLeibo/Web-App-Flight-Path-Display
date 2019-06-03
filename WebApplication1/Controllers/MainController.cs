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
        private bool IsConnected { get; set; }
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
            this.IsConnected = false;
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
                this.displayPath(ip, port);
            }
        }

        public ActionResult DisplayLocation(String ip, int port)
        {
            this.myModel.ioFromSimulator.ConnectInOtherThread(ip, port);
            this.myModel.ioFromSimulator.getPoint();
            return View();
        }

        [HttpGet]
        public ActionResult DisplayAnimation(String ip, int port, int freq)
        {
            Session["freq"] = freq;
            this.myModel.ioFromSimulator.ConnectInOtherThread(ip, port);
            this.myModel.ioFromSimulator.ReadDataFromSimulator();
            return View();
        }

        public ActionResult save(String ip, int port, int freq, int sec, String fileName)
        {
            this.myModel.ioFromSimulator.isWriteToFile = true;
            this.myModel.ioFromSimulator.ConnectInOtherThread(ip, port);
            this.myModel.ioFromSimulator.ReadDataFromSimulator();
            return View();
        }

        public ActionResult displayPath(String fileName, int freq)
        {
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
            return this.ToXml(this.LonAndLat);
        }

        private string ToXml(Point LonAndLat)
        {
            //Initiate XML stuff
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            XmlWriter writer = XmlWriter.Create(sb, settings);
            writer.WriteStartDocument();
            LonAndLat.ToXml(writer);
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            return sb.ToString();
        }
    }
}