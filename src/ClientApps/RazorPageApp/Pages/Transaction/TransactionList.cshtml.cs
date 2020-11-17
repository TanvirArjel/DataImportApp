using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPageApp.Services;
using RazorPageApp.ViewModels;

namespace RazorPageApp.Pages.Transaction
{
    public class TransactionListModel : PageModel
    {
        private readonly ITransactionService _transactionService;

        public TransactionListModel(ITransactionService employeeService)
        {
            _transactionService = employeeService;
        }

        public List<TransactionDetailsViewModel> TransactionList { get; private set; }

        public string ErrorMessage { get; set; }

        public async Task OnGetAsync()
        {
            try
            {
                TransactionList = await _transactionService.GetListAsync();
            }
            catch (Exception)
            {
                ErrorMessage = "There is some problem with the service. Please try again. If the problem persist, Please contract with system administrator.";
            }
        }
    }
}
