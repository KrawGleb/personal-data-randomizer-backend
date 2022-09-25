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
        var totalCount = set
            .Where(v => v.Gender == gender ||
                        v.Gender == Gender.Any)
            .Count();

        var toTake = random.Next(count, totalCount - count);
        var toSkip = random.Next(0, totalCount - count);

        var records = await set
            .AsNoTracking()
            .Where(v => 
                v.Gender == gender ||
                v.Gender == Gender.Any)
            .Skip(toSkip)
            .Take(toTake)
            .ToListAsync();

        return records
            .OrderBy(v => random.Next())
            .Take(count);
    }

    public static async Task<IEnumerable<TEntity>> GetRandomRowsAsync<TEntity>(
        DbSet<TEntity> set,
        Random random,
        int count) where TEntity: class
    {
        var totalCount = set.Count();

        var toTake = random.Next(count, totalCount - count);
        var toSkip = random.Next(0, totalCount - count);

        var records = await set
            .Skip(toSkip)
            .Take(toTake)
            .AsNoTracking()
            .ToListAsync();

        return records
            .OrderBy(v => random.Next())
            .Take(count);
    }
}
