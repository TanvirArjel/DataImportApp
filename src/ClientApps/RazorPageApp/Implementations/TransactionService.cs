using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using RazorPageApp.Services;
using RazorPageApp.ViewModels;

namespace RazorPageApp.Implementations
{
    public class TransactionService : ITransactionService
    {
        private readonly HttpClient _httpClient;

        public TransactionService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("DataImportApp");
        }

        private static JsonSerializerOptions JsonSerializerOptions => new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        };

        public async Task<List<TransactionDetailsViewModel>> GetListAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("transactions");

            if (response.IsSuccessStatusCode)
            {
                string responseString = await response.Content.ReadAsStringAsync();
                List<TransactionDetailsViewModel> transactions = JsonSerializer.Deserialize<List<TransactionDetailsViewModel>>(responseString, JsonSerializerOptions);
                return transactions;
            }

            throw new ApplicationException($"{response.ReasonPhrase}: The status code is: {(int)response.StatusCode}");
        }

        public async Task UploadFilesAsync(IEnumerable<IFormFile> files)
        {
            try
            {
                if (files == null)
                {
                    throw new ArgumentNullException(nameof(files));
                }

                MultipartFormDataContent multiContent = new MultipartFormDataContent();

                foreach (IFormFile file in files)
                {
                    byte[] data;
                    using (BinaryReader br = new BinaryReader(file.OpenReadStream()))
                    {
                        data = br.ReadBytes((int)file.OpenReadStream().Length);
                    }

                    ByteArrayContent bytes = new ByteArrayContent(data);

                    multiContent.Add(bytes, "files", file.FileName);
                }

                HttpResponseMessage response = await _httpClient.PostAsync("transactions/import-from-files", multiContent).ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    throw new ApplicationException($"{response.ReasonPhrase}: The status code is: {(int)response.StatusCode}");
                }

                //string responseMessage = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
