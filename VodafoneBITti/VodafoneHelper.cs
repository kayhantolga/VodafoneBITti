using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TolgaTheWizard.ErrorHandler;
using TolgaTheWizard.Extensions;

namespace VodafoneBITti
{
    public static class VodafoneHelper
    {
        private const string TokenUrl = "https://onelogin.vodafone.com.tr/";
        private const string GameUrl = "https://internetavcilari.vodafone.com.tr/oyun/mobileapi/v1/";
        private static string Token { get; set; }
        private static string PhoneNumber { get; set; }


        public static async Task<TokenRoot> GetTokenAsync(string phoneNumber, string password)
        {
            const string path = "getToken";

            var client = new HttpClient();
            client.BaseAddress = new Uri(TokenUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
            client.DefaultRequestHeaders.Add("User-Agent", "DataHunters/5 CFNetwork/887 Darwin/17.0.0");
           
            var uri = new UriBuilder(TokenUrl + path);
            var query = new NameValueCollection
            {
                {
                    "appId", "DATAHUNT01"
                },
                {
                    "username", phoneNumber
                },
                {
                    "pwd", password
                }
            };
            uri.Query = query.ToQueryString();
            var response = await client.PostAsync(uri.ToString(),null);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsAsync<TokenRoot>();
                Token = result.token.tokenId;
                PhoneNumber = phoneNumber;
                return result;
            }

            throw new Error(await response.Content.ReadAsStringAsync());
        }


        public static async Task<BitPoint> AddNewPointAsync(long flockId,int point)
        {
            const string path = "processendgame";

            BitPoint bitPoint = new BitPoint()
            {
                bitFlockId = flockId,
                bitsHunted = point
            };
            UriBuilder uri = new UriBuilder(GameUrl + path);
            NameValueCollection query = new NameValueCollection();
            uri.Query = query.ToQueryString();

            var client = new HttpClient();
            client.BaseAddress = new Uri(TokenUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            client.DefaultRequestHeaders.Add("x-user-bitflock-id", bitPoint.bitFlockId.ToString());
            client.DefaultRequestHeaders.Add("x-user-msisdn", PhoneNumber);
            client.DefaultRequestHeaders.Add("x-user-token", Token);


            HttpResponseMessage response = await client.PostAsJsonAsync(uri.ToString(), bitPoint);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsAsync<BitPoint>();
                return result;
            }
            throw new Error(await response.Content.ReadAsStringAsync());
        }

        public static async Task<List<Flocks>> GetFlocksAsync(string lati, string longi)
        {

            const string path = "bitlocations";

            var client = new HttpClient();
            client.BaseAddress = new Uri(TokenUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
            client.DefaultRequestHeaders.Add("User-Agent", "DataHunters/5 CFNetwork/887 Darwin/17.0.0");
            
            client.DefaultRequestHeaders.Add("x-user-msisdn", PhoneNumber);
            client.DefaultRequestHeaders.Add("x-user-token", Token);
            var uri = new UriBuilder(GameUrl + path);
            var query = new NameValueCollection
            {
                {
                    "lat", lati
                },
                {
                    "lng", longi
                },
                {
                    "cnt", "1000"
                }
            };

            uri.Query = query.ToQueryString();
            var response = await client.GetAsync(uri.ToString());
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsAsync<List<Flocks>>();
                return result;
            }

            throw new Error(await response.Content.ReadAsStringAsync());
        }
    }
}
