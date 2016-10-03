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
    public partial class frQuery : Form
    {
        public frQuery()
        {
            InitializeComponent();
        }

        //查询
        private void btnQuery_Click(object sender, EventArgs e)
        {
            //判断textbox是否为空
            if (txtSelect.Text == "" || txtFrom.Text == "")
            {
                MessageBox.Show("SELECT和FROM语句不能为空");
            }
            else
            {
                string sql = "";
                if (txtWhere.Text == "")
                {
                    sql = "SELECT " + txtSelect.Text + " FROM " + txtFrom.Text + "";
                }
                else
                {
                    sql = "SELECT " + txtSelect.Text + " FROM " + txtFrom.Text + " WHERE " + txtWhere.Text + "";
                }
                ConnectToMysql connectToMySQL = new ConnectToMysql();
                DataTable dataTable = new DataTable();
                dataTable = connectToMySQL.ReturnServerData(sql);


                EncryptDecrypt encryptDecrypt = new EncryptDecrypt();
                long n = encryptDecrypt.GetN();
                long g = encryptDecrypt.GetG();
                long fn = encryptDecrypt.GetFn();

                //解密
                long m1, row_id, x1, m2, x2;
                sql = "select column_key_m from secret_column where table_name='" + txtFrom.Text + "' and column_name='" + dataTable.Columns[1].ColumnName + "'";
                m1 = connectToMySQL.ReturnFirstData(sql);
                sql = "select column_key_m from secret_column where table_name='" + txtFrom.Text + "' and column_name='" + dataTable.Columns[2].ColumnName + "'";
                m2 = connectToMySQL.ReturnFirstData(sql);
                sql = "select column_key_x from secret_column where table_name='" + txtFrom.Text + "' and column_name='" + dataTable.Columns[1].ColumnName + "'";
                x1 = connectToMySQL.ReturnFirstData(sql);
                sql = "select column_key_x from secret_column where table_name='" + txtFrom.Text + "' and column_name='" + dataTable.Columns[2].ColumnName + "'";
                x2 = connectToMySQL.ReturnFirstData(sql);

                for (int i = 0; i < dataTable.Rows.Count; i++)//解密
                {
                    row_id = Convert.ToInt64(dataTable.Rows[i][0]);
                    dataTable.Rows[i][1] = encryptDecrypt.GetV(Convert.ToInt64(dataTable.Rows[i][1]), encryptDecrypt.GetVk(m1, x1, row_id, g, n, fn), n);
                    dataTable.Rows[i][2] = encryptDecrypt.GetV(Convert.ToInt64(dataTable.Rows[i][2]), encryptDecrypt.GetVk(m2, x2, row_id, g, n, fn), n);
                }
                BindingSource bindingSource = new BindingSource();
                bindingSource.DataSource = dataTable;
                dgvResult.DataSource = bindingSource;
            }
        }
    }
}
