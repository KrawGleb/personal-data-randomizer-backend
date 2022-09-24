using iLearning.PersonalDataRandomizer.Application.Helpers;
using iLearning.PersonalDataRandomizer.Application.Services.Interfaces;
using iLearning.PersonalDataRandomizer.Domain.Enums;
using iLearning.PersonalDataRandomizer.Domain.Models.Data.Name;
using iLearning.PersonalDataRandomizer.Domain.Models.Data.Patronymic;
using iLearning.PersonalDataRandomizer.Domain.Models.Data.Surname;
using iLearning.PersonalDataRandomizer.Infrastructure.Persistence;
using System;

namespace iLearning.PersonalDataRandomizer.Application.Services;

public class NamesService : INamesService
{
    private readonly ApplicationDbContext _context;

    public NamesService(ApplicationDbContext context)
    {
        _context = context;
    }

    public Random Random { get; set; }

    public async Task<IEnumerable<string>> GetRandomFullNames(int count)
    {
        int maleCount = Random.Next(0, count);
        int femaleCount = count - maleCount;

        var surnames = await GetRandomSurnamesAsync(maleCount, femaleCount);
        var names = await GetRadomNamesAsync(maleCount, femaleCount);
        var patronymics = await GetRandomPatronymicsAsync(maleCount, femaleCount);

        var fullNames = ConcatFullNames(surnames, names, patronymics);

        return fullNames;
    }

    private async Task<IEnumerable<RuName>> GetRadomNamesAsync(int maleCount = 1, int femaleCount = 0)
    {
        var males = await DataSetHelper.GetRandomRowsAsync(
            _context.RuNames,
            Random,
            maleCount,
            Gender.Male);

        var females = await DataSetHelper.GetRandomRowsAsync(
            _context.RuNames,
            Random,
            femaleCount,
            Gender.Female);

        return males.Concat(females);
    }

    private async Task<IEnumerable<RuSurname>> GetRandomSurnamesAsync(int maleCount = 1, int femaleCount = 0)
    {
        var males = await DataSetHelper.GetRandomRowsAsync(
            _context.RuSurnames,
            Random,
            maleCount,
            Gender.Male);

        var females = await DataSetHelper.GetRandomRowsAsync(
            _context.RuSurnames,
            Random,
            femaleCount,
            Gender.Female);

        return males.Concat(females);
    }

    private async Task<IEnumerable<RuPatronymic>> GetRandomPatronymicsAsync(int maleCount = 1, int femaleCount = 0)
    {
        var males = await DataSetHelper.GetRandomRowsAsync(
            _context.RuPatronymics,
            Random,
            maleCount,
            Gender.Male);

        var females = await DataSetHelper.GetRandomRowsAsync(
            _context.RuPatronymics,
            Random,
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
