using TasteTrailData.Core.Common.Repositories.Base;
using TasteTrailOwnerExperience.Core.Menus.Models;

namespace TasteTrailOwnerExperience.Core.Menus.Repositories;

public interface IMenuRepository : IGetAsNoTrackingAsync<Menu?, int>, IGetByIdAsync<Menu?, int>,
    ICreateAsync<Menu, int>, IDeleteByIdAsync<int, int?>, IPutAsync<Menu, int?> 
{
    Task<IEnumerable<Menu>> GetAllByVenueIdAsync(int venueId);
}
