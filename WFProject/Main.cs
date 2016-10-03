using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace WFProject
{
    public partial class frMain : Form
    {
        public frMain()
        {
            InitializeComponent();
        }

        //转到创建表
        private void btn_Click(object sender, EventArgs e)
        {
            frCreateTables frcreatetables = new frCreateTables();
            frcreatetables.Show();
        }

        //录入数据
        private void btnInsert_Click(object sender, EventArgs e)
        {
            frInsertData frinsertdata = new frInsertData();
            frinsertdata.Show();
        }

        //DES算法
        private void btnDES_Click(object sender, EventArgs e)
        {
            frOperatorTest frdes = new frOperatorTest();
            frdes.Show();
        }

        //查询
        private void btnQuery_Click(object sender, EventArgs e)
        {
            frQuery frQuery = new frQuery();
            frQuery.Show();
        }

        //重置按钮
        private void btnRestart_Click(object sender, EventArgs e)
        {
            string sql = "DROP TABLE test";
            ConnectToMysql connectToMySQL = new ConnectToMysql();
            connectToMySQL.ServerCommand(sql);
            connectToMySQL.ServerCommand2(sql);
            sql = "DELETE FROM secret_column WHERE table_name = 'test'";
            connectToMySQL.ClientCommand(sql);
            connectToMySQL.ClientCommand2(sql);
            MessageBox.Show("重置成功(All Server)！");
        }
    }
}
