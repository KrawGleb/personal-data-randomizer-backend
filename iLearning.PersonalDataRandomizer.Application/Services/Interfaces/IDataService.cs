using iLearning.PersonalDataRandomizer.Domain.Models;
using iLearning.PersonalDataRandomizer.Domain;

namespace iLearning.PersonalDataRandomizer.Application.Services.Interfaces;

public interface IDataService
{
    Task<IEnumerable<PersonalData>> GeneratePersonalData(RandomOptions options);
}
