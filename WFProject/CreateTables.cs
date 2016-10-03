using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace WFProject
{
    public partial class frCreateTables : Form
    {
        public frCreateTables()
        {
            InitializeComponent();
        }

        //创建表
        private void btnCreateTable_Click(object sender, EventArgs e)
        {
            Thread c2 = new Thread(CreateTable2);  // 创建新线程
            c2.Start();                       // 启动新线程

            //ClientProtocol协议
            string clientSql = "INSERT INTO secret_column VALUES('" + txtTableName.Text + "','row_id',15,13);INSERT INTO secret_column VALUES('" + txtTableName.Text + "','" + txtColumn1.Text + "',2,2);INSERT INTO secret_column VALUES('" + txtTableName.Text + "','" + txtColumn2.Text + "',1,3);INSERT INTO secret_column VALUES('" + txtTableName.Text + "','S',3,1);INSERT INTO secret_column VALUES('" + txtTableName.Text + "','R',2,3)";
            //ServerProtocol协议
            string serverSql = "CREATE TABLE IF NOT EXISTS " + txtTableName.Text + "(row_id INT(11) NOT NULL PRIMARY KEY," + txtColumn0.Text + " INT(11) NOT NULL AUTO_INCREMENT," + txtColumn1.Text + " INT(11) NOT NULL," + txtColumn2.Text + " INT(11) NOT NULL,S INT(11) NOT NULL,R INT(11) NOT NULL,KEY(" + txtColumn0.Text + "))";
            ConnectToMysql connectToMysql = new ConnectToMysql();
            string str = connectToMysql.CreateTables(clientSql, serverSql);
            MessageBox.Show(str);
        }
        private void CreateTable2()
        {
            //ClientProtocol协议
            string clientSql = "INSERT INTO secret_column VALUES('" + txtTableName.Text + "','row_id',15,13);INSERT INTO secret_column VALUES('" + txtTableName.Text + "','" + txtColumn1.Text + "',2,2);INSERT INTO secret_column VALUES('" + txtTableName.Text + "','" + txtColumn2.Text + "',1,3);INSERT INTO secret_column VALUES('" + txtTableName.Text + "','S',3,1);INSERT INTO secret_column VALUES('" + txtTableName.Text + "','R',2,3)";
            //ServerProtocol协议
            string serverSql = "CREATE TABLE IF NOT EXISTS " + txtTableName.Text + "(row_id INT(11) NOT NULL PRIMARY KEY," + txtColumn0.Text + " INT(11) NOT NULL AUTO_INCREMENT," + txtColumn1.Text + " INT(11) NOT NULL," + txtColumn2.Text + " INT(11) NOT NULL,S INT(11) NOT NULL,R INT(11) NOT NULL,KEY(" + txtColumn0.Text + "))";
            ConnectToMysql connectToMysql = new ConnectToMysql();
            string str = connectToMysql.CreateTables2(clientSql, serverSql);
            MessageBox.Show(str);
        }
    }
}
