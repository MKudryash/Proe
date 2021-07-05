using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class CrtPth
    {
        string s = "";
        public int max, maxind;
        public List<List<Rbt>> itog = new List<List<Rbt>>();
        public List<Rbt> pyt;
        public List<Rbt> dan = Flrd();
        public void Way()
        {
            pyt = dan.FindAll(x => x.point1 == dan[Minel(dan)].point1);
            foreach (Rbt rb in pyt)
            {
                Mv(dan, rb);
                itog.Add(RtPrs(dan, s));
                s = "";
            }
        }
        public void kri()
        {
            Way();
            maxind = 0;
            max = itog[0][0].length;
            for (int i = 0; i < pyt.Count; i++)
            {
                if (FnlMv(itog[i]) >= max)
                {
                    max = FnlMv(itog[i]);
                    maxind = i;
                }
            }
            vivod();

        }
        public void vivod()
        {
            foreach (Rbt rb in itog[maxind])
            {
                string s = (rb.point1 + " - " + rb.point2);
                Debug.WriteLine(s);
            }
            Debug.WriteLine(max);
            Console.WriteLine("Сохранить данные в файл?(1/да - 0/нет)");
            int c = Convert.ToInt32(Console.ReadLine());
            if (c == 1)
            {
                using (StreamWriter sr = new StreamWriter("Критический Путь.txt"))
                {
                    foreach (Rbt rb in itog[maxind])
                    {
                        sr.WriteLine(rb.point1 + " - " + rb.point2);
                    }
                    sr.WriteLine(max);

                }
            }
            else
            {
                System.Environment.Exit(0);

            }
        }

        public struct Rbt
        {//Точки, длина
            public int point1;
            public int point2;
            public int length;
            //Не используется
            public bool Equals(Rbt obj)
            {
                if (this.point1 == obj.point1 && this.point2 == obj.point2 && this.length == obj.length) return true;
                else return false;
            }
            //Запись пути
            public override string ToString()
            {
                return point1.ToString() + " - " + point2.ToString() + " " + length.ToString();
            }
        }
        //Поиск начала
        public int Minel(List<Rbt> ls)
        {
            int min = ls[0].point1, minind = 0;
            foreach (Rbt rb in ls)
            {
                if (rb.point1 <= min)
                {
                    min = rb.point1;
                    minind = ls.IndexOf(rb);
                }
            }
            return minind;
        }
        //Определение конечной точки
        int Maxel(List<Rbt> ls)
        {
            int min = ls[0].point2, maxind = 0;
            foreach (Rbt rb in ls)
            {
                if (rb.point2 >= min)
                {
                    min = rb.point1;
                    maxind = ls.IndexOf(rb);
                }
            }
            return maxind;
        }
        int Mv(List<Rbt> ls, Rbt minel)
        {
            int ret = 0;
            Rbt rb = ls.Find(x => x.point1 == minel.point1 && x.point2 == minel.point2);
            s += rb.point1.ToString() + "-" + rb.point2.ToString();
            if (rb.point2 == ls[Maxel(ls)].point2)
            {
                s += ";";
                return rb.length;
            }
            else
            {
                for (int i = 0; i < ls.Count; i++)
                {
                    if (ls[i].point1 == rb.point2)
                    {
                        s += ",";
                        ret = Mv(ls, ls[i]) + rb.length;
                    }
                }
            }
            return ret;
        }

        //Чтение
        public static List<Rbt> Flrd()
        {
            Console.WriteLine("Считать данные из файла?(1/да - 0/нет)");
            int c = Convert.ToInt32(Console.ReadLine());
            List<Rbt> ret = new List<Rbt>();
            if (c == 1)
            {

                try
                {
                    using (StreamReader sr = new StreamReader("Vhod.csv"))
                    {
                        while (sr.EndOfStream != true)
                        {
                            string[] str1 = sr.ReadLine().Split(';');
                            string[] str2 = str1[0].Split('-');
                            ret.Add(new Rbt { point1 = Convert.ToInt32(str2[0]), point2 = Convert.ToInt32(str2[1]), length = Convert.ToInt32(str1[1]) });
                        }
                    }
                    return ret;
                }
                catch
                {
                    Console.WriteLine("Вызвано исключение!");
                    Console.ReadKey();
                    System.Environment.Exit(1);
                    return ret;
                }
            }
            else
            {
                System.Environment.Exit(0);
                return ret;
            }

        }
        //Строка парсится в массив и массивы доставляются до начала ветвления
        public List<Rbt> RtPrs(List<Rbt> ls, string s)
        {
            List<List<Rbt>> ret = new List<List<Rbt>>();
            string[] str1 = s.Split(';');
            foreach (string st1 in str1)
            {
                if (st1 != "")
                {
                    ret.Add(new List<Rbt>());
                    string[] str2 = st1.Split(',');
                    foreach (string st2 in str2)
                    {
                        if (st2 != "")
                        {
                            string[] str3 = st2.Split('-');
                            ret[ret.Count - 1].Add(ls.Find(x => x.point1 == Convert.ToInt32(str3[0]) && x.point2 == Convert.ToInt32(str3[1])));
                        }
                    }
                }
            }
            for (int i = 0; i < ret.Count; i++)
            {
                if (i > 0)
                {
                    if (ret[i][0].point1 != ret[i][ret[i].Count - 1].point2)
                    {
                        ret[i].InsertRange(0, ret[i - 1].FindAll(x => ret[i - 1].IndexOf(x) <= ret[i - 1].FindIndex(y => y.point2 == ret[i][0].point1)));
                    }
                }
            }
            int max = ret[0][0].length, maxind = 0;
            for (int i = 0; i < ret.Count; i++)
            {
                if (FnlMv(ret[i]) >= max)
                {
                    max = FnlMv(ret[i]);
                    maxind = i;
                }
            }
            return ret[maxind];
        }
        //Подсчет длины пути
        int FnlMv(List<Rbt> ls)
        {
            int ret = 0;
            foreach (Rbt rb in ls)
            {
                ret += rb.length;
            }
            return ret;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Debug.Listeners.Add(new TextWriterTraceListener(Console.Out));
            Debug.Listeners.Add(new TextWriterTraceListener(File.CreateText("Отладка.txt")));
            CrtPth crt = new CrtPth();
            crt.kri();

        }
    }
}
