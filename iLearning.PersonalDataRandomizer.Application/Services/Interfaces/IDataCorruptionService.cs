using iLearning.PersonalDataRandomizer.Domain;

namespace iLearning.PersonalDataRandomizer.Application.Services.Interfaces;
public interface IDataCorruptionService
{
    IEnumerable<PersonalData> CorruptData(IEnumerable<PersonalData> data, Random random, float errosCount);
}