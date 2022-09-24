using iLearning.PersonalDataRandomizer.Application.Services.Interfaces;

namespace iLearning.PersonalDataRandomizer.Application.Services;

public class PhonesService : IPhonesService
{
    public Random Random { get; set; }

    public IEnumerable<string> GetRandomPhones(int count)
    {
        var tempRange = Enumerable.Repeat(0, count);

        var codes = tempRange.Select(_ => Random.Next(900, 999));
        var firstNumbers = tempRange.Select(_ => Random.Next(100, 999));
        var secondNumbers = tempRange.Select(_ => Random.Next(10, 99));
        var thirdNumbers = tempRange.Select(_ => Random.Next(10, 99));

        var phones = codes.Zip(firstNumbers, (code, number) => $"+7 ({code}) {number}")
            .Zip(secondNumbers, (phone, number) => $"{phone} {number}")
            .Zip(thirdNumbers, (phone, number) => $"{phone} {number}");

        return phones;
    }
}
