using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApiCsv.Models;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace WebApiCsv.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExportController : ControllerBase
{
    private readonly ILogger<ImportController> _logger;
    private readonly ApplicationDbContext _dbContext;

    public ExportController(ILogger<ImportController> logger, ApplicationDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    [HttpPost]
    public ActionResult Export()
    {
        // convert to xml
        var persons = _dbContext.People.Include(x => x.Pets).ToList();
        if (persons.Count == 0)
        {
            return BadRequest("Import data first");
        }

        var settings = new XmlWriterSettings() { Indent = true, Encoding = Encoding.UTF8 };
        using var stringWriter = new StringWriter();
        using var writer = XmlWriter.Create(stringWriter, settings);
        var serializer = new XmlSerializer(persons.GetType());
        serializer.Serialize(writer, persons);

        // write xml to bytes
        var textToSend = stringWriter.ToString();
        var bytes = Encoding.UTF8.GetBytes(textToSend);

        // serve as file
        return File(bytes, "application/octet-stream", "exported_data.xml");
    }
}
