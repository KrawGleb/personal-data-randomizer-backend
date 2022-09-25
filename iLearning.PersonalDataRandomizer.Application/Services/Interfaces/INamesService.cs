using iLearning.PersonalDataRandomizer.Domain.Models.Data;

namespace iLearning.PersonalDataRandomizer.Application.Services.Interfaces;

public interface INamesService
{
    Random Random { get; set; }

    Task<IEnumerable<string>> GetRandomFullNames<TName, TSurname, TPatronymics>(int count)
        where TName: Record
        where TSurname: Record
        where TPatronymics: Record;

    Task<IEnumerable<string>> GetRandomFullNames<TName, TSurname>(int count)
        where TName : Record
        where TSurname : Record;
}