using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class MainController : Controller
    {
        private MyModel myModel;
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
            }
        }

        public MainController()
        {
            this.myModel = new MyModel();
            this.myModel.ioFromSimulator.IoEvent += getLonAndLat;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DisplayLocation(String ip, int port)
        {
            //this.myModel.connectionRequest(ip, port);
            this.myModel.ioFromSimulator.getPoint(this.myModel.ioFromSimulator.server);
            return View();
        }

        public ActionResult displayAnimation(String ip, int port, int freq)
        {
            this.myModel.connectionWithoutSave(ip, port);
            this.myModel.ioFromSimulator.ReadDataFromSimulator(this.myModel.ioFromSimulator.server);
            return View();
        }

        public ActionResult save(String ip, int port, int freq, int sec, String fileName)
        {
            this.myModel.ioFromSimulator.isWriteToFile = true;
            return View();
        }

        public ActionResult displayPath(String fileName, int freq)
        {
            return View();
        }

        public void getLonAndLat()
        {
            this.LonAndLat = this.myModel.ioFromSimulator.LonAndLat;
        }
    }
}