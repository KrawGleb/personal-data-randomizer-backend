using iLearning.PersonalDataRandomizer.Application.Services.Interfaces;
using iLearning.PersonalDataRandomizer.Domain;
using iLearning.PersonalDataRandomizer.Domain.Enums;
using iLearning.PersonalDataRandomizer.Domain.Models;

namespace iLearning.PersonalDataRandomizer.Application.Services;

public class DataService : IDataService
{
    private readonly IRuDataService _ruDataService;
    private readonly IUSDataService _usDataService;
    private readonly IPlDataService _plDataService;

    public DataService(
        IRuDataService ruDataService,
        IUSDataService usDataService,
        IPlDataService plDataService)
    {
        _ruDataService = ruDataService;
        _usDataService = usDataService;
        _plDataService = plDataService;
    }

    public async Task<IEnumerable<PersonalData>> GeneratePersonalData(RandomOptions options)
    {
        var dataService = GetDataServiceByCountry(options.Country);
        return await dataService.GeneratePersonalDataAsync(options);
    }

    private IGenericDataService GetDataServiceByCountry(string country)
    {
        return country switch
        {
            Country.Russia => _ruDataService,
            Country.USA => _usDataService,
            Country.Poland => _plDataService,
            _ => throw new InvalidOperationException(),
        };
    }
}
