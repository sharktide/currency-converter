using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CurrencyConverter
{
    public partial class MainPage : ContentPage
    {
        public List<string> Currencies { get; set; }

        public MainPage()
        {
            InitializeComponent();
            Currencies = new List<string>();

            FetchAvailableCurrencies();
        }

        private async void OnConvertClicked(object sender, EventArgs e)
        {
            string amountText = amountEntry.Text;

            if (string.IsNullOrEmpty(amountText) || !decimal.TryParse(amountText, out decimal amount))
            {
                resultLabel.Text = "Please enter a valid amount.";
                return;
            }

            if (fromCurrencyPicker.SelectedItem == null || toCurrencyPicker.SelectedItem == null)
            {
                resultLabel.Text = "Please select both currencies.";
                return;
            }

            string fromCurrency = fromCurrencyPicker.SelectedItem.ToString();
            string toCurrency = toCurrencyPicker.SelectedItem.ToString();

            string convertedAmount = await ConvertCurrency(amount, fromCurrency, toCurrency);

            if (convertedAmount == null)
            {
                resultLabel.Text = "Error converting currencies. Please try again.";
            }
            else
            {
                resultLabel.Text = $"{amount} {fromCurrency} = {convertedAmount} {toCurrency}";
            }
        }

        private async Task<string> ConvertCurrency(decimal amount, string fromCurrency, string toCurrency)
        {
            try
            {
                string apiUrl = $"https://sharktide-currency-api.hf.space/convert?amount={amount}&from_currency={fromCurrency}&to_currency={toCurrency}";

                resultLabel.Text = $"FETCHING URL: {apiUrl}\n";

                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    resultLabel.Text += $"Response Status: {response.StatusCode}\n";

                    if (!response.IsSuccessStatusCode)
                    {
                        resultLabel.Text += "API request failed.\n";
                        return null;
                    }

                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    resultLabel.Text += $"Raw JSON Response: {jsonResponse}\n";

                    try
                    {
                        var responseObject = JsonSerializer.Deserialize<JsonResponse>(jsonResponse);

                        if (responseObject == null)
                        {
                            resultLabel.Text += "Failed to deserialize the response. The structure might be wrong.\n";
                            return null;
                        }

                        resultLabel.Text += $"Parsed Converted Amount: {responseObject.ConvertedAmount}\n";

                        return responseObject.ConvertedAmount;
                    }
                    catch (JsonException ex)
                    {
                        resultLabel.Text += $"JSON Deserialization Error: {ex.Message}\n";
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                resultLabel.Text += $"Error: {ex.Message}\n";
                return null;
            }
        }

        private async void FetchAvailableCurrencies()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string apiUrl = "https://sharktide-currency-api.hf.space/exchange-rates";
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();

                        var ratesData = JsonSerializer.Deserialize<JsonRates>(jsonResponse);

                        if (ratesData != null && ratesData.ConversionRates != null)
                        {
                            foreach (var currency in ratesData.ConversionRates)
                            {
                                Currencies.Add(currency.Key);  
                            }

                            fromCurrencyPicker.ItemsSource = Currencies;
                            toCurrencyPicker.ItemsSource = Currencies;
                        }
                    }
                    else
                    {
                        resultLabel.Text = "Failed to fetch available currencies.";
                    }
                }
            }
            catch (Exception ex)
            {
                resultLabel.Text = $"Error fetching currencies: {ex.Message}";
            }
        }

        public class JsonResponse
        {
            [JsonPropertyName("converted_amount")]
            public string ConvertedAmount { get; set; }
        }

        public class JsonRates
        {
            [JsonPropertyName("conversion_rates")]
            public Dictionary<string, decimal> ConversionRates { get; set; }
        }
    }
}
