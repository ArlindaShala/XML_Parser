using Microsoft.AspNetCore.Mvc;
using ParserXML.Core.Services.Interfaces;

namespace ParserXML.Controllers
{
    [ApiController]
    [Route("parsers")]
    public class TableDataController : ControllerBase
    {
        private readonly ITableDatasService _tabelDatasService;
        private readonly ILogger<TableDataController> _logger;

        public TableDataController(ILogger<TableDataController> logger, ITableDatasService parsersService)
        {
            _logger = logger;
            _tabelDatasService = parsersService;
        }

        [HttpPost("import-from-xml")]
        public async Task<IActionResult> ImportFromXml([FromForm] IFormFile uploadedFile, [FromForm] string? nodesJson, [FromForm] string exportOption)
        {
            if (string.IsNullOrWhiteSpace(exportOption) || !(exportOption == "new" || exportOption == "existing"))
            {
                return BadRequest("Invalid exportOption. It must be either 'new' or 'existing'.");
            }

            if (uploadedFile == null || uploadedFile.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }


            List<string> selectedNodes = string.IsNullOrWhiteSpace(nodesJson)
                ? new List<string>()
                : System.Text.Json.JsonSerializer.Deserialize<List<string>>(nodesJson);


            string tempFilePath;
            if (exportOption == "new")
            {
                tempFilePath = Path.Combine(Path.GetTempPath(), $"{Path.GetFileNameWithoutExtension(uploadedFile.FileName)}.xlsx");
            }
            else
            {
                tempFilePath = Path.Combine(Path.GetTempPath(), $"{Path.GetFileNameWithoutExtension(uploadedFile.FileName)}.xlsx");
            }

            using var stream = uploadedFile.OpenReadStream();
            var result = await _tabelDatasService.ProcessFileAsync(stream, tempFilePath, selectedNodes, exportOption);

            var fileBytes = System.IO.File.ReadAllBytes(tempFilePath);
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", Path.GetFileName(tempFilePath));
        }
    }
}
