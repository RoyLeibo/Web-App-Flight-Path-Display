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
                double[] a = new double[2];
                a[0] = this.lonAndLat.getX();
                a[1] = this.lonAndLat.getY();
                this.myModel.ioFromFile.saveData("Text", a);
            }
        }

        public MainController()
        {
            this.myModel = new MyModel();
            this.myModel.ioFromSimulator.IoEvent += getLonAndLat;
            this.myModel.ioFromFile.IoEvent += getLonAndLat;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DisplayLocation(String ip, int port)
        {

            //this.myModel.ioFromSimulator.ConnectInOtherThread(ip, port);
            this.myModel.ioFromSimulator.getPoint();
            this.myModel.ioFromSimulator.getPoint();
            this.myModel.ioFromSimulator.getPoint();
            return View();
        }

        public ActionResult displayAnimation(String ip, int port, int freq)
        {
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
    }
}