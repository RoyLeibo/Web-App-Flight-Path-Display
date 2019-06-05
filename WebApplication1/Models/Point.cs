using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace WebApplication1.Models
{
    public struct Point
    {

        private double x;
        private double y;

        /*
        * This function is the constructor.
        **/
        public Point(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        /*
         * This function return the value of x
         **/
        public double getX() {
            return this.x;
        }

        /*
         * This function return the value of y
         **/
        public double getY()
        {
            return this.y;
        }

        /*
         * This function add the value of lon and lat to the xml
         **/
        public void ToXml(XmlWriter writer)
        {
            writer.WriteStartElement("Point");
            writer.WriteElementString("Lon", this.x.ToString());
            writer.WriteElementString("Lat", this.y.ToString());
            writer.WriteEndElement();
        }
    }
}