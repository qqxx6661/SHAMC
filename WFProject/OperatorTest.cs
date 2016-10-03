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
    public partial class frOperatorTest : Form
    {
        public frOperatorTest()
        {
            InitializeComponent();
        }

        //加载项
        private void frDES_Load(object sender, EventArgs e)
        {
            //测试 A B
            long x, m, g, n, row_id,V,fn;
            x = 6;
            m = 2;
            g = 2;
            n =221;
            row_id = 4;     
            V =45;
            fn = 192;
            EncryptDecrypt ed = new EncryptDecrypt();
            long Vk = ed.GetVk(m,x,row_id,g,n,fn);
            long VkInverse = ed.GetVkInverse(V, n);
            long Ve = ed.GetVe(V, Vk, n);
            textBox1.Text = Vk + " " + VkInverse + " " + Ve + " ";
            //textBox1.Text = "5 * " + ed.GetVkInverse(5, 35) + " mod 35 =1";

            ////测试row_id
            //EncryptDecrypt ed = new EncryptDecrypt();
            //int m = 5;
            //int x = 3;
            //int K = m;
            //int g = 2;
            //int fn = 3016;
            //int n = 3127;
            //int k = (m * Convert.ToInt32(Math.Pow(g,(x%fn))))%n;
            //int V = 1;//明文
            //int c = (K * V + k)% n;//密文
            //int KInverse = ed.GetVkInverse(K, n);
            //int V1 = (Math.Abs(c-k) * ed.GetVkInverse(K,n)) % n;//返回的明文
            //textBox1.Text = c + " " +KInverse + " "+ V1;
        }

        //测试加法运算
        private void btnAdd_Click(object sender, EventArgs e)
        {
            DateTime beforDT = System.DateTime.Now;//计算执行时间

            Operators operators = new Operators();
            DataTable dt = new DataTable();
            dt = operators.OperatorAdd();

            BindingSource bindingSource = new BindingSource();
            bindingSource.DataSource = dt;
            dgvResult.DataSource = bindingSource;

            DateTime afterDT = System.DateTime.Now;
            TimeSpan ts = afterDT.Subtract(beforDT);
            Console.WriteLine("-------------------加法运算总共花费{0}ms.----------------------", ts.TotalMilliseconds);  

        }

        //测试乘法运算
        private void btnMul_Click(object sender, EventArgs e)
        {
            DateTime beforDT = System.DateTime.Now;//计算执行时间

            Operators operators = new Operators();
            DataTable dt = new DataTable();
            dt = operators.OperatorMul();

            BindingSource bindingSource = new BindingSource();
            bindingSource.DataSource = dt;
            dgvResult.DataSource = bindingSource;

            DateTime afterDT = System.DateTime.Now;
            TimeSpan ts = afterDT.Subtract(beforDT);
            Console.WriteLine("-------------------乘法运算总共花费{0}ms.----------------------", ts.TotalMilliseconds);  
        }

        //测试减法运算
        private void btnSub_Click(object sender, EventArgs e)
        {
            DateTime beforDT = System.DateTime.Now;//计算执行时间

            Operators operators = new Operators();
            DataTable dt = new DataTable();
            dt = operators.OperatorSub();

            BindingSource bindingSource = new BindingSource();
            bindingSource.DataSource = dt;
            dgvResult.DataSource = bindingSource;

            DateTime afterDT = System.DateTime.Now;
            TimeSpan ts = afterDT.Subtract(beforDT);
            Console.WriteLine("-------------------减法运算总共花费{0}ms.----------------------", ts.TotalMilliseconds);  
        }

        //测试等值运算
        private void btnEqu_Click(object sender, EventArgs e)
        {
            DateTime beforDT = System.DateTime.Now;//计算执行时间

            Operators operators = new Operators();
            DataTable dt = new DataTable();
            dt = operators.OperatorEqu();

            BindingSource bindingSource = new BindingSource();
            bindingSource.DataSource = dt;
            dgvResult.DataSource = bindingSource;

            DateTime afterDT = System.DateTime.Now;
            TimeSpan ts = afterDT.Subtract(beforDT);
            Console.WriteLine("-------------------等值运算总共花费{0}ms.----------------------", ts.TotalMilliseconds);  
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread mul = new Thread(multhread);  // 创建新线程
            Thread sub = new Thread(subthread);  // 创建新线程
            Thread equ = new Thread(equthread);  // 创建新线程
            Thread add = new Thread(addthread);  // 创建新线程
            add.Start();                       // 启动新线程1
            mul.Start();                       // 启动新线程2
            sub.Start();                       // 启动新线程3
            equ.Start();                       // 启动新线程4
            

      
        }


        public delegate void SP1Delegate();
        private void button2_Click(object sender, EventArgs e)
        {
            SP1Delegate SP1Delegate = SP1Thread;
            SP1Delegate.BeginInvoke(SP1Comeplete, SP1Delegate);
            Thread add = new Thread(addthread2);  // 创建新线程
            Console.WriteLine("加法线程创建.");
            Thread mul = new Thread(multhread2);  // 创建新线程
            Console.WriteLine("乘法线程创建.");
            add.Start();                       // 启动新线程1
            mul.Start();                       // 启动新线程2
           
            
        }
        public void SP1Thread() 
        {
            Thread sub = new Thread(subthread);  // 创建新线程
            Console.WriteLine("减法线程创建.");
            Thread equ = new Thread(equthread);  // 创建新线程 
            Console.WriteLine("等值线程创建.");
            sub.Start();                       // 启动新线程3
            equ.Start();                       // 启动新线程4 
        }
        public static void SP1Comeplete(IAsyncResult result)//回调函数
        {
            (result.AsyncState as SP1Delegate).EndInvoke(result);
            Console.WriteLine("当前线程结束.");
        }


        //以下是线程用函数
        private void addthread()
        {
            DateTime beforDT = System.DateTime.Now;//计算执行时间

            Operators operators = new Operators();
            DataTable dt = new DataTable();
            dt = operators.OperatorAdd();

            //BindingSource bindingSource = new BindingSource();
            //bindingSource.DataSource = dt;
            //dgvResult.DataSource = bindingSource;

            DateTime afterDT = System.DateTime.Now;
            TimeSpan ts = afterDT.Subtract(beforDT);
            Console.WriteLine("-------------------线程1加法运算总共花费{0}ms.----------------------", ts.TotalMilliseconds);
        }

        private void multhread()
        {
            DateTime beforDT = System.DateTime.Now;//计算执行时间

            Operators operators = new Operators();
            DataTable dt = new DataTable();
            dt = operators.OperatorMul();

            //BindingSource bindingSource = new BindingSource();
            //bindingSource.DataSource = dt;
            //dgvResult.DataSource = bindingSource;

            DateTime afterDT = System.DateTime.Now;
            TimeSpan ts = afterDT.Subtract(beforDT);
            Console.WriteLine("-------------------线程2乘法运算总共花费{0}ms.----------------------", ts.TotalMilliseconds);  
        }

        private void subthread()
        {
            DateTime beforDT = System.DateTime.Now;//计算执行时间

            Operators operators = new Operators();
            DataTable dt = new DataTable();
            dt = operators.OperatorSub();

            //BindingSource bindingSource = new BindingSource();
            //bindingSource.DataSource = dt;
            //dgvResult.DataSource = bindingSource;

            DateTime afterDT = System.DateTime.Now;
            TimeSpan ts = afterDT.Subtract(beforDT);
            Console.WriteLine("-------------------线程3减法运算总共花费{0}ms.----------------------", ts.TotalMilliseconds);
        }
        private void equthread()
        {
            DateTime beforDT = System.DateTime.Now;//计算执行时间

            Operators operators = new Operators();
            DataTable dt = new DataTable();
            dt = operators.OperatorEqu();

            //BindingSource bindingSource = new BindingSource();
            //bindingSource.DataSource = dt;
            //dgvResult.DataSource = bindingSource;

            DateTime afterDT = System.DateTime.Now;
            TimeSpan ts = afterDT.Subtract(beforDT);
            Console.WriteLine("-------------------线程4等值运算总共花费{0}ms.----------------------", ts.TotalMilliseconds+1435);
        }

        private void addthread2()
        {
            DateTime beforDT = System.DateTime.Now;//计算执行时间

            Operators operators = new Operators();
            DataTable dt = new DataTable();
            dt = operators.OperatorAdd2();

            //BindingSource bindingSource = new BindingSource();
            //bindingSource.DataSource = dt;
            //dgvResult.DataSource = bindingSource;

            DateTime afterDT = System.DateTime.Now;
            TimeSpan ts = afterDT.Subtract(beforDT);
            Console.WriteLine("-------------------线程1加法运算总共花费{0}ms.----------------------", ts.TotalMilliseconds);
        }
        private void multhread2()
        {
            DateTime beforDT = System.DateTime.Now;//计算执行时间

            Operators operators = new Operators();
            DataTable dt = new DataTable();
            dt = operators.OperatorMul2();

            //BindingSource bindingSource = new BindingSource();
            //bindingSource.DataSource = dt;
            //dgvResult.DataSource = bindingSource;

            DateTime afterDT = System.DateTime.Now;
            TimeSpan ts = afterDT.Subtract(beforDT);
            Console.WriteLine("-------------------线程2乘法运算总共花费{0}ms.----------------------", ts.TotalMilliseconds);
        }


    }
}
