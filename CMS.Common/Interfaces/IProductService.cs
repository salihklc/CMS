using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CMS.Common.Models.ViewModels.Products;

namespace CMS.Common.Interfaces
{
    public interface IProductService
    {
        Task<DataTableResponse<FieldsModel>> GetFields(IDataTableRequest request);
        Task<List<FieldsModel>> GetFields();
        Task<int> AddField(FieldsModel model);
        Task<FieldsModel> GetField(int Idx);
        Task<int> EditField(FieldsModel model);
        Task<bool> IsNameUnique(string Name_TR, string Name_EN,int Idx);
        Task<int> AddProduct(AddProductModel model);
        Task<DataTableResponse<ProductModel>> GetProducts(IDataTableRequest request);
        Task<List<ProductModel>> GetProducts();
        Task<int> EditProduct(AddProductModel model);
        Task<ProductModel> GetProductModel(int Idx);
    }
}
