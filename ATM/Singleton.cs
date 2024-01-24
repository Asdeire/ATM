using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ATM.PincodeWindow;

namespace ATM
{
    public class Singleton
    {
        private static Singleton instance;
        public string cardNumber { get; set; }
        public string Balance { get; set; }
        public string connection { get; set; }
        private Singleton() { }

        public static Singleton Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Singleton();
                }
                return instance;
            }
        }
    }
}
