using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eDosApiNet;
using System.Data;
using System.Threading;

namespace eDosToSqlservice
{
    public class Model
    {
        protected static uint key = 0;
        protected static string ServerIP;
        protected static ushort ServerPort;
        protected static List<string> JudgmentPoint = new List<string>();
        protected static List<string> UnitOne = new List<string>();
        protected static List<string> UnitTwo = new List<string>();
        protected static List<string> Corresponding = new List<string>();
        public void Process()
        {
            Thread thread = new Thread(run);
            thread.Start();

        }
        public void run()
        {
            //读取edos链接信息
            edosConnection();
            //初始化edos
            //  Initialization();
            int result = eDOSApi.InitCommunicationKey(ref key, false, "cache", 1024);
            if (result == 0)
            {
                // Console.WriteLine("edos数据库初始化成功");
                result = eDOSApi.ConnectToServer(key, ServerIP, ServerPort);
                if (result != 0)
                {
                    //  Console.WriteLine("edos数据库连接失败");
                    //WriteLog("edos数据库连接失败");
                }
                //Console.WriteLine("edos数据库连接成功");
                //WriteLog("edos数据库连接成功");
                WriteLog("edos数据库初始化成功");
            }
            DBHelper db = new DBHelper();
            while (true)
            {
                try
                {
                    JudgmentPoint.Clear();
                    UnitOne.Clear();
                    UnitTwo.Clear();
                    Corresponding.Clear();
                    // Console.WriteLine("服务启动时间:" + DateTime.Now);
                    WriteLog("服务启动时间:" + DateTime.Now);
                    ReadFile();
                   
                    //遍历1号机组的点名
                    string[] nameList1 = new string[UnitOne.Count];
                    int z = 0;
                    foreach (string item in UnitOne)
                    {
                        nameList1[z] = item;
                        WriteLog("获取1号机组所有的点名:" + nameList1[z]);
                        z++;

                    }

                    //遍历2号机组的点名
                    string[] nameList2 = new string[UnitTwo.Count];
                    int j = 0;
                    foreach (string item in UnitTwo)
                    {
                        nameList2[j] = item;
                        WriteLog("获取2号机组所有的点名:" + nameList2[j]);
                        j++;

                    }

                    //遍历判断点的点名
                    string[] nameList3 = new string[JudgmentPoint.Count];
                    int u = 0;
                    foreach (string item in JudgmentPoint)
                    {
                        nameList3[u] = item;
                        u++;
                    }

                    //读取edos数据库1号机组的点名的数据
                    eDOSApi.HISTORY_VALUE[] hv;
                    try
                    {
                        //读取1号机组实时数据
                        // WriteLog("1号机组的长度:" + nameList1.Length);
                        hv = new eDOSApi.HISTORY_VALUE[nameList1.Length];
                        //eDOSApi.HISTORY_VALUE h = new eDOSApi.HISTORY_VALUE();
                        //WriteLog("创建数组成功:" + hv.Length);
                        //WriteLog("Key的值" + key);
                        //WriteLog("nameList1[0]:" + nameList1[0]);
                        //WriteLog("nameList1[1]:" + nameList1[1]);
                        //WriteLog("hv的值" + hv);
                        //WriteLog("hv的长度" + hv.Length);
                        //int retul=eDOSApi.GetRTValue(key, nameList1[0], ref h);
                        int retul = eDOSApi.GetRTValueList(key, nameList1, hv, hv.Length);
                        WriteLog("查询1号机组的实时数据返回的值:" + retul);
                        if (retul == 0)
                        {
                            // Console.WriteLine("1号机组读取成功");
                            // WriteLog("1号机组读取成功");
                        }
                    }
                    catch (Exception ex)
                    {

                        Console.WriteLine(ex);
                        throw;
                    }


                    //读取2号机组点名的数据
                    eDOSApi.HISTORY_VALUE[] hv2 = new eDOSApi.HISTORY_VALUE[nameList2.Length];
                    int retul2 = eDOSApi.GetRTValueList(key, nameList2, hv2, hv2.Length);
                    WriteLog("查询2号机组的实时数据返回的值:" + retul2);
                    if (retul2 == 0)
                    {
                        //Console.WriteLine("2号机组读取成功");
                        // WriteLog("2号机组读取成功");
                    }


                    //读取判断点名的值
                    eDOSApi.HISTORY_VALUE[] hv3 = new eDOSApi.HISTORY_VALUE[nameList3.Length];
                    int retul3 = eDOSApi.GetRTValueList(key, nameList3, hv3, hv3.Length);
                    // WriteLog("判断点名的实时数据返回的值:" + retul3);

                    //获取1号机组、2号机组、判断点名的值
                    if (hv3[0].dValue >= 150 && hv3[1].dValue >= 150)
                    {
                        for (int i = 0; i < nameList1.Length; i++)
                        {
                            // Console.WriteLine("1号机组的值:" + hv[i].dValue);
                            // Console.WriteLine("2号机组的值:" + hv2[i].dValue);
                            // WriteLog("1号机组的值:" + hv[i].dValue);
                            // WriteLog("2号机组的值:" + hv2[i].dValue);
                            // WriteLog("判断点名的值:" + hv3[i].dValue);
                            //Console.WriteLine("1号和2号机组正常运行");
                            //  WriteLog("1号和2号机组正常运行");
                            NewMethod(Corresponding[i], Math.Round(hv[i].dValue, 1).ToString(), Math.Round(hv2[i].dValue, 1).ToString());

                        }
                    }
                    else if (hv3[0].dValue <= 150)
                    {

                        for (int c = 0; c < nameList1.Length; c++)
                        {
                            // Console.WriteLine("1号机组的值:" + hv[c].dValue);
                            // Console.WriteLine("2号机组的值:" + hv2[c].dValue);
                            // WriteLog("1号机组的值:" + hv[c].dValue);
                            // WriteLog("2号机组的值:" + hv2[c].dValue);
                            // WriteLog("判断点名的值:" + hv3[i].dValue);
                            // Console.WriteLine("1号机组正常运行");
                            // WriteLog("1号机组正常运行");
                            NewMethod(Corresponding[c], "--", Math.Round(hv2[c].dValue, 1).ToString());
                        }
                    }
                    else
                    {
                        for (int a = 0; a < nameList1.Length; a++)
                        {
                            // Console.WriteLine("1号机组的值:" + hv[a].dValue);
                            // Console.WriteLine("2号机组的值:" + hv2[a].dValue);
                            //WriteLog("1号机组的值:" + hv[a].dValue);
                            //WriteLog("2号机组的值:" + hv2[a].dValue);
                            // WriteLog("判断点名的值:" + hv3[i].dValue);
                            // Console.WriteLine("1号机组正常运行");
                            // WriteLog("1号机组正常运行");
                            //WriteLog("2号机组正常运行");
                            NewMethod(Corresponding[a], Math.Round(hv[a].dValue, 1).ToString(), "--");
                        }
                    }

                }
                catch (Exception)
                {
                    // WriteLog("异常处理错误信息" + ex);
                    throw;
                }
                Thread.Sleep(1000);
            }
        }

