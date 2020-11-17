using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataImportApp.Domain.Entities;
using DataImportApp.Domain.Enums;
using DataImportApp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DataImportApp.Infrastructure.Data.Repositories
{
    internal class TransactionRepository : ITransactionRepository
    {
        private readonly DataImportDbContext _dbContext;

        public TransactionRepository(DataImportDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Transaction>> GetListAsync()
        {
            List<Transaction> transactionList = await _dbContext.Set<Transaction>().AsNoTracking().ToListAsync();

            return transactionList;
        }

        public async Task<List<Transaction>> GetListByCurrecyAsync(string currency)
        {
            List<Transaction> transactionList = await _dbContext.Set<Transaction>()
                .Where(tr => tr.CurrencyCode == currency).AsNoTracking().ToListAsync();

            return transactionList;
        }

        public async Task<List<Transaction>> GetListByDateRangeAsync(DateTime fromDateUtc, DateTime toDateUtc)
        {
            List<Transaction> transactionList = await _dbContext.Set<Transaction>()
                .Where(tr => tr.TransactionDate >= fromDateUtc && tr.TransactionDate <= toDateUtc).AsNoTracking().ToListAsync();

            return transactionList;
        }

        public async Task<List<Transaction>> GetListByStatusAsync(TransactionStatus status)
        {
            List<Transaction> transactionList = await _dbContext.Set<Transaction>()
                .Where(tr => tr.Status == status).AsNoTracking().ToListAsync();

            return transactionList;
        }

        public async Task InsertAsync(IEnumerable<Transaction> transactions)
        {
            if (transactions == null)
            {
                throw new ArgumentNullException(nameof(transactions));
            }

            await _dbContext.Set<Transaction>().AddRangeAsync(transactions);
            await _dbContext.SaveChangesAsync();
        }
    }
}
