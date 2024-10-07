using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;

namespace kisisayim
{
    public partial class Form1 : Form
    {
        public BonintelApi api = new BonintelApi();
        private Panel loadingPanel;
        private Label loadingLabel;
        private readonly Dictionary<string, string> uuidDictionary = new Dictionary<string, string>();
        private readonly Dictionary<string, string> codeDictionary = new Dictionary<string, string>();
        readonly Dictionary<int, string> redRowResponses = new Dictionary<int, string>();
        readonly List<int> redRowIndices = new List<int>();

        public string V3con = ConfigurationManager.ConnectionStrings["V3con"].ConnectionString;
        public string Monitoringcon = ConfigurationManager.ConnectionStrings["Monitoringcon"].ConnectionString;
        readonly string uuid = ConfigurationManager.ConnectionStrings["uuid"].ConnectionString;
        readonly string ForDefaultQuery= ConfigurationManager.ConnectionStrings["ForDefaultQuery"].ConnectionString;
        readonly string ForStoreQuery = ConfigurationManager.ConnectionStrings["ForStoreQuery"].ConnectionString;
        readonly string deleteQuery = ConfigurationManager.ConnectionStrings["deleteQuery"].ConnectionString;
        readonly string insertQuery = ConfigurationManager.ConnectionStrings["insertQuery"].ConnectionString;
        readonly int year = 2024;
        DataTable mergedDataTable = new DataTable();




        public Form1()
        {
            InitializeComponent();
            InitializeLoadingPanel();
            LoadComboBoxItems();


        }
        #region log
        public void AddLog(string logMessage)
        {
            listBox1.Items.Add(DateTime.Now.ToString("HH:mm:ss") + " - " + logMessage);

            // En son eklenen log'a otomatik olarak kaydırma
            listBox1.TopIndex = listBox1.Items.Count - 1;
        }
        #endregion