        //执行sql语句
        public void NewMethod(string name, string value, string value1)
        {
            string sql1 = string.Format("select * from TB_PUB_DP where OBJECT_CODE='{0}'", name);
            WriteLog("查询语句" + sql1);
            DataTable dt = new DataTable();
            dt = DBHelper.select(sql1);
            // DataTable dt = DBHelper.select(sql1);
            WriteLog("返回行数" + dt.Rows.Count);
            if (dt.Rows.Count > 0)
            {
                string sql2 = string.Format("update TB_PUB_DP set VALUE='{0}',VALUE1='{1}' where OBJECT_CODE='{2}'", value, value1, name);
                bool returs1 = DBHelper.UpData(sql2);
                if (returs1 == true)
                {
                    WriteLog("数据库更新成功");
                }
                else
                {
                    //  WriteLog("数据库不更新执行新增");
                }

            }
            else
            {
                string sql = string.Format("insert into TB_PUB_DP values('{0}','{1}','{2}')", name, value, value1);
                WriteLog("增加语句" + sql);
                //DBHelper db = new DBHelper();
                bool returs = DBHelper.UpData(sql);
                if (returs == true)
                {
                    //  WriteLog("增加语句成功");

                }
                else
                {
                    // WriteLog("增加语句失败");
                }
            }
        }

