using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Linq;
using System.Threading;
using TestApp.Helpers;

namespace TestApp
{
    class Program
    {
        static HttpClient client = new HttpClient();
        private static readonly DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        static void Main(string[] args)
        {
            Console.WriteLine("Preparing to interrogate remote REST service...");

            RunAsync().GetAwaiter().GetResult();
        }

        static async Task RunAsync()
        {
            client.BaseAddress = new Uri("http://localhost:5000/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            try 
            {
                int hits = 20;

                for( int cnt=0; cnt < hits; cnt++ )
                {
                    // LAB1: comment when using Retry handler
                    string people = await GetPeopleAsync( "http://localhost:5000/api/people" );

                    // LAB1: Uncomment when using Retry handler
                    // string people = String.Empty;
                    // try 
                    // {
                    //     await RetryHelper.OperationWithBasicRetryAsync(async () =>
                    //     {
                    //         try
                    //         {
                    //             people = await GetPeopleAsync( "http://localhost:5000/api/people" );
                    //         }
                    //         catch (Exception)
                    //         {
                    //             throw;
                    //         }
                    //     });
                    // }
                    // catch( Exception e )
                    // {
                    //     Console.WriteLine( e.Message );
                    // }

                    // Do something with the data

                    Thread.Sleep(500);
                }
            }
            catch( HttpRequestException ex )
            {
                Console.WriteLine( "HTTP Request Exception: "  + ex.ToString() );
            }
            catch( Exception e )
            {
                Console.WriteLine( "Unknown error: "  + e.ToString() );
            }
        }

        static async Task<string> GetPeopleAsync(string path)
        {
            string people = String.Empty;

            DateTime n = DateTime.UtcNow;
            
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine( $"[{n.ToLongTimeString()} UTC] (SUCCESS) Status code: {response.StatusCode}" );
                people  = await response.Content.ReadAsStringAsync(); 
            }
            else
            {
                if( response.StatusCode == (HttpStatusCode)429 )
                {
                    var throttleDelay = GetExpiry(response.Headers);
                    Console.WriteLine( $"[{n.ToLongTimeString()} UTC] (FAILURE) Status code: {response.StatusCode} - THROTTLED until {throttleDelay} UTC" );
                    
                    // LAB1: Uncomment when using Retry handler
                    //throw new WebException( "Too many requests.", null, WebExceptionStatus.RequestCanceled, null);
                }
                else
                {
                    Console.WriteLine( "[FAILURE] Status code: "  + response.StatusCode );
                }

            }

            return people;
        }

        static string GetExpiry(HttpResponseHeaders headers)
        {
            string waitTime = String.Empty;
            
            //IEnumerable<string> values;
            if (headers.TryGetValues("X-RateLimit-Reset", out var values))
            {
                long expiry = 0L;
                
                long.TryParse(values.FirstOrDefault(), out expiry ); 

                DateTime expiryTime = FromUnixTime(expiry);

                waitTime = expiryTime.ToLongTimeString();
            }

            return waitTime;
        }

        public static DateTime FromUnixTime(long unixTime)
        {
            return epoch.AddSeconds(unixTime);
        }
    }
}
