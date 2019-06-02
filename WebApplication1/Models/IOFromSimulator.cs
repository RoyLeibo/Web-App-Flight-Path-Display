﻿using System;
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

        public object MessageBox { get; private set; }

        /*
         * This function is called when the "Connect" button is clicked.
         * The function creates new thread for the server and start it.
         * Moreover, the function accepts a client (the Flight Simulator)
         * to it's server and connect as a client to the Flight Simulator.
         */
        public void Connect()
        {
            //    this.ConnectionThread = new Thread(new ThreadStart(ConnectInOtherThread(String ip, int port));
            //    this.ConnectionThread.Start();
            //    this.isWriteToFile = false;
        }

        public void ConnectInOtherThread(String ip, int port)
        {;
            this.client = new TcpClient(ip, port);
            this.stream = this.client.GetStream();
            System.Diagnostics.Debug.WriteLine("Simulator Just Accepted Me");
        }

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
            System.Diagnostics.Debug.WriteLine("Inside ReadData...");
            byte[] Buffer = new byte[1024];
            int recv;
            double value;
            String num = "";
            List<String> RequestsStringsList = new List<String>();
            RequestsStringsList.Add("get /position/longitude-deg\r\n");
            RequestsStringsList.Add("get /position/latitude-deg\r\n");

            if (this.isWriteToFile)
            {
                RequestsStringsList.Add("get /controls/flight/rudder\r\n");
                RequestsStringsList.Add("get /controls/engines/engine/throttle\r\n");
            }
            for (int i = 0; i < RequestsStringsList.Count; i++)
            {
                num = "";
                System.Diagnostics.Debug.WriteLine("Sent: " + RequestsStringsList[i]);
                Buffer = Encoding.ASCII.GetBytes(RequestsStringsList[i]);
                this.stream.Write(Buffer, 0, Buffer.Length);
                Buffer = new byte[1024];
                recv = this.stream.Read(Buffer, 0, Buffer.Length);
                String c = Encoding.ASCII.GetString(Buffer, 0, recv);
                int u = RequestsStringsList[i].Length - 2;

                for (int j = u; j < c.Length; j++)
                {
                    if (Char.IsDigit(c[j]) || c[j] == '.' || c[j] == '-')
                    {
                        num += c[j];
                    }
                }
                value = Convert.ToDouble(num);

                System.Diagnostics.Debug.WriteLine("Recieved: " + value);
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
            if (this.isWriteToFile)
            {
                double[] dataArray = { this.Lon, this.Lat, this.Throttle, this.Rudder };
                new IOFromFile().saveData("Flight1.txt", dataArray);
            }
        }

        public void setIpAndPort(String ip, int port)
        {
            this.ip = ip;
            this.port = port;
        }
    }
}