        #region Comboboxu doldur
        private void LoadComboBoxItems()
        {
            using (SqlConnection connection = new SqlConnection(Monitoringcon))
            {
                SqlCommand command = new SqlCommand(uuid, connection);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();


                    comboboxStores.Items.Add("Tümünü al");
                    uuidDictionary.Add("Tümünü al", "");
                    codeDictionary.Add("Tümünü al", "");



                    while (reader.Read())
                    {
                        string storeName = reader["StoreName"].ToString();
                        string branchUuid = reader["BranchUuid"].ToString();
                        string storeCode = reader["StoreCode"].ToString();

                        comboboxStores.Items.Add(storeName);
                        uuidDictionary.Add(storeName, branchUuid);
                        codeDictionary.Add(storeName, storeCode);
                    }
                    reader.Close();

                    // Varsayılan olarak "Tümünü al" seçili
                    comboboxStores.SelectedIndex = 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
        #endregion

        #region Loading işlevleri
        private void InitializeLoadingPanel()
        {
            loadingPanel = new Panel
            {
                BackColor = Color.Black,

                Dock = DockStyle.Fill,
                Visible = false // Başlangıçta görünmez
            };

            loadingLabel = new Label
            {
                Text = "Rapor Yükleniyor...",
                ForeColor = Color.White,
                Font = new Font("Arial", 36, FontStyle.Bold),
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.None
            };

            loadingPanel.Controls.Add(loadingLabel);
            this.Controls.Add(loadingPanel);
        }

        private void ShowLoadingScreen()
        {
            loadingPanel.BringToFront();
            loadingPanel.Visible = true;
        }

        private void HideLoadingScreen()
        {
            loadingPanel.Visible = false;
        }

        #endregion

        #region jsonu data tableye aktar
        private DataTable JsonToDataTable(string json) 
        { 
            var dataTable = new DataTable();

            try
            {
                // JSON verisini JObject olarak parse et
                var jObject = JObject.Parse(json);

                // JSON nesnesindeki 'list' anahtarını al ve bir JArray olarak parse et

                if (jObject["list"] is JArray jArray && jArray.Count > 0)
                {
                    // DataTable'ın kolonlarını oluşturuyoruz
                    dataTable.Columns.Add("Mağaza Uuid");
                    dataTable.Columns.Add("Mağaza Kodu");
                    dataTable.Columns.Add("Mağaza Adı");
                    dataTable.Columns.Add("Ziyaretçi sayısı");
                    dataTable.Columns.Add("Gün");


                    // JSON verilerini satır olarak ekliyoruz
                    foreach (var item in jArray)
                    {
                        var row = dataTable.NewRow();
                        string branchUuid = item["branchUuid"]?.ToString();

                        row["Mağaza Uuid"] = item["branchUuid"]?.ToString();
                        row["Mağaza Adı"] = item["branchName"]?.ToString();
                        row["Ziyaretçi sayısı"] = item["count"]?.ToString();
                        row["Gün"] = item["date"]?.ToString();
                        

                        // branchUuid'ye göre storeCode'u getir ve ekle
                        if (uuidDictionary.ContainsValue(branchUuid))
                        {
                            string storeName = uuidDictionary.FirstOrDefault(x => x.Value == branchUuid).Key;
                            row["Mağaza Kodu"] = codeDictionary.ContainsKey(storeName) ? codeDictionary[storeName] : string.Empty;
                        }
                        else
                        {
                            row["Mağaza Kodu"] = string.Empty; // Eğer uuidDictionary'de yoksa boş bırak
                        }
                        dataTable.Rows.Add(row);
                    }
                }
            }
            catch (JsonException ex)
            {
                MessageBox.Show($"Error parsing JSON: {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected error: {ex.Message}");
            }

            return dataTable;
        } 

        #endregion


        #region jsonu data tableye aktar (perHour)
        private DataTable JsonToDataTablePerHour(string json)
        {
            var dataTable = new DataTable();

            try
            {
                // JSON verisini JObject olarak parse et
                var jObject = JObject.Parse(json);

                // JSON nesnesindeki 'list' anahtarını al ve bir JArray olarak parse et

                if (jObject["list"] is JArray jArray && jArray.Count > 0)
                {
                    // DataTable'ın kolonlarını oluşturuyoruz
                    dataTable.Columns.Add("Mağaza Uuid");
                    dataTable.Columns.Add("Mağaza Kodu"); 
                    dataTable.Columns.Add("Mağaza Adı");
                    dataTable.Columns.Add("Ziyaretçi sayısı");
                    dataTable.Columns.Add("Gün");
                    dataTable.Columns.Add("Saat");


                    // JSON verilerini satır olarak ekliyoruz
                    foreach (var item in jArray)
                    {
                        DateTime dateTime = DateTime.Parse(item["date"]?.ToString());
                        var row = dataTable.NewRow();
                        string branchUuid = item["branchUuid"]?.ToString();

                        row["Mağaza Uuid"] = branchUuid;
                        row["Mağaza Adı"] = item["branchName"]?.ToString();
                        row["Ziyaretçi sayısı"] = item["count"]?.ToString();
                        row["Gün"] = dateTime.ToString("dd.MM.yyyy"); 
                        row["Saat"] = dateTime.ToString("HH");

                        // branchUuid'ye göre storeCode'u getir ve ekle
                        if (uuidDictionary.ContainsValue(branchUuid))
                        {
                            string storeName = uuidDictionary.FirstOrDefault(x => x.Value == branchUuid).Key;
                            row["Mağaza Kodu"] = codeDictionary.ContainsKey(storeName) ? codeDictionary[storeName] : string.Empty;
                        }
                        else
                        {
                            row["Mağaza Kodu"] = string.Empty; // Eğer uuidDictionary'de yoksa boş bırak
                        }

                        dataTable.Rows.Add(row);
                    }
                }
            }
            catch (JsonException ex)
            {
                MessageBox.Show($"Error parsing JSON: {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected error: {ex.Message}");
            }

            return dataTable;
        }

        #endregion

        #region apiden toplam sayfayı al
        private int GetTotalPagesFromResponse(string response)
        {
            var jsonResponse = JsonConvert.DeserializeObject<dynamic>(response);
            return jsonResponse.total_page;
        }
        #endregion

        #region satır toplam al
        private double CalculateColumnSum(DataTable dataTable, int columnIndex)
        {
            double sum = 0;

            foreach (DataRow row in dataTable.Rows)
            {
                if (double.TryParse(row[columnIndex].ToString(), out double value))
                {
                    sum += value;
                }
            }

            return sum;
        }
        #endregion

        #region api sorgusu
        private async void Api_Sorgu_Click(object sender, EventArgs e)
        {
            Sql dbHelper = new Sql(V3con);
            DataTable dataTable = new DataTable();

            try
            {
                ShowLoadingScreen();

                string videotimegte = startPicker.Value.ToString("yyyy-MM-dd");
                string videotimelte = finishPicker.Value.ToString("yyyy-MM-dd");

                string branch = uuidDictionary[comboboxStores.SelectedItem.ToString()];
                string storecode = codeDictionary[comboboxStores.SelectedItem.ToString()];
                string ishourly = checkBox1.Checked ? "hourly" : "daily";

                Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "@start", videotimegte },
            { "@finish", videotimelte }
        };

                Dictionary<string, object> parameters2 = new Dictionary<string, object>
        {
            { "@start", videotimegte },
            { "@finish", videotimelte },
            { "@storecode", storecode }
        };

                DataTable result = dbHelper.ExecuteQuery(ForDefaultQuery, parameters);
                DataTable result2 = dbHelper.ExecuteQuery(ForStoreQuery, parameters2);


                double dbTotal = 0;

                // Veritabanı toplamını hesapla
                if (result.Rows.Count > 0 && result.Rows[0]["TotalInVisitorCount"] != DBNull.Value)
                {
                    dbTotal = Convert.ToDouble(result.Rows[0]["TotalInVisitorCount"]);
                }

                if (result2.Rows.Count > 0 && result2.Rows[0]["TotalInVisitorCount"] != DBNull.Value)
                {
                    dbTotal = Convert.ToDouble(result2.Rows[0]["TotalInVisitorCount"]);
                }

                // API'den gelen verileri al
                string response = branch == "Tümünü al"
                    ? await api.GetApiResponseAsync(videotimegte: videotimegte, videotimelte: videotimelte, tip: ishourly)
                    : await api.GetApiResponseAsync(branchUuid: branch, videotimegte: videotimegte, videotimelte: videotimelte, tip: ishourly);

                // Verileri Json'dan DataTable'a dönüştür
                dataTable = checkBox1.Checked ? JsonToDataTablePerHour(response) : JsonToDataTable(response);

                int totalPages = GetTotalPagesFromResponse(response);

                for (int page = 2; page <= totalPages; page++)
                {
                    string pagedResponse = await api.GetApiResponseAsync(videotimegte: videotimegte, videotimelte: videotimelte, tip: ishourly, page: page.ToString());
                    DataTable pagedTable = JsonToDataTable(pagedResponse);

                    // Sayfalanmış verileri ekle
                    foreach (DataRow row in pagedTable.Rows)
                    {
                        dataTable.ImportRow(row);
                    }
                }

                // DataTable'ı DataGridView'e bağla
                dataGridView2.DataSource = dataTable;

                // API toplamını hesapla
                double apiTotal = CalculateColumnSum(dataTable, 3);

                // Sonuçları etiketlere yazdır
                label2.Text = $"Api Toplam: {apiTotal}";
                label3.Text = $"Veritabanı Toplam: {dbTotal}";

                double fark = apiTotal - dbTotal;
                if (fark > 0)
                {
                    label4.Text = $"API toplamı,\n veritabanı toplamından\n {fark} daha fazla.";
                }
                else if (fark < 0)
                {
                    label4.Text = $"Veritabanı toplamı,\n API toplamından \n{-fark} daha fazla.";
                }
                else
                {
                    label4.Text = "API toplamı \n ve veritabanı toplamı \n eşit.";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                HideLoadingScreen();
            }
        }
        #endregion


        #region Fix
        private async void Run_Click(object sender, EventArgs e)
        {
            try
            {
                await GetRedAsync();

                var result = MessageBox.Show("Bu işlemi gerçekleştirmek istediğinizden emin misiniz?",
                 "Onay",
                 MessageBoxButtons.YesNo,
                 MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {

                    await GetHourlyAsync();
                }
                else
                {
                    MessageBox.Show("İşlem iptal edildi.", "İptal", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }


                var result2 = MessageBox.Show("Bu işlemi gerçekleştirmek istediğinizden emin misiniz?",
                                 "Onay",
                                 MessageBoxButtons.YesNo,
                                 MessageBoxIcon.Question);

                if (result2 == DialogResult.Yes)
                {
                    

                    await FixAsync();
                }
                else
                {
                    MessageBox.Show("İşlem iptal edildi.", "İptal", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task GetRedAsync()
        {
            var dbHelper = new Sql(V3con);
            int selectedMonth = comboBox2.SelectedIndex + 1;
            if (selectedMonth == 0)
            {
                MessageBox.Show("Lütfen bir ay seçin.");
                return;
            }

            DateTime startDate = new DateTime(year, selectedMonth, 1);
            DateTime endDate = startDate.AddMonths(1).AddDays(-1);

            try
            {
                var api = new BonintelApi();
                int RIndex = 1;

                foreach (var store in uuidDictionary)
                {
                    if (!string.IsNullOrEmpty(store.Value))
                    {
                        string branch = store.Value;
                        string storecode = codeDictionary[store.Key];

                        var parameters = new Dictionary<string, object>
                {
                    { "@start", startDate.ToString("yyyy-MM-dd") },
                    { "@finish", endDate.ToString("yyyy-MM-dd") },
                    { "@storecode", string.IsNullOrEmpty(storecode) ? (object)DBNull.Value : storecode }
                };

                        DataTable result = dbHelper.ExecuteQuery(ForStoreQuery, parameters);

                        double dbTotal = 0;

                        if (result.Rows.Count > 0 && result.Rows[0]["TotalInVisitorCount"] != DBNull.Value)
                        {
                            dbTotal = Convert.ToDouble(result.Rows[0]["TotalInVisitorCount"]);
                        }

                        string response = await api.GetApiResponseAsync(branchUuid: branch, videotimegte: startDate.ToString("yyyy-MM-dd"), videotimelte: endDate.ToString("yyyy-MM-dd"));
                        DataTable dataTable = JsonToDataTable(response);
                        int totalPages = GetTotalPagesFromResponse(response);

                        for (int page = 2; page <= totalPages; page++)
                        {
                            string pagedResponse = await api.GetApiResponseAsync(videotimegte: startDate.ToString("yyyy-MM-dd"), videotimelte: endDate.ToString("yyyy-MM-dd"), page: page.ToString());
                            DataTable pagedTable = JsonToDataTable(pagedResponse);

                            foreach (DataRow row in pagedTable.Rows)
                            {
                                dataTable.ImportRow(row);
                            }
                        }
                        AddLog($"{store.Key} hatasız.");


                        double apiTotal = CalculateColumnSum(dataTable, 3);


                            if (apiTotal != dbTotal /*&& dbTotal != 0*/)
                            {
                                int rowIndex = dataGridView3.Rows.Add(RIndex, branch, selectedMonth, storecode, store.Key, apiTotal,dbTotal, dbTotal - apiTotal);
                                redRowIndices.Add(rowIndex);
                                AddLog($"{store.Key} eklendi.");
                            }
                            RIndex++;


                    }
                }
            }
            catch (Exception ex)
            {
                AddLog(ex.Message);

            }
        }

        private async Task GetHourlyAsync()
        {
            if (redRowIndices.Count == 0)
            {
                AddLog("Eksik satır bulunamadı");
                return;
            }

            var api = new BonintelApi();

            try
            {
                foreach (int rowIndex in redRowIndices)
                {
                    var row = dataGridView3.Rows[rowIndex];
                    string branch = row.Cells["branch"].Value.ToString();
                    string monthString = row.Cells["month"].Value.ToString();

                    if (!int.TryParse(monthString, out int month) || month < 1 || month > 12)
                    {
                        MessageBox.Show("Ay bilgisi geçerli değil.");
                        continue;
                    }

                    DateTime startDate = new DateTime(year, month, 1);
                    DateTime endDate = startDate.AddMonths(1).AddDays(-1);

                    while (startDate <= endDate)
                    {
                        DateTime weekEndDate = startDate.AddDays(6);
                        if (weekEndDate > endDate)
                        {
                            weekEndDate = endDate;
                        }

                        string response = await api.GetApiResponseAsync(
                            branchUuid: branch,
                            videotimegte: startDate.ToString("yyyy-MM-dd"),
                            videotimelte: weekEndDate.ToString("yyyy-MM-dd"),
                            tip: "hourly"
                        );

                        AddLog($"{startDate.ToString("yyyy-MM-dd")} - {weekEndDate.ToString("yyyy-MM-dd")}  için veri eklendi.");
                        DataTable weeklyDataTable = JsonToDataTablePerHour(response);

                        if (mergedDataTable.Columns.Count == 0)
                        {
                            mergedDataTable = weeklyDataTable.Clone();
                        }

                        // Haftalık verileri mergedDataTable'a ekleyin
                        mergedDataTable.Merge(weeklyDataTable);

                        startDate = weekEndDate.AddDays(1);
                    }
                }

                dataGridView4.DataSource = mergedDataTable;
                AddLog("Veriler başarıyla yüklendi.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"İşlem sırasında bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task FixAsync()
        {
            if (mergedDataTable == null || mergedDataTable.Rows.Count == 0)
            {
                MessageBox.Show("Veriler mevcut değil.");
                return;
            }

            using (var connection = new SqlConnection(V3con))
            {
                await connection.OpenAsync();
                int selectedMonth = comboBox2.SelectedIndex + 1;

                DateTime startDate = new DateTime(year, selectedMonth, 1);
                DateTime endDate = startDate.AddMonths(1).AddDays(-1);

                try
                {
                    // Benzersiz mağaza kodlarını al
                    var distinctStoreCodes = mergedDataTable.AsEnumerable()
                        .Select(row => row["Mağaza Kodu"].ToString())
                        .Distinct();

                    foreach (var storeCode in distinctStoreCodes)
                    {
                        AddLog($"{storeCode} - {startDate} - {endDate} kaydı siliniyor");

                        using (var command = new SqlCommand(deleteQuery, connection))
                        {
                            command.Parameters.AddWithValue("@StoreCode", storeCode);
                            command.Parameters.AddWithValue("@FirstDay", startDate);
                            command.Parameters.AddWithValue("@LastDay", endDate);

                            await command.ExecuteNonQueryAsync();
                        }
                    }

                    foreach (DataRow row in mergedDataTable.Rows)
                    {
                        AddLog($"Kod: {row["Mağaza Kodu"].ToString().Trim()}, Saat: {row["Saat"]}, Gün: {Convert.ToDateTime(row["Gün"])}");

                        using (var command = new SqlCommand(insertQuery, connection))
                        {
                            command.Parameters.AddWithValue("@InVisitorCount", row["Ziyaretçi sayısı"].ToString());
                            command.Parameters.AddWithValue("@CurrentDate", Convert.ToDateTime(row["Gün"]));
                            command.Parameters.AddWithValue("@CurrentHour", row["Saat"].ToString());
                            command.Parameters.AddWithValue("@OfficeCode", row["Mağaza Kodu"].ToString().Trim()); 
                            command.Parameters.AddWithValue("@StoreCode", row["Mağaza Kodu"].ToString().Trim());

                            await command.ExecuteNonQueryAsync();
                        }
                    }

                    AddLog("Kayıtlar yenilendi.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Veritabanı işlemi sırasında bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }



        #endregion


    }
}

