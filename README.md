# SMC_Yang

************Warning: This is a sample application without full functions**************

A secure cloud database model based on multi-cloud

**This is an application for my paper Secure Model based on Multi-cloud for Big Data Storage and Query, published on CBD2016.**

This program is written by C# and Mysql UDF. It mainly realized an prototype with a secure cloud database model based on multi-cloud and consists of all the functions below:

>- Create one/two encrypted databases with Mysql on the server.
>- Support four different operators(+/-/*/=><).
>- Multithreading.

Other functions are on our agenda.

##**Runtime environment:**

>- Mysql5.6
>- Visual studio 2012

Here we begin (use single cloud database for the example),

###**Step1: Build the database**

Create 2 databases for the client and the server called keystore and serverdatabase seperately.
Key store has two tables and six UDFs: Global_info, Secret_column; Add_cal_p, Add_cal_q, gen_ck_m, gen_ck_x, mul_cal_m, mul_cal_x;

![1](http://img.blog.csdn.net/20161003175252491)

Serverdatabase has two tables and three UDFs: Operator_result, test; Add_cal_ce, Mul_cal_ce, Sub_cal_ce;

![2](http://img.blog.csdn.net/20161003175258335)

All the protocols based on UDFs are provided in our code.

###**Step2: Configure the C# application**

In ConnectToMysql.cs, we can set the Mysql settings.

	string clientMysql = "Database=keystore;Data Source=127.0.0.1;User Id=root;Password=root;CharSet=utf8;port=3306";
	string serverMysql = "Database=serverdatabase;Data Source=127.0.0.1;User Id=root;Password=root;CharSet=utf8;port=3306"

###**Step3: Run the application**

You can use the visual stuio to run it.

![3](http://img.blog.csdn.net/20161003180215503)
![4](http://img.blog.csdn.net/20161003180218871)
![5](http://img.blog.csdn.net/20161003180222207)

**Enjoy!**


