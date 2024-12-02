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
using System.Data.OleDb;
using System.IO;
using System.Net.Mail;
using System.Text;
using Outlookapp = Microsoft.Office.Interop.Outlook;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
namespace kisisayim
{
    public partial class Form1 : Form
    {
        public BonintelApi api = new BonintelApi();
        private Panel loadingPanel;
        private Label loadingLabel;
        private int logIndex = 1;  

        readonly List<int> redRowIndices = new List<int>();
        private List<Store> stores = new List<Store>();
        public string V3con = ConfigurationManager.ConnectionStrings["V3con"].ConnectionString;
        public string ksadata = ConfigurationManager.ConnectionStrings["ksadata"].ConnectionString;
        readonly string uuid = ConfigurationManager.ConnectionStrings["uuid"].ConnectionString;
        readonly string ForDefaultQuery= ConfigurationManager.ConnectionStrings["ForDefaultQuery"].ConnectionString;
        readonly string ForStoreQuery = ConfigurationManager.ConnectionStrings["ForStoreQuery"].ConnectionString;
        readonly string deleteQuery = ConfigurationManager.ConnectionStrings["deleteQuery"].ConnectionString;
        readonly string insertQuery = ConfigurationManager.ConnectionStrings["insertQuery"].ConnectionString;
        DataTable mergedDataTable = new DataTable();




        public Form1()
        {
            InitializeComponent();
            InitializeLoadingPanel();
            LoadComboBoxItems();
            this.StartPosition = FormStartPosition.CenterScreen;

            // Form'un boyutunun değişmesini engelle
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.Width = 1350;
            // Kullanıcı boyutlandırmayı engellemek için
            this.MaximizeBox = false;  // Maksimize butonunu kaldır
            this.MinimizeBox = true;   // Minimize butonunu koru (isteğe bağlı)

            yearCb.SelectedItem = DateTime.Now.Year.ToString();
            monthCb.SelectedIndex = DateTime.Now.Month - 1;
        }

