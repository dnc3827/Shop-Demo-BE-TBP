using DEMO_Shop.Models;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;

namespace DEMO_Shop.Services
{
    public class GoogleSheetService
    {
        private readonly SheetsService _service;
        private const string SpreadsheetId = "1bdnSjGvvgR_PRv57vbYRmzv7YASl7zJbBjBQFIVIQK8";

        public GoogleSheetService()
        {
            var credential = GoogleCredential
                .FromFile("credentials.json")
                .CreateScoped(SheetsService.Scope.Spreadsheets);

            _service = new SheetsService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "UserPreferenceApp"
            });
        }

        public void AddPreference(UserPreference p)
        {
            var range = "'Trang tính1'!A:F";
            var values = new List<IList<object>>
            {
                new List<object>
                {
                    p.FirstName ?? "",
                    p.LastName ?? "",
                    p.Phone ?? "",
                    p.Email ?? "",
                    p.Description ?? "",
                    p.CreatedAt.ToString("yyyy-MM-dd HH:mm")
                }
            };


            var body = new ValueRange
            {
                Values = values
            };

            var request = _service.Spreadsheets.Values.Append(
                body,
                SpreadsheetId,
                range
            );

            request.ValueInputOption =
                SpreadsheetsResource.ValuesResource.AppendRequest
                    .ValueInputOptionEnum.USERENTERED;

            try
            {
                request.Execute();
            }
            catch (Google.GoogleApiException ex)
            {
                Console.WriteLine("=== GOOGLE API ERROR ===");
                Console.WriteLine(ex.Message);

                if (ex.Error != null)
                {
                    Console.WriteLine("Error message: " + ex.Error.Message);
                    Console.WriteLine("Error code: " + ex.Error.Code);

                    if (ex.Error.Errors != null)
                    {
                        foreach (var err in ex.Error.Errors)
                        {
                            Console.WriteLine($"Reason: {err.Reason} - {err.Message}");
                        }
                    }
                }

                throw;
            }

        }
    }
}
