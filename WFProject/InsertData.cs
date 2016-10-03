using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WFProject
{
    public partial class frInsertData : Form
    {
        public frInsertData()
        {
            InitializeComponent();
        }
        long rowidMax = 0;//用于计算下一个row_id

        //加载CMB内容，表名的集合
        private void frInsertData_Load(object sender, EventArgs e)
        {
            string sql = "select distinct table_name from secret_column";
            ConnectToMysql connectToMysql = new ConnectToMysql();
            DataTable dataTable = new DataTable();
            dataTable = connectToMysql.ReturnClientData(sql);
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                cmbTable.Items.Add(dataTable.Rows[i][0]);
            }
            if (cmbTable.Items.Count != 0)
            {
                cmbTable.SelectedIndex = 0;
            }
        }

        private void cmbTable_SelectedIndexChanged(object sender, EventArgs e)//CMB发生变化后
        {
            EncryptDecrypt encryptDecrypt = new EncryptDecrypt();
            long n = encryptDecrypt.GetN();
            long g = encryptDecrypt.GetG();
            long fn = encryptDecrypt.GetFn();

            string sql = "select * from " + cmbTable.SelectedItem.ToString() + "";
            ConnectToMysql connectToMySQL = new ConnectToMysql();
            DataTable dataTable1 = connectToMySQL.ReturnServerData(sql);
            //先要解密row_id才能解密其他列
            ColumnOperand ColumnRowid = new ColumnOperand();
            sql = "select column_key_m from secret_column where table_name='" + cmbTable.SelectedItem + "' and column_name='row_id'";
            ColumnRowid.m = connectToMySQL.ReturnFirstData(sql);
            sql = "select column_key_x from secret_column where table_name='" + cmbTable.SelectedItem + "' and column_name='row_id'";
            ColumnRowid.x = connectToMySQL.ReturnFirstData(sql);
            long K = ColumnRowid.m;
            for (int i = 0; i < dataTable1.Rows.Count; i++)
            {
                //先要得到ki
                long k = encryptDecrypt.GetVk(ColumnRowid.m, ColumnRowid.x, 1, g, n, fn);
                dataTable1.Rows[i][0] = encryptDecrypt.DecryptRowid(Convert.ToInt64(dataTable1.Rows[i][0]), k, K, n);
            }

            DataTable dataTable = dataTable1.DefaultView.ToTable(false, new string[] { dataTable1.Columns[1].ColumnName, dataTable1.Columns[2].ColumnName, dataTable1.Columns[3].ColumnName });//选择表中的某一列输出

            lblColumn1.Text = dataTable.Columns[1].ColumnName;
            lblColumn2.Text = dataTable.Columns[2].ColumnName;

            //A列
            ColumnOperand ColumnA = new ColumnOperand();
            sql = "select column_key_m from secret_column where table_name='" + cmbTable.SelectedItem + "' and column_name='" + lblColumn1.Text + "'";
            ColumnA.m = connectToMySQL.ReturnFirstData(sql);
            sql = "select column_key_x from secret_column where table_name='" + cmbTable.SelectedItem + "' and column_name='" + lblColumn1.Text + "'";
            ColumnA.x = connectToMySQL.ReturnFirstData(sql);

            //B列
            ColumnOperand ColumnB = new ColumnOperand();
            sql = "select column_key_m from secret_column where table_name='" + cmbTable.SelectedItem + "' and column_name='" + lblColumn2.Text + "'";
            ColumnB.m = connectToMySQL.ReturnFirstData(sql);
            sql = "select column_key_x from secret_column where table_name='" + cmbTable.SelectedItem + "' and column_name='" + lblColumn2.Text + "'";
            ColumnB.x = connectToMySQL.ReturnFirstData(sql);

            //解密
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                long row_id = Convert.ToInt64(dataTable1.Rows[i][0]);
                long a = row_id;
                dataTable.Rows[i][1] = encryptDecrypt.GetV(Convert.ToInt64(dataTable.Rows[i][1]), encryptDecrypt.GetVk(ColumnA.m, ColumnA.x, row_id, g, n, fn), n);
                dataTable.Rows[i][2] = encryptDecrypt.GetV(Convert.ToInt64(dataTable.Rows[i][2]), encryptDecrypt.GetVk(ColumnB.m, ColumnB.x, row_id, g, n, fn), n);
                //dataTable.Rows[i][3] = encryptDecrypt.GetV(Convert.ToInt32(dataTable.Rows[i][3]), encryptDecrypt.GetVk(m3, x3, row_id, g, n, fn), n);
                //dataTable.Rows[i][4] = encryptDecrypt.GetV(Convert.ToInt32(dataTable.Rows[i][4]), encryptDecrypt.GetVk(ColumnR.m, ColumnR.x, row_id, g, n, fn), n);
            }
            BindingSource bindingSource = new BindingSource();
            bindingSource.DataSource = dataTable;
            dgvData.DataSource = bindingSource;
        }




        //录入数据
        private void btnInsert_Click(object sender, EventArgs e)
        {
            //如果用户没有选择表
            if (cmbTable.SelectedIndex == -1)
            {
                MessageBox.Show("请选择表！");
            }
            else
            {

                //如果用户没有输入列的值，那么值默认是0
                if (txtColumn1.Text == "")
                {
                    txtColumn1.Text = "0";
                }
                if (txtColumn2.Text == "")
                {
                    txtColumn2.Text = "0";
                }

                EncryptDecrypt encryptDecrypt = new EncryptDecrypt();
                long n = encryptDecrypt.GetN();
                long g = encryptDecrypt.GetG();
                long fn = encryptDecrypt.GetFn();

                ConnectToMysql connectToMySQL = new ConnectToMysql();
                string sql = "";
                //Row_id列

                //先要解密row_id，才能得到最大值
                sql = "select row_id from " + cmbTable.SelectedItem + "";
                DataTable dataTable1 = new DataTable();
                dataTable1 = connectToMySQL.ReturnServerData(sql);
                ColumnOperand ColumnRowid = new ColumnOperand();
                sql = "select column_key_m from secret_column where table_name='" + cmbTable.SelectedItem + "' and column_name='row_id'";
                ColumnRowid.m = connectToMySQL.ReturnFirstData(sql);//K
                sql = "select column_key_x from secret_column where table_name='" + cmbTable.SelectedItem + "' and column_name='row_id'";
                ColumnRowid.x = connectToMySQL.ReturnFirstData(sql);
                long K = ColumnRowid.m;
                for (int i = 0; i < dataTable1.Rows.Count; i++)
                {
                    //先要得到ki
                    long k = encryptDecrypt.GetVk(ColumnRowid.m, ColumnRowid.x, 1, g, n, fn);
                    dataTable1.Rows[i][0] = encryptDecrypt.DecryptRowid(Convert.ToInt64(dataTable1.Rows[i][0]), k, K, n);//解密row_id
                    if (Convert.ToInt64(dataTable1.Rows[i][0]) > rowidMax)//计算row_id最大值
                    {
                        rowidMax = Convert.ToInt64(dataTable1.Rows[i][0]);
                    }
                }
                //现在要添加的rowid = rowidMax+1

                //A列
                ColumnOperand ColumnA = new ColumnOperand();
                sql = "select column_key_m from secret_column where table_name='" + cmbTable.SelectedItem + "' and column_name='" + lblColumn1.Text + "'";
                ColumnA.m = connectToMySQL.ReturnFirstData(sql);
                sql = "select column_key_x from secret_column where table_name='" + cmbTable.SelectedItem + "' and column_name='" + lblColumn1.Text + "'";
                ColumnA.x = connectToMySQL.ReturnFirstData(sql);

                //B列
                ColumnOperand ColumnB = new ColumnOperand();
                sql = "select column_key_m from secret_column where table_name='" + cmbTable.SelectedItem + "' and column_name='" + lblColumn2.Text + "'";
                ColumnB.m = connectToMySQL.ReturnFirstData(sql);
                sql = "select column_key_x from secret_column where table_name='" + cmbTable.SelectedItem + "' and column_name='" + lblColumn2.Text + "'";
                ColumnB.x = connectToMySQL.ReturnFirstData(sql);

                ////id列
                //int id;
                //sql = "select MAX(id) from " + cmbTable.SelectedItem + "";
                //id = connectToMySQL.ReturnRowIdMax(sql) + 1;

                //S列
                ColumnOperand ColumnS = new ColumnOperand();
                sql = "select column_key_m from secret_column where table_name='" + cmbTable.SelectedItem + "' and column_name='S'";
                ColumnS.m = connectToMySQL.ReturnFirstData(sql);
                sql = "select column_key_x from secret_column where table_name='" + cmbTable.SelectedItem + "' and column_name='S'";
                ColumnS.x = connectToMySQL.ReturnFirstData(sql);

                //R列
                ColumnOperand ColumnR = new ColumnOperand();
                sql = "select column_key_m from secret_column where table_name='" + cmbTable.SelectedItem + "' and column_name='R'";
                ColumnR.m = connectToMySQL.ReturnFirstData(sql);
                sql = "select column_key_x from secret_column where table_name='" + cmbTable.SelectedItem + "' and column_name='R'";
                ColumnR.x = connectToMySQL.ReturnFirstData(sql);

                //加密
                //对于Row_id
                long row_id = rowidMax + 1;
                long Ve0 = encryptDecrypt.EncryptRowid(K, row_id, encryptDecrypt.GetVk(ColumnRowid.m, ColumnRowid.x, 1, g, n, fn), n);
                //对于A
                long Ve1 = encryptDecrypt.GetVe(Convert.ToInt64(txtColumn1.Text), encryptDecrypt.GetVk(ColumnA.m, ColumnA.x, row_id, g, n, fn), n);
                //对于B
                long Ve2 = encryptDecrypt.GetVe(Convert.ToInt64(txtColumn2.Text), encryptDecrypt.GetVk(ColumnB.m, ColumnB.x, row_id, g, n, fn), n);
                //对于S
                long Ve3 = encryptDecrypt.GetVe(1, encryptDecrypt.GetVk(ColumnS.m, ColumnS.x, row_id, g, n, fn), n);
                //对于R
                Random random = new Random();
                int nn = Convert.ToInt32(n);
                int V = random.Next(1, nn);//////////////////////////////////
                long Ve4 = encryptDecrypt.GetVe(V, encryptDecrypt.GetVk(ColumnR.m, ColumnR.x, row_id, g, n, fn), n);

                //存储
                sql = "INSERT INTO " + cmbTable.SelectedItem.ToString() + "(row_id," + lblColumn1.Text + "," + lblColumn2.Text + ",S,R) VALUES('" + Ve0 + "','" + Ve1 + "','" + Ve2 + "','" + Ve3 + "','" + Ve4 + "')";
                string result = connectToMySQL.ServerCommand(sql);
                if (result == "失败(Server1)！")
                {
                    MessageBox.Show(result);
                }
                else
                {
                    string result2 = connectToMySQL.ServerCommand2(sql);
                    if (result2 == "失败(Server2)！")
                    {
                        MessageBox.Show(result2);
                    }
                    else
                    {
                        //刷新DataGridView
                        cmbTable_SelectedIndexChanged(sender, e);
                        txtColumn1.Text = "";
                        txtColumn2.Text = "";
                    }
                }

            }
        }

        private void btnRamdomInsert_Click(object sender, EventArgs e)//双SP单线程随机200组
        {
            DateTime beforDT, afterDT, beforDTall, afterDTall;
            TimeSpan ts,tscal,all;//ts：录入sql每次时间，tscal：客户端每次生成时间
            TimeSpan tssql = TimeSpan.Zero;//tsall录入sql总时间
            beforDTall = System.DateTime.Now;

            for (int rantimes = 1; rantimes <= 200; rantimes++)
            {
                DateTime beforDTcal = System.DateTime.Now;//计算执行时间
                Random ran=new Random();
                int text1=ran.Next(1,11);
                int text2=ran.Next(1,text1+1);
               
                EncryptDecrypt encryptDecrypt = new EncryptDecrypt();
                long n = encryptDecrypt.GetN();
                long g = encryptDecrypt.GetG();
                long fn = encryptDecrypt.GetFn();

                ConnectToMysql connectToMySQL = new ConnectToMysql();
                string sql = "";
                //Row_id列

                //先要解密row_id，才能得到最大值
                sql = "select row_id from " + cmbTable.SelectedItem + "";
                DataTable dataTable1 = new DataTable();
                dataTable1 = connectToMySQL.ReturnServerData(sql);
                ColumnOperand ColumnRowid = new ColumnOperand();
                sql = "select column_key_m from secret_column where table_name='" + cmbTable.SelectedItem + "' and column_name='row_id'";
                ColumnRowid.m = connectToMySQL.ReturnFirstData(sql);//K
                sql = "select column_key_x from secret_column where table_name='" + cmbTable.SelectedItem + "' and column_name='row_id'";
                ColumnRowid.x = connectToMySQL.ReturnFirstData(sql);
                long K = ColumnRowid.m;
                for (int i = 0; i < dataTable1.Rows.Count; i++)
                {
                    //先要得到ki
                    long k = encryptDecrypt.GetVk(ColumnRowid.m, ColumnRowid.x, 1, g, n, fn);
                    dataTable1.Rows[i][0] = encryptDecrypt.DecryptRowid(Convert.ToInt64(dataTable1.Rows[i][0]), k, K, n);//解密row_id
                    if (Convert.ToInt64(dataTable1.Rows[i][0]) > rowidMax)//计算row_id最大值
                    {
                        rowidMax = Convert.ToInt64(dataTable1.Rows[i][0]);
                    }
                }
                //现在要添加的rowid = rowidMax+1

                //A列
                ColumnOperand ColumnA = new ColumnOperand();
                sql = "select column_key_m from secret_column where table_name='" + cmbTable.SelectedItem + "' and column_name='" + lblColumn1.Text + "'";
                ColumnA.m = connectToMySQL.ReturnFirstData(sql);
                sql = "select column_key_x from secret_column where table_name='" + cmbTable.SelectedItem + "' and column_name='" + lblColumn1.Text + "'";
                ColumnA.x = connectToMySQL.ReturnFirstData(sql);

                //B列
                ColumnOperand ColumnB = new ColumnOperand();
                sql = "select column_key_m from secret_column where table_name='" + cmbTable.SelectedItem + "' and column_name='" + lblColumn2.Text + "'";
                ColumnB.m = connectToMySQL.ReturnFirstData(sql);
                sql = "select column_key_x from secret_column where table_name='" + cmbTable.SelectedItem + "' and column_name='" + lblColumn2.Text + "'";
                ColumnB.x = connectToMySQL.ReturnFirstData(sql);

                ////id列
                //int id;
                //sql = "select MAX(id) from " + cmbTable.SelectedItem + "";
                //id = connectToMySQL.ReturnRowIdMax(sql) + 1;

                //S列
                ColumnOperand ColumnS = new ColumnOperand();
                sql = "select column_key_m from secret_column where table_name='" + cmbTable.SelectedItem + "' and column_name='S'";
                ColumnS.m = connectToMySQL.ReturnFirstData(sql);
                sql = "select column_key_x from secret_column where table_name='" + cmbTable.SelectedItem + "' and column_name='S'";
                ColumnS.x = connectToMySQL.ReturnFirstData(sql);

                //R列
                ColumnOperand ColumnR = new ColumnOperand();
                sql = "select column_key_m from secret_column where table_name='" + cmbTable.SelectedItem + "' and column_name='R'";
                ColumnR.m = connectToMySQL.ReturnFirstData(sql);
                sql = "select column_key_x from secret_column where table_name='" + cmbTable.SelectedItem + "' and column_name='R'";
                ColumnR.x = connectToMySQL.ReturnFirstData(sql);

                //加密
                //对于Row_id
                long row_id = rowidMax + 1;
                long Ve0 = encryptDecrypt.EncryptRowid(K, row_id, encryptDecrypt.GetVk(ColumnRowid.m, ColumnRowid.x, 1, g, n, fn), n);
                //对于A
                long Ve1 = encryptDecrypt.GetVe(Convert.ToInt64(text1), encryptDecrypt.GetVk(ColumnA.m, ColumnA.x, row_id, g, n, fn), n);
                //对于B
                long Ve2 = encryptDecrypt.GetVe(Convert.ToInt64(text2), encryptDecrypt.GetVk(ColumnB.m, ColumnB.x, row_id, g, n, fn), n);
                //对于S
                long Ve3 = encryptDecrypt.GetVe(1, encryptDecrypt.GetVk(ColumnS.m, ColumnS.x, row_id, g, n, fn), n);
                //对于R
                Random random = new Random();
                int nn = Convert.ToInt32(n);
                int V = random.Next(1, nn);//////////////////////////////////
                long Ve4 = encryptDecrypt.GetVe(V, encryptDecrypt.GetVk(ColumnR.m, ColumnR.x, row_id, g, n, fn), n);

                DateTime afterDTcal = System.DateTime.Now;
                tscal = afterDTcal.Subtract(beforDTcal);
                Console.WriteLine("-------------------生成1组数据运算总共花费{0}ms.----------------------", tscal.TotalMilliseconds);  


                beforDT = System.DateTime.Now;//计算执行时间
                //存储
                sql = "INSERT INTO " + cmbTable.SelectedItem.ToString() + "(row_id," + lblColumn1.Text + "," + lblColumn2.Text + ",S,R) VALUES('" + Ve0 + "','" + Ve1 + "','" + Ve2 + "','" + Ve3 + "','" + Ve4 + "')";
                string result = connectToMySQL.ServerCommand(sql);
                if (result == "失败(Server1)！")
                {
                    MessageBox.Show(result);
                }
                else
                {
                    string result2 = connectToMySQL.ServerCommand2(sql);
                    if (result2 == "失败(Server2)！")
                    {
                        MessageBox.Show(result2);
                    }
                }
                afterDT = System.DateTime.Now;
                ts = afterDT.Subtract(beforDT);
                tssql += ts;
                Console.WriteLine("-------------------录入1组数据总共花费{0}ms.----------------------", ts.TotalMilliseconds);  
            }
            Console.WriteLine("-------------------录入100组数据总共花费{0}ms.----------------------", tssql.TotalMilliseconds);
            afterDTall = System.DateTime.Now;
            all = afterDTall.Subtract(beforDTall);
            Console.WriteLine("-------------------生成过程总共花费{0}ms.----------------------", all.TotalMilliseconds);  
            //全部输入完后才刷新DataGridView
            cmbTable_SelectedIndexChanged(sender, e);
            txtColumn1.Text = "";
            txtColumn2.Text = "";

        }

        private void button1_Click(object sender, EventArgs e)//单SP随机200组
        {
            DateTime beforDT, afterDT, beforDTall, afterDTall;
            TimeSpan ts, tscal, all;//ts：录入sql每次时间，tscal：客户端每次生成时间
            TimeSpan tssql = TimeSpan.Zero;//tsall录入sql总时间
            beforDTall = System.DateTime.Now;

            for (int rantimes = 1; rantimes <= 200; rantimes++)
            {
                DateTime beforDTcal = System.DateTime.Now;//计算执行时间
                Random ran = new Random();
                int text1 = ran.Next(1, 11);
                int text2 = ran.Next(1, text1 + 1);

                EncryptDecrypt encryptDecrypt = new EncryptDecrypt();
                long n = encryptDecrypt.GetN();
                long g = encryptDecrypt.GetG();
                long fn = encryptDecrypt.GetFn();

                ConnectToMysql connectToMySQL = new ConnectToMysql();
                string sql = "";
                //Row_id列

                //先要解密row_id，才能得到最大值
                sql = "select row_id from " + cmbTable.SelectedItem + "";
                DataTable dataTable1 = new DataTable();
                dataTable1 = connectToMySQL.ReturnServerData(sql);
                ColumnOperand ColumnRowid = new ColumnOperand();
                sql = "select column_key_m from secret_column where table_name='" + cmbTable.SelectedItem + "' and column_name='row_id'";
                ColumnRowid.m = connectToMySQL.ReturnFirstData(sql);//K
                sql = "select column_key_x from secret_column where table_name='" + cmbTable.SelectedItem + "' and column_name='row_id'";
                ColumnRowid.x = connectToMySQL.ReturnFirstData(sql);
                long K = ColumnRowid.m;
                for (int i = 0; i < dataTable1.Rows.Count; i++)
                {
                    //先要得到ki
                    long k = encryptDecrypt.GetVk(ColumnRowid.m, ColumnRowid.x, 1, g, n, fn);
                    dataTable1.Rows[i][0] = encryptDecrypt.DecryptRowid(Convert.ToInt64(dataTable1.Rows[i][0]), k, K, n);//解密row_id
                    if (Convert.ToInt64(dataTable1.Rows[i][0]) > rowidMax)//计算row_id最大值
                    {
                        rowidMax = Convert.ToInt64(dataTable1.Rows[i][0]);
                    }
                }
                //现在要添加的rowid = rowidMax+1

                //A列
                ColumnOperand ColumnA = new ColumnOperand();
                sql = "select column_key_m from secret_column where table_name='" + cmbTable.SelectedItem + "' and column_name='" + lblColumn1.Text + "'";
                ColumnA.m = connectToMySQL.ReturnFirstData(sql);
                sql = "select column_key_x from secret_column where table_name='" + cmbTable.SelectedItem + "' and column_name='" + lblColumn1.Text + "'";
                ColumnA.x = connectToMySQL.ReturnFirstData(sql);

                //B列
                ColumnOperand ColumnB = new ColumnOperand();
                sql = "select column_key_m from secret_column where table_name='" + cmbTable.SelectedItem + "' and column_name='" + lblColumn2.Text + "'";
                ColumnB.m = connectToMySQL.ReturnFirstData(sql);
                sql = "select column_key_x from secret_column where table_name='" + cmbTable.SelectedItem + "' and column_name='" + lblColumn2.Text + "'";
                ColumnB.x = connectToMySQL.ReturnFirstData(sql);

                ////id列
                //int id;
                //sql = "select MAX(id) from " + cmbTable.SelectedItem + "";
                //id = connectToMySQL.ReturnRowIdMax(sql) + 1;

                //S列
                ColumnOperand ColumnS = new ColumnOperand();
                sql = "select column_key_m from secret_column where table_name='" + cmbTable.SelectedItem + "' and column_name='S'";
                ColumnS.m = connectToMySQL.ReturnFirstData(sql);
                sql = "select column_key_x from secret_column where table_name='" + cmbTable.SelectedItem + "' and column_name='S'";
                ColumnS.x = connectToMySQL.ReturnFirstData(sql);

                //R列
                ColumnOperand ColumnR = new ColumnOperand();
                sql = "select column_key_m from secret_column where table_name='" + cmbTable.SelectedItem + "' and column_name='R'";
                ColumnR.m = connectToMySQL.ReturnFirstData(sql);
                sql = "select column_key_x from secret_column where table_name='" + cmbTable.SelectedItem + "' and column_name='R'";
                ColumnR.x = connectToMySQL.ReturnFirstData(sql);

                //加密
                //对于Row_id
                long row_id = rowidMax + 1;
                long Ve0 = encryptDecrypt.EncryptRowid(K, row_id, encryptDecrypt.GetVk(ColumnRowid.m, ColumnRowid.x, 1, g, n, fn), n);
                //对于A
                long Ve1 = encryptDecrypt.GetVe(Convert.ToInt64(text1), encryptDecrypt.GetVk(ColumnA.m, ColumnA.x, row_id, g, n, fn), n);
                //对于B
                long Ve2 = encryptDecrypt.GetVe(Convert.ToInt64(text2), encryptDecrypt.GetVk(ColumnB.m, ColumnB.x, row_id, g, n, fn), n);
                //对于S
                long Ve3 = encryptDecrypt.GetVe(1, encryptDecrypt.GetVk(ColumnS.m, ColumnS.x, row_id, g, n, fn), n);
                //对于R
                Random random = new Random();
                int nn = Convert.ToInt32(n);
                int V = random.Next(1, nn);//////////////////////////////////
                long Ve4 = encryptDecrypt.GetVe(V, encryptDecrypt.GetVk(ColumnR.m, ColumnR.x, row_id, g, n, fn), n);

                DateTime afterDTcal = System.DateTime.Now;
                tscal = afterDTcal.Subtract(beforDTcal);
                Console.WriteLine("-------------------生成1组数据运算总共花费{0}ms.----------------------", tscal.TotalMilliseconds);


                beforDT = System.DateTime.Now;//计算执行时间
                //存储
                sql = "INSERT INTO " + cmbTable.SelectedItem.ToString() + "(row_id," + lblColumn1.Text + "," + lblColumn2.Text + ",S,R) VALUES('" + Ve0 + "','" + Ve1 + "','" + Ve2 + "','" + Ve3 + "','" + Ve4 + "')";
                string result = connectToMySQL.ServerCommand(sql);
                if (result == "失败(Server1)！")
                {
                    MessageBox.Show(result);
                }

                afterDT = System.DateTime.Now;
                ts = afterDT.Subtract(beforDT);
                tssql += ts;
                Console.WriteLine("-------------------录入1组数据总共花费{0}ms.----------------------", ts.TotalMilliseconds);
            }
            Console.WriteLine("-------------------录入100组数据总共花费{0}ms.----------------------", tssql.TotalMilliseconds);
            afterDTall = System.DateTime.Now;
            all = afterDTall.Subtract(beforDTall);
            Console.WriteLine("-------------------生成过程总共花费{0}ms.----------------------", all.TotalMilliseconds);
            //全部输入完后才刷新DataGridView
            cmbTable_SelectedIndexChanged(sender, e);
            txtColumn1.Text = "";
            txtColumn2.Text = "";

        }


        //textbox中只能输入正数
        private void txtColumn1_KeyPress(object sender, KeyPressEventArgs e)
        {
            //如果输入的不是数字键，也不是回车键、Backspace键，则取消该输入            
            if (!(Char.IsNumber(e.KeyChar)) && e.KeyChar != (char)13 && e.KeyChar != (char)8 && e.KeyChar != (char)45)
            {
                e.Handled = true;
            }
        }
        private void txtColumn2_KeyPress(object sender, KeyPressEventArgs e)
        {
            //如果输入的不是数字键，也不是回车键、Backspace键，则取消该输入            
            if (!(Char.IsNumber(e.KeyChar)) && e.KeyChar != (char)13 && e.KeyChar != (char)8 && e.KeyChar != (char)45)
            {
                e.Handled = true;
            }
        }

      

        
    }
}
