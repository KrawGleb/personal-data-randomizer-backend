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
    private readonly IDataCorruptionService _dataCorruptionService;

    public DataService(
        IRuDataService ruDataService,
        IUSDataService usDataService,
        IPlDataService plDataService,
        IDataCorruptionService dataCorruptionService)
    {
        _ruDataService = ruDataService;
        _usDataService = usDataService;
        _plDataService = plDataService;
        _dataCorruptionService = dataCorruptionService;
    }

    public async Task<IEnumerable<PersonalData>> GeneratePersonalData(RandomOptions options)
    {
        var dataService = GetDataServiceByCountry(options.Country);
        var data = await dataService.GeneratePersonalDataAsync(options);
        var corruptedData = _dataCorruptionService.CorruptData(data, new Random(options.Seed), options.ErrorsCount);

        return corruptedData;
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
