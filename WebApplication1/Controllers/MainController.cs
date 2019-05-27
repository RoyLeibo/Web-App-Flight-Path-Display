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
            }
        }

        public MainController()
        {
            this.myModel = new MyModel();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult displayLocation(String ip, int port)
        {
            this.myModel.connectionRequest(ip, port);
            this.myModel.ioFromSimulator.IoEvent += getLonAndLat;
            return View();
        }

        public ActionResult displayAnimation(String ip, int port, int freq)
        {

        }

        public ActionResult save(String ip, int port, int freq, int sec, String fileName)
        {

        }

        public ActionResult displayPath(String fileName, int freq)
        {

        }

        public void getLonAndLat()
        {
            this.LonAndLat = this.myModel.ioFromSimulator.LonAndLat;
        }
    }
}