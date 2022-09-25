using iLearning.PersonalDataRandomizer.Domain.Constants;

namespace iLearning.PersonalDataRandomizer.Application.Helpers;

public static class CorruptionFunctions
{
    public static string SkipChar(string data, Random random)
    {
        if (data.Length <= 3)
        {
            return AddChar(data, random);
        }

        var index = random.Next(0, data.Length - 1);
        data = data.Remove(index, 1);

        return data;
    }

    public static string AddChar(string data, Random random)
    {
        if (data.Length >= 20)
        {
            return SkipChar(data, random);
        }

        var index = random.Next(data.Length);
        char ch;

        if (data.Any(ch => char.IsDigit(ch)))
        {
            ch = Symbols.Digits.OrderBy(_ => random.Next()).FirstOrDefault();
        }
        else if (data.All(ch => Symbols.USA.Contains(ch)))
        {
            ch = Symbols.USA.OrderBy(_ => random.Next()).FirstOrDefault();
        }
        else if (data.All(ch => Symbols.Poland.Contains(ch)))
        {
            ch = Symbols.Poland.OrderBy(_ => random.Next()).FirstOrDefault();
        }
        else
        {
            ch = Symbols.Russia.OrderBy(_ => random.Next()).FirstOrDefault();
        }

        data = data.Insert(index, ch.ToString());

        return data;
    }

    public static string SwapChars(string data, Random random)
    {
        var dataChars = data.ToCharArray();
        var index = random.Next(0, data.Length - 2);

        var temp = dataChars[index];
        dataChars[index] = dataChars[index + 1];
        dataChars[index + 1] = temp;

        return new string(dataChars);
    }
}
