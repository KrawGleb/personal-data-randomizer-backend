namespace iLearning.PersonalDataRandomizer.Domain.Models;

public class RandomOptions
{
    public int Seed { get; set; }
    public string Country { get; set; }
    public float ErrorsCount { get; set; }
    public int Size { get; set; } = 10;
}
