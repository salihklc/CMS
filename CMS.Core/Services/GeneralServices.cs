using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS.Common.AppConstants;
using CMS.Common.Extensions;
using CMS.Common.Interfaces;
using CMS.Common.Models.CommonModels;
using CMS.Common.Models.ViewModels.Cities;
using CMS.Common.Models.ViewModels.Products;
using CMS.Common.Models.ViewModels.Roles;
using CMS.Core.Entities;
using CMS.Core.Interfaces;

namespace CMS.Core.Services
{
    public class GeneralServices : IGeneralServices
    {
        private readonly ICityRepository _cityRepository;
        private readonly IAsyncRepository<Districts> _districtRepository;
        private readonly IAsyncRepository<Roles> _roleRepository;
        private readonly IMapper _mapper;
        private readonly IAsyncRepository<Priorities> _prioritiesRepository;
        private readonly IAsyncRepository<TicketTypes> _ticketTypesRepository;
        private readonly IAsyncRepository<WorkingTypes> _workingTypeRepository;
        private readonly IAsyncRepository<TicketStatus> _ticketStatusRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAsyncRepository<TicketStatusCategories> _ticketStatusCategoriesRepository;
        private readonly IFirmsRepository _firmsRepository;
        private readonly ITicketsRepository _ticketsRepository;
        private readonly IAsyncRepository<FieldTypes> _fieldTypesRepository;
        private readonly IAsyncRepository<Fields> _fieldRepository;
        private readonly IProductRepository _productRepository;
        public GeneralServices(ICityRepository cityRepository, IMapper mapper, IAsyncRepository<Districts> districtsRepository,
            IAsyncRepository<Roles> roleRepository, IAsyncRepository<Priorities> prioritiesRepository, IAsyncRepository<TicketTypes> ticketTypesRepository,
            IAsyncRepository<WorkingTypes> workingTypesRepository, IUserRepository userRepository, IAsyncRepository<TicketStatus> ticketStatusRepository,
            IAsyncRepository<TicketStatusCategories> ticketStatusCategoriesRepository, IFirmsRepository firmsRepository, ITicketsRepository ticketsRepository,
            IAsyncRepository<FieldTypes> fieldTypesRepository, IAsyncRepository<Fields> fieldRepository, IProductRepository productRepository)
        {
            _cityRepository = cityRepository;
            _mapper = mapper;
            _districtRepository = districtsRepository;
            _roleRepository = roleRepository;
            _prioritiesRepository = prioritiesRepository;
            _ticketTypesRepository = ticketTypesRepository;
            _workingTypeRepository = workingTypesRepository;
            _userRepository = userRepository;
            _ticketStatusRepository = ticketStatusRepository;
            _ticketStatusCategoriesRepository = ticketStatusCategoriesRepository;
            _firmsRepository = firmsRepository;
            _ticketsRepository = ticketsRepository;
            _fieldTypesRepository = fieldTypesRepository;
            _fieldRepository = fieldRepository;
            _productRepository = productRepository;
        }
        public async Task<List<SelectModel>> GetCitiesSelect()
        {
            var cityList = await _cityRepository.GetCitiesWithDistricts();
            var cityModel = new List<SelectModel>();
            cityModel.Add(new SelectModel
            {
                Idx = 0,
                Value = "Seçiniz",
                Description = "Seçiniz"
            });
            foreach (var city in cityList)
            {
                cityModel.Add(new SelectModel
                {
                    Idx = city.Idx,
                    Value = city.CityName,
                    Description = ""
                });
            }
            return cityModel;
        }

        public async Task<List<SelectModel>> GetDistrictsSelect(int cityNo)
        {
            var districtList = await _districtRepository.ListAllAsync();
            if (cityNo > 0)
            {
                districtList = districtList.Where(r => r.CityNo == cityNo).ToList();
            }
            var districtModel = new List<SelectModel>();
            districtModel.Add(new SelectModel
            {
                Idx = 0,
                Value = "Seçiniz",
                Description = "Seçiniz"
            });
            foreach (var district in districtList)
            {
                districtModel.Add(new SelectModel
                {
                    Idx = district.Idx,
                    Value = district.DistrictName,
                    Description = district.CityNo.ToString()
                });
            }
            return districtModel;
        }
        public async Task<List<City>> GetCities()
        {
            var cityList = await _cityRepository.GetCitiesWithDistricts();
            return _mapper.Map<List<City>>(cityList);
        }

        public async Task<List<District>> GetDistricts(int cityNo)
        {
            var districtList = await _districtRepository.ListAllAsync();
            if (cityNo > 0)
            {
                districtList = districtList.Where(r => r.CityNo == cityNo).ToList();
            }
            return _mapper.Map<List<District>>(districtList);
        }

