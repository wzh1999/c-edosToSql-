using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eDosToSqlservice
{
    public class DBHelper
    {
        public string data { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public string server { get; set; }
        public static SqlConnection conn;//new SqlConnection(string.Format("Data Source=.;Initial Catalog='{0}';User ID='{1}';Password='{2}'", data, UserName, PassWord));

        public DBHelper()
        {
            // string file = Environment.CurrentDirectory + "/sql连接配置.txt";
            string file = "D:/eDOSToSQL/sql连接配置.txt";
            if (!File.Exists(file))
            {
                //Console.WriteLine("目录不存在");
              //  WriteLog("目录不存在");
                return;
            }
            using (StreamReader reader = new StreamReader(file))
            {

                string[] line = File.ReadAllLines(file);
                string[] point = new string[line.Length];
                for (int i = 0; i < line.Length; i++)
                {
                    string[] item = line[i].Split(new char[] { '=' });
                    point[i] = item[1];
                }
                this.server = point[0];
                this.UserName = point[1];
                this.PassWord = point[2];
                this.data = point[3];

                //LogHelp.LogInfo("sql服务器:" + server + "数据库:" + data + "用户名:" + UserName + "密码:" + PassWord);
              //  WriteLog("sql服务器:" + server + "数据库:" + data + "用户名:" + UserName + "密码:" + PassWord);

            }
            // conn = new SqlConnection(string.Format("Data Source='{0}';Initial Catalog='{1}';User ID='{2}';Password='{3}'", server,data, UserName, PassWord));
            conn = new SqlConnection(string.Format("server='{0}';database='{1}';uid='{2}';pwd='{3}'", server, data, UserName, PassWord));
        }

        public static DataTable select(string sql)
        {
           // WriteLog("传过来的查询sql语句" + sql);
            SqlDataAdapter dap = new SqlDataAdapter(sql, conn);
           // WriteLog("数据库状态:" + conn.State);
            DataTable dt = new DataTable();
            dap.Fill(dt);
          //  WriteLog("是否有数据" + dt.Rows.Count);
            return dt;

        }
        public static bool UpData(string sql)
        {
           WriteLog("传过来的sql语句" + sql);
            conn.Open();
         //   WriteLog("数据库状态:" + conn.State);
            if (ConnectionState.Open == conn.State)
            {
               // WriteLog("数据库已打开");
            }
            else
            {
             //   WriteLog("数据库打开失败");
            }
            SqlCommand cmd = new SqlCommand(sql, conn);
            int rows = cmd.ExecuteNonQuery();
            conn.Close();
            return rows > 0;
        }
        //日志文件
        public static void WriteLog(string message)
        {
            try
            {
                string filed = @"D:\eDOSToSQL\calcLog.txt";
                //string filed = Environment.CurrentDirectory + "/" + DateTime.Now + "日志文件.txt";
                if (File.Exists(filed))
                {
                    using (StreamWriter sw = File.AppendText(filed))
                    {
                        sw.WriteLine(DateTime.Now.ToString() + " " + message);
                    }
                }
                else
                {
                    using (StreamWriter sw = File.CreateText(filed))
                    {
                        sw.WriteLine(DateTime.Now.ToString() + " " + message);
                    }
                }

            }
            catch (Exception)
            {

            }
        }
    }
}


