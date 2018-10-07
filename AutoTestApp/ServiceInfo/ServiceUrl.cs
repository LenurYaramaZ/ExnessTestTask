using AutoTestApp.Framework;

namespace AutoTestApp.ServiceInfo
{
    internal class ServiceUrl
    {
        public static string BaseUrl => Config.Instance.BaseUrl;
        public static string GetVendorInfoApi => $"{BaseUrl}/api/VendorInformation";
    }
}