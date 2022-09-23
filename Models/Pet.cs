using System.Xml.Serialization;

namespace WebApiCsv.Models;

public class Pet
{
    [XmlAttribute]
    public int Id { get; set; }
    [XmlAttribute]
    public string Name { get; set; }
    [XmlAttribute]
    public string PetType { get; set; }

    [XmlIgnore]
    public int PersonId { get; set; }

    [XmlIgnore]
    public Person Person { get; set; }
}
