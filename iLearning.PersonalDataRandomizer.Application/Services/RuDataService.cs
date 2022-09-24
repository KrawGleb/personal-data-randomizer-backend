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

        var personalData = fullNames
            .Select(fullName => new PersonalData
            {
                Index = 0,
                Identifier = "ToDo",
                FullName = fullName,
                Address = "ToDo",
                Phone = "ToDo"
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
