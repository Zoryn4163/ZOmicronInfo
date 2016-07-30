using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using Newtonsoft.Json;

namespace ZOmicronInfo
{
    public static class Extensions
    {
        public static T[] SubArray<T>(this T[] arr, int index, int length)
        {
            T[] ret = new T[length];
            Array.Copy(arr, index, ret, 0, length);
            return ret;
        }

        public static string ToSingular<T>(this IEnumerable<T> ienum, string split = ", ")
        {
            return string.Join(split, ienum);
        }

        public static T[] CastArrayConvert<T>(this IEnumerable ienum) where T : class
        {
            if (typeof (T) == typeof (LearnedMove))
            {
                List<LearnedMove> ret = new List<LearnedMove>();
                List<string> s = ienum.Cast<String>().ToList();
                LearnedMove cur = null;

                for (int i = 0; i < s.Count / 2; i++)
                {
                    if ((i + 2) % 2 == 0)
                    {
                        //level
                        cur = new LearnedMove();
                        cur.LevelLearned = s[i].AsInt();
                    }
                    else
                    {
                        //move
                        cur.Move = s[i];
                        ret.Add(cur);
                        cur = null;
                    }
                }

                return ret.ToArray() as T[];
            }

            if (typeof (T) == typeof (PokeMove))
            {
                return (from object v in ienum select new PokeMove() {Move = v.ToString()}).ToArray() as T[];
            }

            if (typeof (T) == typeof (PokeEvo))
            {
                List<PokeEvo> ret = new List<PokeEvo>();
                List<string> s = ienum.Cast<String>().ToList();
                PokeEvo cur = new PokeEvo();

                for (int i = 0; i < s.Count; i++)
                {
                    int sw = (i + 3) % 3;

                    if (sw == 0)
                    {
                        cur = new PokeEvo();
                        cur.EvoPoke = s[i];

                        //Console.ForegroundColor = ConsoleColor.Blue;
                        //Console.ForegroundColor = ConsoleColor.Black;

                    }
                    else if (sw == 1)
                    {
                        cur.EvoMethod = s[i];
                        if (s[i].ToLower().Contains("happiness"))
                        {
                            ret.Add(cur);
                            cur = null;
                        }
                    }
                    else if (sw == 2)
                    {
                        cur.EvoData = s[i];
                        ret.Add(cur);
                        cur = null;
                    }
                    else
                    {

                        cur = null;
                    }
                }

                return ret.ToArray() as T[];
            }

            return (from object v in ienum select v as T).ToArray();
        }

        public static T[] CastArrayPull<T>(this IEnumerable ienum)
        {
            return ienum.OfType<T>().ToArray();
        }

        public static Int32[] CastArrayInt(this IEnumerable ienum)
        {
            List<Int32> ret = new List<Int32>();
            int o = 0;
            foreach (var v in ienum)
            {
                if (Int32.TryParse(v.ToString(), out o))
                {
                    ret.Add(o);
                }
            }
            return ret.ToArray();
        }

        public static Int32 AsInt(this string s)
        {
            int ret = 0;
            if (Int32.TryParse(s, out ret))
            {
                return ret;
            }
            return 0;
        }

        public static float AsFloat(this string s)
        {
            float ret = 0;
            if (float.TryParse(s, out ret))
            {
                return ret;
            }
            return 0;
        }

        public static T PullKeyValue<T>(this string[] arr, string key) where T : class
        {
            object ret = "NIL";

            if (arr.Any(x => x.StartsWith(key + "=")))
            {
                ret = arr.First(x => x.StartsWith(key + "=")).Split('=')[1];
            }

            if (typeof(T) == typeof(PokeType))
            {
                ret = new PokeType(ret.ToString());
            }

            if (typeof(T) == typeof(LearnedMove[]))
            {
                //Log.AsyncLine(ret.ToString() + " - "+ "\n");

                List<LearnedMove> mv = new List<LearnedMove>();
                List<string> s = ret.ToString().Split(',').ToList();
                LearnedMove cur = null;

                //Log.AsyncLine(s.ToSingular() + "\n");

                for (int i = 0; i < s.Count; i++)
                {
                    if ((i + 2) % 2 == 0)
                    {
                        //level
                        cur = new LearnedMove();
                        cur.LevelLearned = s[i].AsInt();
                    }
                    else
                    {
                        //move
                        cur.Move = s[i];
                        mv.Add(cur);
                        cur = null;
                    }
                }

                //Log.AsyncLine(mv.ToSingular() + "\n\n\n");

                ret = mv.ToArray() as T;
            }

            if (typeof(T) == typeof(PokeMove[]))
            {
                return (from object v in ret.ToString().Split(',') select new PokeMove() { Move = v.ToString() }).ToArray() as T;
            }

            if (typeof(T) == typeof(PokeEvo[]))
            {
                List<PokeEvo> mv = new List<PokeEvo>();
                List<string> s = ret.ToString().Split(new []{","}, StringSplitOptions.RemoveEmptyEntries).ToList();
                PokeEvo cur = new PokeEvo();

                int offset = 0;

                for (int i = 0; i < s.Count; i++)
                {
                    int ii = i + offset;
                    int sw = (ii + 3) % 3;

                    //Log.AsyncLine(("i: " + sw + " - cn: " + (cur == null) + " - s: " + s[i]);

                    if (sw == 0)
                    {
                        cur = new PokeEvo();
                        cur.EvoPoke = s[i];
                    }
                    else if (sw == 1)
                    {
                        if (cur == null)
                            continue;

                        cur.EvoMethod = s[i];
                        if (s[i].ToLower().Contains("happiness"))
                        {
                            mv.Add(cur);
                            cur = null;
                            offset++;
                        }
                    }
                    else if (sw == 2)
                    {
                        if (cur == null)
                            continue;

                        cur.EvoData = s[i];
                        mv.Add(cur);
                        cur = null;
                    }
                    else
                    {
                        cur = null;
                    }
                }

                return mv.ToArray() as T;
            }


            return ret as T;
        }

        public static string Serialize(this object o)
        {
            return JsonConvert.SerializeObject(o, Formatting.Indented);
        }

        public static BitmapImage ConvertBitmap(this Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                return bitmapImage;
            }
        }
    }
}