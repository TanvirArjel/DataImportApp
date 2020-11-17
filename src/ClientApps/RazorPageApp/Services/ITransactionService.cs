using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using RazorPageApp.ViewModels;
using TanvirArjel.Extensions.Microsoft.DependencyInjection;

namespace RazorPageApp.Services
{
    public interface ITransactionService : IScopedService
    {
        Task<List<TransactionDetailsViewModel>> GetListAsync();
        Task UploadFilesAsync(IEnumerable<IFormFile> files);
    }
}
