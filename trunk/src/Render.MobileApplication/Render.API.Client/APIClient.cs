using System.Net;
using System.Threading;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PDRMobile.API.Client.APIModels;

namespace Render.API.Client
{
    public class APIClient : IDisposable
    {
        public HttpClient Client = null;

        private string BaseUrl = null;

        private string UserName = null;

        private bool isInitialized, hasAuthorization;

        public bool IsInitialized { get { return isInitialized; } }

        public bool HasAuthorization { get { return hasAuthorization; } }

        public APIClient(string baseUrl, string userName = null, string password = null)
        {
            this.BaseUrl = baseUrl;

            if (userName != null || password != null)
				InitializeClient(userName, password);
        }

        private async Task<APIResponse<T>> ExecuteGetAsync<T>(string requestUri, CancellationToken cancellationToken = default(CancellationToken)) //where T : class
        {
            if (!isInitialized)
                throw new ClientException("Client is not initialized");

            var processingStopWatch = new System.Diagnostics.Stopwatch();
            processingStopWatch.Start();
            System.Diagnostics.Debug.WriteLine("ApiClient: {0} - Start - {1}", requestUri, DateTime.UtcNow);

            var apiResponse = new APIResponse<T>() { Result = default(T) };
            try
            {
                using (var responseMessage = await this.Client.GetAsync(requestUri, cancellationToken).ConfigureAwait(false))
                {
                    System.Diagnostics.Debug.WriteLine(
                        "ApiClient: {0} - Response Received - {1} - Processing Time {2}ms ({3})",
                        requestUri, DateTime.UtcNow, processingStopWatch.ElapsedMilliseconds, processingStopWatch.ElapsedTicks);

                    apiResponse.Message = responseMessage.ReasonPhrase;
                    apiResponse.Code = responseMessage.StatusCode;
                    apiResponse.Success = responseMessage.IsSuccessStatusCode;
                    apiResponse.Response = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);

                    System.Diagnostics.Debug.WriteLine(
                        "ApiClient: {0} - Response Read - {1} - Processing Time {2}ms ({3})",
                        requestUri, DateTime.UtcNow, processingStopWatch.ElapsedMilliseconds, processingStopWatch.ElapsedTicks);

                    if (apiResponse.Success)
                    {
                        if (!String.IsNullOrWhiteSpace(apiResponse.Response))
                        {
                            System.Diagnostics.Debug.WriteLine(
                                "ApiClient: {0} - Response Received - {1}",
                                requestUri, apiResponse.Response);

                            System.Diagnostics.Debug.WriteLine(
                                "ApiClient: {0} - Start Deserialization - {1} - Processing Time {2}ms ({3})",
                                requestUri, DateTime.UtcNow, processingStopWatch.ElapsedMilliseconds, processingStopWatch.ElapsedTicks);

                            apiResponse.Result = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<T>(apiResponse.Response), cancellationToken).ConfigureAwait(false);

                            System.Diagnostics.Debug.WriteLine(
                                "ApiClient: {0} - End Deserialization - {1} - Processing Time {2}ms ({3})",
                                requestUri, DateTime.UtcNow, processingStopWatch.ElapsedMilliseconds, processingStopWatch.ElapsedTicks);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                apiResponse.Success = false;
                apiResponse.Code = HttpStatusCode.InternalServerError;
                apiResponse.Message = ex.Message;
            }
            finally
            {
                processingStopWatch.Stop();
                System.Diagnostics.Debug.WriteLine(
                    "ApiClient: {0} - Finished - {1} - Processing Time {2}ms ({3})",
                    requestUri, DateTime.UtcNow, processingStopWatch.ElapsedMilliseconds, processingStopWatch.ElapsedTicks);
            }

            return apiResponse;
        }

        private async Task<APIResponse<T>> ExecutePostAsync<T>(string requestUri, HttpContent content, CancellationToken cancellationToken = default(CancellationToken), bool bypassInitialization = false) //where T : BaseModel
        {
            if (!isInitialized && !bypassInitialization)
                throw new ClientException("Client is not initialized");

            var processingStopWatch = new System.Diagnostics.Stopwatch();
            processingStopWatch.Start();
            System.Diagnostics.Debug.WriteLine("ApiClient: {0} - Start - {1}", requestUri, DateTime.UtcNow);

            var apiResponse = new APIResponse<T>() { Result = default(T) };
            try
            {
                System.Diagnostics.Debug.WriteLine(
                    "ApiClient: {0} - Start Post - {1} - Processing Time {2}ms ({3})",
                    requestUri, DateTime.UtcNow, processingStopWatch.ElapsedMilliseconds, processingStopWatch.ElapsedTicks);

                using (var responseMessage = await this.Client.PostAsync(requestUri, content, cancellationToken).ConfigureAwait(false))
                {
                    apiResponse.Message = responseMessage.ReasonPhrase;
                    apiResponse.Code = responseMessage.StatusCode;
                    apiResponse.Success = responseMessage.IsSuccessStatusCode;

                    System.Diagnostics.Debug.WriteLine(
                        "ApiClient: {0} - End Post - {1} ({2}) - Processing Time {3}ms ({4})",
                        requestUri, apiResponse.Message, apiResponse.Code, processingStopWatch.ElapsedMilliseconds, processingStopWatch.ElapsedTicks);

                    System.Diagnostics.Debug.WriteLine(
                        "ApiClient: {0} - Start Read - {1} - Processing Time {2}ms ({3})",
                        requestUri, DateTime.UtcNow, processingStopWatch.ElapsedMilliseconds, processingStopWatch.ElapsedTicks);

                    apiResponse.Response = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);

                    System.Diagnostics.Debug.WriteLine(
                        "ApiClient: {0} - End Read - {1} - Processing Time {2}ms ({3})",
                        requestUri, DateTime.UtcNow, processingStopWatch.ElapsedMilliseconds, processingStopWatch.ElapsedTicks);

                    if (apiResponse.Success)
                    {
                        if (!String.IsNullOrWhiteSpace(apiResponse.Response))
                        {
                            System.Diagnostics.Debug.WriteLine(
                                "ApiClient: {0} - Response Received - {1}",
                                requestUri, apiResponse.Response);

                            System.Diagnostics.Debug.WriteLine(
                                "ApiClient: {0} - Start Deserialize - {1} - Processing Time {2}ms ({3})",
                                requestUri, DateTime.UtcNow, processingStopWatch.ElapsedMilliseconds, processingStopWatch.ElapsedTicks);

                            apiResponse.Result = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<T>(apiResponse.Response), cancellationToken).ConfigureAwait(false);

                            System.Diagnostics.Debug.WriteLine(
                                "ApiClient: {0} - End Deserialize - {1} - Processing Time {2}ms ({3})",
                                requestUri, DateTime.UtcNow, processingStopWatch.ElapsedMilliseconds, processingStopWatch.ElapsedTicks);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception: ", ex);

                apiResponse.Success = false;
                apiResponse.Code = HttpStatusCode.InternalServerError;
                apiResponse.Message = ex.Message;
            }
            finally
            {
                processingStopWatch.Stop();
                System.Diagnostics.Debug.WriteLine(
                    "ApiClient: {0} - Finished - {1} - Processing Time {2}ms ({3})",
                    requestUri, DateTime.UtcNow, processingStopWatch.ElapsedMilliseconds, processingStopWatch.ElapsedTicks);
            }

            return apiResponse;
        }

        private async Task<APIResponse<T>> ExecutePostAsync<T>(string requestUri, object o, CancellationToken cancellationToken = default(CancellationToken), bool bypassInitialization = false) //where T : BaseModel
        {
            if (!isInitialized && !bypassInitialization)
                throw new ClientException("Client is not initialized");

            var processingStopWatch = new System.Diagnostics.Stopwatch();
            processingStopWatch.Start();
            System.Diagnostics.Debug.WriteLine("ApiClient: {0} - Start - {1}", requestUri, DateTime.UtcNow);

            var apiResponse = new APIResponse<T>() { Result = default(T) };
            try
            {
                System.Diagnostics.Debug.WriteLine(
                    "ApiClient: {0} - Start Post - {1} - Processing Time {2}ms ({3})",
                    requestUri, DateTime.UtcNow, processingStopWatch.ElapsedMilliseconds, processingStopWatch.ElapsedTicks);

                using (var responseMessage = await this.Client.PostAsync(requestUri, new StringContent(await Task.Factory.StartNew(() => JsonConvert.SerializeObject(o), cancellationToken).ConfigureAwait(false), Encoding.UTF8, "application/json"), cancellationToken).ConfigureAwait(false))
                {
                    apiResponse.Message = responseMessage.ReasonPhrase;
                    apiResponse.Code = responseMessage.StatusCode;
                    apiResponse.Success = responseMessage.IsSuccessStatusCode;

                    System.Diagnostics.Debug.WriteLine(
                        "ApiClient: {0} - End Post - {1} ({2}) - Processing Time {3}ms ({4})",
                        requestUri, apiResponse.Message, apiResponse.Code, processingStopWatch.ElapsedMilliseconds, processingStopWatch.ElapsedTicks);

                    System.Diagnostics.Debug.WriteLine(
                        "ApiClient: {0} - Start Read - {1} - Processing Time {2}ms ({3})",
                        requestUri, DateTime.UtcNow, processingStopWatch.ElapsedMilliseconds, processingStopWatch.ElapsedTicks);

                    apiResponse.Response = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);

                    System.Diagnostics.Debug.WriteLine(
                        "ApiClient: {0} - End Read - {1} - Processing Time {2}ms ({3})",
                        requestUri, DateTime.UtcNow, processingStopWatch.ElapsedMilliseconds, processingStopWatch.ElapsedTicks);

                    if (apiResponse.Success)
                    {
                        if (!String.IsNullOrWhiteSpace(apiResponse.Response))
                        {
                            System.Diagnostics.Debug.WriteLine(
                                "ApiClient: {0} - Response Received - {1}",
                                requestUri, apiResponse.Response);

                            System.Diagnostics.Debug.WriteLine(
                                "ApiClient: {0} - Start Deserialize - {1} - Processing Time {2}ms ({3})",
                                requestUri, DateTime.UtcNow, processingStopWatch.ElapsedMilliseconds, processingStopWatch.ElapsedTicks);

                            apiResponse.Result = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<T>(apiResponse.Response), cancellationToken).ConfigureAwait(false);

                            System.Diagnostics.Debug.WriteLine(
                                "ApiClient: {0} - End Deserialize - {1} - Processing Time {2}ms ({3})",
                                requestUri, DateTime.UtcNow, processingStopWatch.ElapsedMilliseconds, processingStopWatch.ElapsedTicks);
                        }
                    }
                    else
                        System.Diagnostics.Debug.WriteLine("ApiClient: Exception - {0} - {1}", apiResponse.Code, apiResponse.Message);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(
                    "ApiClient: {0} - Exception - {1} - Processing Time {2}ms ({3})",
                    requestUri, ex, processingStopWatch.ElapsedMilliseconds, processingStopWatch.ElapsedTicks);

                apiResponse.Success = false;
                apiResponse.Code = HttpStatusCode.InternalServerError;
                apiResponse.Message = ex.Message;
            }
            finally
            {
                processingStopWatch.Stop();
                System.Diagnostics.Debug.WriteLine(
                    "ApiClient: {0} - Finished - {1} - Processing Time {2}ms ({3})",
                    requestUri, DateTime.UtcNow, processingStopWatch.ElapsedMilliseconds, processingStopWatch.ElapsedTicks);
            }

            return apiResponse;
        }

        private async Task<APIResponse<T>> ExecutePutAsync<T>(string requestUri, object o, CancellationToken cancellationToken = default(CancellationToken)) //where T : BaseModel
        {
            if (!isInitialized)
                throw new ClientException("Client is not initialized");

            var apiResponse = new APIResponse<T>() { Result = default(T) };
            try
            {
                using (var responseMessage = await this.Client.PutAsync(requestUri, new StringContent(await Task.Factory.StartNew(() => JsonConvert.SerializeObject(o), cancellationToken).ConfigureAwait(false), Encoding.UTF8, "application/json"), cancellationToken).ConfigureAwait(false))
                {
                    apiResponse.Message = responseMessage.ReasonPhrase;
                    apiResponse.Code = responseMessage.StatusCode;
                    apiResponse.Success = responseMessage.IsSuccessStatusCode;
                    apiResponse.Response = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);

                    if (apiResponse.Success)
                    {
                        if (!String.IsNullOrWhiteSpace(apiResponse.Response))
                        {
                            apiResponse.Result = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<T>(apiResponse.Response), cancellationToken).ConfigureAwait(false);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                apiResponse.Success = false;
                apiResponse.Code = HttpStatusCode.InternalServerError;
                apiResponse.Message = ex.Message;
            }

            return apiResponse;
        }

        private async Task<APIResponse<bool>> ExecuteDeleteAsync(string requestUri, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (!isInitialized)
                throw new ClientException("Client is not initialized");

            var apiResponse = new APIResponse<bool>() { Result = false };
            try
            {
                using (var responseMessage = await this.Client.DeleteAsync(requestUri, cancellationToken).ConfigureAwait(false))
                {
                    apiResponse.Message = responseMessage.ReasonPhrase;
                    apiResponse.Code = responseMessage.StatusCode;
                    apiResponse.Success = responseMessage.IsSuccessStatusCode;
                    apiResponse.Result = responseMessage.IsSuccessStatusCode;
                    apiResponse.Response = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                apiResponse.Success = false;
                apiResponse.Code = HttpStatusCode.InternalServerError;
                apiResponse.Message = ex.Message;
            }

            return apiResponse;
        }

		public HttpClient InitializeClient(string username = null, string password = null)
		{
			if (!string.IsNullOrEmpty(username))
				this.UserName = username;

			//TODO: Add in support for modern http client once we have a real certificate...
			this.Client = new HttpClient(/*new NativeMessageHandler()*/);
			this.Client.BaseAddress = new Uri(this.BaseUrl);
			this.Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

			if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
			{
				this.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("basic", Convert.ToBase64String(Encoding.UTF8.GetBytes(String.Format("{0}:{1}", this.UserName, password))));
				hasAuthorization = true;
			}

			isInitialized = true;

			return Client;
		}

        public async Task<HttpClient> InitializeClientAsync(string username = null, string password = null)
        {
			return await Task.Run(() => InitializeClient(username, password)).ConfigureAwait(false);
        }

        public void Dispose()
        {
            if (this.Client != null)
            {
                this.Client.Dispose();
            }
        }


		public async Task<IList<AuthenticationResult>> authenticate(string username, string password, 
			CancellationToken cancellationToken = default(CancellationToken))
		{

			AuthenticationModel _model = new AuthenticationModel ();
			_model.Password = password;
			_model.Username = username;

			string jsonStr =  JsonConvert.SerializeObject (_model); //await Task.Factory.StartNew (() => JsonConvert.SerializeObject (_model), cancellationToken).ConfigureAwait (false);
			var _content = new StringContent(jsonStr, Encoding.UTF8, "application/json");

			var result = await ExecutePostAsync<IList<AuthenticationResult>>("authenticate", _content, cancellationToken).ConfigureAwait(false);

			return result.Result;
		}

		public async Task<bool> getactivity(AuthenticationResult auth,CancellationToken cancellationToken = default(CancellationToken))
		{
			this.Client.DefaultRequestHeaders.Add ("account-name", auth.AccountName);
			this.Client.DefaultRequestHeaders.Add ("token", auth.Token);

			this.Client.DefaultRequestHeaders.Authorization = 
				new AuthenticationHeaderValue("token", auth.Token);

			var result = await ExecuteGetAsync<bool> ("activity", cancellationToken);

			return true;
		}
    }
}