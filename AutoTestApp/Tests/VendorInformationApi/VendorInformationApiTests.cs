using AutoTestApp.Api;
using AutoTestApp.Models.Api.GetVendorInformation.Response;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using VendorService.Models.BusinessObjects;

namespace AutoTestApp.Tests.VendorInformationApi
{
    [TestFixture]
    public class VendorInformationApiTests : BaseTest
    {
        private GetVendorInformationApi _getVendorInformationApi;
        private VendorInformationBo _vendor1;
        private VendorInformationBo _vendor2;

        [SetUp]
        public void ResetTestData()
        {
            _vendor1 = new VendorInformationBo
            {
                Id = Guid.NewGuid(),
                Name = "Test corporation",
                Rating = 2,
                Categories = new List<CategoryBo>
                {
                    new CategoryBo
                    {
                        Id = Guid.NewGuid(),
                        Name = "Test name"
                    },
                    new CategoryBo
                    {
                        Id= Guid.NewGuid(),
                        Name = "Test name2"
                    }
                }
            };

            _vendor2 = new VendorInformationBo
            {
                Id = Guid.NewGuid(),
                Name = "Test vendor",
                Rating = 99,
                Categories = new List<CategoryBo>
                {
                    new CategoryBo
                    {
                        Id = Guid.NewGuid(),
                        Name = "Test software for second vendor, first category"
                    }
                }
            };
        }

        [Test]
        [Description(@"
        1. Insert vendors information to DB.
        2. Call request with valid ID.
            Check:
            - Check vendor id")]
        public void CheckVendorId()
        {
            _vendorInformationService.InsertVendorInformation(_vendor1);
            _vendorInformationService.InsertVendorInformation(_vendor2);

            GetVendorInformationApiResponse response = GetVendorInformationApi.CallGetVendorInfoApi(_vendor1.Id);
            Assert.AreEqual(_vendor1.Id, response.Id, "Check vendor id");
        }

        [Test]
        [Description(@"
        1. Insert vendors information to DB.
        2. Call request with valid ID.
            Check:
            - Check vendor name")]
        public void CheckVendorName()
        {
            _vendorInformationService.InsertVendorInformation(_vendor1);
            _vendorInformationService.InsertVendorInformation(_vendor2);

            GetVendorInformationApiResponse response = GetVendorInformationApi.CallGetVendorInfoApi(_vendor1.Id);
            Assert.AreEqual(_vendor1.Name, response.Name, "Check vendor name");
        }

        [Test]
        [Description(@"
        1. Insert vendors information to DB.
        2. Set vendor name as empty.
        3. Call request with valid ID.
            Check:
            - Check vendor name")]
        public void CheckVendorEmptyName()
        {
            _vendor1.Name = null;

            _vendorInformationService.InsertVendorInformation(_vendor1);
            _vendorInformationService.InsertVendorInformation(_vendor2);

            GetVendorInformationApiResponse response = GetVendorInformationApi.CallGetVendorInfoApi(_vendor1.Id);
            Assert.AreEqual(_vendor1.Name, response.Name, "Check vendor name");
        }

        [Test]
        [Description(@"
        1. Insert vendors information to DB.
        2. Call request with valid ID.
            Check:
            - Check vendor Rating")]
        public void CheckVendorRating()
        {
            _vendorInformationService.InsertVendorInformation(_vendor1);
            _vendorInformationService.InsertVendorInformation(_vendor2);

            GetVendorInformationApiResponse response = GetVendorInformationApi.CallGetVendorInfoApi(_vendor1.Id);
            Assert.AreEqual(_vendor1.Rating, response.Rating, "Check vendor Rating");
        }

        [Test]
        [Description(@"
        1. Insert vendors information to DB.
        2. Set vendor Rating as empty.
        3. Call request with valid ID.
            Check:
            - Check vendor Rating")]
        public void CheckVendorEmptyRating()
        {
            _vendor1.Rating = null;

            _vendorInformationService.InsertVendorInformation(_vendor1);
            _vendorInformationService.InsertVendorInformation(_vendor2);

            GetVendorInformationApiResponse response = GetVendorInformationApi.CallGetVendorInfoApi(_vendor1.Id);
            Assert.AreEqual(_vendor1.Rating, response.Rating, "Check vendor Rating");
        }

        [Test]
        [Description(@"
        1. Insert vendors information to DB.
        2. Call request with valid ID.
            Check:
            - Check vendor categories.")]
        public void CheckVendorCategories()
        {
            _vendorInformationService.InsertVendorInformation(_vendor1);
            _vendorInformationService.InsertVendorInformation(_vendor2);

            GetVendorInformationApiResponse response = GetVendorInformationApi.CallGetVendorInfoApi(_vendor1.Id);

            foreach (CategoryBo category in _vendor1.Categories)
            {
                Assert.True(response.Categories.Any(x => x.Id.Equals(category.Id)), "Category exist in the list");
                Assert.AreEqual(category.Name, response.Categories.Find(x => x.Id.Equals(category.Id)).Name, "Check Category Name");
            }
        }

        [Test]
        [Description(@"
        1. Insert vendors information to DB.
        2. Set vendor Categories list as empty.
        3. Call request with valid ID.
            Check:
            - Check vendor categories count.")]
        public void CheckVendorEmptyCategoriesList()
        {
            _vendor1.Categories = null;

            _vendorInformationService.InsertVendorInformation(_vendor1);
            _vendorInformationService.InsertVendorInformation(_vendor2);

            GetVendorInformationApiResponse response = GetVendorInformationApi.CallGetVendorInfoApi(_vendor1.Id);
            Assert.Zero(response.Categories.Count, "Check vendor categories count");
        }

        [Test]
        [Description(@"
        1. Insert vendors information to DB.
        2. Set empty name for vendor category.
        3. Call request with valid ID.
            Check:
            - Check vendor categories.")]
        public void CheckVendorWithEmptyCategoryName()
        {
            _vendor1.Categories.First().Name = null;

            _vendorInformationService.InsertVendorInformation(_vendor1);
            _vendorInformationService.InsertVendorInformation(_vendor2);

            GetVendorInformationApiResponse response = GetVendorInformationApi.CallGetVendorInfoApi(_vendor1.Id);

            foreach (CategoryBo category in _vendor1.Categories)
            {
                Assert.True(response.Categories.Any(x => x.Id.Equals(category.Id)), "Category exist in the list");
                Assert.AreEqual(category.Name, response.Categories.Find(x => x.Id.Equals(category.Id)).Name, "Check Category Name");
            }
        }

        [Test]
        [Description(@"
        1. Insert vendors information to DB.
        2. Call request with Not Exist ID.
            Check:
            - Check StatusCode
            - Server returns correct message for not existing id")]
        public void CheckResponseMessageForNotExistId()
        {
            _vendorInformationService.InsertVendorInformation(_vendor1);
            _vendorInformationService.InsertVendorInformation(_vendor2);

            Guid guid = Guid.Empty;

            GetVendorInformationApiResponse response = GetVendorInformationApi.CallGetVendorInfoApi(guid, HttpStatusCode.NotFound);

            Assert.AreEqual($"Vendor {guid} is not found", response.Message, "Server returns correct message for not existing id");
        }

        private GetVendorInformationApi GetVendorInformationApi
            => _getVendorInformationApi ?? (_getVendorInformationApi = new GetVendorInformationApi());
    }
}
