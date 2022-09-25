using iLearning.PersonalDataRandomizer.Application.Helpers;
using iLearning.PersonalDataRandomizer.Application.Services.Interfaces;
using iLearning.PersonalDataRandomizer.Domain.Enums;
using iLearning.PersonalDataRandomizer.Domain.Models.Data;
using iLearning.PersonalDataRandomizer.Infrastructure.Persistence;

namespace iLearning.PersonalDataRandomizer.Application.Services;

public class NamesService : INamesService
{
    private readonly ApplicationDbContext _context;

    public NamesService(ApplicationDbContext context)
    {
        _context = context;
    }

    public Random Random { get; set; }

    public async Task<IEnumerable<string>> GetRandomFullNames<TName, TSurname, TPatronymics>(int count)
        where TName : Record
        where TSurname : Record
        where TPatronymics : Record
    {
        int maleCount = Random.Next(0, count);
        int femaleCount = count - maleCount;

        var surnames = await GetRandomRecords<TSurname>(maleCount, femaleCount);
        var names = await GetRandomRecords<TName>(maleCount, femaleCount);
        var patronymics = await GetRandomRecords<TPatronymics>(maleCount, femaleCount);

        var fullNames = ConcatFullNames(surnames, names, patronymics);

        return fullNames;
    }

    public async Task<IEnumerable<string>> GetRandomFullNames<TName, TSurname>(int count)
        where TName: Record
        where TSurname : Record
    {
        int maleCount = Random.Next(0, count);
        int femaleCount = count - maleCount;

        var surnames = await GetRandomRecords<TSurname>(maleCount, femaleCount);
        var names = await GetRandomRecords<TName>(maleCount, femaleCount);

        var fullNames = ConcatFullNames(surnames, names);

        return fullNames;
    }

    private async Task<IEnumerable<T>> GetRandomRecords<T>(int maleCount = 1, int femaleCount = 0)
        where T: Record
    {
        var males = await DataSetHelper.GetRandomRowsAsync<T>(
            _context.Set<T>(),
            Random,
            maleCount,
            Gender.Male);

        var females = await DataSetHelper.GetRandomRowsAsync<T>(
            _context.Set<T>(),
            Random,
            femaleCount,
            Gender.Female);

        return males.Concat(females);
    }

    private IEnumerable<string> ConcatFullNames<TSurname, TName, TPatronymic>(
        IEnumerable<TSurname> surnames,
        IEnumerable<TName> names,
        IEnumerable<TPatronymic> patronymics)
        where TSurname : Record
        where TName : Record
        where TPatronymic: Record
    {
        var surnamesWithNames = surnames.Zip(names, (surname, name) => $"{surname.Value} {name.Value}");
        var fullNames = surnamesWithNames.Zip(patronymics, (name, patronymics) => $"{name} {patronymics.Value}");

        return fullNames;
    }

    private IEnumerable<string> ConcatFullNames<TSurname, TName>(
        IEnumerable<TSurname> surnames,
        IEnumerable<TName> names)
        where TName: Record
        where TSurname: Record
    {
        return surnames.Zip(names, (surname, name) => $"{surname.Value} {name.Value}").ToList();
    }
}
