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

        public Point(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public double getX() {
            return this.x;
        }

        public double getY()
        {
            return this.y;
        }

        public void ToXml(XmlWriter writer)
        {
            writer.WriteStartElement("Point");
            writer.WriteElementString("Lon", this.x.ToString());
            writer.WriteElementString("Lat", this.y.ToString());
            writer.WriteEndElement();
        }
    }
}