using TasteTrailOwnerExperience.Core.Common.MessageBroker;
using TasteTrailOwnerExperience.Core.MenuItems.Repositories;
using TasteTrailOwnerExperience.Core.MenuItems.Services;
using TasteTrailOwnerExperience.Core.Menus.Repositories;
using TasteTrailOwnerExperience.Core.Menus.Services;
using TasteTrailOwnerExperience.Core.Venues.Repositories;
using TasteTrailOwnerExperience.Core.Venues.Services;
using TasteTrailOwnerExperience.Infrastructure.Common.RabbitMq;
using TasteTrailOwnerExperience.Infrastructure.MenuItems.Managers;
using TasteTrailOwnerExperience.Infrastructure.MenuItems.Repositories;
using TasteTrailOwnerExperience.Infrastructure.MenuItems.Services;
using TasteTrailOwnerExperience.Infrastructure.Menus.Repositories;
using TasteTrailOwnerExperience.Infrastructure.Menus.Services;
using TasteTrailOwnerExperience.Infrastructure.Venues.Managers;
using TasteTrailOwnerExperience.Infrastructure.Venues.Repositories;
using TasteTrailOwnerExperience.Infrastructure.Venues.Services;

namespace TasteTrailOwnerExperience.Api.Common.Extensions.ServiceCollection;

public static class RegisterDependencyInjectionMethod
{
    public static void RegisterDependencyInjection(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<IVenueRepository, VenueEfCoreRepository>();
        serviceCollection.AddTransient<IMenuRepository, MenuEfCoreRepository>();
        serviceCollection.AddTransient<IMenuItemRepository, MenuItemEfCoreRepository>();

        serviceCollection.AddTransient<IVenueService, VenueService>();
        serviceCollection.AddTransient<IMenuService, MenuService>();
        serviceCollection.AddTransient<IMenuItemService, MenuItemService>();
        serviceCollection.AddTransient<IMessageBrokerService, RabbitMqService>();

        serviceCollection.AddTransient<VenueImageManager>();
        serviceCollection.AddTransient<MenuItemImageManager>();
        serviceCollection.AddTransient<MenuImageManager>();
    } 
}
