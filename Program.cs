using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace TemperatureSensorConsoleApp
{
    public class Program
    {
        //static Random random = new Random();
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public static async Task Main(string[] args)
        {


            //Timer timer = new Timer();
            //timer.Elapsed += Timer_Elapsed;
            //timer.Start();
            //Console.ReadKey();
            //timer.Stop();
            var rng = new Random();
            var temperature = new Temperature() { Date = DateTime.Now, TemperatureC = rng.Next(-4, 50), Summary = Summaries[rng.Next(Summaries.Length)] };
            await PostTemperatureData(temperature);
        }

        private static async Task PostTemperatureData(Temperature temperature)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:58397");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.PostAsJsonAsync("/Temperature", temperature);
                if (response.IsSuccessStatusCode)
                {
                    // Get the URI of the created resource.  
                    Uri returnUrl = response.Headers.Location;
                    Console.WriteLine(returnUrl);
                }
                else
                    Console.WriteLine("Internal Server Error");

            }
        }
    }




    //private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
    //{
    //    var currentTemperature = random.Next(-4, 45);
    //    Console.WriteLine("DateTime :"+DateTime.Now +"\tTemperature :"+currentTemperature );
    //}

    public class Temperature
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; }
    }
}

