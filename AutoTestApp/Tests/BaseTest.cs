using NUnit.Framework;
using VendorService.Services;

namespace AutoTestApp.Tests
{
    public class BaseTest
    {
        protected IVendorInformationService _vendorInformationService;

        [OneTimeSetUp]
        public void Init()
        {
            _vendorInformationService = new VendorInformationService();
        }

        [SetUp]
        public void InitTest()
        {
            _vendorInformationService.DropVendorInformation();
        }

        [TearDown]
        public void CleanupTest()
        {
            /* ... */

        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            /* ... */
        }
    }
}
