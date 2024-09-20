using ParserXML.Core.Providers;
using ParserXML.Core.Repositories;
using ParserXML.Core.Services.Interfaces;

namespace ParserXML.Core.Services.Implementations
{
    public class TableDatasService : ITableDatasService
    {
        private readonly IExcelService _excelService;
        private readonly ITableDatasRepository _tabelDatasRepository;

        public TableDatasService(ITableDatasRepository tabelDatasRepository, IExcelService excelService)
        {
            _tabelDatasRepository = tabelDatasRepository;
            _excelService = excelService;
        }

        public async Task<bool> ProcessFileAsync(Stream xmlStream, string excelFilePath, List<string> selectedNodes, string exportOption)
        {
            var dataList = await Task.Run(() => _tabelDatasRepository.ParseXml(xmlStream, selectedNodes));

            if (dataList.Count == 0)
            {
                Console.WriteLine("No data found in XML.");
                return false;
            }

            Console.WriteLine($"Parsed {dataList.Count} entries from XML.");

            if (exportOption == "new")
            {
                _excelService.ExportToExcel(dataList, excelFilePath);
                Console.WriteLine($"New file created: {excelFilePath}");
            }
            else if (exportOption == "existing")
            {
                if (File.Exists(excelFilePath))
                {
                    Console.WriteLine($"Appending to existing file: {excelFilePath}");
                    _excelService.AppendToExistingExcel(dataList, excelFilePath, selectedNodes);
                }
                else
                {
                    Console.WriteLine("File does not exist. Creating a new file.");
                    _excelService.ExportToExcel(dataList, excelFilePath);
                }
            }

            return true;
        }
    }
}
