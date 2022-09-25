using iLearning.PersonalDataRandomizer.Application.Helpers;
using iLearning.PersonalDataRandomizer.Application.Helpers.Extensions;
using iLearning.PersonalDataRandomizer.Application.Services.Interfaces;
using iLearning.PersonalDataRandomizer.Domain.Constants;
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

        var maxHouseNumber = Random.Next(AddressConstants.MIN_HOUSE_NUMBER, AddressConstants.MAX_HOUSE_NUMBER);
        var maxFlatNumber = Random.Next(AddressConstants.MIN_FLAT_NUMBER, AddressConstants.MAX_FLAT_NUMBER);
        var addresses = cities.Zip(streets, (city, street) =>
            $"{city.Name}, {street.Name}, д.{Random.Next(1, maxHouseNumber)} {GetFlat(maxFlatNumber)}");

        return addresses;
    }

    private string GetFlat(int maxFlatNumber)
    {
        return Random.NextBool()
            ? ""
            : $" кв.{Random.Next(1, maxFlatNumber)}";
    }
}
