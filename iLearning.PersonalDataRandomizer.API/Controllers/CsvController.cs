using CsvHelper;
using CsvHelper.Configuration;
using iLearning.PersonalDataRandomizer.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text;

namespace iLearning.PersonalDataRandomizer.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CsvController : ControllerBase
{
    [HttpPost]
    public async Task<FileResult> GenerateCsv(IEnumerable<PersonalData> data)
    {
        var stringWriter = new StringWriter();

        TextWriter writer = stringWriter;

        var csvWriter = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ";"
        });

        await csvWriter.WriteRecordsAsync(data);

        return File(Encoding.UTF8.GetBytes(stringWriter.ToString()), "text/csv", "data.csv");
    }
}
