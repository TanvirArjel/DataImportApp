using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPageApp.Services;
using RazorPageApp.ValidationAttributes;

namespace RazorPageApp.Pages
{
    public class ImportModel : PageModel
    {
        private readonly ITransactionService _dataImportService;

        public ImportModel(ITransactionService dataImportService)
        {
            _dataImportService = dataImportService;
        }

        [AllowedFiles(new string[] { ".csv", ".xml" }, ErrorMessage = "Only csv and xml files are allowed.")]
        [MaxFileSize(1 * 1024 * 1024)]
        [Required]
        public List<IFormFile> Files { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _dataImportService.UploadFilesAsync(Files);
                    return RedirectToPage("./TransactionList");
                }
                catch (Exception)
                {
                    ModelState.AddModelError(string.Empty, "There is some problem with the service. Please try again. If the problem persist, Please contract with system administrator.");
                }
            }

            return Page();
        }
    }
}
