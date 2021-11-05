# googlecsv2yealink
Convert Google Contacts CSV to Yealink Remote Phone Book XML.

## NuGet Dependencies
|Package|Use|
|---|---|
|CsvHelper|Used to parse CSV export from [Google Contacts](https://contacts.google.com)


**Example Usage**: ./googlecsv2yealink /Users/myuser/Downloads/contacts.csv

By executing the main method, this will take the name and primary phone number and convert it to the XML file format used by YeaLink IP Phones.

`<IPPhoneDirectory xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
<DirectoryEntry>
<Name>My Contacts Name</Name>
<Telephone>15554448888</Telephone>
</DirectoryEntry>
<DirectoryEntry>
<Name>My Contacts Name</Name>
<Telephone>15558883333</Telephone>
</DirectoryEntry></IPPhoneDirectory>`

## Note
This is not an exhaustive conversion process. The code fairly trivial at best and may not work out of the box for you. I've handled some of the common formatting cleanup for USA-based numbers. Modify it as you need to!