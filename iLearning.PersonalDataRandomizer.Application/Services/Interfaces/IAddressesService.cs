using iLearning.PersonalDataRandomizer.Domain.Models.Data.City;
using iLearning.PersonalDataRandomizer.Domain.Models.Data.Street;

namespace iLearning.PersonalDataRandomizer.Application.Services.Interfaces;

public interface IAddressesService
{
    Random Random { get; set; }

    Task<IEnumerable<string>> GetRandomAddresses<TCity, TStreet>(int count)
        where TCity : City, new()
        where TStreet : Street, new();
}