using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using RealEstate.Mobile.Utils;
using RealEstate.Models.API;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RealEstate.Mobile.Services.SAL
{
    public class SimpleService
    {
        HttpClient _client => HttpClientManager.Instance.HttpClient;

        static SimpleService()
        {
        }

        public static JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
			DateTimeZoneHandling = DateTimeZoneHandling.Local
        };

		public static JsonSerializer JsonSerializer = new JsonSerializer();

        public async Task<RealEstatetServicesResponseWrapper<TResponse, TError>> Get<TResponse, TError>(
            string url,
            object queryObject = null,
            Func<JToken, TResponse> manualDataMapping = null,
            CancellationToken ctoken = new CancellationToken())
        {
            if (queryObject != null)
            {
                string query = ConvertObjectToQueryUrl(queryObject);
                if (string.IsNullOrEmpty(query) == false)
                {
                    if (url.Contains("?") == false) url += "?";
                    url += "&" + query;
                }
            }

            RealEstatetServicesResponseWrapper<TResponse, TError> result = new RealEstatetServicesResponseWrapper<TResponse, TError>();
			List<Exception> errorContainer = new List<Exception>();

			try
            {
                Uri uri = new Uri(string.Format(url, string.Empty));

                HttpResponseMessage response = await HttpClientManager.Instance.HttpClient.FillAuthenticationHeader().GetAsync(uri, ctoken).ConfigureAwait(false);
                string content = await response.Content.ReadAsStringAsync();

                if (!string.IsNullOrWhiteSpace(content)) {
                    result.Response = MapData<TResponse, TError>(content);
                }
                HandleServiceErrors(result, response);
			}
            catch (Exception e)
            {
				errorContainer.Add(e);
			}

			HandleErrors(result, errorContainer);

			return result;
        }

        RealEstateResponseWithIssue<TResponse, TError> MapData<TResponse, TError>(string content)
        {
            return JsonConvert.DeserializeObject<RealEstateResponseWithIssue<TResponse, TError>>(content, JsonSerializerSettings);
        }

        public async Task<RealEstatetServicesResponseWrapper<TResponse, TError>> Post<TResponse, TError>(
            string url,
            object parameters,
            CancellationToken ctoken = new CancellationToken())
        {
            string postPayload = JsonConvert.SerializeObject(parameters, JsonSerializerSettings);
            return await InnerPost<TResponse, TError>(
                url, 
                new StringContent(content: postPayload, encoding: Encoding.UTF8, mediaType: "application/json"), ctoken)
                .ConfigureAwait(false);
        }

        public async Task<RealEstatetServicesResponseWrapper<bool, NullServiceObject>> SimplePost(string url, object parameters = null, CancellationToken ctoken = new CancellationToken())
        {
            string postPayload = parameters != null
                ? JsonConvert.SerializeObject(parameters, JsonSerializerSettings)
                : null;

            return await Post<bool, NullServiceObject>(url, postPayload, ctoken);
        }

        public async Task<RealEstatetServicesResponseWrapper<TResponse, TError>> UploadFile<TResponse, TError>(
            string url,
            string fileName,
            byte[] files,
            CancellationToken ctoken = new CancellationToken())
        {

            MultipartFormDataContent multiPartFormData =
                new MultipartFormDataContent
                {
                    {new ByteArrayContent(files), "file", fileName}
                };

            return await InnerPost<TResponse, TError>(url, multiPartFormData, ctoken).ConfigureAwait(false);
        }

        public async Task<RealEstatetServicesResponseWrapper<TResponse, TError>> UploadFileStream<TResponse, TError>(
            string url,
            string fileName,
            Stream fileStream,
            CancellationToken ctoken = new CancellationToken())
        {
            HttpContent fileStreamContent = new StreamContent(fileStream);
            fileStreamContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data") { Name = "file", FileName = fileName };
            fileStreamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
            MultipartFormDataContent multiPartFormData = new MultipartFormDataContent();
            multiPartFormData.Add(fileStreamContent);

            return await InnerPost<TResponse, TError>(url, multiPartFormData, ctoken).ConfigureAwait(false);
        }

        async Task<RealEstatetServicesResponseWrapper<TResponse, TError>> InnerPost<TResponse, TError>(
            string url,
            HttpContent uploadContent,
            CancellationToken ctoken = new CancellationToken())
        {
            RealEstatetServicesResponseWrapper<TResponse, TError> result =
                new RealEstatetServicesResponseWrapper<TResponse, TError>();
            List<Exception> errorContainer = new List<Exception>();

            try
            {
                Uri uri = new Uri(string.Format(url, string.Empty));


                HttpResponseMessage response = await HttpClientManager.Instance.HttpClient.FillAuthenticationHeader().PostAsync(
                    uri,
                    uploadContent,
                    ctoken);
                string content = await response.Content.ReadAsStringAsync();

                if (!string.IsNullOrWhiteSpace(content))
                {
                    result.Response = MapData<TResponse, TError>(content);
                }
                HandleServiceErrors(result, response);
            }
            catch (Exception e)
            {
                errorContainer.Add(e);
            }
            HandleErrors(result, errorContainer);
            return result;
        }

        public void HandleServiceErrors<TResponse, TError>(RealEstatetServicesResponseWrapper<TResponse, TError> aulaResponse, HttpResponseMessage response) {
			if (!response.IsSuccessStatusCode) {
				aulaResponse.HttpCode = response.StatusCode;
			}
		}

		public RealEstatetServicesResponseWrapper<TResponse, TError> HandleException<TResponse, TError>(RealEstatetServicesResponseWrapper<TResponse, TError> result, Exception e)
        {

            if (e is HttpRequestException exception)
            {
                result.Exception = exception;
            }
            else if (e is TaskCanceledException canceledException)
            {
                result.Exception = canceledException;
			}
			else if (e is JsonException jsonException)
			{
			    result.Exception = jsonException;
			}
			else
            {
                result.Exception = e;
			}

            return result;
        }

		public void HandleErrors<TResponse, TError>(RealEstatetServicesResponseWrapper<TResponse, TError> result, List<Exception> exceptions) {

			if (exceptions.Any()) {
				HandleException(result, exceptions[0]);
			}
		}

        public static string ConvertObjectToQueryUrl(object instance)
        {
            StringBuilder urlBuilder = new StringBuilder();
            List<PropertyInfo> properties = instance.GetType().GetRuntimeProperties().ToList();
            for (int i = 0; i < properties.Count; i++)
            {
                object value = properties[i].GetValue(instance, null);
                string nameStr = properties[i].Name;
                if (char.IsUpper(nameStr[0]))
                {
                    nameStr = char.ToLowerInvariant(nameStr[0]) + nameStr.Substring(1);
                }

                if (value is IEnumerable<string> valueEnumerable)
                {
                    foreach (string valueStr in valueEnumerable)
                    {
                        urlBuilder.AppendFormat("{0}[]={1}&", nameStr, valueStr);
                    }
                }
				else if( value is int[] valueArray)
				{
					foreach(int itemValue in valueArray)
					{
						urlBuilder.AppendFormat("{0}[]={1}&", nameStr, itemValue.ToString());
					}
				}
                else if (value is IEnumerable<int> listValueEnumerable)
                {
                    foreach (int valueStr in listValueEnumerable)
                    {
                        urlBuilder.AppendFormat("{0}[]={1}&", nameStr, valueStr);
                    }
                }
				else	
                {
                    if (value == null)
                    {
                        continue;
                    }
                    string valueStr;
                    if (value is DateTime dateTime)
                    {
                        DateTime localDateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day,
                            dateTime.Hour, dateTime.Minute, dateTime.Second, dateTime.Millisecond, DateTimeKind.Local);

                        valueStr = localDateTime.ToString("o");
                    }
                    else if (value is bool)
                    {
                        valueStr = value.ToString().ToLower();
                    }
                    else 
                    {
                        valueStr = value?.ToString() ?? string.Empty;
                    }

                    urlBuilder.AppendFormat("{0}={1}&", nameStr, UrlEncodeQuery(valueStr));
                }
            }
            if (urlBuilder.Length > 1)
            {
                urlBuilder.Remove(urlBuilder.Length - 1, 1);
            }
            return urlBuilder.ToString();
        }

        static string UrlEncodeQuery(string query)
        {
            return WebUtility.UrlEncode(query);
        }
    }

    // Fake Object for mapping when there is no data returned from server
    public class NullServiceObject
    {

    }
}