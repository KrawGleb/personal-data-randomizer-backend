using iLearning.PersonalDataRandomizer.Application.Services.Interfaces;
using iLearning.PersonalDataRandomizer.Domain;

namespace iLearning.PersonalDataRandomizer.Application.Services;

public class DataCorruptionService : IDataCorruptionService
{
    private const string PlChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyzÓóĄąĆćĘęŁłŃńŚśŹźŻż";
    private const string RuChars = "ЙЦУКЕНГШЩЗХЪФЫВАПРОЛДЖЭЯЧСМИТЬБЮйцукенгшщзхъфывапролджэячсмитьбю";
    private const string UsChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
    private const string Digit = "1234567890";

    public IEnumerable<string> DataFields = new List<string> { "FullName", "Address", "Phone" };
    private List<Func<string, Random, string>> ErrorsFuncs = new();

    public DataCorruptionService()
    {
        ErrorsFuncs.Add(SkipChar);
        ErrorsFuncs.Add(AddChar);
        ErrorsFuncs.Add(SwapChars);
    }

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
            data = CorrupField(data, random);
        }

        if (CheckChance(random, errorChance))
        {
            data = CorrupField(data, random);
        }

        return data;
    }

    private PersonalData CorrupField(PersonalData data, Random random)
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
        return ErrorsFuncs
            .OrderBy(_ => random.Next())
            .FirstOrDefault()!;
    }

    private string SkipChar(string data, Random random)
    {
        if (data.Length <= 3)
        {
            return AddChar(data, random);
        }

        var index = random.Next(0, data.Length - 1);
        data = data.Remove(index, 1);

        return data;
    }

    private string AddChar(string data, Random random)
    {
        if (data.Length >= 20)
        {
            return SkipChar(data, random);
        }

        var index = random.Next(data.Length);
        char ch;

        if (data.Any(ch => char.IsDigit(ch)))
        {
            ch = Digit.OrderBy(_ => random.Next()).FirstOrDefault();
        }
        else if (data.All(ch => UsChars.Contains(ch)))
        {
            ch = UsChars.OrderBy(_ => random.Next()).FirstOrDefault();
        }
        else if (data.All(ch => PlChars.Contains(ch)))
        {
            ch = PlChars.OrderBy(_ => random.Next()).FirstOrDefault();
        }
        else
        {
            ch = RuChars.OrderBy(_ => random.Next()).FirstOrDefault();
        }

        data = data.Insert(index, ch.ToString());

        return data;
    }

    private string SwapChars(string data, Random random)
    {
        var dataChars = data.ToCharArray();
        var index = random.Next(0, data.Length - 2);

        var temp = dataChars[index];
        dataChars[index] = dataChars[index + 1];
        dataChars[index + 1] = temp;

        return new string(dataChars);
    }

    private bool CheckChance(Random random, float chance)
    {
        var value = random.NextDouble();

        return value <= chance;
    }
}
