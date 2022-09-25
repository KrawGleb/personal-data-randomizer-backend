using iLearning.PersonalDataRandomizer.Application.Services.Interfaces;
using iLearning.PersonalDataRandomizer.Application.Helpers;
using iLearning.PersonalDataRandomizer.Domain;

namespace iLearning.PersonalDataRandomizer.Application.Services;

public class DataCorruptionService : IDataCorruptionService
{
    private readonly List<string> DataFields = new() { 
        "FullName",
        "Address",
        "Phone" };

    private readonly List<Func<string, Random, string>> CorruptionFunctionsList = new() {
        CorruptionFunctions.AddChar,
        CorruptionFunctions.SwapChars,
        CorruptionFunctions.SkipChar };

    public IEnumerable<PersonalData> CorruptData(IEnumerable<PersonalData> data, Random random, float errosCount)
    {
        IEnumerable<PersonalData> corruptedData =
            data.Select(personalData => CorruptData(personalData, random, errosCount));

        return corruptedData;
    }

    private PersonalData CorruptData(PersonalData data, Random random, float errorsCount)
    {
        float exactErrors = MathF.Floor(errorsCount);
        float errorChance = errorsCount - exactErrors;

        for (int i = 0; i < exactErrors; i++)
        {
            data = CorruptField(data, random);
        }

        if (CheckChance(random, errorChance))
        {
            data = CorruptField(data, random);
        }

        return data;
    }

    private PersonalData CorruptField(PersonalData data, Random random)
    {
        var prop = GetRandomFiled(random);
        var func = GetRandomErrorFunc(random);

        var oldValue = data.GetType().GetProperty(prop)?.GetValue(data)?.ToString();
        var newValue = func?.Invoke(oldValue ?? "", random);

        data.GetType().GetProperty(prop)?.SetValue(data, newValue);

        return data;
    }

    private string GetRandomFiled(Random random)
    {
        return DataFields
            .OrderBy(_ => random.Next())
            .FirstOrDefault()!;
    }

    private Func<string, Random, string> GetRandomErrorFunc(Random random)
    {
        return CorruptionFunctionsList
            .OrderBy(_ => random.Next())
            .FirstOrDefault()!;
    }

    private bool CheckChance(Random random, float chance)
    {
        var value = random.NextDouble();

        return value <= chance;
    }
}
