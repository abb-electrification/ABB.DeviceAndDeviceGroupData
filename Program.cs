using ABB.Developers.SDK.Services.Devices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Electrification.ABB.SDK.Device.Data
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                //Put application keys in here
                //you will get this below appId,APIkey,subId from the application section of the sdk portal.
                Guid appId = new Guid("3c7eb1bf-e1db-4fa3-8199-e80bc5477711");
                string APIkey = "ABB hqpI9jQiKBYPlvJC07zGU88WdpJKFCE7Og==";
                string subId = "c15dc231099b40f89c5175106f4d5145";

                //Plant Id
                var plantId = new Guid("a09fe0a1-b981-4e04-aeb5-2fa9ce3a9be1");

                //Setup the headers
                Dictionary<string, string> headers = new Dictionary<string, string>();
                headers.Add("Authorization", APIkey);
                headers.Add("Subscription-Key", subId);

                // Instantiate device service
                DeviceService ds = new DeviceService(headers);

                //Get list of devices
                var deviceResult = await ds.GetDevices(appId, plantId, 1);

                //Get list of device groups
                var deviceGroupResult = await ds.GetDevicegroups(appId, plantId, 1);

                //Converting the platform data into json format
                var deviceGroupData = JsonConvert.SerializeObject(deviceGroupResult.Data);
                var deviceData = JsonConvert.SerializeObject(deviceResult.Data);

                //Exporting platform data to an external API service
                HttpClient client = new HttpClient();

                //External API service end point url
                string baseUrl = "http://localhost:15445/ExtractedData/export";
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.PostAsync(baseUrl, new StringContent(deviceGroupData, Encoding.UTF8, "application/json")).Result;
                Console.WriteLine(response.Content);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
