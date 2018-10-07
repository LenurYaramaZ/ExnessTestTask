using AutoTestApp.ServiceInfo;
using RestSharp;
using System;
using System.Net;
using AutoTestApp.Models.Api.GetVendorInformation.Response;

namespace AutoTestApp.Api
{
    public class GetVendorInformationApi : BaseApi
    {
        public GetVendorInformationApi()
        {
            _restRequest = new RestRequest(ServiceUrl.GetVendorInfoApi, Method.GET);
        }

        public GetVendorInformationApiResponse CallGetVendorInfoApi(Guid guid, HttpStatusCode expectedStatus = HttpStatusCode.OK)
        {
            _restRequest.AddOrUpdateParameter("id", guid.ToString("D"), ParameterType.QueryString);

            IRestResponse<GetVendorInformationApiResponse> response = ExecuteRequest<GetVendorInformationApiResponse>();

            CheckResponse(response, expectedStatus);

            return response.Data;
        }
    }
}
