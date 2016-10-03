using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

//实现各种运算符的类
namespace WFProject
{
    class ColumnOperand
    {
        public long m;
        public long x;
    }
    class Operators
    {
        public Operators()
        {
            //删除operator_result表中数据
            ConnectToMysql connectToMySQL = new ConnectToMysql();
            string sql = "delete from operator_result";
            connectToMySQL = new ConnectToMysql();
            connectToMySQL.ServerCommand(sql);
            connectToMySQL.ServerCommand2(sql);//在构造函数里删除SP2的结果表
        }

        //键更新
        public void KeyUpdate()
        { 
            
        }

        //加法运算
        public DataTable OperatorAdd()
        {
            ConnectToMysql connectToMySQL = new ConnectToMysql();
            connectToMySQL = new ConnectToMysql();
            EncryptDecrypt encryptDecrypt = new EncryptDecrypt();
            long n = encryptDecrypt.GetN();
            long g = encryptDecrypt.GetG();
            long fn = encryptDecrypt.GetFn();

            string sql = "";

            //执行ClientProtocol
            ColumnOperand ColumnC = new ColumnOperand();
            ColumnC.x = 4;//测试
            ColumnC.m = 4;
            //string sql = "select gen_ck_x()";
            //ColumnC.x = connectToMySQL.ReturnFirstData(sql);
            //sql = "select gen_ck_m()";
            //ColumnC.m = connectToMySQL.ReturnFirstData(sql);
            ColumnOperand ColumnS = new ColumnOperand();
            sql = "select column_key_x from secret_column where table_name='test' and column_name='S'";
            ColumnS.x = connectToMySQL.ReturnFirstData(sql);
            long XsInverse = encryptDecrypt.GetVkInverse(ColumnS.x, n);
            long McInverse = encryptDecrypt.GetVkInverse(ColumnC.m, n);
            sql = "select add_cal_p(" + ColumnC.x + "," + XsInverse + ",'A')";
            long p1 = connectToMySQL.ReturnFirstData(sql);
            sql = "select add_cal_q(" + p1 + "," + McInverse + ",'A')";
            long q1 = connectToMySQL.ReturnFirstData(sql);
            sql = "select add_cal_p(" + ColumnC.x + "," + XsInverse + ",'B')";
            long p2 = connectToMySQL.ReturnFirstData(sql);
            sql = "select add_cal_q(" + p2 + "," + McInverse + ",'B')";
            long q2 = connectToMySQL.ReturnFirstData(sql);

            //执行ServerProtocol
            sql = "call add_cal_ce(" + p1 + "," + q1 + "," + p2 + "," + q2 + "," + n + ")";
            connectToMySQL.ServerCommand(sql);

            //解密
            sql = "SELECT test.row_id,id,A,B,result FROM test,operator_result WHERE test.row_id = operator_result.row_id";
            DataTable dt = new DataTable();
            dt = connectToMySQL.ReturnServerData(sql);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //解密row_id
                ColumnOperand ColumnRowid = new ColumnOperand();
                sql = "select column_key_m from secret_column where table_name='test' and column_name='row_id'";
                ColumnRowid.m = connectToMySQL.ReturnFirstData(sql);
                sql = "select column_key_x from secret_column where table_name='test' and column_name='row_id'";
                ColumnRowid.x = connectToMySQL.ReturnFirstData(sql);
                long row_id = encryptDecrypt.DecryptRowid(Convert.ToInt64(dt.Rows[i][0]),encryptDecrypt.GetVk(ColumnRowid.m,ColumnRowid.x,1,g,n,fn),ColumnRowid.m,n);

                //解密A
                ColumnOperand ColumnA = new ColumnOperand();
                sql = "select column_key_m from secret_column where table_name='test' and column_name='A'";
                ColumnA.m = connectToMySQL.ReturnFirstData(sql);
                sql = "select column_key_x from secret_column where table_name='test' and column_name='A'";
                ColumnA.x = connectToMySQL.ReturnFirstData(sql);
                dt.Rows[i][2] = encryptDecrypt.GetV(Convert.ToInt64(dt.Rows[i][2]), encryptDecrypt.GetVk(ColumnA.m, ColumnA.x, row_id, g, n, fn), n);

                //解密B
                ColumnOperand ColumnB = new ColumnOperand();
                sql = "select column_key_m from secret_column where table_name='test' and column_name='B'";
                ColumnB.m = connectToMySQL.ReturnFirstData(sql);
                sql = "select column_key_x from secret_column where table_name='test' and column_name='B'";
                ColumnB.x = connectToMySQL.ReturnFirstData(sql);
                dt.Rows[i][3] = encryptDecrypt.GetV(Convert.ToInt64(dt.Rows[i][3]), encryptDecrypt.GetVk(ColumnB.m, ColumnB.x, row_id, g, n, fn), n);

                //解密result
                //Vk = encryptDecrypt.GetVk(ColumnC.m, ColumnC.x, row_id, g, n, fn);
                dt.Rows[i][4] = encryptDecrypt.GetV(Convert.ToInt64(dt.Rows[i][4]), encryptDecrypt.GetVk(ColumnC.m, ColumnC.x, row_id, g, n, fn), n);
            }
            DataTable dataTable = dt.DefaultView.ToTable(false, new string[] { "id", "A", "B", "result" });//选择表中的某一列输出
            return dataTable;
        }

