using iLearning.PersonalDataRandomizer.Application.Helpers;
using iLearning.PersonalDataRandomizer.Application.Helpers.Extensions;
using iLearning.PersonalDataRandomizer.Application.Services.Interfaces;
using iLearning.PersonalDataRandomizer.Domain.Constants;
using iLearning.PersonalDataRandomizer.Domain.Enums;
using iLearning.PersonalDataRandomizer.Domain.Models.Data.City;
using iLearning.PersonalDataRandomizer.Domain.Models.Data.Street;
using iLearning.PersonalDataRandomizer.Infrastructure.Persistence;

namespace iLearning.PersonalDataRandomizer.Application.Services;

public class AddressesService : IAddressesService
{
    private readonly ApplicationDbContext _context;

    public AddressesService(ApplicationDbContext context)
    {
        _context = context;

        Random = new Random();
    }

    public Random Random { get; set; }

    public async Task<IEnumerable<string>> GetRandomAddresses<TCity, TStreet>(int count)
        where TCity : City, new()
        where TStreet : Street, new()
    {
        var cities = await DataSetHelper.GetRandomRowsAsync(
            _context.Set<TCity>(),
            Random,
            count);

        var streets = await DataSetHelper.GetRandomRowsAsync(
            _context.Set<TStreet>(),
            Random,
            count);

        var country = CountryHelper.GetCountry<TCity>();

        var maxHouseNumber = Random.Next(AddressConstants.MIN_HOUSE_NUMBER, AddressConstants.MAX_HOUSE_NUMBER);
        var maxFlatNumber = Random.Next(AddressConstants.MIN_FLAT_NUMBER, AddressConstants.MAX_FLAT_NUMBER);
        var addresses = cities.Zip(streets, (city, street) => 
            $"{city.Name}, {street.Name}, {GetHouseAndFlat(country, maxHouseNumber, maxFlatNumber)}");

        return addresses;
    }

    private string GetHouseAndFlat(string country, int maxHouseNumber, int maxFlatNumber)
    {
        return country.ToLower() switch
        {
            Country.Russia => GetRuHouseAndFlat(maxHouseNumber, maxFlatNumber),
            Country.Poland => "",
            Country.USA => "",
            _ => "",
        };
    }

    private string GetRuHouseAndFlat(int maxHouseNumber, int maxFlatNumber)
    {
        return $"д.{Random.Next(1, maxHouseNumber)} {GetFlat(maxFlatNumber)}";
    }

    private string GetFlat(int maxFlatNumber)
    {
        return Random.NextBool()
            ? ""
            : $" кв.{Random.Next(1, maxFlatNumber)}";
    }
}
