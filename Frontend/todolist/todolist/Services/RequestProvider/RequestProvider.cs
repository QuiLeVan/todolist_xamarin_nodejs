using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using todolist.Exceptions;

namespace todolist.Services.RequestProvider
{
    public class RequestProvider : IRequestProvider
    {
        private readonly JsonSerializerSettings _serializerSettings;

        public RequestProvider()
        {
            _serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                NullValueHandling = NullValueHandling.Ignore
            };
            _serializerSettings.Converters.Add(new StringEnumConverter());
        }

    /// <summary>
    /// --------------------------------------------
    /// [START] Implement IRequestProvidier Area
    /// --------------------------------------------
    /// </summary>
    #region Implement IRequestProvidier

        /// <summary>
        /// Function to GET Information from server then use NEWTONSOFT.JSON LIB parse to object model type
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="uri"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<TResult> GetAsync<TResult>(string uri, string token = "")
        {
            HttpClient httpClient = CreateHttpClient(token);
            HttpResponseMessage response = await httpClient.GetAsync(uri);

            await HandleResponse(response);
            string serialized = await response.Content.ReadAsStringAsync();

            TResult result = await Task.Run(() =>
                JsonConvert.DeserializeObject<TResult>(serialized, _serializerSettings));
            return result;
        }

        /// <summary>
        /// Post data to server ( Add new data)
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="uri"></param>
        /// <param name="data"></param>
        /// <param name="token"></param>
        /// <param name="header"></param>
        /// <returns></returns>
        public async Task<TResult> PostAsync<TResult>(string uri, TResult data, string token = "", string header = "")
        {
            HttpClient httpClient = CreateHttpClient(token);

            if (!string.IsNullOrEmpty(header))
            {
                AddHeaderParameter(httpClient, header);
            }

            var content = new StringContent(JsonConvert.SerializeObject(data));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = await httpClient.PostAsync(uri, content);

            await HandleResponse(response);
            string serialized = await response.Content.ReadAsStringAsync();

            TResult result = await Task.Run(() =>
                JsonConvert.DeserializeObject<TResult>(serialized, _serializerSettings));

            return result;
        }

        /// <summary>
        /// Use for case Get Token / Refresh Token ...
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="uri"></param>
        /// <param name="data"></param>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        /// <returns></returns>
        public async Task<TResult> PostAsync<TResult>(string uri, string data, string clientId, string clientSecret)
        {
            //Make fake function
            await Task.Delay(10);
            TResult result = default(TResult);
            return result;

            //HttpClient httpClient = CreateHttpClient(string.Empty);

            //if (!string.IsNullOrWhiteSpace(clientId) && !string.IsNullOrWhiteSpace(clientSecret))
            //{
            //    AddBasicAuthenticationHeader(httpClient, clientId, clientSecret);
            //}

            //var content = new StringContent(data);
            //content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            //HttpResponseMessage response = await httpClient.PostAsync(uri, content);

            //await HandleResponse(response);
            //string serialized = await response.Content.ReadAsStringAsync();

            //TResult result = await Task.Run(() =>
            //    JsonConvert.DeserializeObject<TResult>(serialized, _serializerSettings));

            //return result;
        }

        /// <summary>
        /// Update data in server ( modify old data)
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="uri"></param>
        /// <param name="data"></param>
        /// <param name="token"></param>
        /// <param name="header"></param>
        /// <returns></returns>
        public async Task<TResult> PutAsync<TResult>(string uri, TResult data, string token = "", string header = "")
        {
            HttpClient httpClient = CreateHttpClient(token);

            if (!string.IsNullOrEmpty(header))
            {
                AddHeaderParameter(httpClient, header);
            }

            var content = new StringContent(JsonConvert.SerializeObject(data));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = await httpClient.PutAsync(uri, content);

            await HandleResponse(response);
            string serialized = await response.Content.ReadAsStringAsync();

            TResult result = await Task.Run(() =>
                JsonConvert.DeserializeObject<TResult>(serialized, _serializerSettings));

            return result;
        }

        /// <summary>
        /// Delete Data in Server
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task DeleteAsync(string uri, string token = "")
        {
            HttpClient httpClient = CreateHttpClient(token);
            await httpClient.DeleteAsync(uri);
        }

    #endregion
    /// --------------------------------------------
    /// [END] Implement IRequestProvidier Area
    /// --------------------------------------------


    /// <summary>
    /// --------------------------------------------
    /// [START] Implement Logic Function Area
    /// --------------------------------------------
    /// </summary>
    #region Implement Logic Function

        private HttpClient CreateHttpClient(string token)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (!string.IsNullOrEmpty(token))
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            return httpClient;
        }

        private async Task HandleResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == HttpStatusCode.Forbidden ||
                    response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new ServiceAuthenticationException(content);
                }

                throw new HttpRequestExceptionEx(response.StatusCode, content);
            }
        }

        private void AddHeaderParameter(HttpClient httpClient, string parameter)
        {
            if (httpClient == null)
                return;

            if (string.IsNullOrEmpty(parameter))
                return;

            httpClient.DefaultRequestHeaders.Add(parameter, Guid.NewGuid().ToString());
        }

    #endregion
    /// --------------------------------------------
    /// [END] Implement Logic Function Area
    /// --------------------------------------------
    }
}
