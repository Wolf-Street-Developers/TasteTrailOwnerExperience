using TasteTrailData.Core.Common.Repositories.Base;
using TasteTrailOwnerExperience.Core.MenuItems.Models;


namespace TasteTrailOwnerExperience.Core.MenuItems.Repositories;

public interface IMenuItemRepository : IGetAsNoTrackingAsync<MenuItem?, int>, IGetByIdAsync<MenuItem?, int>, ICreateAsync<MenuItem, int>, IDeleteByIdAsync<int, int?>, IPutAsync<MenuItem, int?>
{
    Task<IEnumerable<MenuItem>> GetAllByMenuIdAsync(int menuId);
}
