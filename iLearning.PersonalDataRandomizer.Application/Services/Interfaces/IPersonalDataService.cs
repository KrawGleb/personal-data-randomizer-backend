using iLearning.PersonalDataRandomizer.Domain;

namespace iLearning.PersonalDataRandomizer.Application.Services.Interfaces;
public interface IPersonalDataService
{
    IEnumerable<PersonalData> BuildPersonalData(Random random, IEnumerable<string> names, IEnumerable<string> addresses, IEnumerable<string> phones);
}