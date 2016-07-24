using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ZOmicronInfo
{
    public static class Log
    {
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
            Task.Run(() =>
            {
                //Debug.WriteLine("Async Logging Begins");
                LogMessage tmp = null;
                while (true)
                {
                    //lock (Messages)
                    {
                        if (!Messages.IsEmpty)
                        {
                            for (int i = 0; i < (Messages.Count > 100 ? 100 : Messages.Count); i++)
                            {
                                if (Messages.TryDequeue(out tmp))
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
                    Thread.Sleep(100);
                }
            });
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
