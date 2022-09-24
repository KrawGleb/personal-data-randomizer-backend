using iLearning.PersonalDataRandomizer.Application.Helpers;
using iLearning.PersonalDataRandomizer.Application.Services.Interfaces;
using iLearning.PersonalDataRandomizer.Infrastructure.Persistence;
using System;

namespace iLearning.PersonalDataRandomizer.Application.Services;

public class AddressesService : IAddressesService
{
    private readonly ApplicationDbContext _context;

    public AddressesService(ApplicationDbContext context)
    {
        _context = context;
    }

    public Random Random { get; set; }

    public async Task<IEnumerable<string>> GetRandomAddresses(int count)
    {
        var cities = await DataSetHelper.GetRandomRowsAsync(
            _context.RuCities,
            Random,
            count);

        var streets = await DataSetHelper.GetRandomRowsAsync(
        _context.RuStreets,
            Random,
        count);

        var maxHouseNumber = Random.Next(100, 400);
        var maxFlatNumber = Random.Next(50, 150);
        var addresses = cities.Zip(streets, (city, street) =>
            $"{city.Name} {street.Name} дом №{Random.Next(1, maxHouseNumber)}" +
        (Random.Next() % 2 == 0
                    ? ""
                    : $" кв.{Random.Next(1, maxFlatNumber)}"));

        return addresses;
    }
}
