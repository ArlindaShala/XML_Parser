using ParserXML.Core.DTOs;


namespace ParserXML.Core.Repositories
{
    public interface ITableDatasRepository
    {
        List<TableDataDto> ParseXml(Stream xmlStream, List<string> selectedNodes);
    }
}
