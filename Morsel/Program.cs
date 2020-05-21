using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

using Windows.Devices.Enumeration;
using Windows.Devices.Midi;
using System.Threading;


namespace Morsel
{



    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            MidiListener.form = new Form1();
            var t = new Thread(MidiListener.Listen);
            t.Start();
            Application.Run(MidiListener.form);
        }
    }
}
