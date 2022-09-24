using iLearning.PersonalDataRandomizer.Domain;
using iLearning.PersonalDataRandomizer.Domain.Models;

namespace iLearning.PersonalDataRandomizer.Application.Services.Interfaces;

public interface IGenericDataService
{
    Task<IEnumerable<PersonalData>> GeneratePersonalDataAsync(RandomOptions options);
}
