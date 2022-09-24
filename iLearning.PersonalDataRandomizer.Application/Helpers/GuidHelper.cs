namespace iLearning.PersonalDataRandomizer.Application.Helpers;

public static class GuidHelper
{
    public static string GetGuid(Random random)
    {
        var guid = new byte[16];
        random.NextBytes(guid);

        return new Guid(guid).ToString();
    }
}
