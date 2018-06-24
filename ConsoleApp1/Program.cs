using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace ConsoleApp1
{
    public class Program
    {
        static readonly string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };
        static readonly string ApplicationName = "Data sheet";
        static readonly string SpreadsheetId = "1Kao4gA0lwVwos-YP_G2dHPFa4R1AP81YS84TBoRsr6k";
        static SheetsService service;
        CultureInfo info = CultureInfo.GetCultureInfo("en-EN");

        static void Main(string[] args)
        {
            GoogleCredential credential;
            using (var stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream)
                    .CreateScoped(Scopes);
            }

            service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            ReadEntries();
            Console.Read();
        }

        static void ReadEntries()
        {
            var range = "SampleData.csv!A1:ALL";
            SpreadsheetsResource.ValuesResource.GetRequest request =
                    service.Spreadsheets.Values.Get(SpreadsheetId, range);

            var response = request.Execute();
            IList<object> values = response.Values[0];

            if (values != null && values.Count > 0)
            {
                decimal ArithmeticMean = GetArithmeticMean(values);
                decimal StandardDeviation = GetStandardDeviation(values);
                int[] a = GetDataHistogram(values);

                Console.WriteLine($"Arithmetic mean: {ArithmeticMean}");
                Console.WriteLine($"Standard Deviation: {StandardDeviation}");
                int[] histogram = GetDataHistogram(values);
                ShowDataFromHistogram(histogram);
            }
            else
            {
                Console.WriteLine("No data found.");
            }

            Console.ReadKey();
        }

        public static decimal GetArithmeticMean(IList<object> dataList)
        {
            decimal sum = 0;
            foreach (var item in dataList)
            {
                CultureInfo info = CultureInfo.GetCultureInfo("en-EN");
                decimal number;
                Decimal.TryParse(item.ToString(), NumberStyles.Any, info, out number);
                sum += number;
            }

            decimal arithmeticMean = sum / dataList.Count;
            return arithmeticMean;
        }

        public static decimal GetStandardDeviation(IList<object> dataList)
        {
            decimal arithmeticMean = GetArithmeticMean(dataList);
            decimal partialSum = 0;

            foreach (var item in dataList)
            {
                CultureInfo info = CultureInfo.GetCultureInfo("en-EN");
                decimal number;
                Decimal.TryParse(item.ToString(), NumberStyles.Any, info, out number);
                partialSum += (number - arithmeticMean) * (number - arithmeticMean);
            }

            decimal variance = partialSum / dataList.Count;
            decimal standardDeviation = (decimal)Math.Sqrt((double)variance);

            return standardDeviation;
        }

        public static int[] GetDataHistogram(IList<object> dataList)
        {
            int[] histogram = new int[10];

            foreach (var item in dataList)
            {
                CultureInfo info = CultureInfo.GetCultureInfo("en-EN");
                decimal number;
                Decimal.TryParse(item.ToString(), NumberStyles.Any, info, out number);

                if (number < 10)
                    histogram[0]++;
                else if (number < 20)
                    histogram[1]++;
                else if (number < 30)
                    histogram[2]++;
                else if (number < 40)
                    histogram[3]++;
                else if (number < 50)
                    histogram[4]++;
                else if (number < 60)
                    histogram[5]++;
                else if (number < 70)
                    histogram[6]++;
                else if (number < 80)
                    histogram[7]++;
                else if (number < 90)
                    histogram[8]++;
                else 
                    histogram[9]++;
            }

            return histogram;
        }

        public static void ShowDataFromHistogram(int[] histogram)
        {
            for (int i = 0; i < histogram.Length; i++)
            {
                Console.WriteLine($"Amount of numbers higher or equal {i} and lower than {i+1}: {histogram[i]}");
            }
        }
    }

}
