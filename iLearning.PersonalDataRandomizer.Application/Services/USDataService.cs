using iLearning.PersonalDataRandomizer.Application.Services.Interfaces;
using iLearning.PersonalDataRandomizer.Domain;
using iLearning.PersonalDataRandomizer.Domain.Models;

namespace iLearning.PersonalDataRandomizer.Application.Services;

public class USDataService : IUSDataService
{
    public async Task<IEnumerable<PersonalData>> GeneratePersonalDataAsync(RandomOptions options)
    {
        throw new NotImplementedException();
    }
}
