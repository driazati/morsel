using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Morsel
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public void setNote(string note)
        {
            this.lastMidiID.Invoke((MethodInvoker)delegate
           {
               this.lastMidiID.Text = note;
           });
        }

        public void addChar(string c)
        {
            this.outputChars.Invoke((MethodInvoker)delegate
            {
                this.outputChars.Text += c;
            });
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            MidiListener.MIDI_NOTE_ID = UInt64.Parse(this.textBox1.Text);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.textBox1.Text = MidiListener.MIDI_NOTE_ID.ToString();
            this.trackBar2.Value = (int)MidiListener.DASH_THRESHOLD_MS;
            this.label3.Text = MidiListener.DASH_THRESHOLD_MS.ToString() + " (ms)";
            setNote("meow");
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            MidiListener.DASH_THRESHOLD_MS = (UInt64) this.trackBar2.Value;
            this.label3.Text = MidiListener.DASH_THRESHOLD_MS.ToString() + " (ms)";
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
    }
}
