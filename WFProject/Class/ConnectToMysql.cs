using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;

namespace WFProject
{
    class ConnectToMysql
    {
        string clientMysql = "Database=keystore;Data Source=127.0.0.1;User Id=root;Password=root;CharSet=utf8;port=3306";
        string serverMysql = "Database=serverdatabase;Data Source=127.0.0.1;User Id=root;Password=root;CharSet=utf8;port=3306";//IP设置为Server的IP
        string clientMysql2 = "Database=keystore2;Data Source=127.0.0.1;User Id=root;Password=root;CharSet=utf8;port=3306";
        string serverMysql2 = "Database=serverdatabase2;Data Source=127.0.0.1;User Id=root;Password=root;CharSet=utf8;port=3306";//IP设置为Server的IP


        public string CreateTables(string clientSql, string serverSql)//SP1创建表
        {
            try
            {
                int m, n;
                //在Client创建表
                MySqlConnection mysqlConnection = new MySqlConnection(clientMysql);//建立mysql数据库链接
                mysqlConnection.Open();
                MySqlCommand mySqlCommand = new MySqlCommand(clientSql, mysqlConnection);//建立执行命令语句对象
                m = Convert.ToInt32(mySqlCommand.ExecuteNonQuery());
                //在Server创建表
                mysqlConnection = new MySqlConnection(serverMysql);//建立mysql数据库链接
                mysqlConnection.Open();
                mySqlCommand = new MySqlCommand(serverSql, mysqlConnection);//建立执行命令语句对象
                n = Convert.ToInt32(mySqlCommand.ExecuteNonQuery());
                mysqlConnection.Close();
                if (m == 5)
                {
                    return "表创建成功(Server1)！";
                }
                else
                {
                    return "表创建失败(Server1)！";
                }
            }
            catch (Exception ex)
            {
                return "异常：" + ex.Message;
            }
        }

        public string CreateTables2(string clientSql, string serverSql)//SP2创建表
        {
            try
            {
                int m, n;
                //在Client创建表
                MySqlConnection mysqlConnection = new MySqlConnection(clientMysql2);//建立mysql数据库链接
                mysqlConnection.Open();
                MySqlCommand mySqlCommand = new MySqlCommand(clientSql, mysqlConnection);//建立执行命令语句对象
                m = Convert.ToInt32(mySqlCommand.ExecuteNonQuery());
                //在Server创建表
                mysqlConnection = new MySqlConnection(serverMysql2);//建立mysql数据库链接
                mysqlConnection.Open();
                mySqlCommand = new MySqlCommand(serverSql, mysqlConnection);//建立执行命令语句对象
                n = Convert.ToInt32(mySqlCommand.ExecuteNonQuery());
                mysqlConnection.Close();
                if (m == 5)
                {
                    return "表创建成功(Server2)！";
                }
                else
                {
                    return "表创建失败(Server2)！";
                }
            }
            catch (Exception ex)
            {
                return "异常：" + ex.Message;
            }
        }


        public DataTable ReturnClientData(string sql)//SP1返回查询结果表（Client）
        {
            MySqlConnection mysqlConnection = new MySqlConnection(clientMysql);
            mysqlConnection.Open();
            MySqlDataAdapter mysqlDataAdapter = new MySqlDataAdapter(sql, mysqlConnection);
            DataSet ds = new DataSet();
            mysqlDataAdapter.Fill(ds, "table");
            mysqlConnection.Close();
            return ds.Tables[0];
        }
        public DataTable ReturnClientData2(string sql)//SP2返回查询结果表（Client）
        {
            MySqlConnection mysqlConnection = new MySqlConnection(clientMysql2);
            mysqlConnection.Open();
            MySqlDataAdapter mysqlDataAdapter = new MySqlDataAdapter(sql, mysqlConnection);
            DataSet ds = new DataSet();
            mysqlDataAdapter.Fill(ds, "table");
            mysqlConnection.Close();
            return ds.Tables[0];
        }
        public DataTable ReturnServerData(string sql)//SP1返回查询结果表（Server）
        {
            MySqlConnection mysqlConnection = new MySqlConnection(serverMysql);
            mysqlConnection.Open();
            MySqlDataAdapter mysqlDataAdapter = new MySqlDataAdapter(sql, mysqlConnection);
            DataSet ds = new DataSet();
            mysqlDataAdapter.Fill(ds, "table");
            mysqlConnection.Close();
            return ds.Tables[0];
        }
        public DataTable ReturnServerData2(string sql)//SP1返回查询结果表（Server）
        {
            MySqlConnection mysqlConnection = new MySqlConnection(serverMysql2);
            mysqlConnection.Open();
            MySqlDataAdapter mysqlDataAdapter = new MySqlDataAdapter(sql, mysqlConnection);
            DataSet ds = new DataSet();
            mysqlDataAdapter.Fill(ds, "table");
            mysqlConnection.Close();
            return ds.Tables[0];
        }

