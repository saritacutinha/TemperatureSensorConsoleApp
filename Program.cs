using System;
using System.Timers;

namespace TemperatureSensorConsoleApp
{
    class Program
    {
        static Random random = new Random();
        static void Main(string[] args)
        {
            
            Timer timer = new Timer(60000);
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
            Console.ReadKey();
            timer.Stop();
        }

        private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var currentTemperature = random.Next(-4, 45);
            Console.WriteLine("DateTime :"+DateTime.Now +"\tTemperature :"+currentTemperature );
        }
    }
}
