using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using VendorService.Models.BusinessObjects;
using VendorService.Services;

namespace VendorService.Controllers
{
    public class VendorInformationController : ApiController
    {
        public HttpResponseMessage GetVendorInformation(string id)
        {
            VendorInformationBo vendorModel = null;

            bool isIdInGuidFormat = Guid.TryParseExact(id, "D", out Guid guid);

            if (isIdInGuidFormat)
            {
                IVendorInformationService service = new VendorInformationService();
                vendorModel = service.GetVendorInformation(guid);
            }

            if (vendorModel == null)
            {
                string message = $"Vendor {id} is not found";
                HttpError err = new HttpError(message);
                return Request.CreateResponse(HttpStatusCode.NotFound, err);
            }

            return Request.CreateResponse(HttpStatusCode.OK, vendorModel);
        }
    }
}
