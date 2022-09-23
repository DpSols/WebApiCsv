using CsvHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApiCsv.Models;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper.Configuration;

namespace WebApiCsv.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ImportController : ControllerBase
{
    private readonly ILogger<ImportController> _logger;
    private readonly ApplicationDbContext _dbContext;

    public ImportController(ILogger<ImportController> logger, ApplicationDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    [HttpPost]
    public ActionResult Import(IFormFile file)
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Database.EnsureCreated();
        if (!file.FileName.EndsWith(".csv"))
        {
            return BadRequest("Invalid file format");
        }
        else if (file.Length <= 0)
        {
            return BadRequest("Empty file");
        }

        var persons = ProcessFile(file);
        _dbContext.People.AddRange(persons);
        _dbContext.SaveChanges();

        return Ok();
    }

    private List<Person> ProcessFile(IFormFile file)
    {
        var people = new List<Person>();
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            MissingFieldFound = null,
        };
        using var reader = new StreamReader(file.OpenReadStream());
        using var csv = new CsvReader(reader, config);

        csv.Read();
        csv.ReadHeader();
        while (csv.Read())
        {
            people.Add(new Person
            {
                Name = csv.GetField<string>("PersonName"),
                Age = csv.GetField<int>("Age"),
                Pets = new List<Pet>()
                {
                    new Pet
                    {
                        Name = csv.GetField<string>("Pet 1"),
                        PetType = csv.GetField<string>("Pet 1 Type"),
                    },
                    new Pet
                    {
                        Name = csv.GetField<string>("Pet 2"),
                        PetType = csv.GetField<string>("Pet 2 Type"),
                    },
                    new Pet
                    {
                        Name = csv.GetField<string>("Pet 3"),
                        PetType = csv.GetField<string>("Pet 3 Type"),
                    },
                },
            });
        }

        RemoveEmptyData(people);
        return people;
    }

    private void RemoveEmptyData(List<Person> persons)
        //removes dashes
    {
        for (int i = 0; i < persons.Count; ++i)
        {
            for (int j = persons[i].Pets.Count - 1; j >= 0; --j)
            {
                persons[i].Pets[j].Person = persons[i];
                if (persons[i].Pets[j].Name == "-")
                {
                    persons[i].Pets.RemoveAt(j);
                }
            }
        }
    }
}
