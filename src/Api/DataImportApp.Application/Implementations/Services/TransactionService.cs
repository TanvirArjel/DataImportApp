using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DataImportApp.Application.Common;
using DataImportApp.Application.Dtos;
using DataImportApp.Application.Services;
using DataImportApp.Domain.Entities;
using DataImportApp.Domain.Enums;
using DataImportApp.Domain.Repositories;
using Microsoft.AspNetCore.Http;

namespace DataImportApp.Application.Implementations.Services
{
    internal class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ITransactionDataImporter _transactionDataImporter;

        public TransactionService(ITransactionRepository transactionRepository, ITransactionDataImporter transactionDataImporter)
        {
            _transactionRepository = transactionRepository;
            _transactionDataImporter = transactionDataImporter;
        }

        public async Task<List<TransactionDto>> GetListAsync()
        {
            List<Transaction> transactions = await _transactionRepository.GetListAsync();
            List<TransactionDto> transactionDtoList = GetTransactionDtoList(transactions);
            return transactionDtoList;
        }

        public async Task<List<TransactionDto>> GetListByCurrecyAsync(string currency)
        {
            if (string.IsNullOrWhiteSpace(currency))
            {
                throw new ArgumentNullException(nameof(currency));
            }

            List<Transaction> transactions = await _transactionRepository.GetListByCurrecyAsync(currency);
            List<TransactionDto> transactionDtoList = GetTransactionDtoList(transactions);
            return transactionDtoList;
        }

        public async Task<List<TransactionDto>> GetListByDateRangeAsync(DateTime fromDateUtc, DateTime toDateUtc)
        {
            if (fromDateUtc >= toDateUtc)
            {
                throw new ArgumentException($"{nameof(fromDateUtc)} cannot be greater than {nameof(toDateUtc)}");
            }

            List<Transaction> transactions = await _transactionRepository.GetListByDateRangeAsync(fromDateUtc, toDateUtc);
            List<TransactionDto> transactionDtoList = GetTransactionDtoList(transactions);
            return transactionDtoList;
        }

        public async Task<List<TransactionDto>> GetListByStatusAsync(TransactionStatus status)
        {
            List<Transaction> transactions = await _transactionRepository.GetListByStatusAsync(status);
            List<TransactionDto> transactionDtoList = GetTransactionDtoList(transactions);
            return transactionDtoList;
        }

        public async Task ImportDataFromFilesAsync(IEnumerable<IFormFile> files)
        {
            if (files == null || files.Any() == false)
            {
                throw new ArgumentException($"`{nameof(files)}` collection can not be null or empty.");
            }

            foreach (IFormFile file in files)
            {
                if (Path.GetExtension(file.FileName) == ".csv" || Path.GetExtension(file.FileName) == ".txt")
                {
                    List<Transaction> transactions = _transactionDataImporter.GetListFromCsvFile(file);

                    await _transactionRepository.InsertAsync(transactions);
                }

                if (Path.GetExtension(file.FileName) == ".xml")
                {
                    List<Transaction> transactions = _transactionDataImporter.GetListFromXmlFile(file);

                    await _transactionRepository.InsertAsync(transactions);
                }
            }
        }

        private static List<TransactionDto> GetTransactionDtoList(List<Transaction> transactions)
        {
            List<TransactionDto> transactionDtoList = transactions.Select(tr => new TransactionDto
            {
                Id = tr.Id,
                Payment = tr.Amount + " " + tr.CurrencyCode,
                Status = tr.Status == TransactionStatus.Approved ? "A" : (tr.Status == TransactionStatus.Rejected || tr.Status == TransactionStatus.Failed ? "R" : "D")
            }).ToList();

            return transactionDtoList;
        }
    }
}
