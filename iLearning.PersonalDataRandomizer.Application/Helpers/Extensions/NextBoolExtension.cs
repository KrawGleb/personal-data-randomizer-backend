using System.Runtime.CompilerServices;

namespace iLearning.PersonalDataRandomizer.Application.Helpers.Extensions;

public static class NextBoolExtension
{
    public static bool NextBool(this Random random)
    {
        return random.Next() % 2 == 0;
    }
}
