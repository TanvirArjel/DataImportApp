using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace RazorPageApp.ValidationAttributes
{
    public class AllowedFilesAttribute : ValidationAttribute
    {
        private readonly string[] _extensions;
        public AllowedFilesAttribute(string[] extensions)
        {
            _extensions = extensions;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            List<IFormFile> files = value as List<IFormFile>;

            foreach (IFormFile file in files)
            {
                if (file != null)
                {
                    string extension = Path.GetExtension(file.FileName);
                    if (!_extensions.Contains(extension.ToLower()))
                    {
                        string errorMesssage = ErrorMessage ?? GetErrorMessage();
                        return new ValidationResult(errorMesssage);
                    }
                }
            }


            return ValidationResult.Success;
        }

        public string GetErrorMessage()
        {
            return $"This file extension is not allowed!";
        }
    }
}
