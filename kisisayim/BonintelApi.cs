using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;

public class BonintelApi
{
    private static readonly HttpClient client = new HttpClient();
    private string _token;
    private DateTime _tokenExpiry; 

    public async Task<string> GetApiResponseAsync(string videotimegte, string videotimelte, string branchUuid = null, string page = null, string tip = "daily")
    {
        try
        {
            await EnsureTokenAsync();

            // Token'ı başlığa ekliyoruz
            client.DefaultRequestHeaders.Clear(); // Önce mevcut başlıkları temizleyin
            client.DefaultRequestHeaders.Add("token", _token); // Token'ı doğrudan ekleyin

            // URL parametrelerini oluşturuyoruz
            var queryParams = new Dictionary<string, string>
            {
                ["account_uuid"] = ConfigurationManager.ConnectionStrings["account_uuid"].ConnectionString,
                ["video_time__gte"] = videotimegte,
                ["video_time__lte"] = videotimelte
            };

            if (!string.IsNullOrEmpty(branchUuid))
            {
                queryParams["branch_uuid"] = branchUuid;
            }

            if (!string.IsNullOrEmpty(page))
            {
                queryParams["page"] = page;
            }

            // Query string oluşturma
            var queryString = string.Join("&", queryParams.Select(p =>
                $"{Uri.EscapeDataString(p.Key)}={Uri.EscapeDataString(p.Value)}"));

            // URL'yi oluşturuyoruz
            var uriBuilder = new UriBuilder("https://platform-gateway.boncam.io/reporting_v2/api/visitors/total/" + tip)
            {
                Query = queryString
            };

            // İstek gönderiyoruz
            HttpResponseMessage response = await client.GetAsync(uriBuilder.ToString());
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }
        catch (Exception e)
        {

            MessageBox.Show($"Request error: {e.Message}");
            return null;
        }
    }

    private async Task EnsureTokenAsync()
    {
        if (_token == null || DateTime.UtcNow >= _tokenExpiry)
        {
            _token = await GetNewTokenAsync();
            _tokenExpiry = DateTime.UtcNow.AddMinutes(30); // Token'ın geçerlilik süresi yarım saat olarak ayarlandı
        }
    }

    private async Task<string> GetNewTokenAsync()
    {
        var requestData = new
        {
            email = ConfigurationManager.ConnectionStrings["email"].ConnectionString,
            password = ConfigurationManager.ConnectionStrings["password"].ConnectionString,
        };

        try
        {
            var content = new StringContent(
                Newtonsoft.Json.JsonConvert.SerializeObject(requestData),
                System.Text.Encoding.UTF8,
                "application/json");

            HttpResponseMessage response = await client.PostAsync(ConfigurationManager.ConnectionStrings["bonapi"].ConnectionString, content);

            // Hata yönetimi
            if (!response.IsSuccessStatusCode)
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Request failed with status code {response.StatusCode}: {errorContent}");
            }

            string responseBody = await response.Content.ReadAsStringAsync();
            var json = Newtonsoft.Json.Linq.JObject.Parse(responseBody);

            // Token'ı mesaj kutusunda göster


            return json["access"]?.ToString() ?? throw new Exception("Token not found in response.");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"An error occurred: {ex.Message}");
            return null;
        }
    }

}
