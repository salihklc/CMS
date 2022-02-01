using AutoMapper;
using Microsoft.Extensions.Logging;
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
    public class HomeService : IHomeService
    {
        private readonly ILogger<HomeService> _logger;
        private readonly ICityRepository _cityRepository;
        private readonly ITicketsRepository _ticketsRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProductRepository _productRepository;
        private readonly IAsyncRepository<Firms> _firmRepo;
        public HomeService(
            ILogger<HomeService> logger,
            ICityRepository cityRepository,
        IAsyncRepository<Firms> firmRepo,
         ITicketsRepository ticketsRepository,
         IUserRepository userRepository,
         IProductRepository productRepository
        )
        {
            _logger = logger;
            _cityRepository = cityRepository;
            _firmRepo = firmRepo;
            _ticketsRepository = ticketsRepository;
            _userRepository = userRepository;
            _productRepository = productRepository;
        }

        public async Task<Common.Models.ViewModels.Home.HomeCountersModel> GetHomeCounters()
        {
            Common.Models.ViewModels.Home.HomeCountersModel homeCounters = new Common.Models.ViewModels.Home.HomeCountersModel();

            try
            {
                DTParameterModel requestModel = new DTParameterModel() { Length = 1 };

                var myFirmRepoEntity = await _firmRepo.ListAsync(requestModel);
                var myTicketEntity = await _ticketsRepository.ListAsync(requestModel);
                var myUserEntity = await _userRepository.ListAsync(requestModel);
                var myProductEntity = await _productRepository.ListAsync(requestModel);

                // var result = await Task.WhenAll(
                //     myFirmRepoEntity,
                //     myTicketEntity,
                //     myUserEntity,
                //     myProductEntity);

                homeCounters.FirmCount = myFirmRepoEntity.RecordsTotal;
                homeCounters.TicketCount = myTicketEntity.RecordsTotal;
                homeCounters.UserCount = myUserEntity.RecordsTotal;
                homeCounters.ProductCount = myProductEntity.RecordsTotal;

                return homeCounters;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "HATA {msg}", ex.Message);
            }

            return homeCounters;
        }
    }
}
