using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Brushes = System.Windows.Media.Brushes;

namespace ZOmicronInfo
{
    public static class Log
    {
        public static Thread LogThread { get; internal set; }
        public static ConcurrentQueue<LogMessage> Messages = new ConcurrentQueue<LogMessage>();

        public static void Async(object o = null, ConsoleColor c = ConsoleColor.Gray)
        {
            if (Program.Piping)
                return;

            if (o == null)
                o = " ";

            Messages.Enqueue(new LogMessage(o.ToString(), c));
        }

        public static void AsyncLine(object o = null, ConsoleColor c = ConsoleColor.Gray)
        {
            if (o == null)
                o = " ";

            Async(o + "\n", c);
        }

        internal static void StartWriting()
        {
            LogThread = new Thread(() =>
            {
                //Debug.WriteLine("Async Logging Begins");
                LogMessage tmp = null;
                while (true)
                {
                    //lock (Messages)
                    {
                        if (Messages?.Any() != null && !Messages.IsEmpty)
                        {
                            //for (int i = 0; i < (Messages.Count > 100 ? 100 : Messages.Count); i++)
                            {
                                if (Messages.TryDequeue(out tmp))
                                {
                                    if (Program.UsingGui)
                                    {
                                        if (Program.MainForm?.LogWindow?.txtbxLog != null)
                                        {
                                            //Program.MainForm.LogWindow.txtbxLog.Foreground = Brushes.Cyan;
                                            Program.MainForm.LogWindow.AppendText(tmp.Message);
                                        }
                                        else
                                        {
                                            Debug.WriteLine("DROPPED: " + tmp.Message);
                                        }
                                    }
                                    else
                                    {
                                        if (!Program.DisableColour)
                                            Console.ForegroundColor = tmp.Colour;

                                        Console.Write(tmp.Message);

                                        if (!Program.DisableColour)
                                            Console.ForegroundColor = ConsoleColor.Gray;
                                    }
                                }
                            }
                        }
                    }
                    Thread.Sleep(100);
                }
            });
            LogThread.Start();
        }

        internal static void StopWriting()
        {
            LogThread.Abort();
            Messages = new ConcurrentQueue<LogMessage>();
        }
    }

    public class LogMessage
    {
        public string Message { get; set; }
        public ConsoleColor Colour { get; set; }

        public LogMessage(string msg, ConsoleColor c)
        {
            Message = msg;
            Colour = c;
        }
    }
}
