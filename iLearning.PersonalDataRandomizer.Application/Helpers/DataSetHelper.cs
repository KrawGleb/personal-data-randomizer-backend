using iLearning.PersonalDataRandomizer.Domain.Enums;
using iLearning.PersonalDataRandomizer.Domain.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace iLearning.PersonalDataRandomizer.Application.Helpers;

public static class DataSetHelper
{
    public static async Task<IEnumerable<T>> GetRandomRowsAsync<T>(
        DbSet<T> set,
        Random random,
        int count,
        Gender gender) where T : Record
    {
        // TODO: store total counts for each table in database
        var totalCount = set
            .Where(v => v.Gender == gender ||
                        v.Gender == Gender.Any)
            .AsNoTracking()
            .Count();

        var randomCount = random.Next(count, totalCount - count);
        var startIndex = random.Next(0, totalCount - count);

        var records = await set
            .Where(v => v.Gender == gender)
            .Skip(startIndex)
            .Take(randomCount)
            .AsNoTracking()
            .ToListAsync();

        return records
            .OrderBy(v => random.Next())
            .Take(count);
    }

    public static async Task<IEnumerable<TEntity>> GetRandomRowsAsync<TEntity>(
        DbSet<TEntity> set,
        Random random,
        int count) where TEntity : class, new()
    {
        var totalCount = set.Count();

        var randomCount = random.Next(count, totalCount - count);
        var startIndex = random.Next(0, totalCount - count);

        var records = await set
            .Skip(startIndex)
            .Take(randomCount)
            .AsNoTracking()
            .ToListAsync();

        return records
            .OrderBy(v => random.Next())
            .Take(count);
    }
}