        public async Task<List<SelectModel>> GetRoles()
        {
            var roleList = await _roleRepository.ListAllAsync();
            var roleViewModels = new List<SelectModel>();
            foreach (var item in roleList)
            {
                roleViewModels.Add(new SelectModel
                {
                    Idx = item.Idx,
                    Value = item.RoleName
                });
            }
            return roleViewModels;
        }
        public async Task<List<SelectModel>> GetPriorities()
        {
            var priorities = await _prioritiesRepository.ListAllAsync();

            var prioritiesModel = new List<SelectModel>();
            prioritiesModel.Add(new SelectModel
            {
                Idx = 0,
                Value = "Seçiniz",
                Description = "Seçiniz"
            });
            foreach (var priority in priorities)
            {
                prioritiesModel.Add(new SelectModel
                {
                    Idx = priority.Idx,
                    Value = priority.Name_TR,
                    Description = priority.Description_TR
                });
            }

            return prioritiesModel;
        }

        public async Task<List<SelectModel>> GetTicketTypes()
        {

            var ticketTypes = await _ticketTypesRepository.ListAllAsync();

            var ticketTypesModel = new List<SelectModel>();
            ticketTypesModel.Add(new SelectModel
            {
                Idx = 0,
                Value = "Seçiniz",
                Description = "Seçiniz"
            });
            foreach (var ticketType in ticketTypes)
            {
                ticketTypesModel.Add(new SelectModel
                {
                    Idx = ticketType.Idx,
                    Value = ticketType.TicketTypeName_TR,
                    Description = ticketType.Description_TR
                });
            }
            return ticketTypesModel;
        }

        public async Task<List<SelectModel>> GetWorkingTypes()
        {
            var workingTypes = await _workingTypeRepository.ListAllAsync();

            var model = new List<SelectModel>();
            foreach (var type in workingTypes)
            {
                model.Add(new SelectModel
                {
                    Idx = type.Idx,
                    Value = type.WorkingTypeName_TR
                });
            }
            return model;

        }

        public async Task<List<SelectModel>> GetTicketUsers()
        {
            // Buraya belki tüm kullanıcılar gelmez
            var users = await _userRepository.ListAllAsync();

            var model = new List<SelectModel>();
            foreach (var user in users)
            {
                model.Add(new SelectModel
                {
                    Idx = user.Idx,
                    Value = user.FirstName + " " + user.LastName
                });
            }

            return model;
        }
        public async Task<List<SelectModel>> GetTicketStatus()
        {

            var ticketStatuses = await _ticketStatusRepository.ListAllAsync();

            var model = new List<SelectModel>();
            model.Add(new SelectModel
            {
                Idx = 0,
                Value = "Seçiniz",
                Description = "Seçiniz"
            });
            foreach (var status in ticketStatuses)
            {
                model.Add(new SelectModel
                {
                    Idx = status.Idx,
                    Value = status.StatusName_TR,
                    Description = status.Description_TR
                });
            }

            return model;
        }

        public async Task<List<SelectModel>> GetTicketStatusCategories()
        {
            var ticketStatuses = await _ticketStatusCategoriesRepository.ListAllAsync();

            var model = new List<SelectModel>();
            model.Add(new SelectModel
            {
                Idx = 0,
                Value = "Seçiniz",
                Description = "Seçiniz"
            });
            foreach (var status in ticketStatuses)
            {
                model.Add(new SelectModel
                {
                    Idx = status.Idx,
                    Value = status.StatusCategoryName_TR,
                    Description = status.Description_TR
                });
            }

            return model;
        }

        public async Task<List<SelectModel>> GetFirms()
        {
            var firms = await _firmsRepository.ListAllAsync();
            var model = new List<SelectModel>();
            model.Add(new SelectModel
            {
                Idx = 0,
                Value = "Seçiniz",
                Description = "Seçiniz"
            });
            foreach (var firm in firms)
            {
                model.Add(new SelectModel
                {
                    Idx = firm.Idx,
                    Value = firm.FirmName,
                    Description = firm.TaxNo
                });
            }
            return model;
        }

        public async Task<List<SelectModel>> GetEnvironment()
        {
            var model = new List<SelectModel>();
            model.Add(new SelectModel
            {
                Idx = 0,
                Value = "Seçiniz",
                Description = "Seçiniz"
            });
            return model;
        }

