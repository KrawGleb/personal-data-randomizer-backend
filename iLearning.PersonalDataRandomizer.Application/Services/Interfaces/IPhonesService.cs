namespace iLearning.PersonalDataRandomizer.Application.Services.Interfaces;

public interface IPhonesService
{
    Random Random { get; set; }

    IEnumerable<string> GetRandomPhones(int count);
}