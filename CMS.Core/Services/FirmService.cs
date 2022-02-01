using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Common.AppConstants;
using CMS.Common.Extensions;
using CMS.Common.Interfaces;
using CMS.Common.Models.ViewModels.Firms;
using CMS.Common.Models.ViewModels.Tickets;
using CMS.Core.Entities;
using CMS.Core.Interfaces;

namespace CMS.Core.Services
{
    public class FirmService : IFirmService
    {
        private readonly IFirmsRepository _firmsRepository;
        private readonly IAsyncRepository<FirmProductFields> _firmProductFieldsRepository;

        public FirmService(IFirmsRepository firmsRepository, IAsyncRepository<FirmProductFields> firmProductFieldsRepository)
        {
            _firmsRepository = firmsRepository;
            _firmProductFieldsRepository = firmProductFieldsRepository;
        }

        public async Task<int> AddFirm(FirmsModel model)
        {
            var firm = model.xCopyTo<Firms>();
            var res = await _firmsRepository.AddAsync(firm);
            return res.Idx;
        }

        public async Task<int> AddFirmProducts(FirmProductFieldsModel model)
        {
            try
            {

                FirmProducts products = new FirmProducts
                {
                    FirmIdx = model.FirmIdx,
                    InsertDate = DateTime.Now,
                    Status = (int)GeneralStatus.Active,
                    ProductIdx = model.ProductIdx,
                    InsertUserIdx = model.UserIdx,
                    UpdateUserIdx = model.UserIdx,
                    UpdateDate = DateTime.Now
                };

                var firmProductIdx = await _firmsRepository.AddProduct(products);

                foreach (var field in model.ProductFields)
                {
                    FirmProductFields firmProductFields = new FirmProductFields
                    {
                        FirmProductIdx = firmProductIdx,
                        InsertUserIdx = model.UserIdx,
                        InsertDate = DateTime.Now,
                        Status = (int)GeneralStatus.Active,
                        ProductFieldIdx = field.FieldIdx,
                        Value = field.Value,
                        UpdateUserIdx = model.UserIdx,
                        UpdateDate = DateTime.Now
                    };
                    await _firmsRepository.AddFirmProducts(firmProductFields);
                }
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }


        }

        public async Task<int> AddFirms(List<FirmsModel> model)
        {
            List<Firms> firms = new List<Firms>();
            foreach (var firm in model)
            {
                firms.Add(firm.xCopyTo<Firms>());
            }
            var res = await _firmsRepository.BulkInsert(firms);
            return res;
        }

        public async Task<bool> DeleteFirm(int Idx)
        {
            var res = await _firmsRepository.DeleteFirm(Idx);
            return res;
        }

        public async Task<int> EditFirm(FirmsModel model)
        {
            var firm = model.xCopyTo<Firms>();
            await _firmsRepository.UpdateAsync(firm);
            return firm.Idx;
        }

        public async Task<int> EditFirmProducts(FirmProductFieldsModel model)
        {
            foreach (var field in model.ProductFields)
            {
                FirmProductFields firmProductFields = await _firmsRepository.GetFirmProductFields(model.FirmProductIdx, field.FieldIdx);
                if (firmProductFields != null)
                {
                    firmProductFields.UpdateDate = DateTime.Now;
                    firmProductFields.UpdateUserIdx = model.UserIdx;
                    firmProductFields.Value = field.Value;
                    await _firmProductFieldsRepository.UpdateAsync(firmProductFields);
                }
                else
                {
                    FirmProductFields newfirmProductFields = new FirmProductFields
                    {
                        FirmProductIdx = model.FirmProductIdx,
                        InsertUserIdx = model.UserIdx,
                        InsertDate = DateTime.Now,
                        Status = (int)GeneralStatus.Active,
                        ProductFieldIdx = field.FieldIdx,
                        Value = field.Value,
                        UpdateUserIdx = model.UserIdx,
                        UpdateDate = DateTime.Now
                    };
                    await _firmsRepository.AddFirmProducts(newfirmProductFields);
                }

            }

            return model.FirmProductIdx;
        }

        public async Task<bool> FirmNameIsUnique(string firmName, int Idx)
        {
            return await _firmsRepository.FirmNameIsUnique(firmName, Idx);
        }

        public async Task<FirmsModel> GetFirm(int Idx)
        {
            var firm = await _firmsRepository.GetByIdAsync(Idx);
            var firmModel = firm.xCopyTo<FirmsModel>();
            return firmModel;
        }

