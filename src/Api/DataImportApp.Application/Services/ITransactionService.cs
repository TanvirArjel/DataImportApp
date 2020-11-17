using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataImportApp.Application.Dtos;
using DataImportApp.Domain.Enums;
using Microsoft.AspNetCore.Http;
using TanvirArjel.Extensions.Microsoft.DependencyInjection;

namespace DataImportApp.Application.Services
{
    public interface ITransactionService : IScopedService
    {
        Task<List<TransactionDto>> GetListAsync();

        Task<List<TransactionDto>> GetListByCurrecyAsync(string currency);

        Task<List<TransactionDto>> GetListByDateRangeAsync(DateTime fromDateUtc, DateTime toDateUtc);

        Task<List<TransactionDto>> GetListByStatusAsync(TransactionStatus status);

        Task ImportDataFromFilesAsync(IEnumerable<IFormFile> files);
    }
}
