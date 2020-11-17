using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml.Serialization;
using CsvHelper;
using DataImportApp.Application.Common;
using DataImportApp.Domain.Entities;
using DataImportApp.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace DataImportApp.Application.Implementations.Common
{
    internal class TransactionDataImporter : ITransactionDataImporter
    {
        public List<Transaction> GetListFromCsvFile(IFormFile csvFile)
        {
            try
            {
                if (csvFile == null)
                {
                    throw new ArgumentNullException(nameof(csvFile));
                }

                List<Transaction> transactions = new List<Transaction>();

                StreamReader streamReader = new StreamReader(csvFile.OpenReadStream());
                using (CsvReader csv = new CsvReader(streamReader, CultureInfo.InvariantCulture))
                {
                    while (csv.Read())
                    {
                        string transactionId = csv[0];
                        decimal amount = Convert.ToDecimal(csv[1], CultureInfo.InvariantCulture);
                        string currency = csv[2];
                        string transactionDate = csv[3];
                        string status = csv[4];

                        string[] record = csv.Context.Record;

                        Transaction transaction = new Transaction()
                        {
                            Id = transactionId,
                            Amount = amount,
                            CurrencyCode = currency,
                            TransactionDate = DateTime.ParseExact(transactionDate, "dd/MM/yyyy hh:mm:ss", CultureInfo.InvariantCulture),
                            Status = (TransactionStatus)Enum.Parse(typeof(TransactionStatus), status)
                        };

                        transactions.Add(transaction);
                    }
                }

                return transactions;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        public List<Transaction> GetListFromXmlFile(IFormFile xmlFile)
        {
            try
            {
                if (xmlFile == null)
                {
                    throw new ArgumentNullException(nameof(xmlFile));
                }

                List<Transaction> transactions = new List<Transaction>();

                using (StreamReader reader = new StreamReader(xmlFile.OpenReadStream()))
                {
                    XmlRootAttribute xRoot = new XmlRootAttribute
                    {
                        ElementName = "Transactions"
                    };
                    XmlSerializer serializer = new XmlSerializer(typeof(XmlTransactions), xRoot);
                    XmlTransactions xmlTransactions = (XmlTransactions)serializer.Deserialize(reader);

                    if (xmlTransactions?.Transactions.Count > 0)
                    {
                        foreach (XmlTransaction item in xmlTransactions.Transactions)
                        {
                            Transaction transaction = new Transaction()
                            {
                                Id = item.Id,
                                Amount = item.PaymentDetails.Amount,
                                CurrencyCode = item.PaymentDetails.CurrencyCode,
                                TransactionDate = DateTime.ParseExact(item.TransactionDate, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture),
                                Status = (TransactionStatus)Enum.Parse(typeof(TransactionStatus), item.Status)
                            };

                            transactions.Add(transaction);
                        }
                    }
                }

                return transactions;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }
    }

    [Serializable]
    [XmlRootAttribute("Transactions")]
    public class XmlTransactions
    {
        [XmlElement("Transaction")]
        public List<XmlTransaction> Transactions { get; set; }
    }

    [Serializable]
    public class XmlTransaction
    {
        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlElement("TransactionDate")]
        public string TransactionDate { get; set; }

        [XmlElement("PaymentDetails")]
        public PaymentDetails PaymentDetails { get; set; }

        [XmlElement("Status")]
        public string Status { get; set; }
    }

    [Serializable]
    public class PaymentDetails
    {
        [XmlElement("Amount")]
        public decimal Amount { get; set; }

        [XmlElement("CurrencyCode")]
        public string CurrencyCode { get; set; }
    }
}
