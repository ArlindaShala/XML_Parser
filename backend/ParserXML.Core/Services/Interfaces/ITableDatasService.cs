namespace ParserXML.Core.Services.Interfaces
{
    public interface ITableDatasService
    {
        Task<bool> ProcessFileAsync(Stream xmlStream, string excelFilePath, List<string> selectedNodes, string exportOption);
    }
}
