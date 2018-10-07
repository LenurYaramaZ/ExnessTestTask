using AutoTestApp.ServiceInfo;
using NUnit.Framework;
using RestSharp;
using System.Net;

namespace AutoTestApp.Api
{
    public class BaseApi
    {
        protected RestRequest _restRequest;
        private RestClient _apiClient;

        protected RestClient VendorServiceClient => _apiClient ?? (_apiClient = new RestClient(ServiceUrl.BaseUrl));

        protected IRestResponse<T> ExecuteRequest<T>()
        {
            IRestResponse originalResponse = VendorServiceClient.Execute(_restRequest);

            IRestResponse<T> parsedResponse = VendorServiceClient.Deserialize<T>(originalResponse);

            return parsedResponse;
        }

        public void CheckResponse<T>(IRestResponse<T> response, HttpStatusCode expectedStatus)
        {
            if (response == null || response.Data == null) Assert.Fail("Response is null or can't be parsed.");

            Assert.AreEqual(expectedStatus, response.StatusCode, "Wrong actual API Response StatusCode.");
        }
    }
}
