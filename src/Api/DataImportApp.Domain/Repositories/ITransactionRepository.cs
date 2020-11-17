using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataImportApp.Domain.Entities;
using DataImportApp.Domain.Enums;
using TanvirArjel.Extensions.Microsoft.DependencyInjection;

namespace DataImportApp.Domain.Repositories
{
    public interface ITransactionRepository : IScopedService
    {
        Task<List<Transaction>> GetListAsync();

        Task<List<Transaction>> GetListByCurrecyAsync(string currency);

        Task<List<Transaction>> GetListByDateRangeAsync(DateTime fromDateUtc, DateTime toDateUtc);

        Task<List<Transaction>> GetListByStatusAsync(TransactionStatus status);

        Task InsertAsync(IEnumerable<Transaction> transactions);
    }
}
