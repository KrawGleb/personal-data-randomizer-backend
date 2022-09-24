namespace iLearning.PersonalDataRandomizer.Application.Services.Interfaces;

public interface INamesService
{
    Random Random { get; set; }

    Task<IEnumerable<string>> GetRandomFullNames(int count);
}