using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        protected static List<string> UnitOne = new List<string>();
        protected static List<string> UnitTwo = new List<string>();
        protected static List<string> Corresponding = new List<string>();
        public  static void Main(string[] args)
        {

            //读取点表文件
            string file1 =Environment.CurrentDirectory + "/点表配置文件.txt";
            if (!File.Exists(file1))
            {
                Console.WriteLine("sql连接配置.txt");
            }
            using (StreamReader reader1 = new StreamReader(file1))
            {
                string[] line1 = File.ReadAllLines(file1);
                string[] point1 = new string[line1.Length];
                //获取判断点名
                 string JudgmentPoint = line1[0].Split(new char[] { ':' })[1];
                Console.WriteLine("JudgmentPoint:"+JudgmentPoint);
                for (int j = 0; j < line1.Length; j++)
                {
                    string[] item = line1[j].Split(new char[] { ':' });
                    point1[j] = item[1];
                    Console.WriteLine("point:"+point1);
                }

                string[] item1 = point1[1].Split(new char[] { ';' });
                string[] item2 = point1[2].Split(new char[] { ';' });
                Console.WriteLine(item1+":"+item2);
                for (int k = 0; k < item1.Length; k++)
                {
                    string[] etms = item1[k].Split(new char[] { ',' });
                    UnitOne.Add(etms[0]);
                    Console.WriteLine(etms[0]);
                    Corresponding.Add(etms[1]);
                    Console.WriteLine(etms[1]);
                    string[] etms1 = item2[k].Split(new char[] { ',' });
                    UnitTwo.Add(etms1[0]);
                    Console.WriteLine(etms1[0]);
                    Corresponding.Add(etms1[1]);
                    Console.WriteLine(etms1[1]);
                }
                for (int w = 0;w < Corresponding.Count; w++)  //外循环是循环的次数
                {
                    for (int z = Corresponding.Count - 1; z > w; z--)  //内循环是 外循环一次比较的次数
                    {

                        if (Corresponding[w] == Corresponding[z])
                        {
                            Corresponding.RemoveAt(z);
                        }

                    }
                }
                foreach (string item in Corresponding)
                {
                    Console.WriteLine("item"+ item);
                }
            }
        }
    }
}
