using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    static class MidiListener
    {
        public static bool isPressed;
        public static UInt64 timer = 0;
        public static UInt64 DASH_THRESHOLD_MS = 75;
        public static UInt64 MIDI_NOTE_ID = 64;
        public static Form1 form;
        static Dictionary<string, string> code = new Dictionary<string, string>() {
            { "..-.", "f" },
            { "-..-", "x" },
            { ".--.", "p" },
            { "-", "t" },
            { "..---", "2" },
            { "....-", "4" },
            { "-----", "0" },
            { "--...", "7" },
            { "...-", "v" },
            { "-.-.", "c" },
            { ".", "e" },
            { ".---", "j" },
            { "---", "o" },
            { "-.-", "k" },
            { "----.", "9" },
            { "..", "i" },
            { ".-..", "l" },
            { ".....", "5" },
            { "...--", "3" },
            { "-.--", "y" },
            { "-....", "6" },
            { ".--", "w" },
            { "....", "h" },
            { "-.", "n" },
            { ".-.", "r" },
            { "-...", "b" },
            { "---..", "8" },
            { "--..", "z" },
            { "-..", "d" },
            { "--.-", "q" },
            { "--.", "g" },
            { "--", "m" },
            { "..-", "u" },
            { ".-", "a" },
            { "...", "s" },
            { ".----", "1" },
        };



        public static void Listen()
        {
            EnumerateMidiInputDevices().Wait();

            bool lastPressed = false;

            UInt64 msPressed = 0;
            UInt64 msUnpressed = 0;
            string buffer = "";

            while (true)
            {
                timer += 1;

                if (isPressed)
                {
                    msPressed += 1;
                }
                else
                {
                    msUnpressed += 1;
                }


                if (lastPressed != isPressed)
                {
                    if (!isPressed)
                    {
                        if (msPressed < DASH_THRESHOLD_MS)
                        {
                            buffer += ".";
                        }
                        else
                        {
                            buffer += "-";
                        }
                    }

                    msPressed = 0;
                    msUnpressed = 0;
                }

                if (msUnpressed >= DASH_THRESHOLD_MS * 2 && buffer != "")
                {
                    Console.WriteLine("Writing " + buffer);
                    if (code.ContainsKey(buffer))
                    {
                        Console.WriteLine(code[buffer]);
                        SendKeys.SendWait(code[buffer]);
                        form.addChar(code[buffer]);
                    }
                    else
                    {
                        Console.WriteLine("??");
                    }
                    buffer = "";
                }

                lastPressed = isPressed;


                Thread.Sleep(1);
            }
        }


        public static void MessageHandler(MidiInPort sender, MidiMessageReceivedEventArgs args)
        {
            var dataReader = Windows.Storage.Streams.DataReader.FromBuffer(args.Message.RawData);
            byte garbage = dataReader.ReadByte();
            byte noteId = dataReader.ReadByte();
            byte velocity = dataReader.ReadByte();
            //form.Text = noteId.ToString();
            //form.lastMidiID.Text = noteId.ToString();
            form.setNote(noteId.ToString());
            if (noteId == MIDI_NOTE_ID)
            {
                if (velocity == 0)
                {
                    isPressed = false;
                }
                else
                {
                    isPressed = true;
                }
            }
        }

        private static async Task EnumerateMidiInputDevices()
        {
            // Find all input MIDI devices
            string midiInputQueryString = MidiInPort.GetDeviceSelector();
            DeviceInformationCollection midiInputDevices = await DeviceInformation.FindAllAsync(midiInputQueryString);


            // Return if no external devices are connected
            if (midiInputDevices.Count == 0)
            {
                Console.WriteLine("No devices");
                return;
            }

            // Else, add each connected input device to the list
            foreach (DeviceInformation deviceInfo in midiInputDevices)
            {
                MidiInPort x = await MidiInPort.FromIdAsync(deviceInfo.Id);
                x.MessageReceived += MessageHandler;
                Console.WriteLine("Got device " + deviceInfo.Name);
            }
        }
    }
}
