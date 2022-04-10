using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace suntvaccinat.Services
{
    public class APIConnectionService  
    {
        public static async Task<string> PostPdfCertificate( Models.INSPValidationModel validationModel, string URL)
        {
            HttpClient client = new HttpClient();

            // Request headers.
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _subscriptionKey);
            string requestParameters = "visualFeatures=" +
                    "Description&" +
                    "details=Celebrities&language=en&model-version=latest";
            string uri = $"{_endpoint}/vision/v3.2/{recognisionType}?{requestParameters}";

            // Read the contents of the specified local image
            // into a byte array.

            using (ByteArrayContent content = new(byteData))
            {
                // This example uses the "application/octet-stream" content type.
                // The other content types you can use are "application/json" and "multipart/form-data".
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                // Asynchronously call the REST API method.
                HttpResponseMessage response = await client.PostAsync(uri, content);
                // Asynchronously get the JSON response.
                string contentString = await response.Content.ReadAsStringAsync();

                // Display the JSON response.
                return JToken.Parse(contentString).ToString();
            }
        }

    }
}
