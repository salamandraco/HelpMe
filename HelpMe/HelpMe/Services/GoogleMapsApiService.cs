using HelpMe.Connections;
using HelpMe.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HelpMe.Services
{
    public class GoogleMapsApiService : IGoogleMapsApiService
    {
        private const string ApiBaseAddress = "https://maps.googleapis.com/maps/";

        private HttpClient CreateClient()
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(ApiBaseAddress)
            };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }

        public async Task<GooglePlaceAutoCompleteResult> ApiPlaces(string text)
        {
            GooglePlaceAutoCompleteResult result = null;
            using (var client = CreateClient())
            {
                // var response = await client.GetAsync(ApiBaseAddress + text);
                var response = await client.GetAsync($"api/place/autocomplete/json?input={Uri.EscapeUriString(text)}&key={Constants.GoogleMapsApiKey}").ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    var googlePlaces = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                    if (!string.IsNullOrWhiteSpace(googlePlaces) && googlePlaces != "ERROR")
                    {
                        result = await Task.Run(() => JsonConvert.DeserializeObject<GooglePlaceAutoCompleteResult>(googlePlaces)).ConfigureAwait(false);
                    }
                }
            }
            return result;
        }

        public async Task<GooglePlace> ApiPlacesDetails(string placeId)
        {
            GooglePlace result = null;

            using (var client = CreateClient())
            {
                var response = await client.GetAsync($"api/place/details/json?placeid={placeId}&key={Constants.GoogleMapsApiKey}").ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                    if (!string.IsNullOrWhiteSpace(json) && json != "ERROR")
                    {
                        result = new GooglePlace(JObject.Parse(json));
                    }
                }
            }

            return result;
        }
        /*
        public async Task<GooglePlaceAutoCompleteResult> ApiPlacesNearbySearch(string text)
        {
            //const result = null;
            using (var client = CreateClient())
            {
                // var response = await client.GetAsync(ApiBaseAddress + text);
                var response = await client.GetAsync($"api/place/nearbysearch/json?" +
                    $"keyword={Uri.EscapeUriString(text)}&" +
                    $"keyword={Uri.EscapeUriString(text)}&" +
                    $"key={Constants.GoogleMapsApiKey}").ConfigureAwait(false);


            https://maps.googleapis.com/maps/api/place/nearbysearch/json
  ? keyword = cruise
  & location = -33.8670522 % 2C151.1957362
  & radius = 1500
  & type = restaurant
  & key = YOUR_API_KEY




                if (response.IsSuccessStatusCode)
                {
                    var googlePlaces = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                    if (!string.IsNullOrWhiteSpace(googlePlaces) && googlePlaces != "ERROR")
                    {
                        result = await Task.Run(() => JsonConvert.DeserializeObject<GooglePlaceAutoCompleteResult>(googlePlaces)).ConfigureAwait(false);
                    }
                }
            }
        }


        */


    }
}
