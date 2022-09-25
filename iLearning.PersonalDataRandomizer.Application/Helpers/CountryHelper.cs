namespace iLearning.PersonalDataRandomizer.Application.Helpers;

public static class CountryHelper
{
    public static string GetCountry<T>()
        where T: new()
    {
        return new string(new T().GetType().Name.Take(2).ToArray());
    }
}
