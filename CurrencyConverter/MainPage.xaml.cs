/*using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace CurrencyConverter
{
    public partial class MainPage : ContentPage
    {
        private const string apiUrl = "https://sharktide-currency-api.hf.space/convert";  // Replace with your Hugging Face Space URL

        public List<string> Currencies { get; set; }

        public MainPage()
        {
            InitializeComponent();

            Currencies = new List<string> { "USD", "EUR", "GBP", "INR" };

            // Bind the Picker to the list of currencies
            fromCurrencyPicker.ItemsSource = Currencies;
            toCurrencyPicker.ItemsSource = Currencies;
        }

        private async void OnConvertClicked(object sender, EventArgs e)
        {
            string amountText = amountEntry.Text;
            if (string.IsNullOrEmpty(amountText) || !decimal.TryParse(amountText, out decimal amount))
            {
                resultLabel.Text = "Please enter a valid amount.";
                return;
            }

            string fromCurrency = fromCurrencyPicker.SelectedItem.ToString();
            string toCurrency = toCurrencyPicker.SelectedItem.ToString();

            decimal convertedAmount = await ConvertCurrency(amount, fromCurrency, toCurrency);

            if (convertedAmount == -1)
            {
                resultLabel.Text = "Error converting currencies. Please try again.";
            }
            else
            {
                resultLabel.Text = $"{amount} {fromCurrency} = {convertedAmount} {toCurrency}";
            }
        }

        private async Task<decimal> ConvertCurrency(decimal amount, string fromCurrency, string toCurrency)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Construct the API request URL
                    string url = $"{apiUrl}?amount={amount}&from_currency={fromCurrency}&to_currency={toCurrency}";

                    // Send the GET request to the API
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();

                    // Read the JSON response
                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    // Parse the JSON response using System.Text.Json
                    var jsonDoc = JsonDocument.Parse(jsonResponse);
                    if (jsonDoc.RootElement.TryGetProperty("converted_amount", out JsonElement convertedAmountElement))
                    {
                        return convertedAmountElement.GetDecimal();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }

            return -1; // Error occurred
        }
    }
}

*/
/*
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using System.Text.Json;

namespace CurrencyConverter
{
    public partial class MainPage : ContentPage
    {
        // List of available currencies
        public List<string> Currencies { get; set; }

        public MainPage()
        {
            InitializeComponent();

            // Initialize the list of currencies
            Currencies = new List<string> { "USD", "EUR", "GBP", "INR" };

            // Bind the Picker controls to the list of currencies
            fromCurrencyPicker.ItemsSource = Currencies;
            toCurrencyPicker.ItemsSource = Currencies;
        }

        // Event handler for the Convert button click
        private async void OnConvertClicked(object sender, EventArgs e)
        {
            string amountText = amountEntry.Text;

            // Validate the input amount
            if (string.IsNullOrEmpty(amountText) || !decimal.TryParse(amountText, out decimal amount))
            {
                resultLabel.Text = "Please enter a valid amount.";
                return;
            }

            // Check if both currencies are selected
            if (fromCurrencyPicker.SelectedItem == null || toCurrencyPicker.SelectedItem == null)
            {
                resultLabel.Text = "Please select both currencies.";
                return;
            }

            string fromCurrency = fromCurrencyPicker.SelectedItem.ToString();
            string toCurrency = toCurrencyPicker.SelectedItem.ToString();

            // Perform the conversion using the API
            decimal convertedAmount = await ConvertCurrency(amount, fromCurrency, toCurrency);

            // Show the result
            if (convertedAmount == -1)
            {
                resultLabel.Text = "Error converting currencies. Please try again.";
            }
            else
            {
                resultLabel.Text = $"{amount} {fromCurrency} = {convertedAmount} {toCurrency}";
            }
        }

        // Method to call the currency conversion API
        private async Task<decimal> ConvertCurrency(decimal amount, string fromCurrency, string toCurrency)
        {
            try
            {
                // Define the API endpoint and build the query URL
                string apiUrl = $"https://sharktide-currency-api.hf.space/convert?amount={amount}&from_currency={fromCurrency}&to_currency={toCurrency}";

                // Create an HttpClient instance
                using (HttpClient client = new HttpClient())
                {
                    // Make the API call and get the response
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    // Ensure the response is successful
                    if (!response.IsSuccessStatusCode)
                    {
                        resultLabel.Text = "API request failed.";
                        return -1;
                    }

                    // Read the response content as a string
                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    // Parse the JSON response
                    var responseObject = JsonSerializer.Deserialize<JsonResponse>(jsonResponse);

                    // Return the converted amount
                    return responseObject?.ConvertedAmount ?? -1;
                }
            }
            catch (Exception ex)
            {
                // Handle any errors
                resultLabel.Text = $"Error: {ex.Message}";
                return -1;
            }
        }

        // Class to map the response JSON
        public class JsonResponse
        {
            public decimal ConvertedAmount { get; set; }
        }
    }
}

*/

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
        // List of available currencies
        public List<string> Currencies { get; set; }

        public MainPage()
        {
            InitializeComponent();

            // Initialize the list of currencies
            Currencies = new List<string> { "USD", "EUR", "GBP", "INR" };

            // Bind the Picker controls to the list of currencies
            fromCurrencyPicker.ItemsSource = Currencies;
            toCurrencyPicker.ItemsSource = Currencies;
        }

        // Event handler for the Convert button click
        private async void OnConvertClicked(object sender, EventArgs e)
        {
            string amountText = amountEntry.Text;

            // Validate the input amount
            if (string.IsNullOrEmpty(amountText) || !decimal.TryParse(amountText, out decimal amount))
            {
                resultLabel.Text = "Please enter a valid amount.";
                return;
            }

            // Check if both currencies are selected
            if (fromCurrencyPicker.SelectedItem == null || toCurrencyPicker.SelectedItem == null)
            {
                resultLabel.Text = "Please select both currencies.";
                return;
            }

            string fromCurrency = fromCurrencyPicker.SelectedItem.ToString();
            string toCurrency = toCurrencyPicker.SelectedItem.ToString();

            // Perform the conversion using the API
            string convertedAmount = await ConvertCurrency(amount, fromCurrency, toCurrency);

            // Show the result
            if (convertedAmount == null)
            {
                resultLabel.Text = "Error converting currencies. Please try again.";
            }
            else
            {
                // Display the result
                resultLabel.Text = $"{amount} {fromCurrency} = {convertedAmount} {toCurrency}";
            }
        }

        // Method to call the currency conversion API
        private async Task<string> ConvertCurrency(decimal amount, string fromCurrency, string toCurrency)
{
    try
    {
        // Define the API endpoint and build the query URL
        string apiUrl = $"https://sharktide-currency-api.hf.space/convert?amount={amount}&from_currency={fromCurrency}&to_currency={toCurrency}";

        // Log the API URL to ensure it's correct (Display in UI)
        resultLabel.Text = $"FETCHING URL: {apiUrl}\n";

        // Create an HttpClient instance
        using (HttpClient client = new HttpClient())
        {
            // Make the API call and get the response
            HttpResponseMessage response = await client.GetAsync(apiUrl);

            // Log the status code of the response (Display in UI)
            resultLabel.Text += $"Response Status: {response.StatusCode}\n";

            // Ensure the response is successful
            if (!response.IsSuccessStatusCode)
            {
                resultLabel.Text += "API request failed.\n";
                return null;
            }

            // Read the response content as a string
            string jsonResponse = await response.Content.ReadAsStringAsync();

            // Log the raw JSON response (Display in UI)
            resultLabel.Text += $"Raw JSON Response: {jsonResponse}\n";

            // Attempt to deserialize the JSON response
            try
            {
                var responseObject = JsonSerializer.Deserialize<JsonResponse>(jsonResponse);

                // Check if the responseObject is null (error case)
                if (responseObject == null)
                {
                    resultLabel.Text += "Failed to deserialize the response. The structure might be wrong.\n";
                    return null;
                }

                // Log the parsed converted amount (Display in UI)
                resultLabel.Text += $"Parsed Converted Amount: {responseObject.ConvertedAmount}\n";

                // Return the converted amount as a string
                return responseObject.ConvertedAmount;
            }
            catch (JsonException ex)
            {
                // Handle any JSON deserialization issues
                resultLabel.Text += $"JSON Deserialization Error: {ex.Message}\n";
                return null;
            }
        }
    }
    catch (Exception ex)
    {
        // Handle any network or other errors and display on the UI
        resultLabel.Text += $"Error: {ex.Message}\n";
        return null;
    }
}




        // Class to map the response JSON
        public class JsonResponse
        {
            [JsonPropertyName("converted_amount")]
            public string ConvertedAmount { get; set; }
        }

    }
}