        public string ServerCommand(string sql)//执行ServerCommand，如录入数据
        {
            int m;
            MySqlConnection mysqlConnection = new MySqlConnection(serverMysql);
            mysqlConnection.Open();
            try
            {
                MySqlCommand mySqlCommand = new MySqlCommand(sql, mysqlConnection);//建立执行命令语句对象
                m = Convert.ToInt32(mySqlCommand.ExecuteNonQuery());
                mysqlConnection.Close();
                if (m == 1)
                {
                    return "成功(Server1)！";
                }
                else
                {
                    return "失败(Server1)！";
                }
            }
            catch (Exception ex)
            {
                return "异常：" + ex.Message;
            }
        }

        public string ServerCommand2(string sql)//执行ServerCommand，如录入数据
        {
            int m;
            MySqlConnection mysqlConnection = new MySqlConnection(serverMysql2);
            mysqlConnection.Open();
            try
            {
                MySqlCommand mySqlCommand = new MySqlCommand(sql, mysqlConnection);//建立执行命令语句对象
                m = Convert.ToInt32(mySqlCommand.ExecuteNonQuery());
                mysqlConnection.Close();
                if (m == 1)
                {
                    return "成功(Server2)！";
                }
                else
                {
                    return "失败(Server2)！";
                }
            }
            catch (Exception ex)
            {
                return "异常：" + ex.Message;
            }
        }


        public string ClientCommand(string sql)//执行ClientCommand
        {
            int m;
            MySqlConnection mysqlConnection = new MySqlConnection(clientMysql);
            mysqlConnection.Open();
            try
            {
                MySqlCommand mySqlCommand = new MySqlCommand(sql, mysqlConnection);//建立执行命令语句对象
                m = Convert.ToInt32(mySqlCommand.ExecuteNonQuery());
                mysqlConnection.Close();
                if (m == 1)
                {
                    return "成功(Server1)！";
                }
                else
                {
                    return "失败(Server1)！";
                }
            }
            catch (Exception ex)
            {
                return "异常：" + ex.Message;
            }
        }

        public string ClientCommand2(string sql)//执行ClientCommand
        {
            int m;
            MySqlConnection mysqlConnection = new MySqlConnection(clientMysql2);
            mysqlConnection.Open();
            try
            {
                MySqlCommand mySqlCommand = new MySqlCommand(sql, mysqlConnection);//建立执行命令语句对象
                m = Convert.ToInt32(mySqlCommand.ExecuteNonQuery());
                mysqlConnection.Close();
                if (m == 1)
                {
                    return "成功(Server2)！";
                }
                else
                {
                    return "失败(Server2)！";
                }
            }
            catch (Exception ex)
            {
                return "异常：" + ex.Message;
            }
        }

        public int ReturnFirstData(string sql)//SP1返回Client数据库中的一个值
        {
            int m = -1;
            MySqlConnection mysqlConnection = new MySqlConnection(clientMysql);
            mysqlConnection.Open();
            try
            {
                MySqlCommand mySqlCommand = new MySqlCommand(sql, mysqlConnection);//建立执行命令语句对象
                m = Convert.ToInt32(mySqlCommand.ExecuteScalar());
                mysqlConnection.Close();
                return m;
            }
            catch (Exception)
            {
                return m;
            }
        }
        public int ReturnFirstData2(string sql)//SP2返回Client数据库中的一个值
        {
            int m = -1;
            MySqlConnection mysqlConnection = new MySqlConnection(clientMysql2);
            mysqlConnection.Open();
            try
            {
                MySqlCommand mySqlCommand = new MySqlCommand(sql, mysqlConnection);//建立执行命令语句对象
                m = Convert.ToInt32(mySqlCommand.ExecuteScalar());
                mysqlConnection.Close();
                return m;
            }
            catch (Exception)
            {
                return m;
            }
        }

        public int ReturnRowIdMax(string sql)//SP1返回Row_id的最大值，下一个数据是这个值+1
        {
            int m = 0;
            MySqlConnection mysqlConnection = new MySqlConnection(serverMysql);
            mysqlConnection.Open();
            try
            {
                MySqlCommand mySqlCommand = new MySqlCommand(sql, mysqlConnection);//建立执行命令语句对象
                m = Convert.ToInt32(mySqlCommand.ExecuteScalar());
                mysqlConnection.Close();
                return m;
            }
            catch (Exception)
            {
                return m;
            }
        }
        public int ReturnRowIdMax2(string sql)//SP2返回Row_id的最大值，下一个数据是这个值+1
        {
            int m = 0;
            MySqlConnection mysqlConnection = new MySqlConnection(serverMysql2);
            mysqlConnection.Open();
            try
            {
                MySqlCommand mySqlCommand = new MySqlCommand(sql, mysqlConnection);//建立执行命令语句对象
                m = Convert.ToInt32(mySqlCommand.ExecuteScalar());
                mysqlConnection.Close();
                return m;
            }
            catch (Exception)
            {
                return m;
            }
        }


    }
}