        public async Task<DataTableResponse<FirmProductsModels>> GetFirmProducts(IDataTableRequest request, int FirmIdx)
        {
            var firmProducts = await _firmsRepository.GetFirmProducts(request, FirmIdx);
            DataTableResponse<FirmProductsModels> returnModel = new DataTableResponse<FirmProductsModels>();
            returnModel.Draw = firmProducts.Draw;
            returnModel.RecordsFiltered = firmProducts.RecordsFiltered;
            returnModel.RecordsTotal = firmProducts.RecordsTotal;

            returnModel.Data = firmProducts.Data.Select(f =>
            new FirmProductsModels()
            {
                Idx = f.Idx,
                FirmIdx = f.FirmIdx,
                FirmName = f.Firms.FirmName,
                TaxNo = f.Firms.TaxNo,
                ProductIdx = f.ProductIdx,
                ProductName_EN = f.Products.Name_EN,
                ProductName_TR = f.Products.Name_TR
            }).ToList();

            return returnModel;
        }

        public async Task<List<FirmProductsModels>> GetFirmProducts(int FirmIdx)
        {
            var firmProducts = await _firmsRepository.GetFirmAllProducts(FirmIdx);
            List<FirmProductsModels> firmProductsModels = new List<FirmProductsModels>();
            foreach (var firmProduct in firmProducts)
            {
                FirmProductsModels model = new FirmProductsModels
                {
                    FirmIdx = firmProduct.FirmIdx,
                    FirmName = firmProduct.Firms.FirmName,
                    Idx = firmProduct.Idx,
                    ProductIdx = firmProduct.ProductIdx,
                    ProductName_EN = firmProduct.Products.Name_EN,
                    ProductName_TR = firmProduct.Products.Name_TR
                };
                firmProductsModels.Add(model);
            }
            return firmProductsModels;
        }

        public async Task<FirmProductsModels> GetFirmProductsModels(int firmProductIdx)
        {
            var firmProduct = await _firmsRepository.GetFirmProducts(firmProductIdx);
            FirmProductsModels firmProductFieldsModel = new FirmProductsModels
            {
                Idx = firmProduct.Idx,
                FirmIdx = firmProduct.FirmIdx,
                FirmName = firmProduct.Firms.FirmName,
                ProductIdx = firmProduct.ProductIdx,
                ProductName_TR = firmProduct.Products.Name_TR,
                ProductName_EN = firmProduct.Products.Name_EN
            };

            return firmProductFieldsModel;
        }

        public async Task<DataTableResponse<FirmsModel>> GetFirms(IDataTableRequest request)
        {
            var firms = await _firmsRepository.ListAsync(request);
            DataTableResponse<FirmsModel> returnModel = new DataTableResponse<FirmsModel>();
            returnModel.Draw = firms.Draw;
            returnModel.RecordsFiltered = firms.RecordsFiltered;
            returnModel.RecordsTotal = firms.RecordsTotal;

            returnModel.Data = firms.Data.Select(f =>
            new FirmsModel()
            {
                Idx = f.Idx,
                FirmName = f.FirmName,
                TaxNo = f.TaxNo,
                Gsm = f.Gsm,
                Email = f.Email,
                Status_Desc = f.Status == (int)GeneralStatus.Active ? "Aktif" : "Pasif"
            }).ToList();

            return returnModel;
        }

        public async Task<List<FirmsModel>> GetFirms()
        {
            var firms = await _firmsRepository.ListAllAsync();
            List<FirmsModel> firmsModels = new List<FirmsModel>();
            foreach (var firm in firms)
            {
                firmsModels.Add(firm.xCopyTo<FirmsModel>());
            }
            return firmsModels;
        }

        public async Task<DataTableResponse<TicketsModel>> GetFirmTickets(IDataTableRequest request, FirmTicketFilterModel filter)
        {
            var tickets = await _firmsRepository.GetFirmTickets(request,filter);
            DataTableResponse<TicketsModel> returnModel = new DataTableResponse<TicketsModel>();
            returnModel.Draw = tickets.Draw;
            returnModel.RecordsFiltered = tickets.RecordsFiltered;
            returnModel.RecordsTotal = tickets.RecordsTotal;

            returnModel.Data = tickets.Data.Select(f =>
            new TicketsModel()
            {
                Idx = f.Idx,
                FirmIdx = f.FirmIdx,
                FirmName = f.Firms == null ?"":f.Firms.FirmName,
                TicketNumber = f.TicketNumber,
                Status = f.Status,
                StartDate = f.StartDate,
                PriorityIdx = f.PriorityIdx,
                PriorityName = f.Priorities == null ? "" : f.Priorities.Name_TR,
                TypeIdx = f.TypeIdx,
                TypeName = f.TicketTypes == null ? "" : f.TicketTypes.TicketTypeName_TR,
                Name = f.Name,
                AssigneeUserIdx = f.AssigneeUserIdx
            }).ToList();

            return returnModel;
        }

        public Task<List<TicketsModel>> GetFirmTickets(int FirmIdx)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> TaxNoIsUnique(string taxNo, int Idx)
        {
            return await _firmsRepository.TaxNoIsUnique(taxNo, Idx);
        }
    }
}
