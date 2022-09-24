namespace iLearning.PersonalDataRandomizer.Application.Services.Interfaces;

public interface IAddressesService
{
    Random Random { get; set; }

    Task<IEnumerable<string>> GetRandomAddresses(int count);
}