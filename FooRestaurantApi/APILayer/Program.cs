using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Threading;

namespace FooRestaurantApi.APILayer
{

    public class C
    {
        public int MyProperty { get; set; }

    }
    public struct A
    {
        public A()
        {

        }
        public int MyProperty1 = 1;
        public C Name1 { get; set; } = new C() { MyProperty=1};
    }

    public struct B
    {
        public A MyProperty { get; set; } = new A();
        static B()
        {


        }
        public static int A = 1;
        public string Name2=null;
        public B()
        {
            Name2 = "";

        }
        public B(
            int i)
        {

        }
        public string Name { get; set; } = "";
    }
    //public  class A1 {
    //    public int MyProperty { get; set; } = 4;
    //    public  void method2()
    //    {


    //    }

    //    }
    //public  class A2:A1
    //{
    //    public int A = 2;
    //    public virtual int MyProperty { get; set; } = 2;
    //    public virtual void method2() { }
    //    public A2()
    //    {
    //        MyProperty = 3;
    //    }

    //}
    //public class A3 : A2
    //{
    //    public  override int MyProperty { get; set; } = 2;
    //    public  int BField = 1;
    //    public static int AField = 1;
    //    public static int CField = 1;
    //    static A3()
    //    {

    //    }
    //    public A3():base()
    //    {

    //    }
    //    public override void method2()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public  void method3()
    //    {

    //       ( (A2)this).method2();
    //        base.method2();
    //        throw new NotImplementedException();
    //    }
    //}

    //public interface I1
    //{

    //    void method1();

    //}


    //public class A
    //{
    //    public virtual void method1()
    //    {


    //    }

    //    public void method2()
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
    //public class B:A,I1
    //{
    //    //public override void method1()
    //    //{


    //    //}

    //}
    //public class C:B,I1
    //{
    //    public new void method1()
    //    {


    //    }

    //}
    //public class D : C, I1

    //{
    //    public void method1()
    //    {


    //    }
    //}
    //public abstract class E: I1

    //{
    //    public abstract void method1();

    //    public void method2()
    //    {


    //    }
    //}
    //public abstract class F : A,I1

    //{
    //    public abstract void method1();

    //    public void method2()
    //    {


    //    }
    //}
    //public class G : F

    //{
    //    //public abstract void method1();

    //    //public void method2()
    //    //{


    //    //}
    //    public override void method1()
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
   


    public class TherTest
    {


        public int MyProperty { get; set; } = 1;
        public void M1()
        {
            throw new Exception();
            bool a;
           var mut =  new Mutex(true,"asd",out a);
            mut.WaitOne();
            Thread.Sleep(100000);
            MyProperty += 1;
        }
    }
    public class Program
    {
 
        public static void Main(string[] args)
        {
          //  var threadtrest = new TherTest();
          //var t1 =   new Thread(threadtrest.M1);

          //  t1.IsBackground = true;

          //  t1.Start();
   
     
          //  var a1 = new A();
          //  object objecta1 = a1;
          //  var a2 = (A)objecta1;
       

          //  a1.MyProperty1 = 34;

          //  B b1 = new B();
          //  B b2 = b1;
          //  b2.MyProperty.Name1.MyProperty = 44;
          //  //a1.Name1.MyProperty = 2;


            var configuration = new ConfigurationBuilder()
                                 .AddJsonFile("appsettings.json")
                                 .Build();
            Log.Logger = new LoggerConfiguration()
                            .ReadFrom.Configuration(configuration)
                            .CreateLogger();

            try
            {
                Log.Information("Starting web host");
                CreateHostBuilder(args).Build().Run();

            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");

            }
            finally
            {
                Log.CloseAndFlush();
            }
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
             Host.CreateDefaultBuilder(args)
                 .UseSerilog()
             .ConfigureAppConfiguration((hostingContext, config) =>
             {


                 config.AddEnvironmentVariables();
             })
                 .ConfigureWebHostDefaults(webBuilder =>
                 {
                     webBuilder.UseStartup<Startup>();
                 });
    }
}
