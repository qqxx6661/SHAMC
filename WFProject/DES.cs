using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO;

namespace WFProject
{
    public partial class frDES : Form
    {
        public frDES()
        {
            InitializeComponent();
        }

        //加载项
        private void frDES_Load(object sender, EventArgs e)
        {
            int x, m, g, n, row_id,V;
            x = 4;
            m = 4;
            g = 2;
            n = 35;
            row_id = 1;
            V = 2;
            EncryptDecrypt ed = new EncryptDecrypt();
            int Vk = ed.GetVk(m,x,row_id,g,n);
            int VkInverse = ed.GetVkInverse(Vk, n);
            int Ve = ed.GetVe(V,Vk,n);
            //textBox1.Text = x.ToString() + " " + (2 * x) % 35 + " " +ed.GetVe(2,8,35);
            textBox1.Text = Vk + " " + VkInverse + " " + Ve + " ";
        }

        //测试加法运算
        private void btnAdd_Click(object sender, EventArgs e)
        {
            ColumnOperand ColumnA = new ColumnOperand();
            ColumnOperand ColumnB = new ColumnOperand();
            ColumnOperand ColumnS = new ColumnOperand();
            ConnectToMysql connectToMySQL = new ConnectToMysql();
            string sql = "select column_key_m from secret_column where table_name='test' and column_name='A'";
            ColumnA.m = connectToMySQL.ReturnFirstData(sql);
            sql = "select column_key_x from secret_column where table_name='test' and column_name='A'";
            ColumnA.x = connectToMySQL.ReturnFirstData(sql);
            sql = "select column_key_m from secret_column where table_name='test' and column_name='B'";
            ColumnB.m = connectToMySQL.ReturnFirstData(sql);
            sql = "select column_key_x from secret_column where table_name='test' and column_name='B'";
            ColumnB.x = connectToMySQL.ReturnFirstData(sql);
            sql = "select column_key_m from secret_column where table_name='test' and column_name='S'";
            ColumnS.m = connectToMySQL.ReturnFirstData(sql);
            sql = "select column_key_x from secret_column where table_name='test' and column_name='S'";
            ColumnS.x = connectToMySQL.ReturnFirstData(sql);

            //sql = "select * from test";
            //DataTable dataTable = new DataTable();
            //dataTable = connectToMySQL.ReturnServerData(sql);
            //Operators operators = new Operators();
            //int[] Ce = new int[dataTable.Rows.Count];
            //Ce = operators.OperatorAdd(ColumnA, ColumnB, ColumnS, dataTable);
            //for (int i = 0; i < Ce.Length; i++)
            //{
            //    textBox2.Text = textBox2.Text + " " + Ce[i].ToString();
            //}

            //使用Mysql测试加法
            sql = "select n from global_info";
            int n = connectToMySQL.ReturnFirstData(sql);
            ColumnOperand ColumnC = new ColumnOperand();
            EncryptDecrypt encryptDecrypt = new EncryptDecrypt();
            ColumnC.x = 4;//encryptDecrypt.GetX(n);
            ColumnC.m = 4;//encryptDecrypt.GetM(n);
            int XsInverse = encryptDecrypt.GetVkInverse(ColumnS.x, n);
            int p1 = XsInverse * Math.Abs(ColumnC.x - ColumnA.x);
            int q1 = (ColumnA.m * Convert.ToInt32(Math.Pow(ColumnS.m, p1)) * encryptDecrypt.GetVkInverse(ColumnC.m, n)) % n;
            int p2 = XsInverse * Math.Abs(ColumnC.x - ColumnB.x);
            int q2 = (ColumnB.m * Convert.ToInt32(Math.Pow(ColumnS.m, p2)) * encryptDecrypt.GetVkInverse(ColumnC.m, n)) % n;
            //textBox2.Text = p1 + " " + q1 + " " + p2 + " " + q2;
            sql = "call add_cal_ce(" + p1 + "," + q1 + "," + p2 + "," + q2 + "," + n + ")";
            connectToMySQL.InsertServerData(sql);

            //解密
            sql = "select * from operator_result";
            DataTable dt = new DataTable();
            dt = connectToMySQL.ReturnServerData(sql);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int Vk = encryptDecrypt.GetVk(ColumnC.m, ColumnC.x, Convert.ToInt32(dt.Rows[i][0]), 2, n);
                int V = encryptDecrypt.GetV(Convert.ToInt32(dt.Rows[i][1]), Vk, n);
                textBox2.Text = textBox2.Text + " " + V + " ";
            }

        }

        //测试乘法运算
        private void btnMul_Click(object sender, EventArgs e)
        {
            //ColumnOperand ColumnA = new ColumnOperand();
            //ColumnOperand ColumnB = new ColumnOperand();
            //ConnectToMysql connectToMySQL = new ConnectToMysql();
            //string sql = "select column_key_m from secret_column where table_name='test' and column_name='A'";
            //ColumnA.m = connectToMySQL.ReturnFirstData(sql);
            //sql = "select column_key_x from secret_column where table_name='test' and column_name='A'";
            //ColumnA.x = connectToMySQL.ReturnFirstData(sql);
            //sql = "select column_key_m from secret_column where table_name='test' and column_name='B'";
            //ColumnB.m = connectToMySQL.ReturnFirstData(sql);
            //sql = "select column_key_x from secret_column where table_name='test' and column_name='B'";
            //ColumnB.x = connectToMySQL.ReturnFirstData(sql);
            //sql = "select * from test";
            //DataTable dataTable = new DataTable();
            //dataTable = connectToMySQL.ReturnServerData(sql);
            //Operators operators = new Operators();
            //int[] Ce = new int[dataTable.Rows.Count];
            //Ce = operators.OperatorMul(ColumnA, ColumnB, dataTable);
            //for (int i = 0; i < Ce.Length; i++)
            //{
            //    textBox2.Text = textBox2.Text + " " + Ce[i].ToString();
            //}

            //使用MYSQL
            //string sql1 = "SELECT mul_cal_m('test','A','B'),mul_cal_x('test','A','B')";
            //DataTable dt = new DataTable();
            //dt = connectToMySQL.ReturnClientData(sql1);
            //int Cm = Convert.ToInt32(dt.Rows[0][0]);
            //int Cx = Convert.ToInt32(dt.Rows[0][1]);

            string sql = "call mul_cal_ce()";
            ConnectToMysql connectToMySQL = new ConnectToMysql();
            string result = connectToMySQL.InsertServerData(sql);

        }
    }
}