        //读取点表配置文件
        public void ReadFile()
        {

            //读取点表文件
            //  string file1 = Environment.CurrentDirectory + "/点表配置文件.txt";
            // WriteLog("开始读取点表配置文件");
            string file1 = "D:/eDOSToSQL/点表配置文件.txt";
            if (!File.Exists(file1))
            {
                //Console.WriteLine("点表配置文件.txt文件路径不正确");
                // WriteLog("点表配置文件.txt");
            }
            using (StreamReader reader1 = new StreamReader(file1))
            {
                string[] line1 = File.ReadAllLines(file1);
                //  WriteLog("文件读取完毕");
                string[] point1 = new string[line1.Length];
                //   WriteLog("读取文件的长度:" + line1.Length);
                //获取判断点名
                string[] judg = line1[0].Split(new char[] { ':' });
                string[] judgs = judg[1].Split(new char[] { ',' });
                for (int u = 0; u < judgs.Length; u++)
                {
                    JudgmentPoint.Add(judgs[u]);
                    //  WriteLog("1号机组判断点名:" + judgs[0]);
                    //  WriteLog("2号机组判断点名:" + judgs[1]);
                }




                for (int j = 0; j < line1.Length; j++)
                {
                    string[] item = line1[j].Split(new char[] { ':' });
                    point1[j] = item[1];

                }

                string[] item1 = point1[1].Split(new char[] { ';' });
                string[] item2 = point1[2].Split(new char[] { ';' });
                //读取1号机组所有的点名
                for (int k = 0; k < item1.Length; k++)
                {
                    //获取1号机组
                    string[] etms = item1[k].Split(new char[] { ',' });
                    //获取点名
                    UnitOne.Add(etms[0]);
                    // WriteLog("1号机组的点名:" + etms[0]);
                    //获取数据库名
                    Corresponding.Add(etms[1]);
                    // WriteLog("1号机组对应SQL列名:" + etms[1]);
                }
                //读取2号机组所有的点名
                for (int m = 0; m < item2.Length; m++)
                {
                    //获取2号机组
                    string[] etms1 = item2[m].Split(new char[] { ',' });
                    UnitTwo.Add(etms1[0]);
                    //  WriteLog("2号机组的点名:" + etms1[0]);
                    Corresponding.Add(etms1[1]);
                    //  WriteLog("2号机组对应SQL列名:" + etms1[1]);
                }
                for (int w = 0; w < Corresponding.Count; w++)  //外循环是循环的次数
                {
                    for (int z = Corresponding.Count - 1; z > w; z--)  //内循环是 外循环一次比较的次数
                    {

                        if (Corresponding[w] == Corresponding[z])
                        {
                            Corresponding.RemoveAt(z);
                        }

                    }
                }


            }
        }
        //读取edos数据库链接信息
        public void edosConnection()
        {
            //读取edos连接信息
            //string file = Environment.CurrentDirectory + "/eDOS数据库连接信息.txt";
            string file = "D:/eDOSToSQL/eDOS数据库连接信息.txt";
            if (!File.Exists(file))
            {
                // Console.WriteLine("eDOS数据库连接信息.txt文件路径不正确");
                // WriteLog("没有该配置文件，文件名为:eDOS数据库连接信息.txt");
            }
            using (StreamReader reader = new StreamReader(file))
            {
                string[] line = File.ReadAllLines(file);
                string[] point = new string[line.Length];
                for (int i = 0; i < line.Length; i++)
                {
                    string[] item = line[i].Split(new char[] { '=' });
                    point[i] = item[1];
                    //  WriteLog("item的值:" + item[1]);
                    // Console.WriteLine("item的值:" + item[1]);
                }
                //读取edos 连接ip地址和端口
                ServerIP = point[0];
                // Console.WriteLine("edos连接IP" + ServerIP);
                //LogHelp.LogInfo("成功读取IP为" + ServerIP + "端口号为" + ServerPort);
                ServerPort = Convert.ToUInt16(point[1]);
                // WriteLog("端口值：" + Convert.ToUInt16(point[1].ToString()));
                // Console.WriteLine("edos连接端口号" + ServerPort);

            }
        }

        //日志文件
        public void WriteLog(string message)
        {
            try
            {
                string filed = @"D:\eDOSToSQL\calcLog.txt";
                //string filed = Environment.CurrentDirectory + "/" + DateTime.Now + "日志文件.txt";
                if (File.Exists(filed))
                {
                    // Console.WriteLine(filed);
                    using (StreamWriter sw = File.AppendText(filed))
                    {
                        sw.WriteLine(DateTime.Now.ToString() + " " + message);
                    }
                }
                else
                {
                    // Console.WriteLine(filed);
                    using (StreamWriter sw = File.CreateText(filed))
                    {
                        sw.WriteLine(DateTime.Now.ToString() + " " + message);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("出现异常信息:" + ex);
            }
        }
    }
}

