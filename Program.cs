// .net6.0
// Input CSV file to convert, passed from arguments.
using CsvHelper;
using CsvHelper.Configuration.Attributes;
using System.Globalization;
using System.Xml.Serialization;

if(String.IsNullOrEmpty(args[0]))
    throw new ConversionException("Filename was not specified.");

string filename = args[0];
if(!File.Exists(filename))
    throw new ConversionException("Filename specified does not exist.");

//ConversionHelper.Log("Starting Conversion");

StreamReader reader = new StreamReader(filename);
CsvReader csv = new CsvReader(reader, CultureInfo.InvariantCulture);
var records = csv.GetRecords<ConverstionLayout>().ToList();

Phonebook phonebook = new Phonebook();
phonebook.Entries = new List<PhonebookEntry>();

foreach(var record in records)
{
    // We can't convert the primary number if it is null or empty.
    if(!String.IsNullOrEmpty(record.Phone))
    {
        // We can't create an entry if we don't know who it is.
        if(String.IsNullOrEmpty(record.Name))
            continue;
        
        // It seems sometimes the primary phone contains multiple numbers delimited
        // by three colons. For my case, I'm OK with just taking the first number.
        if(record.Phone.Contains(":::"))
        {
            record.Phone = record.Phone.Split(":::")[0];
        }

        record.Phone = ConversionHelper.CleanNumber(record.Phone);
        //Console.WriteLine(String.Format("{0} > {1}", record.Name, record.Phone));

        phonebook.Entries.Add(new PhonebookEntry {
            Name = record.Name.Trim(),
            Telephone = record.Phone
        });
    }
}

// Create the XML.
string xml = ConversionHelper.CreateXML(phonebook);
Console.WriteLine(xml);

public class ConversionException : Exception
{
    public ConversionException() {}

    public ConversionException(string message) : base(message) {}
}

public class ConverstionLayout
{
    public string? Name {get; set;}

    [Name("Phone 1 - Value")]
    public string? Phone {get;set;}
}

[XmlRoot(ElementName = "IPPhoneDirectory")]
public class Phonebook
{
    [XmlElement(ElementName = "DirectoryEntry")]
    public List<PhonebookEntry> Entries {get; set;}
}

[XmlRoot(ElementName = "DirectoryEntry")]
public class PhonebookEntry
{
    [XmlElement(ElementName = "Name")]
    public string Name {get; set;}

    [XmlElement(ElementName = "Telephone")]
    public string Telephone {get; set;}
}

public static class ConversionHelper
{
    public static void Log(string message)
    {
        Console.WriteLine(String.Format("{0}: {1}", DateTime.UtcNow.ToString("yyyyMMddThhmmss"), message));
    }

    public static string CleanNumber(string phoneNumber)
    {
        // This could be handled much better, such as an array of regexes to apply
        // in a loop however for new programmers, I think this is easier to understand.
        // This is by no means an exhaustive way to clean up phone numbers. No complaining.
        phoneNumber = phoneNumber.Replace("+","");
        phoneNumber = phoneNumber.Replace("-","");
        phoneNumber = phoneNumber.Replace(" ","");
        phoneNumber = phoneNumber.Replace("(","");
        phoneNumber = phoneNumber.Replace(")","");

        if(!phoneNumber.StartsWith("1") && !phoneNumber.StartsWith("*") && !phoneNumber.StartsWith("#"))
            phoneNumber = String.Concat("1", phoneNumber);

        return phoneNumber.Trim();
    }

    public static string CreateXML(Phonebook phonebook)
    {
        string output = String.Empty;
        XmlSerializer xml = new XmlSerializer(phonebook.GetType());
        using(StringWriter writer = new StringWriter())
        {
            xml.Serialize(writer, phonebook);
            output = writer.ToString();
        }

        return output;
    }
}