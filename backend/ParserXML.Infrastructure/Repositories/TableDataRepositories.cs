using ParserXML.Core.DTOs;
using ParserXML.Core.Repositories;
using System.Xml.Linq;

namespace ParserXML.Infrastructure.Repositories
{
    public class TableDataRepositories : ITableDatasRepository
    {
        public TableDataRepositories()
        {

        }

        public List<TableDataDto> ParseXml(Stream xmlStream, List<string> selectedNodes)
        {
            var dataList = new List<TableDataDto>();
            var xmlDoc = XDocument.Load(xmlStream);

            foreach (var element in xmlDoc.Descendants("Person"))
            {
                var data = new TableDataDto();

                
                if (selectedNodes == null || selectedNodes.Count == 0 || selectedNodes.Contains("Name"))
                    data.Name = element.Element("Name")?.Value;

                if (selectedNodes == null || selectedNodes.Count == 0 || selectedNodes.Contains("Vorname"))
                    data.Vorname = element.Element("Vorname")?.Value;

                if (selectedNodes == null || selectedNodes.Count == 0 || selectedNodes.Contains("Geschlecht"))
                    data.Geschlecht = element.Element("Geschlecht")?.Value;

                if (selectedNodes == null || selectedNodes.Count == 0 || selectedNodes.Contains("Alter"))
                    data.Alter = int.TryParse(element.Element("Alter")?.Value, out var age) ? age : 0;

                if (selectedNodes == null || selectedNodes.Count == 0 || selectedNodes.Contains("Adresse"))
                    data.Adresse = element.Element("Adresse")?.Value;

                if (selectedNodes == null || selectedNodes.Count == 0 || selectedNodes.Contains("Geburtsort"))
                    data.Geburtsort = element.Element("Geburtsort")?.Value;

                if (selectedNodes == null || selectedNodes.Count == 0 || selectedNodes.Contains("Telefonnummer"))
                    data.Telefonnummer = element.Element("Telefonnummer")?.Value;

                if (selectedNodes == null || selectedNodes.Count == 0 || selectedNodes.Contains("Mobilnummer"))
                    data.Mobilnummer = element.Element("Mobilnummer")?.Value;

                if (selectedNodes == null || selectedNodes.Count == 0 || selectedNodes.Contains("Kommentar"))
                    data.Kommentar = element.Element("Kommentar")?.Value;

                dataList.Add(data);
            }

            return dataList;
        }

    }
}
