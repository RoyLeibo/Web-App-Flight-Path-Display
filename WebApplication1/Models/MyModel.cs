using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
namespace WebApplication1.Models
{
    public class MyModel
    {

        private String ip { get; set; }
        private int port { get; set; }
        public IOFromSimulator ioFromSimulator { get; set; }
        public IOFromFile ioFromFile { get; set; }

        /*
         * This function is the constructor.
         **/
        public MyModel()
        {
            this.ioFromFile = IOFromFile.Instance;
            this.ioFromSimulator = IOFromSimulator.Instance;
        }

        /*
         * This function close the connection with the simulator.
         **/
        public void CloseConnection()
        {
            this.ioFromSimulator.client.Close();
        }
    }
}