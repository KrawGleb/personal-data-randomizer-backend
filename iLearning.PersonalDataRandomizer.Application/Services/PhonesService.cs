using iLearning.PersonalDataRandomizer.Application.Services.Interfaces;
using iLearning.PersonalDataRandomizer.Domain.Constants;
using iLearning.PersonalDataRandomizer.Domain.Enums;

namespace iLearning.PersonalDataRandomizer.Application.Services;

public class PhonesService : IPhonesService
{
    public Random Random { get; set; }

    public IEnumerable<string> GetRandomPhones(string country, int count)
    {
        return country switch
        {
            Country.Russia => GetRuPhones(count),
            Country.USA => GetUsPhones(count),
            _ => Enumerable.Empty<string>(),
        };
    }

    private IEnumerable<string> GetRuPhones(int count)
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

    private IEnumerable<string> GetUsPhones(int count)
    {
        var tempRange = Enumerable.Repeat(0, count);

        var areaCodes = tempRange.Select(_ => Random.Next(PhoneConstants.US_MIN_AREA_CODE, PhoneConstants.US_MAX_AREA_CODE));
        var phoneNumbers = tempRange.Select(_ => Random.Next(1_000_000, 9_999_999));

        var phones = areaCodes.Zip(phoneNumbers, (code, number) => $"+1 ({code}) {number}");

        return phones;
    }
}