        #region Comboboxu doldur
        private void LoadComboBoxItems()
        {
            using (SqlConnection connection = new SqlConnection(V3con))
            {
                SqlCommand command = new SqlCommand(uuid, connection);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    comboboxStores.Items.Add("Tümünü Al");

                    while (reader.Read())
                    {
                        var store = new Store
                        {
                            CompanyCode = (int)reader["CompanyCode"],
                            OfficeCode = (int)reader["OfficeCode"],
                            StoreCode = (int)reader["StoreCode"],
                            StoreName = reader["StoreName"].ToString().Trim(),
                            BranchUuid = reader["BranchUuid"].ToString().Trim(),
                        };
                  
                        stores.Add(store);

                        comboboxStores.Items.Add(store.StoreName);

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


        #region apiden toplam sayfayı ve satır toplamını al
        private int GetTotalPagesFromResponse(string response)
        {
            var jsonResponse = JsonConvert.DeserializeObject<dynamic>(response);
            return jsonResponse.total_page;
        }

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
                        var selectedStore = stores.FirstOrDefault(s => s.StoreName.Trim() == item["branchName"]?.ToString().Trim());

                        row["Mağaza Uuid"] = item["branchUuid"]?.ToString();
                        row["Mağaza Adı"] = item["branchName"]?.ToString();
                        row["Ziyaretçi sayısı"] = item["count"]?.ToString();
                        row["Gün"] = item["date"]?.ToString();
                        row["Mağaza Kodu"] = selectedStore.StoreCode.ToString();



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
                    dataTable.Columns.Add("Şirket Kodu");
                    dataTable.Columns.Add("Ofis Kodu");
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
                        var selectedStore = stores.FirstOrDefault(s => s.StoreName.Trim() == item["branchName"].ToString().Trim());

                        row["Mağaza Uuid"] = branchUuid;
                        row["Mağaza Adı"] = item["branchName"]?.ToString();
                        row["Ziyaretçi sayısı"] = item["count"]?.ToString();
                        row["Gün"] = dateTime.ToString("dd.MM.yyyy"); 
                        row["Saat"] = dateTime.ToString("HH");
                        row["Şirket Kodu"] = selectedStore.CompanyCode;
                        row["Ofis KOdu"] = selectedStore.OfficeCode;
                        row["Mağaza Kodu"] = selectedStore.StoreCode;



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

        #region api sorgusu Yap
        private async void Api_Sorgu_Click(object sender, EventArgs e)
        {
            Sql dbHelper = new Sql(V3con);
            DataTable dataTable = new DataTable();

            try
            {
                ShowLoadingScreen();

                var selectedStore = stores.FirstOrDefault(s => s.StoreName == comboboxStores.SelectedItem.ToString());
                string videotimegte = startPicker.Value.ToString("yyyy-MM-dd");
                string videotimelte = finishPicker.Value.ToString("yyyy-MM-dd");
                string ishourly = checkBox1.Checked ? "hourly" : "daily";
                string branch = "";
                string storecode = "";


                if (comboboxStores.SelectedIndex == 0)
                {
                     branch = "";
                     storecode = "";
                }
                else {
                     branch = selectedStore.BranchUuid ?? string.Empty;
                     storecode = selectedStore.StoreCode.ToString() ?? string.Empty;

                }
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
                string response = comboboxStores.SelectedIndex == 0
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
                    var logMessage = $"API toplamı, veritabanı toplamından {fark} daha fazla.";
                    listBox2.Items.Add(DateTime.Now.ToString("HH:mm:ss") + " - " + logMessage);
                    listBox2.TopIndex = listBox1.Items.Count - 1;

                }
                else if (fark < 0)
                {
                    var logMessage = $"Veritabanı toplamı, API toplamından {-fark} daha fazla.";
                    listBox2.Items.Add(DateTime.Now.ToString("HH:mm:ss") + " - " + logMessage);
                    listBox2.TopIndex = listBox1.Items.Count - 1;
                }
                else
                {
                    var logMessage = "API toplamı ve veritabanı toplamı eşit.";
                    listBox2.Items.Add(DateTime.Now.ToString("HH:mm:ss") + " - " + logMessage);
                    listBox2.TopIndex = listBox1.Items.Count - 1;
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

        #region Eksikleri düzelt
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
                    return; // Burada işlemi sonlandırıyoruz.
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
                    return; // Burada işlemi sonlandırıyoruz.
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Burada işlemi sonlandırıyoruz.
            }
        }

        private async Task GetRedAsync()
        {
            var dbHelper = new Sql(V3con);
            int selectedYear = int.Parse(yearCb.SelectedItem.ToString());
            int selectedMonth = monthCb.SelectedIndex + 1;

            DateTime startDate = new DateTime(selectedYear, selectedMonth, 1);
            DateTime endDate = startDate.AddMonths(1).AddDays(-1);

            try
            {
                var api = new BonintelApi();
                int RIndex = 1;

                foreach (var store in stores)
                {

                        string branch = store.BranchUuid;
                    string storecode = store.StoreCode.ToString();

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
                        AddLog($"{store.StoreName} hatasız.");


                        double apiTotal = CalculateColumnSum(dataTable, 3);


                            if (apiTotal != dbTotal )
                            {
                                int rowIndex = dataGridView3.Rows.Add(RIndex, branch, selectedMonth, storecode, store.StoreName, apiTotal,dbTotal, dbTotal - apiTotal);
                                redRowIndices.Add(rowIndex);
                                AddLog($"{store.StoreName} eklendi.");
                            }
                            RIndex++;


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

                    DateTime startDate = new DateTime(int.Parse(yearCb.SelectedItem.ToString()), month, 1);
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
                int selectedMonth = monthCb.SelectedIndex + 1;

                DateTime startDate = new DateTime(int.Parse(yearCb.SelectedItem.ToString()), selectedMonth, 1);
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
                        var selectedStore = stores.FirstOrDefault(s => s.StoreCode.ToString() == row["Mağaza Kodu"].ToString());
                        AddLog($"Kod: {row["Mağaza Kodu"].ToString().Trim()}, Saat: {row["Saat"]}, Gün: {Convert.ToDateTime(row["Gün"])}");

                        using (var command = new SqlCommand(insertQuery, connection))
                        {
                            command.Parameters.AddWithValue("@CompanyCode", selectedStore.CompanyCode);
                            command.Parameters.AddWithValue("@InVisitorCount", row["Ziyaretçi sayısı"].ToString());
                            command.Parameters.AddWithValue("@CurrentDate", Convert.ToDateTime(row["Gün"]));
                            command.Parameters.AddWithValue("@CurrentHour", row["Saat"].ToString());
                            command.Parameters.AddWithValue("@OfficeCode", selectedStore.OfficeCode);
                            command.Parameters.AddWithValue("@StoreCode", selectedStore.StoreCode);

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

        #region Loglama ve Loading işlevleri


        public void AddLog(string logMessage)
        {
            // 3 haneli numara formatı ile log index oluşturuluyor
            string formattedIndex = logIndex.ToString("D3");  // "D3" 3 haneli format için

            // Log mesajı oluşturuluyor
            string logEntry = $"{formattedIndex} - {DateTime.Now.ToString("HH:mm:ss")} - {logMessage}";

            // Log mesajı ListBox'a ekleniyor
            listBox1.Items.Add(logEntry);

            // En son eklenen log'a otomatik kaydırma
            listBox1.TopIndex = listBox1.Items.Count - 1;

            // Sayaç artırılıyor
            logIndex++;
        }


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

        #region
        private void ksaexec_Click(object sender, EventArgs e)
        {
            yurtdisi_mail_okuma();
        }

        private string messageBody;

        private void yurtdisi_mail_okuma()
        {
            Outlookapp.Application oApp = null;
            Outlookapp.NameSpace oNS = null;

            try
            {
                oApp = new Outlookapp.Application();
                oNS = oApp.GetNamespace("MAPI");

                // Klasörleri al
                Outlookapp.MAPIFolder parentFolder = oNS.GetDefaultFolder(Outlookapp.OlDefaultFolders.olFolderInbox).Parent;
                Outlookapp.MAPIFolder gelenmail = parentFolder.Folders["KSAgelen"];
                Outlookapp.MAPIFolder islenenmail = parentFolder.Folders["KSAislenen"];

                // Gelen mailleri işle
                List<Outlookapp.MailItem> mails = GetMailItems(gelenmail);

                foreach (var mail in mails)
                {
                    try
                    {
                        if (mail.Attachments.Count > 0)
                        {
                            string filePath = SaveAttachment(mail.Attachments[1]);
                            import(filePath);
                            datalariduzenle();
                            dbyeaktar();
                        }
                        mail.Move(islenenmail); // İşlenen mailleri taşı
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Hata oluştu: {mail.Subject} - {ex.Message}");
                    }
                }

                mailgonder(); 
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message);
            }
            finally
            {
                if (oNS != null) Marshal.ReleaseComObject(oNS);
                if (oApp != null) Marshal.ReleaseComObject(oApp);
            }
        }

        // Mailleri getir
        private List<Outlookapp.MailItem> GetMailItems(Outlookapp.MAPIFolder folder)
        {
            var mailItems = new List<Outlookapp.MailItem>();
            foreach (var item in folder.Items)
            {
                if (item is Outlookapp.MailItem mailItem)
                {
                    mailItems.Add(mailItem);
                }
            }
            return mailItems;
        }

        // Ekleri kaydet
        private string SaveAttachment(Outlookapp.Attachment attachment)
        {
            string filePath = Path.Combine(@"C:\Log\", attachment.FileName);
            attachment.SaveAsFile(filePath);
            return filePath;
        }

 
        private void mailgonder()
        {
            gethtml();

            SmtpClient client = new SmtpClient("smtp.office365.com", 587);
            client.EnableSsl = true;
            client.Credentials = new System.Net.NetworkCredential("destek@panco.com.tr", "Pnc321!!");
            MailAddress from = new MailAddress("destek@panco.com.tr", String.Empty, System.Text.Encoding.UTF8);
            MailAddress to = new MailAddress("bilgiislem@panco.com.tr");
            MailMessage message = new MailMessage(from, to);
            message.IsBodyHtml = true;

            string textBody1 = messageBody;


            message.Body = textBody1;
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.Subject = "KSA dataları işleme " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToString("HH:mm:ss");
            message.SubjectEncoding = System.Text.Encoding.UTF8;
            client.Send(message);

        }
        private void datalariduzenle()
        {
            for (int i = 0; i < ilkdata.RowCount - 1; i++)
            {

                if (ilkdata.Rows[i].Cells[2].Value.ToString().Length < 3)
                {
                    tabloaktar.Rows.Add(Convert.ToDateTime(ilkdata.Rows[i].Cells[0].Value), "120 90 028", "00" + ilkdata.Rows[i].Cells[2].Value, "", "", "", ilkdata.Rows[i].Cells[6].Value, ilkdata.Rows[i].Cells[7].Value, ilkdata.Rows[i].Cells[8].Value, ilkdata.Rows[i].Cells[9].Value, ilkdata.Rows[i].Cells[10].Value, ilkdata.Rows[i].Cells[11].Value, ilkdata.Rows[i].Cells[12].Value, ilkdata.Rows[i].Cells[13].Value);
                }
                

            }
        }

        private void dbyeaktar()
        {
            try
            {
                // Her satır için verileri al ve parametreleri hazırla
                List<Dictionary<string, object>> parametersList = new List<Dictionary<string, object>>();

                for (int i = 0; i < tabloaktar.RowCount - 1; i++)
                {
                    var parameters = new Dictionary<string, object>();

                    for (int j = 0; j < 14; j++) 
                    {
                        string parameterKey = $"@parametre{j + 1}";
                        string parameterValue = tabloaktar.Rows[i].Cells[j].Value?.ToString() ?? string.Empty;

                        parameters.Add(parameterKey, parameterValue);

                    }

                    parametersList.Add(parameters);
                }

                Sql sqlHelper = new Sql(V3con);

                MessageBox.Show("Veriler aktarılsın mı ?");

                foreach (var parameters in parametersList)
                {
                    sqlHelper.Insert(ksadata, parameters);
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata oluştu: " + Environment.NewLine + ex.Message);
            }
        }

        private void import(string filename)
        {

            this.ilkdata.DataSource = null;
            this.ilkdata.Rows.Clear();
            ilkdata.Refresh();

            this.tabloaktar.DataSource = null;
            this.tabloaktar.Rows.Clear();
            tabloaktar.Refresh();

            string constr = "";

            if (Path.GetExtension(filename).Equals(".xls", StringComparison.OrdinalIgnoreCase))
            {
                // .xls dosyaları için
                constr = $"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={filename};Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\";";
            }
            else if (Path.GetExtension(filename).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                // .xlsx dosyaları için
                constr = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={filename};Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\";";
            }

            OleDbConnection con = new OleDbConnection(constr);
            var onlyFileName = System.IO.Path.GetFileName(filename);
            OleDbCommand oconn = new OleDbCommand("Select *    From [" + "Sheet1" + "$]  ", con);
            con.Open();

            OleDbDataAdapter sda = new OleDbDataAdapter(oconn);
            System.Data.DataTable data = new System.Data.DataTable();
            sda.Fill(data);
            ilkdata.DataSource = data;

        }
        private void gethtml()
        {
            try
            {
                messageBody = "";

                messageBody = "<font>KSA dataları işlenmiştir. </font><br><br>";


                string htmlTableStart = "<table style=\"border-collapse:collapse; text-align:center;\" >";
                string htmlTableEnd = "</table>";
                string htmlHeaderRowStart = "<tr style =\"background-color:#6FA1D2; color:#ffffff;\">";
                string htmlHeaderRowEnd = "</tr>";
                string htmlTrStart = "<tr style =\"color:#555555;\">";
                string htmlTrEnd = "</tr>";
                string htmlTdStart = "<td style=\" border-color:#5c87b2; border-style:solid; border-width:thin; padding: 5px;\">";
                string htmlTdEnd = "</td>";

                messageBody += htmlTableStart;
                messageBody += htmlHeaderRowStart;
                messageBody += htmlTdStart + "Column1 " + htmlTdEnd;
                messageBody += htmlHeaderRowEnd;
                for (int i = 0; i < tabloaktar.RowCount - 1; i++)
                {

                    messageBody = messageBody + htmlTrStart;
                    messageBody = messageBody + htmlTdStart + tabloaktar.Rows[i].Cells[0].Value.ToString() + " " + tabloaktar.Rows[i].Cells[1].Value.ToString() + htmlTdEnd;
                    messageBody = messageBody + htmlTrEnd;
                }
                messageBody = messageBody + htmlTableEnd;



            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}

