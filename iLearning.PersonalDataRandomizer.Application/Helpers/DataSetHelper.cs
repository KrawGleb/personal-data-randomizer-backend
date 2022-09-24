using iLearning.PersonalDataRandomizer.Domain.Enums;
using iLearning.PersonalDataRandomizer.Domain.Models.Data;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity.SqlServer;

namespace iLearning.PersonalDataRandomizer.Application.Helpers;

public static class DataSetHelper<T>
    where T : Record
{
    public static async Task<IEnumerable<T>> GetRandomRowsAsync(
        DbSet<T> set,
        Random random,
        int count,
        Gender gender)
    {
        // TODO: store total counts for each table in database
        var totalCount = await set
            .Where(v => v.Gender == gender)
            .CountAsync();

        var randomCount = random.Next(count, totalCount - count);
        var startIndex = random.Next(0, totalCount - count);

        var records = await set
            .Where(v => v.Gender == gender)
            .Skip(startIndex)
            .Take(randomCount)
            .ToListAsync();

        return records
            .OrderBy(v => random.Next())
            .Take(count);
    }
}
