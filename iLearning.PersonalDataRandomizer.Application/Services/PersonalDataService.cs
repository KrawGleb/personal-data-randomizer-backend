using iLearning.PersonalDataRandomizer.Application.Helpers;
using iLearning.PersonalDataRandomizer.Application.Services.Interfaces;
using iLearning.PersonalDataRandomizer.Domain;

namespace iLearning.PersonalDataRandomizer.Application.Services;

public class PersonalDataService : IPersonalDataService
{
    public IEnumerable<PersonalData> BuildPersonalData(
        Random random,
        IEnumerable<string> names,
        IEnumerable<string> addresses,
        IEnumerable<string> phones)
    {
        var personalData = GetEmptyData(names.Count());

        personalData = WithNames(personalData, names);
        personalData = WithAddresses(personalData, addresses);
        personalData = WithPhones(personalData, phones);
        personalData = WithRandomIndexes(personalData, random);
        personalData = WithRandomIdentifiers(personalData, random);

        return personalData;
    }

    private IEnumerable<PersonalData> GetEmptyData(int count)
    {
        return Enumerable.Repeat(0, count).Select(_ => new PersonalData());
    }

    private IEnumerable<PersonalData> WithNames(IEnumerable<PersonalData> data, IEnumerable<string> names)
    {
        data = data.Zip(names, (record, name) =>
        {
            record.FullName = name;
            return record;
        });

        return data;
    }

    private IEnumerable<PersonalData> WithPhones(IEnumerable<PersonalData> data, IEnumerable<string> phones)
    {
        data = data.Zip(phones, (record, phone) =>
        {
            record.Phone = phone;
            return record;
        });

        return data;
    }

    private IEnumerable<PersonalData> WithAddresses(IEnumerable<PersonalData> data, IEnumerable<string> addresses)
    {
        data = data.Zip(addresses, (record, address) =>
        {
            record.Address = address;
            return record;
        });

        return data;
    }

    private IEnumerable<PersonalData> WithRandomIndexes(IEnumerable<PersonalData> data, Random random)
    {
        data = data.Select(record =>
        {
            record.Index = random.Next();
            return record;
        });

        return data;
    }

    private IEnumerable<PersonalData> WithRandomIdentifiers(IEnumerable<PersonalData> data, Random random)
    {
        data = data.Select(record =>
        {
            record.Identifier = GuidHelper.GetGuid(random);
            return record;
        });

        return data;
    }
}
