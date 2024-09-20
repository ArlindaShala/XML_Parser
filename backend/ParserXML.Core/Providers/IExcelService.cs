using ParserXML.Core.DTOs;

namespace ParserXML.Core.Providers
{
    public interface IExcelService
    {
        void ExportToExcel(List<TableDataDto> dataList, string filePath);
        void AppendToExistingExcel(List<TableDataDto> dataList, string filePath, List<string> selectedNodes);
    }
}
