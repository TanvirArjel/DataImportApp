using System.Collections.Generic;
using DataImportApp.Domain.Entities;
using Microsoft.AspNetCore.Http;
using TanvirArjel.Extensions.Microsoft.DependencyInjection;

namespace DataImportApp.Application.Common
{
    public interface ITransactionDataImporter : IScopedService
    {
        List<Transaction> GetListFromCsvFile(IFormFile csvFile);

        List<Transaction> GetListFromXmlFile(IFormFile xmlFile);
    }
}
