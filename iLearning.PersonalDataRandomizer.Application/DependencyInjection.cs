using iLearning.PersonalDataRandomizer.Application.Services;
using iLearning.PersonalDataRandomizer.Application.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace iLearning.PersonalDataRandomizer.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services) 
    {
        services.AddScoped<IRuDataService, RuDataService>();
        services.AddScoped<IPlDataService, PlDataService>();
        services.AddScoped<IUSDataService, USDataService>();

        services.AddScoped<IDataService, DataService>();

        services.AddScoped<INamesService, NamesService>();
        services.AddScoped<IPhonesService, PhonesService>();
        services.AddScoped<IAddressesService, AddressesService>();
        services.AddScoped<IPersonalDataService, PersonalDataService>();
        services.AddScoped<IDataCorruptionService, DataCorruptionService>();

        return services;
    }
}
