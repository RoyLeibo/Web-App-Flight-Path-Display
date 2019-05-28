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
        public Socket socket { get; set; }
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
            }
        }

        /*
         * This function is called when the "Connect" button is clicked.
         * The function creates new thread for the server and start it.
         * Moreover, the function accepts a client (the Flight Simulator)
         * to it's server and connect as a client to the Flight Simulator.
         */
        public void Connect()
        {
            this.ConnectionThread = new Thread(new ThreadStart(ConnectInOtherThread));
            this.ConnectionThread.Start();
        }

        public void ConnectInOtherThread()
        {
            this.server = new TcpListener(IPAddress.Parse(this.ip), this.port);
            this.server.Start();
            this.socket = this.server.AcceptSocket();
            this.ConnectionThread.Abort();
        }

        public void getPoint(TcpListener client)
        {
            //byte[] Buffer = new byte[1024];
            //// reads data from simulator into buffer in bytes
            //int recv = this.socket.Receive(Buffer);
            //// convert bytes recieved into a string
            //String StringData = Encoding.ASCII.GetString(Buffer, 0, recv);
            //this.ParseAndUpdate(StringData);
            this.LonAndLat = new Point(500, 0);
        }

        /*
         * This function is not stop running until the connection to the Flight 
         * Simulator is ending.
         * The function reads all the data from the simulator as define in
         * the Generic Small file and, call to another function to parse the data
         * and in the end create a point from the Lon and Lat data.
         */
        public void ReadDataFromSimulator(TcpListener client)
        {
            byte[] Buffer = new byte[1024];
            bool isEndOfLine;
            int recv = 0;
            int EndOfLine = 0;
            String StringData = "";
            String Result = "";
            String Remainder = "";
            while (true)
            {
                StringData = "";
                Array.Clear(Buffer, 0, Buffer.Length);
                // reads data from simulator into buffer in bytes
                recv = this.socket.Receive(Buffer);
                // convert bytes recieved into a string
                StringData = Encoding.ASCII.GetString(Buffer, 0, recv);
                Result = Remainder;
                isEndOfLine = true;
                // finding the closest end of line
                while (isEndOfLine)
                {
                    EndOfLine = StringData.IndexOf('\n');
                    if (EndOfLine != -1)
                    {
                        // An end of line is found, the function adds the remaining
                        // data into the Result string and take it of from StringData.
                        Result += StringData.Substring(0, EndOfLine);
                        StringData = StringData.Substring(EndOfLine + 1);
                        ParseAndUpdate(Result);
                        // clear Result and Buffer
                        Result = "";
                        Remainder = "";
                    }
                    else
                    {
                        // An end of line is not found, move the data to the remainder
                        // and start loop again
                        Remainder += StringData;
                        isEndOfLine = false;
                    }
                }
            }
        }

        /*
         * This function receives a string which contains all data recieved from
         * simulator in a single time and extracting the Lon and Lat properties. 
         */
        public void ParseAndUpdate(String StringData)
        {
            int StartOfLon = 0;
            int EndOfLon = StringData.IndexOf(',', StartOfLon);
            // Extract the Lon property from the data string by finding the closest
            // ',' to it from start.
            double Lon = Double.Parse(StringData.Substring(StartOfLon, EndOfLon - StartOfLon));
            int StartOfLat = EndOfLon + 1;
            int EndOfLat = StringData.IndexOf(',', StartOfLat);
            // Extract the Lat property from the data string by finding the closest
            // ',' to it after the Lon.
            double Lat = Double.Parse(StringData.Substring(StartOfLat, EndOfLat - StartOfLat));
            this.LonAndLat = new Point(Lat, Lon);
        }

        public void setIpAndPort(String ip, int port)
        {
            this.ip = ip;
            this.port = port;
            this.Connect();
        }
    }
}