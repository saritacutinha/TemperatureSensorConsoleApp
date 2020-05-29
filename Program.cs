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
        
        private static readonly string[] Summaries = new[]
        {
            "Freezing",  "Chilly", "Cool", "Mild", "Warm",  "Hot",  "Scorching"
        };

        public static void Main(string[] args)
        {
            /*Setting the timer for every minute
             * creating http client 
             * creating data to be posted */

            Timer timer = new Timer(60000);
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:58397");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
              
            var rng = new Random();
            var temperature = new Temperature() { Date = DateTime.Now, TemperatureC = rng.Next(-4, 50), Summary = Summaries[rng.Next(Summaries.Length)] };       
            timer.Elapsed += (sender, e) => Timer_Elapsed(sender, e, client, temperature);
            timer.Start();
            Console.ReadKey();
            timer.Stop();
        }

        private static void Timer_Elapsed(object sender, ElapsedEventArgs e, HttpClient client,Temperature temperature)
        {
            /* On timer elapsed post data to WebApi */
            Console.WriteLine("Sending Temperature Data:\n" +
                "Date:"+temperature.Date+
                "\tTemperature in Fahrenhight"+temperature.TemperatureF+
                "\tTemperature in Celsius"+ temperature.TemperatureC+
                "\tSummary:"+temperature.Summary);
            PostTemperatureData(client, temperature).Wait();
        }

        private static async Task PostTemperatureData(HttpClient client,Temperature temperature)
        {
              HttpResponseMessage response = await client.PostAsJsonAsync("/Temperature", temperature);
              if (response.IsSuccessStatusCode)
               { 
                    Console.WriteLine(response.StatusCode);
               }
                else
                    Console.WriteLine("Internal Server Error");
        }
    }
    
    public class Temperature
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; }
    }
}

