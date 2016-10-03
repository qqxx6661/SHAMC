using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WFProject
{
    class EncryptDecrypt
    {
        //global_info表中设置的参数
        long n;
        long g;
        long fn;
        public EncryptDecrypt()
        {
            ConnectToMysql connectToMySQL = new ConnectToMysql();
            string sql = "select n from global_info";
            n = connectToMySQL.ReturnFirstData(sql);

            sql = "select g from global_info";
            g = connectToMySQL.ReturnFirstData(sql);

            sql = "select fn from global_info";
            fn = connectToMySQL.ReturnFirstData(sql);
        }
        public long GetN()
        {
            return n;
        }
        public long GetG()
        {
            return g;
        }
        public long GetFn()
        {
            return fn;
        }

        public long GetVk(long m,long x,long r,long g,long n,long fn)//生成Vk
        {
            long a;
            a = (r * x)%fn;
            long Vk = (m * Convert.ToInt64(Math.Pow(g, a) % n)) % n;
            return Vk;
        }

        public long GetVe(long V, long Vk, long n)//生成Ve
        {
            long VkInverse = GetVkInverse(Vk, n);
            long Ve = (V * VkInverse) % n;
            //注意这段代码
          // while (Ve < n)
         //  {
          //    Ve = Ve + n;
         //  }
           return Ve;
      }

        public long GetV(long Ve, long Vk, long n)//生成V
        {
            long V = (Ve * Vk) % n;
            return V;
        }

        public long GetVsub(long Ve, long Vk, long n)//生成V减法专用,暂时无用
        {
            long V = (Ve * Vk) % n;
            V = V - n;
            return V;
        }

        //*************************************求模逆运算********************************************
        public class GCD
        {
            public long d;
            public long x;
            public long y;
        }
        GCD EXTENDED_EUCLID(long a, long b)
        {
            GCD aa = new GCD();
            GCD bb = new GCD();
            if (b == 0)
            {
                aa.d = a;
                aa.x = 1;
                aa.y = 0;
                return aa;
            }
            else
            {
                bb = EXTENDED_EUCLID(b, a % b);
                aa.d = bb.d;
                aa.x = bb.y;
                aa.y = bb.x - bb.y * (a / b);
            }
            return aa;
        } 
        public long GetVkInverse(long Vk,long n)
        {
            GCD aa;
            aa = EXTENDED_EUCLID(Vk, n);
            while (aa.x < 0)
            {
                aa.x = aa.x + n;
            }
            return aa.x;
        }
        //*******************************************************************************************

        //SIES，加法同态加密算法，用来加密row_id
        public long EncryptRowid(long K,long V,long k,long n)
        {
            long Ve = (K * V + k) % n;
            return Ve;
        }
        //SIES，加法同态加密算法，用来解密row_id
        public long DecryptRowid(long Ve,long k,long K,long n)
        {
            long KInverse = GetVkInverse(K,n);
            long V = (Math.Abs(Ve - k) * KInverse) % n;
            return V;
        }
    }
}
