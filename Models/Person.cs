using System.Collections.Generic;
using System.Xml.Serialization;

namespace WebApiCsv.Models;

public class Person
{
    [XmlAttribute]
    public int Id { get; set; }
    [XmlAttribute]
    public string Name { get; set; }
    [XmlAttribute]
    public int Age { get; set; }
    public List<Pet> Pets { get; set; }
}
