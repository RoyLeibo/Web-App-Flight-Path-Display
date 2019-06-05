using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Web;

namespace WebApplication1.Models
{
    public delegate void handler();
    public class IOFromSimulator
    {
        public event handler IoEvent;
        private String ip { get; set; }
        private int port { get; set; }
        public Thread ConnectionThread { get; set; }
        public TcpListener server { get; set; }
        public TcpClient client { get; set; }
        public NetworkStream stream { get; set; }
        public bool isWriteToFile { get; set; }
        public String FileName { get; set; }
        public IOFromFile ioFromFile {get; set;}
        public double Lon { get; set; }
        public double lat;
        public double Lat
        {
            get
            {
                return this.lat;
            }
            set
            {
                this.lat = value;
                this.LonAndLat = new Point(this.Lon, value);
            }
        }
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
                // notify the view model that this property is changed
                IoEvent?.Invoke();
                System.Diagnostics.Debug.WriteLine("({0}, {1})", this.lonAndLat.getX(), this.lonAndLat.getY());
            }
        }
        private double throttle;
        public double Throttle
        {
            get
            {
                return this.throttle;
            }
            set
            {
                System.Diagnostics.Debug.WriteLine("Throttle: {0}", this.rudder);
                this.throttle = value;
            }
        }
        private double rudder;
        public double Rudder
        {
            get
            {
                return this.rudder;
            }
            set
            {
                this.rudder = value;
                System.Diagnostics.Debug.WriteLine("Rudder: {0}", this.rudder);
            }
        }

        private static IOFromSimulator instance = null;
        /*
         * This function is the constructor.
         **/
        private IOFromSimulator()
        {
            this.ioFromFile = IOFromFile.Instance;
        }

        /*
         * This function return the some instance of IOFromSimulator 
         * when we do get
         **/
        public static IOFromSimulator Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new IOFromSimulator();
                }
                return instance;
            }
        }

        /*
         * This functiom connet to the simulator
         **/
        public void ConnectInOtherThread(String ip, int port)
        {
            if(ip == null && port == 0)
            {
                //take the previous ip and port
                this.client = new TcpClient(this.ip, this.port);
                this.stream = this.client.GetStream();
            }
            else
            {
                //set the ip and port that we get
                this.ip = ip;
                this.port = port;
                this.client = new TcpClient(ip, port);
                this.stream = this.client.GetStream();
            } 
        }

        /*
         * This function call ReadDataFromSimulator
         * to read new point from the simulator.
         **/
        public void getPoint()
        {
            this.ReadDataFromSimulator();
        }

        /*
         * This function is not stop running until the connection to the Flight 
         * Simulator is ending.
         * The function reads all the data from the simulator as define in
         * the Generic Small file and, call to another function to parse the data
         * and in the end create a point from the Lon and Lat data.
         */
        public void ReadDataFromSimulator()
        {
            byte[] Buffer = new byte[1024];
            int recv;
            double value;
            String num = "";
            List<String> RequestsStringsList = new List<String>();
            RequestsStringsList.Add("get /position/longitude-deg\r\n");
            RequestsStringsList.Add("get /position/latitude-deg\r\n");
            //if we will write to the file
            // read from the simulator also the value of the rudder and throttle
            if (this.isWriteToFile)
            {
                RequestsStringsList.Add("get /controls/flight/rudder\r\n");
                RequestsStringsList.Add("get /controls/engines/engine/throttle\r\n");
            }
            // move on the list and read from the simulator the values
            for (int i = 0; i < RequestsStringsList.Count; i++)
            {
                num = "";
                Buffer = Encoding.ASCII.GetBytes(RequestsStringsList[i]);
                this.stream.Write(Buffer, 0, Buffer.Length);
                Buffer = new byte[1024];
                recv = this.stream.Read(Buffer, 0, Buffer.Length);
                String c = Encoding.ASCII.GetString(Buffer, 0, recv);
                int u = RequestsStringsList[i].Length - 2;
                //take only the number that we get
                for (int j = u; j < c.Length; j++)
                {
                    if (Char.IsDigit(c[j]) || c[j] == '.' || c[j] == '-')
                    {
                        num += c[j];
                    }
                }
                value = Convert.ToDouble(num);
                //set the value to the right proprtie
                switch (i)
                {
                    case 0:
                        this.Lon = value;
                        break;
                    case 1:
                        this.Lat = value;
                        break;
                    case 2:
                        this.Rudder = value;
                        break;
                    case 3:
                        this.Throttle = value;
                        break;
                }
            }
            //if the when to write to the file
            if (this.isWriteToFile)
            {
                //add all the value to array
                double[] dataArray = { this.Lon, this.Lat, this.Throttle, this.Rudder };
                //save the values
                this.ioFromFile.saveData(this.FileName, dataArray);
            }
        }

        /*
         * This function set new ip and port
         **/
        public void setIpAndPort(String ip, int port)
        {
            this.ip = ip;
            this.port = port;
        }
    }
}