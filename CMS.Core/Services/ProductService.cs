using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Common.AppConstants;
using CMS.Common.Extensions;
using CMS.Common.Interfaces;
using CMS.Common.Models.ViewModels.Products;
using CMS.Core.Entities;
using CMS.Core.Interfaces;

namespace CMS.Core.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IAsyncRepository<Fields> _fieldsRepository;
        private readonly IAsyncRepository<ProductFields> _productFieldsRepository;
        public ProductService(IProductRepository productRepository, IAsyncRepository<Fields> fieldsRepository,
            IAsyncRepository<ProductFields> productFieldsRepository)
        {
            _productRepository = productRepository;
            _fieldsRepository = fieldsRepository;
            _productFieldsRepository = productFieldsRepository;
        }

        public async Task<int> AddField(FieldsModel model)
        {
            Fields fields = model.xCopyTo<Fields>();
            var res = await _fieldsRepository.AddAsync(fields);
            return res.Idx;
        }

        public async Task<int> EditField(FieldsModel model)
        {
            try
            {
                Fields fields = model.xCopyTo<Fields>();
                await _fieldsRepository.UpdateAsync(fields);
                return fields.Idx;
            }
            catch (Exception ex)
            {
                return 0;
            }

        }

        public async Task<FieldsModel> GetField(int Idx)
        {
            try
            {
                var fields = await _productRepository.GetField(Idx);
                var model = fields.xCopyTo<FieldsModel>();
                return model;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public async Task<DataTableResponse<FieldsModel>> GetFields(IDataTableRequest request)
        {
            var fields = await _productRepository.GetFields(request);

            DataTableResponse<FieldsModel> returnModel = new DataTableResponse<FieldsModel>();
            returnModel.Draw = fields.Draw;
            returnModel.RecordsFiltered = fields.RecordsFiltered;
            returnModel.RecordsTotal = fields.RecordsTotal;

            returnModel.Data = fields.Data.Select(f =>
            new FieldsModel()
            {
                Idx = f.Idx,
                InsertDate = f.InsertDate,
                InsertUserIdx = f.InsertUserIdx,
                Name_TR = f.Name_TR,
                Name_EN = f.Name_EN,
                TypeIdx = f.TypeIdx,
                TypeName = f.Types.Name_TR,
                IsRequired = f.IsRequired,
                MaxLength = f.MaxLength,
                MinLength = f.MinLength
            }).ToList();
            return returnModel;
        }

        public async Task<List<FieldsModel>> GetFields()
        {
            var fields = await _productRepository.GetFields();
            List<FieldsModel> fieldsModels = new List<FieldsModel>();
            foreach (var field in fields)
            {
                fieldsModels.Add(field.xCopyTo<FieldsModel>());
            }
            return fieldsModels;
        }
        public async Task<bool> IsNameUnique(string Name_TR, string Name_EN,int Idx)
        {
            return await _productRepository.IsNameUnique(Name_TR, Name_EN,Idx);
        }

        public async Task<int> AddProduct(AddProductModel model)
        {
            try
            {
                Products product = model.xCopyTo<Products>();
                product.InsertUserIdx = model.UserIdx;
                product.InsertDate = DateTime.Now;
                await _productRepository.AddAsync(product);

                foreach (var field in model.Fields)
                {
                    ProductFields productFields = new ProductFields
                    {
                        ProductIdx = product.Idx,
                        InsertUserIdx = model.UserIdx,
                        InsertDate = DateTime.Now,
                        FieldIdx = field,
                        Status = (int)Common.AppConstants.GeneralStatus.Active
                    };
                    await _productFieldsRepository.AddAsync(productFields);
                }

                return product.Idx;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public async Task<DataTableResponse<ProductModel>> GetProducts(IDataTableRequest request)
        {

            var products = await _productRepository.ListAsync(request);

            DataTableResponse<ProductModel> returnModel = new DataTableResponse<ProductModel>();
            returnModel.Draw = products.Draw;
            returnModel.RecordsFiltered = products.RecordsFiltered;
            returnModel.RecordsTotal = products.RecordsTotal;

            returnModel.Data = products.Data.Select(f =>
            new ProductModel()
            {
                Idx = f.Idx,
                InsertDate = f.InsertDate,
                InsertUserIdx = f.InsertUserIdx,
                Name_TR = f.Name_TR,
                Name_EN = f.Name_EN,
                Description_EN = f.Description_EN,
                Description_TR = f.Description_TR
            }).ToList();
            return returnModel;
        }
        public async Task<List<ProductModel>> GetProducts()
        {
            var products = await _productRepository.ListAllAsync();
            List<ProductModel> productModels = new List<ProductModel>();
            foreach (var product in products)
            {
                ProductModel productModel = new ProductModel
                {
                    Idx = product.Idx,
                    Description_EN = product.Description_EN,
                    Description_TR = product.Description_TR,
                    InsertDate = product.InsertDate,
                    InsertUserIdx = product.InsertUserIdx,
                    Name_EN = product.Name_EN,
                    Name_TR = product.Name_TR,
                    UpdateDate = product.UpdateDate,
                    UpdateUserIdx = product.UpdateUserIdx
                };
                productModels.Add(productModel);
            }
            return productModels;
        }
        public async Task<int> EditProduct(AddProductModel model)
        {
            try
            {
                Products product = model.xCopyTo<Products>();
                product.UpdateUserIdx = model.UserIdx;
                product.UpdateDate = DateTime.Now;
                await _productRepository.UpdateAsync(product);

                var productModel = await _productRepository.GetProduct(model.Idx);



                foreach (var field in model.Fields)
                {
                    // Yeni eklenen
                    if (!productModel.ProductFields.Any(r => r.FieldIdx == field))
                    {
                        ProductFields productFields = new ProductFields
                        {
                            ProductIdx = product.Idx,
                            InsertUserIdx = model.UserIdx,
                            InsertDate = DateTime.Now,
                            FieldIdx = field,
                            Status = (int)Common.AppConstants.GeneralStatus.Active
                        };
                        await _productFieldsRepository.AddAsync(productFields);
                    }

                }
                var fieldList = new List<ProductFields>();
                fieldList = productModel.ProductFields.ToList();
                foreach (var field in fieldList)
                {
                    // Çıkarılan
                    if (!model.Fields.Any(r => r == field.FieldIdx))
                    {
                        ProductFields productFields = new ProductFields
                        {
                            Idx = field.Idx,
                            ProductIdx = product.Idx,
                            InsertUserIdx = model.UserIdx,
                            InsertDate = DateTime.Now,
                            FieldIdx = field.FieldIdx,
                            Status = (int)Common.AppConstants.GeneralStatus.Passsive
                        };
                        await _productFieldsRepository.DeleteAsync(productFields);
                    }

                  
                }
                return product.Idx;
            }
            catch (Exception ex)
            {

                throw ex;
            }
          
        }

        public async Task<ProductModel> GetProductModel(int Idx)
        {
            var products = await _productRepository.GetProduct(Idx);
            ProductModel model = new ProductModel();
            model.Name_EN = products.Name_EN;
            model.Name_TR = products.Name_TR;
            model.Description_TR = products.Description_TR;
            model.Description_EN = products.Description_EN;
            model.InsertUserIdx = products.InsertUserIdx;
            model.InsertDate = products.InsertDate;
            model.Idx = products.Idx;
            model.ProductFieldsModel = new ProductFieldsModel
            {
                ProductIdx = products.Idx,
                FieldsModels = new List<FieldsModel>()
            };
            foreach (var productFields in products.ProductFields.Where(r=> r.Status == (int)GeneralStatus.Active))
            {
                FieldsModel fieldsModel = new FieldsModel();
                fieldsModel.Idx = productFields.FieldIdx;
                fieldsModel.Name_TR = productFields.Fields.Name_TR;
                fieldsModel.Name_EN = productFields.Fields.Name_EN;
                fieldsModel.IsRequired = productFields.Fields.IsRequired;
                fieldsModel.MaxLength = productFields.Fields.MaxLength;
                fieldsModel.MinLength = productFields.Fields.MinLength;
                fieldsModel.TypeName = productFields.Fields.Types.Name_TR;
                model.ProductFieldsModel.FieldsModels.Add(fieldsModel);
            }
            return model;
        }

    }
}
