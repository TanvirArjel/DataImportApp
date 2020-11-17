using System;
using DataImportApp.Domain.Enums;

namespace DataImportApp.Domain.Entities
{
    public class Transaction
    {
        public string Id { get; set; }

        public decimal Amount { get; set; }

        public string CurrencyCode { get; set; }

        public DateTime TransactionDate { get; set; }

        public TransactionStatus Status { get; set; }
    }
}
