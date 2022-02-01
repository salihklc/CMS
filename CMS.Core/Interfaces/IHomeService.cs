
using System.Threading.Tasks;

namespace CMS.Core.Interfaces
{
    public interface IHomeService
    {
        Task<Common.Models.ViewModels.Home.HomeCountersModel> GetHomeCounters();
        
    }
}