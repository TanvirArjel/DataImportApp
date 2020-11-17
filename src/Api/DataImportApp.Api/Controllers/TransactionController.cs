using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DataImportApp.Application.Dtos;
using DataImportApp.Application.Services;
using DataImportApp.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DataImportApp.Api.Controllers
{
    [Route("api/transactions")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        // GET: api/transactions
        [HttpGet]
        public async Task<IEnumerable<TransactionDto>> Get()
        {
            List<TransactionDto> transactions = await _transactionService.GetListAsync();
            return transactions;
        }

        // GET: api/transactions/by-currency
        [Route("by-currency")]
        [HttpGet]
        public async Task<IActionResult> GetByCurrency(string currency)
        {
            if (string.IsNullOrWhiteSpace(currency))
            {
                return BadRequest($"`{nameof(currency)}` cannot be null or empty.");
            }

            List<TransactionDto> transactions = await _transactionService.GetListByCurrecyAsync(currency);
            return Ok(transactions);
        }

        // GET: api/transactions/by-date-range
        [Route("by-date-range")]
        [HttpGet]
        public async Task<IActionResult> GetByDateRange(DateTime fromDateUtc, DateTime toDateUtc)
        {
            if (fromDateUtc >= toDateUtc)
            {
                return BadRequest($"`{nameof(fromDateUtc)}` cannot be greater than `{nameof(toDateUtc)}`");
            }

            List<TransactionDto> transactions = await _transactionService.GetListByDateRangeAsync(fromDateUtc, toDateUtc);
            return Ok(transactions);
        }

        // GET: api/transactions/by-status
        [Route("by-status")]
        [HttpGet]
        public async Task<IActionResult> GetByStatus(string status)
        {
            if (Enum.TryParse(status, out TransactionStatus transactionStatus))
            {
                List<TransactionDto> transactions = await _transactionService.GetListByStatusAsync(transactionStatus);
                return Ok(transactions);
            }

            return BadRequest($"`{nameof(status)}` value is not valid.");
        }

        // POST api/transactions/import-from-files
        [Route("import-from-files")]
        [HttpPost]
        public async Task<IActionResult> ImportFromFiles(List<IFormFile> files)
        {
            if (files == null || files.Count < 1)
            {
                return BadRequest($"`{nameof(files)}` collection can not be null or empty.");
            }

            foreach (IFormFile formFile in files)
            {
                string[] allowdFiles = new string[] { ".csv", ".xml", ".txt" };
                string fileExtension = Path.GetExtension(formFile.FileName);
                if (allowdFiles.Contains(fileExtension) == false)
                {
                    return BadRequest("Only csv and xml files are allowed");
                }
            }

            await _transactionService.ImportDataFromFilesAsync(files);
            return Ok();
        }
    }
}