        public async Task<List<SelectModel>> GetTicketUsers(int FirmIdx)
        {

            var users = await _userRepository.GetUsersByFirmIdx(FirmIdx);

            var model = new List<SelectModel>();
            model.Add(new SelectModel
            {
                Idx = 0,
                Value = "Seçiniz",
                Description = "Seçiniz"
            });
            foreach (var user in users)
            {
                model.Add(new SelectModel
                {
                    Idx = user.Idx,
                    Value = user.FirstName + " " + user.LastName
                });
            }

            return model;
        }

        public async Task<List<SelectModel>> GetRelatedTickets(int FirmIdx)
        {
            var tickets = await _ticketsRepository.GetTicketsByFirm(FirmIdx);
            var model = new List<SelectModel>();
            model.Add(new SelectModel
            {
                Idx = 0,
                Value = "Seçiniz",
                Description = "Seçiniz"
            });
            foreach (var ticket in tickets)
            {
                model.Add(new SelectModel
                {
                    Idx = ticket.Idx,
                    Value = ticket.Name,
                    Description = ticket.Summary
                });
            }

            return model;

        }

        public async Task<List<SelectModel>> GetFieldTypes()
        {
            var fields = await _fieldTypesRepository.ListAllAsync();
            var model = new List<SelectModel>();
            model.Add(new SelectModel
            {
                Idx = 0,
                Value = "Seçiniz",
                Description = "Seçiniz"
            });
            foreach (var field in fields)
            {
                model.Add(new SelectModel
                {
                    Idx = field.Idx,
                    Value = field.Name_TR,
                    Description = field.Description_TR
                });
            }

            return model;
        }

        public async Task<List<SelectModel>> GetFields()
        {
            var fields = await _fieldRepository.ListAllAsync();
            var model = new List<SelectModel>();
            model.Add(new SelectModel
            {
                Idx = 0,
                Value = "Seçiniz",
                Description = "Seçiniz"
            });
            foreach (var field in fields)
            {
                model.Add(new SelectModel
                {
                    Idx = field.Idx,
                    Value = field.Name_TR,

                });
            }

            return model;
        }

        public async Task<List<FieldsModel>> GetFieldsAllData(int productIdx = 0)
        {
            var fields = await _productRepository.GetFieldAllData(productIdx);
            var model = new List<FieldsModel>();
            foreach (var field in fields)
            {
                var fieldModel = field.xCopyTo<FieldsModel>();
                fieldModel.TypeName = field.Types.Name_TR;
                model.Add(fieldModel);
            }
            return model;
        }

        public async Task<List<FieldsModel>> GetSelectedFieldsAllData(int productIdx)
        {
            var fields = await _productRepository.GetSelectedFieldsAllData(productIdx);
            var model = new List<FieldsModel>();
            foreach (var field in fields)
            {
                var fieldModel = field.xCopyTo<FieldsModel>();
                fieldModel.TypeName = field.Types.Name_TR;
                model.Add(fieldModel);
            }
            return model;
        }

        public async Task<List<SelectModel>> GetProducts()
        {
            var products = (await _productRepository.ListAllAsync()).Where(r => r.Status == (int)GeneralStatus.Active);
            var model = new List<SelectModel>();
            model.Add(new SelectModel
            {
                Idx = 0,
                Value = "Seçiniz",
                Description = "Seçiniz"
            });
            foreach (var product in products)
            {
                model.Add(new SelectModel
                {
                    Idx = product.Idx,
                    Value = product.Name_TR + " " + product.ProductCode,
                });
            }

            return model;
        }

        public async Task<List<FieldsModel>> GetProductFields(int productIdx)
        {
            List<FieldsModel> fieldsModels = new List<FieldsModel>();
            var fields = await _productRepository.GetProductFields(productIdx);
            foreach (var field in fields)
            {
                fieldsModels.Add(field.Fields.xCopyTo<FieldsModel>());
            }
            return fieldsModels;
        }

        public async Task<List<Common.Models.ViewModels.Firms.ProductFieldsModel>> GetFirmProductFields(int firmProductIdx)
        {
            List<Common.Models.ViewModels.Firms.ProductFieldsModel> productFields = new List<Common.Models.ViewModels.Firms.ProductFieldsModel>();
            var firmProductFields = await _productRepository.GetFirmProductFields(firmProductIdx);
            foreach (var field in firmProductFields)
            {
                Common.Models.ViewModels.Firms.ProductFieldsModel firmProduct = new Common.Models.ViewModels.Firms.ProductFieldsModel
                {
                    FieldIdx = field.ProductFieldIdx,
                    Value = field.Value
                };
                productFields.Add(firmProduct);
            }

            return productFields;
        }
    }
}
