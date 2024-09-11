using TasteTrailData.Core.Common.Repositories.Base;
using TasteTrailOwnerExperience.Core.Venues.Models;

namespace TasteTrailOwnerExperience.Core.Venues.Repositories;

public interface IVenueRepository : IGetAsNoTrackingAsync<Venue?, int>, IGetByIdAsync<Venue?, int>, 
ICreateAsync<Venue, int>, IDeleteByIdAsync<int, int?>, IPutAsync<Venue, int?>
{
    
}
