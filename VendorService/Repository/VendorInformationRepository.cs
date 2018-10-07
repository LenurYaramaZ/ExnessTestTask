using System;
using LiteDB;
using System.Linq;
using VendorService.Models.DbTableModels;

namespace VendorService.Repository
{
    internal class VendorInformationRepository : IVendorInformationRepository
    {
        private readonly string _filePath;
        private const string COLLECTION_NAME = "VendorInformation";

        public VendorInformationRepository(string filePath)
        {
            _filePath = filePath;
        }

        public void DropDb()
        {
            using (LiteDatabase database = new LiteDatabase(_filePath))
            {
                database.DropCollection(COLLECTION_NAME);
            }
        }

        public void InsertVendorInformation(VendorInformationDbModel vendorInfo)
        {
            using (LiteDatabase database = new LiteDatabase(_filePath))
            {
                var collection = database.GetCollection<VendorInformationDbModel>(COLLECTION_NAME);
                collection.Insert(vendorInfo);
            }
        }

        public VendorInformationDbModel GetVendorInformation(Guid id)
        {
            using (LiteDatabase database = new LiteDatabase(_filePath))
            {
                LiteCollection<VendorInformationDbModel> result = database.GetCollection<VendorInformationDbModel>(COLLECTION_NAME);

                return result.Find(x => x.Id.Equals(id)).FirstOrDefault();
            }
        }
    }
}