using System;
using System.Drawing;
using System.Windows.Forms;

namespace kisisayim
{
    partial class Form1
    {
        /// <summary>
        ///Gerekli tasarımcı değişkeni.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///Kullanılan tüm kaynakları temizleyin.
        /// </summary>
        ///<param name="disposing">yönetilen kaynaklar dispose edilmeliyse doğru; aksi halde yanlış.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer üretilen kod

        /// <summary>
        /// Tasarımcı desteği için gerekli metot - bu metodun 
        ///içeriğini kod düzenleyici ile değiştirmeyin.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.farkSonuc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabloaktar = new System.Windows.Forms.DataGridView();
            this.Number_Of_Ticket = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Total_Sales_Currrent_Price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Total_Sales_First_Price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Discount_Ratio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.KSA_RP_CURRENT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.KSA_RP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Barcode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemDim1Code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColorCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProductCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SubCurrAccCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CustomerCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ilkdata = new System.Windows.Forms.DataGridView();
            this.ksaexec = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.monthCb = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.yearCb = new System.Windows.Forms.ComboBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.dataGridView3 = new System.Windows.Forms.DataGridView();
            this.fark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dbTotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.apiTotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.storeName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.storeCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.month = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.branch = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Index = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridView4 = new System.Windows.Forms.DataGridView();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.button3 = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.finishPicker = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.startPicker = new System.Windows.Forms.DateTimePicker();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.comboboxStores = new System.Windows.Forms.ComboBox();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabloaktar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ilkdata)).BeginInit();
            this.tabPage3.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView4)).BeginInit();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.groupBox6.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // farkSonuc
            // 
            this.farkSonuc.HeaderText = "Fark";
            this.farkSonuc.MinimumWidth = 6;
            this.farkSonuc.Name = "farkSonuc";
            this.farkSonuc.Width = 125;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.ksaexec);
            this.tabPage2.Controls.Add(this.ilkdata);
            this.tabPage2.Controls.Add(this.tabloaktar);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1697, 758);
            this.tabPage2.TabIndex = 3;
            this.tabPage2.Text = "Yurtdışı satış işle";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabloaktar
            // 
            this.tabloaktar.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tabloaktar.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Date,
            this.CustomerCode,
            this.SubCurrAccCode,
            this.ProductCode,
            this.ColorCode,
            this.ItemDim1Code,
            this.Barcode,
            this.Qty,
            this.KSA_RP,
            this.KSA_RP_CURRENT,
            this.Discount_Ratio,
            this.Total_Sales_First_Price,
            this.Total_Sales_Currrent_Price,
            this.Number_Of_Ticket});
            this.tabloaktar.Location = new System.Drawing.Point(26, 276);
            this.tabloaktar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabloaktar.Name = "tabloaktar";
            this.tabloaktar.RowHeadersWidth = 62;
            this.tabloaktar.RowTemplate.Height = 28;
            this.tabloaktar.Size = new System.Drawing.Size(1643, 463);
            this.tabloaktar.TabIndex = 4;
            // 
            // Number_Of_Ticket
            // 
            this.Number_Of_Ticket.HeaderText = "Number_Of_Ticket";
            this.Number_Of_Ticket.MinimumWidth = 8;
            this.Number_Of_Ticket.Name = "Number_Of_Ticket";
            this.Number_Of_Ticket.Width = 150;
            // 
            // Total_Sales_Currrent_Price
            // 
            this.Total_Sales_Currrent_Price.HeaderText = "Total_Sales_Currrent_Price";
            this.Total_Sales_Currrent_Price.MinimumWidth = 8;
            this.Total_Sales_Currrent_Price.Name = "Total_Sales_Currrent_Price";
            this.Total_Sales_Currrent_Price.Width = 150;
            // 
            // Total_Sales_First_Price
            // 
            this.Total_Sales_First_Price.HeaderText = "Total_Sales_First_Price";
            this.Total_Sales_First_Price.MinimumWidth = 8;
            this.Total_Sales_First_Price.Name = "Total_Sales_First_Price";
            this.Total_Sales_First_Price.Width = 150;
            // 
            // Discount_Ratio
            // 
            this.Discount_Ratio.HeaderText = "Discount_Ratio";
            this.Discount_Ratio.MinimumWidth = 8;
            this.Discount_Ratio.Name = "Discount_Ratio";
            this.Discount_Ratio.Width = 150;
            // 
            // KSA_RP_CURRENT
            // 
            this.KSA_RP_CURRENT.HeaderText = "KSA_RP_CURRENT";
            this.KSA_RP_CURRENT.MinimumWidth = 8;
            this.KSA_RP_CURRENT.Name = "KSA_RP_CURRENT";
            this.KSA_RP_CURRENT.Width = 150;
            // 
            // KSA_RP
            // 
            this.KSA_RP.HeaderText = "KSA_RP";
            this.KSA_RP.MinimumWidth = 8;
            this.KSA_RP.Name = "KSA_RP";
            this.KSA_RP.Width = 150;
            // 
            // Qty
            // 
            this.Qty.HeaderText = "Qty";
            this.Qty.MinimumWidth = 8;
            this.Qty.Name = "Qty";
            this.Qty.Width = 150;
            // 
            // Barcode
            // 
            this.Barcode.HeaderText = "Barcode";
            this.Barcode.MinimumWidth = 8;
            this.Barcode.Name = "Barcode";
            this.Barcode.Width = 150;
            // 
            // ItemDim1Code
            // 
            this.ItemDim1Code.HeaderText = "ItemDim1Code";
            this.ItemDim1Code.MinimumWidth = 8;
            this.ItemDim1Code.Name = "ItemDim1Code";
            this.ItemDim1Code.Width = 150;
            // 
            // ColorCode
            // 
            this.ColorCode.HeaderText = "ColorCode";
            this.ColorCode.MinimumWidth = 8;
            this.ColorCode.Name = "ColorCode";
            this.ColorCode.Width = 150;
            // 
            // ProductCode
            // 
            this.ProductCode.HeaderText = "ProductCode";
            this.ProductCode.MinimumWidth = 8;
            this.ProductCode.Name = "ProductCode";
            this.ProductCode.Width = 150;
            // 
            // SubCurrAccCode
            // 
            this.SubCurrAccCode.HeaderText = "SubCurrAccCode";
            this.SubCurrAccCode.MinimumWidth = 8;
            this.SubCurrAccCode.Name = "SubCurrAccCode";
            this.SubCurrAccCode.Width = 150;
            // 
            // CustomerCode
            // 
            this.CustomerCode.HeaderText = "CustomerCode";
            this.CustomerCode.MinimumWidth = 8;
            this.CustomerCode.Name = "CustomerCode";
            this.CustomerCode.Width = 150;
            // 
            // Date
            // 
            this.Date.HeaderText = "Date";
            this.Date.MinimumWidth = 8;
            this.Date.Name = "Date";
            this.Date.Width = 150;
            // 
            // ilkdata
            // 
            this.ilkdata.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ilkdata.Location = new System.Drawing.Point(26, 80);
            this.ilkdata.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ilkdata.Name = "ilkdata";
            this.ilkdata.RowHeadersWidth = 62;
            this.ilkdata.RowTemplate.Height = 28;
            this.ilkdata.Size = new System.Drawing.Size(1643, 180);
            this.ilkdata.TabIndex = 5;
            // 
            // ksaexec
            // 
            this.ksaexec.Location = new System.Drawing.Point(807, 30);
            this.ksaexec.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ksaexec.Name = "ksaexec";
            this.ksaexec.Size = new System.Drawing.Size(165, 30);
            this.ksaexec.TabIndex = 6;
            this.ksaexec.Text = "YURDIŞISATIŞ İŞLE";
            this.ksaexec.UseVisualStyleBackColor = true;
            this.ksaexec.Click += new System.EventHandler(this.ksaexec_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.dataGridView4);
            this.tabPage3.Controls.Add(this.dataGridView3);
            this.tabPage3.Controls.Add(this.groupBox5);
            this.tabPage3.Location = new System.Drawing.Point(4, 25);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(1697, 758);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Karşılaştır ve Düzelt";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.listBox1);
            this.groupBox5.Controls.Add(this.yearCb);
            this.groupBox5.Controls.Add(this.button1);
            this.groupBox5.Controls.Add(this.monthCb);
            this.groupBox5.Location = new System.Drawing.Point(12, 13);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(1669, 140);
            this.groupBox5.TabIndex = 11;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "seçim";
            // 
            // monthCb
            // 
            this.monthCb.AllowDrop = true;
            this.monthCb.BackColor = System.Drawing.SystemColors.MenuText;
            this.monthCb.DropDownHeight = 120;
            this.monthCb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.monthCb.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.monthCb.ForeColor = System.Drawing.Color.Gray;
            this.monthCb.IntegralHeight = false;
            this.monthCb.Items.AddRange(new object[] {
            "Ocak",
            "Şubat",
            "Mart",
            "Nisan",
            "Mayıs",
            "Haziran",
            "Temmuz",
            "Ağustos",
            "Eylül",
            "Ekim",
            "Kasım",
            "Aralık"});
            this.monthCb.Location = new System.Drawing.Point(14, 61);
            this.monthCb.Name = "monthCb";
            this.monthCb.Size = new System.Drawing.Size(166, 24);
            this.monthCb.TabIndex = 9;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(14, 104);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(166, 30);
            this.button1.TabIndex = 10;
            this.button1.Text = "Çalıştır";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Run_Click);
            // 
            // yearCb
            // 
            this.yearCb.AllowDrop = true;
            this.yearCb.BackColor = System.Drawing.SystemColors.MenuText;
            this.yearCb.DropDownHeight = 120;
            this.yearCb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.yearCb.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.yearCb.ForeColor = System.Drawing.Color.Gray;
            this.yearCb.IntegralHeight = false;
            this.yearCb.Items.AddRange(new object[] {
            "2023",
            "2024",
            "2025",
            "2026",
            "2027",
            "2028",
            "2029",
            "2030"});
            this.yearCb.Location = new System.Drawing.Point(14, 21);
            this.yearCb.Name = "yearCb";
            this.yearCb.Size = new System.Drawing.Size(166, 24);
            this.yearCb.TabIndex = 11;
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 16;
            this.listBox1.Location = new System.Drawing.Point(216, 21);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(1439, 116);
            this.listBox1.TabIndex = 16;
            // 
            // dataGridView3
            // 
            this.dataGridView3.AllowUserToAddRows = false;
            this.dataGridView3.AllowUserToDeleteRows = false;
            this.dataGridView3.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView3.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Index,
            this.branch,
            this.month,
            this.storeCode,
            this.storeName,
            this.apiTotal,
            this.dbTotal,
            this.fark});
            this.dataGridView3.Location = new System.Drawing.Point(12, 159);
            this.dataGridView3.Name = "dataGridView3";
            this.dataGridView3.ReadOnly = true;
            this.dataGridView3.RowHeadersWidth = 51;
            this.dataGridView3.RowTemplate.Height = 24;
            this.dataGridView3.Size = new System.Drawing.Size(1669, 224);
            this.dataGridView3.TabIndex = 13;
            // 
            // fark
            // 
            this.fark.HeaderText = "Fark";
            this.fark.MinimumWidth = 6;
            this.fark.Name = "fark";
            this.fark.ReadOnly = true;
            this.fark.Width = 125;
            // 
            // dbTotal
            // 
            this.dbTotal.HeaderText = "Database toplamı";
            this.dbTotal.MinimumWidth = 6;
            this.dbTotal.Name = "dbTotal";
            this.dbTotal.ReadOnly = true;
            this.dbTotal.Width = 125;
            // 
            // apiTotal
            // 
            this.apiTotal.HeaderText = "Api Toplamı";
            this.apiTotal.MinimumWidth = 6;
            this.apiTotal.Name = "apiTotal";
            this.apiTotal.ReadOnly = true;
            this.apiTotal.Width = 125;
            // 
            // storeName
            // 
            this.storeName.HeaderText = "Mağaza Adı";
            this.storeName.MinimumWidth = 6;
            this.storeName.Name = "storeName";
            this.storeName.ReadOnly = true;
            this.storeName.Width = 125;
            // 
            // storeCode
            // 
            this.storeCode.HeaderText = "MağazaKodu";
            this.storeCode.MinimumWidth = 6;
            this.storeCode.Name = "storeCode";
            this.storeCode.ReadOnly = true;
            this.storeCode.Width = 125;
            // 
            // month
            // 
            this.month.HeaderText = "Ay";
            this.month.MinimumWidth = 6;
            this.month.Name = "month";
            this.month.ReadOnly = true;
            this.month.Width = 125;
            // 
            // branch
            // 
            this.branch.HeaderText = "BranchUuid";
            this.branch.MinimumWidth = 6;
            this.branch.Name = "branch";
            this.branch.ReadOnly = true;
            this.branch.Width = 125;
            // 
            // Index
            // 
            this.Index.HeaderText = "Index";
            this.Index.MinimumWidth = 6;
            this.Index.Name = "Index";
            this.Index.ReadOnly = true;
            this.Index.Width = 125;
            // 
            // dataGridView4
            // 
            this.dataGridView4.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView4.Location = new System.Drawing.Point(12, 406);
            this.dataGridView4.Name = "dataGridView4";
            this.dataGridView4.RowHeadersWidth = 51;
            this.dataGridView4.RowTemplate.Height = 24;
            this.dataGridView4.Size = new System.Drawing.Size(1669, 349);
            this.dataGridView4.TabIndex = 14;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox6);
            this.tabPage1.Controls.Add(this.dataGridView2);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1697, 758);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Api sorgulaması Yap";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // dataGridView2
            // 
            this.dataGridView2.ColumnHeadersHeight = 29;
            this.dataGridView2.Location = new System.Drawing.Point(22, 119);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.RowHeadersWidth = 51;
            this.dataGridView2.Size = new System.Drawing.Size(1656, 645);
            this.dataGridView2.TabIndex = 7;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.listBox2);
            this.groupBox6.Controls.Add(this.groupBox2);
            this.groupBox6.Controls.Add(this.groupBox4);
            this.groupBox6.Controls.Add(this.label3);
            this.groupBox6.Controls.Add(this.groupBox3);
            this.groupBox6.Controls.Add(this.label2);
            this.groupBox6.Controls.Add(this.checkBox1);
            this.groupBox6.Controls.Add(this.button3);
            this.groupBox6.Location = new System.Drawing.Point(22, 6);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(1656, 107);
            this.groupBox6.TabIndex = 20;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Seçim";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(718, 54);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(122, 33);
            this.button3.TabIndex = 10;
            this.button3.Text = "Rapor çek";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.Api_Sorgu_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(737, 21);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(68, 20);
            this.checkBox1.TabIndex = 16;
            this.checkBox1.Text = "saatlik";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1347, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 16);
            this.label2.TabIndex = 17;
            this.label2.Text = "api";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.finishPicker);
            this.groupBox3.Location = new System.Drawing.Point(483, 21);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(212, 66);
            this.groupBox3.TabIndex = 14;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Bitiş Tarihi";
            // 
            // finishPicker
            // 
            this.finishPicker.Location = new System.Drawing.Point(6, 29);
            this.finishPicker.Name = "finishPicker";
            this.finishPicker.Size = new System.Drawing.Size(200, 22);
            this.finishPicker.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1347, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 16);
            this.label3.TabIndex = 18;
            this.label3.Text = "veri tabanı";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.startPicker);
            this.groupBox4.Location = new System.Drawing.Point(251, 21);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(213, 66);
            this.groupBox4.TabIndex = 15;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Başlangıç tarihi";
            // 
            // startPicker
            // 
            this.startPicker.Location = new System.Drawing.Point(0, 28);
            this.startPicker.Name = "startPicker";
            this.startPicker.Size = new System.Drawing.Size(207, 22);
            this.startPicker.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.comboboxStores);
            this.groupBox2.Location = new System.Drawing.Point(16, 21);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(213, 66);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Mağaza Adı";
            // 
            // comboboxStores
            // 
            this.comboboxStores.FormattingEnabled = true;
            this.comboboxStores.Location = new System.Drawing.Point(6, 26);
            this.comboboxStores.Name = "comboboxStores";
            this.comboboxStores.Size = new System.Drawing.Size(201, 24);
            this.comboboxStores.TabIndex = 0;
            // 
            // listBox2
            // 
            this.listBox2.FormattingEnabled = true;
            this.listBox2.ItemHeight = 16;
            this.listBox2.Location = new System.Drawing.Point(872, 19);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(450, 68);
            this.listBox2.TabIndex = 21;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(24, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1705, 787);
            this.tabControl1.TabIndex = 17;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Purple;
            this.ClientSize = new System.Drawing.Size(1752, 830);
            this.Controls.Add(this.tabControl1);
            this.ForeColor = System.Drawing.Color.DarkMagenta;
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "IT Tool by Ufuk Kardeşiniz...";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tabloaktar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ilkdata)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView4)).EndInit();
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }


        #endregion
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private DataGridViewTextBoxColumn farkSonuc;
        private TabPage tabPage2;
        private Button ksaexec;
        private DataGridView ilkdata;
        private DataGridView tabloaktar;
        private DataGridViewTextBoxColumn Date;
        private DataGridViewTextBoxColumn CustomerCode;
        private DataGridViewTextBoxColumn SubCurrAccCode;
        private DataGridViewTextBoxColumn ProductCode;
        private DataGridViewTextBoxColumn ColorCode;
        private DataGridViewTextBoxColumn ItemDim1Code;
        private DataGridViewTextBoxColumn Barcode;
        private DataGridViewTextBoxColumn Qty;
        private DataGridViewTextBoxColumn KSA_RP;
        private DataGridViewTextBoxColumn KSA_RP_CURRENT;
        private DataGridViewTextBoxColumn Discount_Ratio;
        private DataGridViewTextBoxColumn Total_Sales_First_Price;
        private DataGridViewTextBoxColumn Total_Sales_Currrent_Price;
        private DataGridViewTextBoxColumn Number_Of_Ticket;
        private TabPage tabPage3;
        private DataGridView dataGridView4;
        private DataGridView dataGridView3;
        private DataGridViewTextBoxColumn Index;
        private DataGridViewTextBoxColumn branch;
        private DataGridViewTextBoxColumn month;
        private DataGridViewTextBoxColumn storeCode;
        private DataGridViewTextBoxColumn storeName;
        private DataGridViewTextBoxColumn apiTotal;
        private DataGridViewTextBoxColumn dbTotal;
        private DataGridViewTextBoxColumn fark;
        private GroupBox groupBox5;
        private ListBox listBox1;
        private ComboBox yearCb;
        private Button button1;
        private ComboBox monthCb;
        private TabPage tabPage1;
        private GroupBox groupBox6;
        private ListBox listBox2;
        private GroupBox groupBox2;
        private ComboBox comboboxStores;
        private GroupBox groupBox4;
        private DateTimePicker startPicker;
        private Label label3;
        private GroupBox groupBox3;
        private DateTimePicker finishPicker;
        private Label label2;
        private CheckBox checkBox1;
        private Button button3;
        private DataGridView dataGridView2;
        private TabControl tabControl1;
    }
}

