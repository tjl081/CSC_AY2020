using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CSC_AY2020.Controllers
{
    public class WeatherAPIController : System.Web.Http.ApiController
    {
        static readonly HttpClient client = new HttpClient();

        [HttpGet]
        [Route("api/weatherapi/getWeather")]
        public async Task<string> GetWeather()
        {

            // Call asynchronous network methods in a try/catch block to handle exceptions.
            try
            {
                HttpResponseMessage response = await client.GetAsync("https://api.data.gov.sg/v1//environment/4-day-weather-forecast");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                // Above three lines can be replaced with new helper method below
                // string responseBody = await client.GetStringAsync(uri);
                return await Task.FromResult(responseBody);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
                return await Task.FromResult("\nError occured!");
            }

        }
    }
}
