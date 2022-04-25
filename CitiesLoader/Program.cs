using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using ExcelDataReader;
using Newtonsoft.Json;
using OfficeOpenXml;

namespace CitiesLoader
{
    class Program
    {
        static void Main(string[] args)
        {
            var program = new Program();
            program.LoadExcelFile();
        }

        private async void LoadExcelFile()
        {
            HttpClient client = new HttpClient();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var package = new ExcelPackage(new FileInfo(@"C:\Users\ddgam\Downloads\miasta_1.xlsx"));
            var firstSheet = package.Workbook.Worksheets["Sheet1"];
            var rowCount = firstSheet.Dimension.End.Row; //get row count
            for (var row = 2; row <= rowCount; row++)
            {
                var cityName = firstSheet.Cells[row, 2].Value.ToString()?.Trim();

                var values = new DataRequest()
                {
                    cityName = cityName,
                    countryId = "596a299c-e86e-4254-a0ea-fb030726263c",
                    verified = true
                };

                var content = JsonConvert.SerializeObject(values);
                var httpContent = new StringContent(content, Encoding.UTF8, "application/json");
                
                var response = client.PostAsync("https://pietnascie40.com.pl:5001/cities/create/city", httpContent)
                    .Result;

                var responseString = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine(response.StatusCode == HttpStatusCode.OK
                    ? $"{cityName} added successfully!"
                    : $"Error with {cityName}! Message: {responseString}");
            }
        }
    }

    class DataRequest
    {
        public string cityName { get; set; }
        public string countryId { get; set; }
        public bool verified { get; set; }
    }
}