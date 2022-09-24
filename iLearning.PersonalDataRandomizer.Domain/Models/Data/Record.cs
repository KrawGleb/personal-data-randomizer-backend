using iLearning.PersonalDataRandomizer.Domain.Enums;

namespace iLearning.PersonalDataRandomizer.Domain.Models.Data;

public class Record
{
    public int Id { get; set; }
    public string Value { get; set; }
    public Gender Gender { get; set; }
}
