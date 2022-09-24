using iLearning.PersonalDataRandomizer.Application.Helpers;
using iLearning.PersonalDataRandomizer.Application.Services.Interfaces;
using iLearning.PersonalDataRandomizer.Domain;
using iLearning.PersonalDataRandomizer.Domain.Enums;
using iLearning.PersonalDataRandomizer.Domain.Models;
using iLearning.PersonalDataRandomizer.Domain.Models.Data.Name;
using iLearning.PersonalDataRandomizer.Domain.Models.Data.Patronymic;
using iLearning.PersonalDataRandomizer.Domain.Models.Data.Surname;
using iLearning.PersonalDataRandomizer.Infrastructure.Persistence;

namespace iLearning.PersonalDataRandomizer.Application.Services;

public class RuDataService : IRuDataService
{
    private readonly ApplicationDbContext _context;

    private Random _random;

    public RuDataService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PersonalData>> GeneratePersonalDataAsync(RandomOptions options)
    {
        _random = new Random(options.Seed);

        var fullNames = await GetRandomFullNames(options.Size);
        var phones = GetRandomPhones(options.Size);

        var personalData = fullNames
            .Select(fullName => new PersonalData
            {
                Index = _random.Next(),
                Identifier = GetSeededGuid(_random.Next()),
                FullName = fullName,
                Address = "",
                Phone = ""
            })
            .Zip(phones, (data, phone) =>
            {
                data.Phone = phone;
                return data;
            });

        return personalData;
    }

    private async Task<IEnumerable<string>> GetRandomFullNames(int count)
    {
        int maleCount = _random.Next(0, count);
        int femaleCount = count - maleCount;

        var surnames = await GetRandomSurnamesAsync(maleCount, femaleCount);
        var names = await GetRadomNamesAsync(maleCount, femaleCount);
        var patronymics = await GetRandomPatronymicsAsync(maleCount, femaleCount);

        var fullNames = ConcatFullNames(surnames, names, patronymics);

        return fullNames;
    }

    private async Task<IEnumerable<RuName>> GetRadomNamesAsync(int maleCount = 1, int femaleCount = 0)
    {
        var males = await DataSetHelper<RuName>.GetRandomRowsAsync(
            _context.RuNames,
            _random,
            maleCount,
            Gender.Male);

        var females = await DataSetHelper<RuName>.GetRandomRowsAsync(
            _context.RuNames,
            _random,
            femaleCount,
            Gender.Female);

        return males.Concat(females);
    }

    private async Task<IEnumerable<RuSurname>> GetRandomSurnamesAsync(int maleCount = 1, int femaleCount = 0)
    {
        var males = await DataSetHelper<RuSurname>.GetRandomRowsAsync(
            _context.RuSurnames,
            _random,
            maleCount,
            Gender.Male);

        var females = await DataSetHelper<RuSurname>.GetRandomRowsAsync(
            _context.RuSurnames,
            _random,
            femaleCount,
            Gender.Female);

        return males.Concat(females);
    }

    private async Task<IEnumerable<RuPatronymic>> GetRandomPatronymicsAsync(int maleCount = 1, int femaleCount = 0)
    {
        var males = await DataSetHelper<RuPatronymic>.GetRandomRowsAsync(
            _context.RuPatronymics,
            _random,
            maleCount,
            Gender.Male);

        var females = await DataSetHelper<RuPatronymic>.GetRandomRowsAsync(
            _context.RuPatronymics,
            _random,
            femaleCount,
            Gender.Female);

        return males.Concat(females);
    }

    private IEnumerable<string> GetRandomPhones(int count)
    {
        var tempRange = Enumerable.Repeat(0, count);

        var codes = tempRange.Select(_ => _random.Next(900, 999));
        var firstNumbers = tempRange.Select(_ => _random.Next(100, 999));
        var secondNumbers = tempRange.Select(_ => _random.Next(10, 99));
        var thirdNumbers = tempRange.Select(_ => _random.Next(10, 99));

        var phones = codes.Zip(firstNumbers, (code, number) => $"+7 ({code}) {number}")
            .Zip(secondNumbers, (phone, number) => $"{phone} {number}")
            .Zip(thirdNumbers, (phone, number) => $"{phone} {number}");

        return phones;
    }

    private string GetSeededGuid(int seed)
    {
        var guid = new byte[16];
        _random.NextBytes(guid);

        return new Guid(guid).ToString();
    }

    private IEnumerable<string> ConcatFullNames(
        IEnumerable<RuSurname> surnames,
        IEnumerable<RuName> names,
        IEnumerable<RuPatronymic> patronymics)
    {
        var surnamesWithNames = surnames.Zip(names, (surname, name) => $"{surname.Value} {name.Value}");
        var fullNames = surnamesWithNames.Zip(patronymics, (name, patronymics) => $"{name} {patronymics.Value}");

        return fullNames;
    }
}