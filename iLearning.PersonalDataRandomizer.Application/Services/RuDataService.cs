using iLearning.PersonalDataRandomizer.Application.Services.Interfaces;
using iLearning.PersonalDataRandomizer.Domain;
using iLearning.PersonalDataRandomizer.Domain.Models;
using iLearning.PersonalDataRandomizer.Domain.Models.Data.City;
using iLearning.PersonalDataRandomizer.Domain.Models.Data.Name;
using iLearning.PersonalDataRandomizer.Domain.Models.Data.Patronymic;
using iLearning.PersonalDataRandomizer.Domain.Models.Data.Street;
using iLearning.PersonalDataRandomizer.Domain.Models.Data.Surname;

namespace iLearning.PersonalDataRandomizer.Application.Services;

public class RuDataService : IRuDataService
{
    private readonly IPersonalDataService _personalDataService;
    private readonly INamesService _namesService;
    private readonly IPhonesService _phonesService;
    private readonly IAddressesService _addressesService;
    private Random _random;

    public RuDataService(
        IPersonalDataService personalDataService,
        INamesService namesService,
        IPhonesService phonesService,
        IAddressesService addressesService)
    {
        _personalDataService = personalDataService;
        _namesService = namesService;
        _phonesService = phonesService;
        _addressesService = addressesService;
    }

    public async Task<IEnumerable<PersonalData>> GeneratePersonalDataAsync(RandomOptions options)
    {
        _random = new Random(options.Seed);

        _namesService.Random = _random;
        _phonesService.Random = _random;
        _addressesService.Random = _random;

        var fullNames = await _namesService.GetRandomFullNames<RuName, RuSurname, RuPatronymic>(options.Size);
        var phones = _phonesService.GetRandomPhones(options.Country, options.Size);
        var addresses = await _addressesService.GetRandomAddresses<RuCity, RuStreet>(options.Size);

        var personalData = _personalDataService.BuildPersonalData(_random, fullNames, addresses, phones);

        return personalData;
    }
}