        //乘法运算
        public DataTable OperatorMul()
        {
            EncryptDecrypt encryptDecrypt = new EncryptDecrypt();
            long n = encryptDecrypt.GetN();
            long g = encryptDecrypt.GetG();
            long fn = encryptDecrypt.GetFn();

            DateTime beforDT1 = System.DateTime.Now;//计算执行时间

            ConnectToMysql connectToMySQL = new ConnectToMysql();
            connectToMySQL = new ConnectToMysql();
            //执行ClientProtocol
            string sql = "SELECT mul_cal_m('test','A','B'),mul_cal_x('test','A','B')";
            DataTable dt1 = new DataTable();
            dt1 = connectToMySQL.ReturnClientData(sql);
            ColumnOperand ColumnC = new ColumnOperand();
            ColumnC.m = Convert.ToInt64(dt1.Rows[0][0]);
            ColumnC.x = Convert.ToInt64(dt1.Rows[0][1]);

             DateTime afterDT1 = System.DateTime.Now;
            TimeSpan ts1 = afterDT1.Subtract(beforDT1);
            Console.WriteLine("-------------------乘法运算DO总共花费{0}ms.----------------------", ts1.TotalMilliseconds);

            DateTime beforDT2 = System.DateTime.Now;//计算执行时间

            //执行ServerProtocol
            sql = "call mul_cal_ce('test','A','B'," + n + ")";
            string result = connectToMySQL.ServerCommand(sql);

            DateTime afterDT2 = System.DateTime.Now;
            TimeSpan ts2= afterDT2.Subtract(beforDT2);
            Console.WriteLine("-------------------乘法运算CDB总共花费{0}ms.----------------------", ts2.TotalMilliseconds);
            //解密
            sql = "SELECT test.row_id,id,A,B,result FROM test,operator_result WHERE test.row_id = operator_result.row_id";
            DataTable dt = new DataTable();
            dt = connectToMySQL.ReturnServerData(sql);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //解密row_id
                ColumnOperand ColumnRowid = new ColumnOperand();
                sql = "select column_key_m from secret_column where table_name='test' and column_name='row_id'";
                ColumnRowid.m = connectToMySQL.ReturnFirstData(sql);
                sql = "select column_key_x from secret_column where table_name='test' and column_name='row_id'";
                ColumnRowid.x = connectToMySQL.ReturnFirstData(sql);
                long row_id = encryptDecrypt.DecryptRowid(Convert.ToInt64(dt.Rows[i][0]), encryptDecrypt.GetVk(ColumnRowid.m, ColumnRowid.x, 1, g, n, fn), ColumnRowid.m, n);

                //解密A
                ColumnOperand ColumnA = new ColumnOperand();
                sql = "select column_key_m from secret_column where table_name='test' and column_name='A'";
                ColumnA.m = connectToMySQL.ReturnFirstData(sql);
                sql = "select column_key_x from secret_column where table_name='test' and column_name='A'";
                ColumnA.x = connectToMySQL.ReturnFirstData(sql);
                dt.Rows[i][2] = encryptDecrypt.GetV(Convert.ToInt64(dt.Rows[i][2]), encryptDecrypt.GetVk(ColumnA.m, ColumnA.x, row_id, g, n, fn), n);

                //解密B
                ColumnOperand ColumnB = new ColumnOperand();
                sql = "select column_key_m from secret_column where table_name='test' and column_name='B'";
                ColumnB.m = connectToMySQL.ReturnFirstData(sql);
                sql = "select column_key_x from secret_column where table_name='test' and column_name='B'";
                ColumnB.x = connectToMySQL.ReturnFirstData(sql);
                dt.Rows[i][3] = encryptDecrypt.GetV(Convert.ToInt64(dt.Rows[i][3]), encryptDecrypt.GetVk(ColumnB.m, ColumnB.x, row_id, g, n, fn), n);
                
                //解密result
                dt.Rows[i][4] = encryptDecrypt.GetV(Convert.ToInt64(dt.Rows[i][4]), encryptDecrypt.GetVk(ColumnC.m, ColumnC.x, row_id, g, n, fn), n);
            }
            DataTable dataTable = dt.DefaultView.ToTable(false, new string[] { "id", "A", "B", "result" });//选择表中的某一列输出
            return dataTable;
        }

        //减法运算
        public DataTable OperatorSub()
        {
            ConnectToMysql connectToMySQL = new ConnectToMysql();
            connectToMySQL = new ConnectToMysql();
            EncryptDecrypt encryptDecrypt = new EncryptDecrypt();
            long n = encryptDecrypt.GetN();
            long g = encryptDecrypt.GetG();
            long fn = encryptDecrypt.GetFn();

            DateTime beforDT1 = System.DateTime.Now;//计算执行时间

            //执行ClientProtocol
            ColumnOperand ColumnC = new ColumnOperand();
            ColumnC.x = 4;//测试
            ColumnC.m = 4;
            //sql = "select gen_ck_x()";
            //ColumnC.x = connectToMySQL.ReturnFirstData(sql);
            //sql = "select gen_ck_m()";
            //ColumnC.m = connectToMySQL.ReturnFirstData(sql);
            ColumnOperand ColumnS = new ColumnOperand();
            string sql = "select column_key_x from secret_column where table_name='test' and column_name='S'";
            ColumnS.x = connectToMySQL.ReturnFirstData(sql);
            long XsInverse = encryptDecrypt.GetVkInverse(ColumnS.x, n);
            long McInverse = encryptDecrypt.GetVkInverse(ColumnC.m, n);
            sql = "select add_cal_p(" + ColumnC.x + "," + XsInverse + ",'A')";
            long p1 = connectToMySQL.ReturnFirstData(sql);
            sql = "select add_cal_q(" + p1 + "," + McInverse + ",'A')";
            long q1 = connectToMySQL.ReturnFirstData(sql);
            sql = "select add_cal_p(" + ColumnC.x + "," + XsInverse + ",'B')";
            long p2 = connectToMySQL.ReturnFirstData(sql);
            sql = "select add_cal_q(" + p2 + "," + McInverse + ",'B')";
            long q2 = connectToMySQL.ReturnFirstData(sql);

            DateTime afterDT1 = System.DateTime.Now;
            TimeSpan ts1 = afterDT1.Subtract(beforDT1);
            Console.WriteLine("-------------------减法运算DO总共花费{0}ms.----------------------", ts1.TotalMilliseconds);
            DateTime beforDTsubcal = System.DateTime.Now;//计算执行时间

            //执行ServerProtocol
            sql = "call sub_cal_ce(" + p1 + "," + q1 + "," + p2 + "," + q2 + "," + n + ")";
            connectToMySQL.ServerCommand(sql);

            DateTime afterDTsubcal = System.DateTime.Now;
            TimeSpan ts = afterDTsubcal.Subtract(beforDTsubcal);
            Console.WriteLine("-------------------执行减法协议CDB总共花费{0}ms.----------------------", ts.TotalMilliseconds);

            DateTime beforDToutall = System.DateTime.Now;//计算执行时间

            //解密
            sql = "SELECT test.row_id,id,A,B,result FROM test,operator_result WHERE test.row_id = operator_result.row_id";
            DataTable dt = new DataTable();
            dt = connectToMySQL.ReturnServerData(sql);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
            
                //解密row_id
                ColumnOperand ColumnRowid = new ColumnOperand();
                sql = "select column_key_m from secret_column where table_name='test' and column_name='row_id'";
                ColumnRowid.m = connectToMySQL.ReturnFirstData(sql);
                sql = "select column_key_x from secret_column where table_name='test' and column_name='row_id'";
                ColumnRowid.x = connectToMySQL.ReturnFirstData(sql);
                long row_id = encryptDecrypt.DecryptRowid(Convert.ToInt64(dt.Rows[i][0]), encryptDecrypt.GetVk(ColumnRowid.m, ColumnRowid.x, 1, g, n, fn), ColumnRowid.m, n);

                //解密A
                ColumnOperand ColumnA = new ColumnOperand();
                sql = "select column_key_m from secret_column where table_name='test' and column_name='A'";
                ColumnA.m = connectToMySQL.ReturnFirstData(sql);
                sql = "select column_key_x from secret_column where table_name='test' and column_name='A'";
                ColumnA.x = connectToMySQL.ReturnFirstData(sql);
                dt.Rows[i][2] = encryptDecrypt.GetV(Convert.ToInt64(dt.Rows[i][2]), encryptDecrypt.GetVk(ColumnA.m, ColumnA.x, row_id, g, n, fn), n);

                //解密B
                ColumnOperand ColumnB = new ColumnOperand();
                sql = "select column_key_m from secret_column where table_name='test' and column_name='B'";
                ColumnB.m = connectToMySQL.ReturnFirstData(sql);
                sql = "select column_key_x from secret_column where table_name='test' and column_name='B'";
                ColumnB.x = connectToMySQL.ReturnFirstData(sql);
                dt.Rows[i][3] = encryptDecrypt.GetV(Convert.ToInt64(dt.Rows[i][3]),encryptDecrypt.GetVk(ColumnB.m, ColumnB.x, row_id, g, n, fn), n);

                //解密result
                dt.Rows[i][4] = encryptDecrypt.GetV(Convert.ToInt64(dt.Rows[i][4]), encryptDecrypt.GetVk(ColumnC.m, ColumnC.x, row_id, g, n, fn), n);

            }
            DataTable dataTable = dt.DefaultView.ToTable(false, new string[] { "id", "A", "B", "result" });//选择表中的某一列输出

            DateTime afterDToutall = System.DateTime.Now;
            TimeSpan ts3 = afterDToutall.Subtract(beforDToutall);
            //Console.WriteLine("-------------------减法运算结束输出表格总共花费{0}ms.----------------------", ts3.TotalMilliseconds);

            return dataTable;
        }

        //等值运算
        public DataTable OperatorEqu()
        {
            ConnectToMysql connectToMySQL = new ConnectToMysql();
            connectToMySQL = new ConnectToMysql();
            EncryptDecrypt encryptDecrypt = new EncryptDecrypt();
            long n = encryptDecrypt.GetN();
            long g = encryptDecrypt.GetG();
            long fn = encryptDecrypt.GetFn();

            //第一步，计算C=A-B
            //执行ClientProtocol
            ColumnOperand ColumnC = new ColumnOperand();
            ColumnC.x = 4;//测试
            ColumnC.m = 4;
            ColumnOperand ColumnS = new ColumnOperand();
            string sql = "select column_key_x from secret_column where table_name='test' and column_name='S'";
            ColumnS.x = connectToMySQL.ReturnFirstData(sql);
            sql = "select column_key_m from secret_column where table_name='test' and column_name='S'";
            ColumnS.m = connectToMySQL.ReturnFirstData(sql);

            long XsInverse = encryptDecrypt.GetVkInverse(ColumnS.x, n);
            long McInverse = encryptDecrypt.GetVkInverse(ColumnC.m, n);
            sql = "select add_cal_p(" + ColumnC.x + "," + XsInverse + ",'A')";
            long p1 = connectToMySQL.ReturnFirstData(sql);
            sql = "select add_cal_q(" + p1 + "," + McInverse + ",'A')";
            long q1 = connectToMySQL.ReturnFirstData(sql);
            sql = "select add_cal_p(" + ColumnC.x + "," + XsInverse + ",'B')";
            long p2 = connectToMySQL.ReturnFirstData(sql);
            sql = "select add_cal_q(" + p2 + "," + McInverse + ",'B')";
            long q2 = connectToMySQL.ReturnFirstData(sql);

            //执行ServerProtocol
            sql = "call sub_cal_ce(" + p1 + "," + q1 + "," + p2 + "," + q2 + "," + n + ")";
            connectToMySQL.ServerCommand(sql);

            //第二步，计算Z=R*C
            //执行ClientProtocol
            ColumnOperand ColumnR = new ColumnOperand();
            sql = "select column_key_x from secret_column where table_name='test' and column_name='R'";
            ColumnR.x = connectToMySQL.ReturnFirstData(sql);
            sql = "select column_key_m from secret_column where table_name='test' and column_name='R'";
            ColumnR.m = connectToMySQL.ReturnFirstData(sql);
            ColumnOperand ColumnZ = new ColumnOperand();
            ColumnZ.x = (ColumnC.x + ColumnR.x) % fn;
            ColumnZ.m = (ColumnC.m * ColumnR.m) % n;
            //执行ServerProtocol
            sql = "UPDATE operator_result SET result = (result * (SELECT R FROM test WHERE test.row_id = operator_result.row_id))"; //MOD " + n + "
            string result = connectToMySQL.ServerCommand(sql);

            //第三步，键更新Z'
            //执行ClientProtocol
            ColumnOperand ColumnNewZ = new ColumnOperand();
            ColumnNewZ.m = 1;
            ColumnNewZ.x = 0;
            //执行ServerProtocol
            long p = (XsInverse * (ColumnNewZ.x - ColumnZ.x)) % fn;
           // float tt=Convert.ToInt64(Math.Pow(ColumnS.m, p));
            double tt =Math.Pow(ColumnS.m, p);
            double q = (ColumnZ.m * tt * encryptDecrypt.GetVkInverse(ColumnNewZ.m, n)) % n;
            DataTable dataTable1 = new DataTable();
            sql = "select row_id,A,S from test";
            dataTable1 = connectToMySQL.ReturnServerData(sql);
            DataTable dt = new DataTable();
            sql = "select * from operator_result";
            dt = connectToMySQL.ReturnServerData(sql);
            for (int i = 0; i < dataTable1.Rows.Count; i++)
            {   long ae = Convert.ToInt64(dt.Rows[i][1]);
                double se = Math.Pow(Convert.ToInt64(dataTable1.Rows[i][2]), p);
                dt.Rows[i][1] = (q * ae * se ) % n;
   
            }                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         

            return dt;
        }




        //以下是SP2任务
        public DataTable OperatorAdd2()
        {
            ConnectToMysql connectToMySQL = new ConnectToMysql();
            connectToMySQL=new ConnectToMysql();
            EncryptDecrypt encryptDecrypt = new EncryptDecrypt();
            long n = encryptDecrypt.GetN();
            long g = encryptDecrypt.GetG();
            long fn = encryptDecrypt.GetFn();

            string sql = "";

            //执行ClientProtocol
            ColumnOperand ColumnC = new ColumnOperand();
            ColumnC.x = 4;//测试
            ColumnC.m = 4;
            //string sql = "select gen_ck_x()";
            //ColumnC.x = connectToMySQL.ReturnFirstData(sql);
            //sql = "select gen_ck_m()";
            //ColumnC.m = connectToMySQL.ReturnFirstData(sql);
            ColumnOperand ColumnS = new ColumnOperand();
            sql = "select column_key_x from secret_column where table_name='test' and column_name='S'";
            ColumnS.x = connectToMySQL.ReturnFirstData2(sql);
            long XsInverse = encryptDecrypt.GetVkInverse(ColumnS.x, n);
            long McInverse = encryptDecrypt.GetVkInverse(ColumnC.m, n);
            sql = "select add_cal_p(" + ColumnC.x + "," + XsInverse + ",'A')";
            long p1 = connectToMySQL.ReturnFirstData2(sql);
            sql = "select add_cal_q(" + p1 + "," + McInverse + ",'A')";
            long q1 = connectToMySQL.ReturnFirstData2(sql);
            sql = "select add_cal_p(" + ColumnC.x + "," + XsInverse + ",'B')";
            long p2 = connectToMySQL.ReturnFirstData2(sql);
            sql = "select add_cal_q(" + p2 + "," + McInverse + ",'B')";
            long q2 = connectToMySQL.ReturnFirstData2(sql);

            //执行ServerProtocol
            sql = "call add_cal_ce(" + p1 + "," + q1 + "," + p2 + "," + q2 + "," + n + ")";
            connectToMySQL.ServerCommand2(sql);

            //解密
            sql = "SELECT test.row_id,id,A,B,result FROM test,operator_result WHERE test.row_id = operator_result.row_id";
            DataTable dt = new DataTable();
            dt = connectToMySQL.ReturnServerData2(sql);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //解密row_id
                ColumnOperand ColumnRowid = new ColumnOperand();
                sql = "select column_key_m from secret_column where table_name='test' and column_name='row_id'";
                ColumnRowid.m = connectToMySQL.ReturnFirstData2(sql);
                sql = "select column_key_x from secret_column where table_name='test' and column_name='row_id'";
                ColumnRowid.x = connectToMySQL.ReturnFirstData2(sql);
                long row_id = encryptDecrypt.DecryptRowid(Convert.ToInt64(dt.Rows[i][0]), encryptDecrypt.GetVk(ColumnRowid.m, ColumnRowid.x, 1, g, n, fn), ColumnRowid.m, n);

                //解密A
                ColumnOperand ColumnA = new ColumnOperand();
                sql = "select column_key_m from secret_column where table_name='test' and column_name='A'";
                ColumnA.m = connectToMySQL.ReturnFirstData2(sql);
                sql = "select column_key_x from secret_column where table_name='test' and column_name='A'";
                ColumnA.x = connectToMySQL.ReturnFirstData2(sql);
                dt.Rows[i][2] = encryptDecrypt.GetV(Convert.ToInt64(dt.Rows[i][2]), encryptDecrypt.GetVk(ColumnA.m, ColumnA.x, row_id, g, n, fn), n);

                //解密B
                ColumnOperand ColumnB = new ColumnOperand();
                sql = "select column_key_m from secret_column where table_name='test' and column_name='B'";
                ColumnB.m = connectToMySQL.ReturnFirstData2(sql);
                sql = "select column_key_x from secret_column where table_name='test' and column_name='B'";
                ColumnB.x = connectToMySQL.ReturnFirstData2(sql);
                dt.Rows[i][3] = encryptDecrypt.GetV(Convert.ToInt64(dt.Rows[i][3]), encryptDecrypt.GetVk(ColumnB.m, ColumnB.x, row_id, g, n, fn), n);

                //解密result
                //Vk = encryptDecrypt.GetVk(ColumnC.m, ColumnC.x, row_id, g, n, fn);
                dt.Rows[i][4] = encryptDecrypt.GetV(Convert.ToInt64(dt.Rows[i][4]), encryptDecrypt.GetVk(ColumnC.m, ColumnC.x, row_id, g, n, fn), n);
            }
            DataTable dataTable = dt.DefaultView.ToTable(false, new string[] { "id", "A", "B", "result" });//选择表中的某一列输出
            return dataTable;
        }

        public DataTable OperatorMul2()
        {
            EncryptDecrypt encryptDecrypt = new EncryptDecrypt();
            long n = encryptDecrypt.GetN();
            long g = encryptDecrypt.GetG();
            long fn = encryptDecrypt.GetFn();

            ConnectToMysql connectToMySQL = new ConnectToMysql();
            connectToMySQL = new ConnectToMysql();

            //执行ClientProtocol
            string sql = "SELECT mul_cal_m('test','A','B'),mul_cal_x('test','A','B')";
            DataTable dt1 = new DataTable();
            dt1 = connectToMySQL.ReturnClientData2(sql);
            ColumnOperand ColumnC = new ColumnOperand();
            ColumnC.m = Convert.ToInt64(dt1.Rows[0][0]);
            ColumnC.x = Convert.ToInt64(dt1.Rows[0][1]);

            //执行ServerProtocol
            sql = "call mul_cal_ce('test','A','B'," + n + ")";
            string result = connectToMySQL.ServerCommand2(sql);

            //解密
            sql = "SELECT test.row_id,id,A,B,result FROM test,operator_result WHERE test.row_id = operator_result.row_id";
            DataTable dt = new DataTable();
            dt = connectToMySQL.ReturnServerData2(sql);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //解密row_id
                ColumnOperand ColumnRowid = new ColumnOperand();
                sql = "select column_key_m from secret_column where table_name='test' and column_name='row_id'";
                ColumnRowid.m = connectToMySQL.ReturnFirstData2(sql);
                sql = "select column_key_x from secret_column where table_name='test' and column_name='row_id'";
                ColumnRowid.x = connectToMySQL.ReturnFirstData2(sql);
                long row_id = encryptDecrypt.DecryptRowid(Convert.ToInt64(dt.Rows[i][0]), encryptDecrypt.GetVk(ColumnRowid.m, ColumnRowid.x, 1, g, n, fn), ColumnRowid.m, n);

                //解密A
                ColumnOperand ColumnA = new ColumnOperand();
                sql = "select column_key_m from secret_column where table_name='test' and column_name='A'";
                ColumnA.m = connectToMySQL.ReturnFirstData2(sql);
                sql = "select column_key_x from secret_column where table_name='test' and column_name='A'";
                ColumnA.x = connectToMySQL.ReturnFirstData2(sql);
                dt.Rows[i][2] = encryptDecrypt.GetV(Convert.ToInt64(dt.Rows[i][2]), encryptDecrypt.GetVk(ColumnA.m, ColumnA.x, row_id, g, n, fn), n);

                //解密B
                ColumnOperand ColumnB = new ColumnOperand();
                sql = "select column_key_m from secret_column where table_name='test' and column_name='B'";
                ColumnB.m = connectToMySQL.ReturnFirstData2(sql);
                sql = "select column_key_x from secret_column where table_name='test' and column_name='B'";
                ColumnB.x = connectToMySQL.ReturnFirstData2(sql);
                dt.Rows[i][3] = encryptDecrypt.GetV(Convert.ToInt64(dt.Rows[i][3]), encryptDecrypt.GetVk(ColumnB.m, ColumnB.x, row_id, g, n, fn), n);

                //解密result
                dt.Rows[i][4] = encryptDecrypt.GetV(Convert.ToInt64(dt.Rows[i][4]), encryptDecrypt.GetVk(ColumnC.m, ColumnC.x, row_id, g, n, fn), n);
            }
            DataTable dataTable = dt.DefaultView.ToTable(false, new string[] { "id", "A", "B", "result" });//选择表中的某一列输出
            return dataTable;
        }

    }
}
 