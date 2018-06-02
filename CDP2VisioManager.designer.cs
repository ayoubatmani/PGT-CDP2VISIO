namespace CDP2VISIO
{
  partial class CDP2VisioManager
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CDP2VisioManager));
      this.Display = new System.Windows.Forms.TabControl();
      this.tpInput = new System.Windows.Forms.TabPage();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.dgStencils = new System.Windows.Forms.DataGridView();
      this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.stencilsBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
      this.StencilsDataSet = new CDP2VISIO.StencilsDS();
      this.button_add_new_stencil = new System.Windows.Forms.Button();
      this.groupBox2 = new System.Windows.Forms.GroupBox();
      this.btnPickFavHosts = new System.Windows.Forms.Button();
      this.cbActiveDiscovery = new System.Windows.Forms.CheckBox();
      this.cbStartScript = new System.Windows.Forms.CheckBox();
      this.btnClearDeviceList = new System.Windows.Forms.Button();
      this.btnAddNetwork = new System.Windows.Forms.Button();
      this.label1 = new System.Windows.Forms.Label();
      this.tbNetwork = new System.Windows.Forms.TextBox();
      this.label5 = new System.Windows.Forms.Label();
      this.tb_Devices = new System.Windows.Forms.TextBox();
      this.btn_CreateInventory = new System.Windows.Forms.Button();
      this.cbxJumpServers = new System.Windows.Forms.ComboBox();
      this.label6 = new System.Windows.Forms.Label();
      this.cbxProtocols = new System.Windows.Forms.ComboBox();
      this.label7 = new System.Windows.Forms.Label();
      this.groupBox3 = new System.Windows.Forms.GroupBox();
      this.btnSelectTemplateDrawing = new System.Windows.Forms.Button();
      this.label13 = new System.Windows.Forms.Label();
      this.tbTemplateDrawing = new System.Windows.Forms.TextBox();
      this.btnOutputPath = new System.Windows.Forms.Button();
      this.label3 = new System.Windows.Forms.Label();
      this.tbOutputPath = new System.Windows.Forms.TextBox();
      this.cbBindInventoryData = new System.Windows.Forms.CheckBox();
      this.cbBindDeviceData = new System.Windows.Forms.CheckBox();
      this.cbBindVLANData = new System.Windows.Forms.CheckBox();
      this.cbBindInterfaces = new System.Windows.Forms.CheckBox();
      this.label2 = new System.Windows.Forms.Label();
      this.cbxConnectorType = new System.Windows.Forms.ComboBox();
      this.label9 = new System.Windows.Forms.Label();
      this.btnCreateVisio = new System.Windows.Forms.Button();
      this.cbxGraphAlg = new System.Windows.Forms.ComboBox();
      this.cbxDocumentSize = new System.Windows.Forms.ComboBox();
      this.label8 = new System.Windows.Forms.Label();
      this.tpDiscoveryDomain = new System.Windows.Forms.TabPage();
      this.label12 = new System.Windows.Forms.Label();
      this.splitContainer2 = new System.Windows.Forms.SplitContainer();
      this.tbIPRangesIncluded = new System.Windows.Forms.TextBox();
      this.panel3 = new System.Windows.Forms.Panel();
      this.label11 = new System.Windows.Forms.Label();
      this.tbIPRangesExcluded = new System.Windows.Forms.TextBox();
      this.panel4 = new System.Windows.Forms.Panel();
      this.label10 = new System.Windows.Forms.Label();
      this.tpDatabaseView = new System.Windows.Forms.TabPage();
      this.btnExportInventory = new PGT.Common.MenuButton();
      this.cmExportButton = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.cmStandardExport = new System.Windows.Forms.ToolStripMenuItem();
      this.cmVisioDataExport = new System.Windows.Forms.ToolStripMenuItem();
      this.label4 = new System.Windows.Forms.Label();
      this.dataGridView1 = new System.Windows.Forms.DataGridView();
      this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.nameDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.iPAddressDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.typeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.Platform = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.SystemSerial = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.L3InterfaceInformation = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.VLANInformation = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.InterfaceDescriptions = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.devicesBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
      this.InventoryDS = new CDP2VISIO.CDPDataSet();
      this.btnSaveXML = new System.Windows.Forms.Button();
      this.btnLoadXML = new System.Windows.Forms.Button();
      this.tpNeighbors = new System.Windows.Forms.TabPage();
      this.dataGridView2 = new System.Windows.Forms.DataGridView();
      this.parentIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.neighborIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.neighbourInterfaceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.localInterfaceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.neighboursBindingSource = new System.Windows.Forms.BindingSource(this.components);
      this.tpMSAGL = new System.Windows.Forms.TabPage();
      this.btnCreateGraph = new System.Windows.Forms.Button();
      this.gViewer1 = new Microsoft.Msagl.GraphViewerGdi.GViewer();
      this.devicesBindingSource = new System.Windows.Forms.BindingSource(this.components);
      this.dataSet11BindingSource = new System.Windows.Forms.BindingSource(this.components);
      this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
      this.openFileDialog2 = new System.Windows.Forms.OpenFileDialog();
      this.openFileDialog3 = new System.Windows.Forms.OpenFileDialog();
      this.stencilsBindingSource = new System.Windows.Forms.BindingSource(this.components);
      this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
      this.Display.SuspendLayout();
      this.tpInput.SuspendLayout();
      this.groupBox1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.dgStencils)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.stencilsBindingSource1)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.StencilsDataSet)).BeginInit();
      this.groupBox2.SuspendLayout();
      this.groupBox3.SuspendLayout();
      this.tpDiscoveryDomain.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
      this.splitContainer2.Panel1.SuspendLayout();
      this.splitContainer2.Panel2.SuspendLayout();
      this.splitContainer2.SuspendLayout();
      this.panel3.SuspendLayout();
      this.panel4.SuspendLayout();
      this.tpDatabaseView.SuspendLayout();
      this.cmExportButton.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.devicesBindingSource1)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.InventoryDS)).BeginInit();
      this.tpNeighbors.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.neighboursBindingSource)).BeginInit();
      this.tpMSAGL.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.devicesBindingSource)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.dataSet11BindingSource)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.stencilsBindingSource)).BeginInit();
      this.SuspendLayout();
      // 
      // Display
      // 
      this.Display.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.Display.Controls.Add(this.tpInput);
      this.Display.Controls.Add(this.tpDiscoveryDomain);
      this.Display.Controls.Add(this.tpDatabaseView);
      this.Display.Controls.Add(this.tpNeighbors);
      this.Display.Controls.Add(this.tpMSAGL);
      this.Display.Location = new System.Drawing.Point(32, 18);
      this.Display.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.Display.Name = "Display";
      this.Display.SelectedIndex = 0;
      this.Display.Size = new System.Drawing.Size(1198, 785);
      this.Display.TabIndex = 9;
      // 
      // tpInput
      // 
      this.tpInput.BackColor = System.Drawing.Color.DimGray;
      this.tpInput.Controls.Add(this.groupBox1);
      this.tpInput.Controls.Add(this.groupBox2);
      this.tpInput.Controls.Add(this.groupBox3);
      this.tpInput.Location = new System.Drawing.Point(4, 29);
      this.tpInput.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.tpInput.Name = "tpInput";
      this.tpInput.Size = new System.Drawing.Size(1190, 752);
      this.tpInput.TabIndex = 5;
      this.tpInput.Text = "Settings & Create Visio document ";
      this.tpInput.Paint += new System.Windows.Forms.PaintEventHandler(this.DrawGradientBackground);
      // 
      // groupBox1
      // 
      this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBox1.BackColor = System.Drawing.Color.SteelBlue;
      this.groupBox1.Controls.Add(this.dgStencils);
      this.groupBox1.Controls.Add(this.button_add_new_stencil);
      this.groupBox1.Location = new System.Drawing.Point(21, 306);
      this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.groupBox1.Size = new System.Drawing.Size(1138, 255);
      this.groupBox1.TabIndex = 34;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Stencil Settings";
      this.groupBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.DrawGradientBackground);
      // 
      // dgStencils
      // 
      this.dgStencils.AllowUserToResizeRows = false;
      this.dgStencils.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.dgStencils.AutoGenerateColumns = false;
      this.dgStencils.BackgroundColor = System.Drawing.Color.WhiteSmoke;
      this.dgStencils.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgStencils.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3});
      this.dgStencils.DataSource = this.stencilsBindingSource1;
      this.dgStencils.Location = new System.Drawing.Point(16, 37);
      this.dgStencils.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.dgStencils.Name = "dgStencils";
      this.dgStencils.RowHeadersVisible = false;
      this.dgStencils.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
      this.dgStencils.Size = new System.Drawing.Size(1102, 154);
      this.dgStencils.TabIndex = 11;
      // 
      // dataGridViewTextBoxColumn1
      // 
      this.dataGridViewTextBoxColumn1.DataPropertyName = "sh_inv_PID";
      this.dataGridViewTextBoxColumn1.HeaderText = "PID Name";
      this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
      // 
      // dataGridViewTextBoxColumn2
      // 
      this.dataGridViewTextBoxColumn2.DataPropertyName = "stencilName";
      this.dataGridViewTextBoxColumn2.HeaderText = "Stencil file";
      this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
      this.dataGridViewTextBoxColumn2.Width = 250;
      // 
      // dataGridViewTextBoxColumn3
      // 
      this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
      this.dataGridViewTextBoxColumn3.DataPropertyName = "masterNameU";
      this.dataGridViewTextBoxColumn3.HeaderText = "Shape name";
      this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
      // 
      // stencilsBindingSource1
      // 
      this.stencilsBindingSource1.DataMember = "Stencils";
      this.stencilsBindingSource1.DataSource = this.StencilsDataSet;
      // 
      // StencilsDataSet
      // 
      this.StencilsDataSet.DataSetName = "Settings";
      this.StencilsDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
      // 
      // button_add_new_stencil
      // 
      this.button_add_new_stencil.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.button_add_new_stencil.BackColor = System.Drawing.Color.SteelBlue;
      this.button_add_new_stencil.ForeColor = System.Drawing.Color.White;
      this.button_add_new_stencil.Location = new System.Drawing.Point(870, 202);
      this.button_add_new_stencil.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.button_add_new_stencil.Name = "button_add_new_stencil";
      this.button_add_new_stencil.Size = new System.Drawing.Size(249, 45);
      this.button_add_new_stencil.TabIndex = 21;
      this.button_add_new_stencil.Text = "Add New stencil";
      this.button_add_new_stencil.UseVisualStyleBackColor = false;
      this.button_add_new_stencil.Click += new System.EventHandler(this.button_add_new_stencil_Click);
      // 
      // groupBox2
      // 
      this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBox2.BackColor = System.Drawing.Color.SteelBlue;
      this.groupBox2.Controls.Add(this.btnPickFavHosts);
      this.groupBox2.Controls.Add(this.cbActiveDiscovery);
      this.groupBox2.Controls.Add(this.cbStartScript);
      this.groupBox2.Controls.Add(this.btnClearDeviceList);
      this.groupBox2.Controls.Add(this.btnAddNetwork);
      this.groupBox2.Controls.Add(this.label1);
      this.groupBox2.Controls.Add(this.tbNetwork);
      this.groupBox2.Controls.Add(this.label5);
      this.groupBox2.Controls.Add(this.tb_Devices);
      this.groupBox2.Controls.Add(this.btn_CreateInventory);
      this.groupBox2.Controls.Add(this.cbxJumpServers);
      this.groupBox2.Controls.Add(this.label6);
      this.groupBox2.Controls.Add(this.cbxProtocols);
      this.groupBox2.Controls.Add(this.label7);
      this.groupBox2.Location = new System.Drawing.Point(21, 25);
      this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.groupBox2.Size = new System.Drawing.Size(1138, 271);
      this.groupBox2.TabIndex = 35;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "CDP Discovery Settings";
      this.groupBox2.Paint += new System.Windows.Forms.PaintEventHandler(this.DrawGradientBackground);
      // 
      // btnPickFavHosts
      // 
      this.btnPickFavHosts.BackColor = System.Drawing.Color.SteelBlue;
      this.btnPickFavHosts.ForeColor = System.Drawing.Color.White;
      this.btnPickFavHosts.Location = new System.Drawing.Point(221, 102);
      this.btnPickFavHosts.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.btnPickFavHosts.Name = "btnPickFavHosts";
      this.btnPickFavHosts.Size = new System.Drawing.Size(159, 37);
      this.btnPickFavHosts.TabIndex = 41;
      this.btnPickFavHosts.Text = "Pick favorite hosts";
      this.btnPickFavHosts.UseVisualStyleBackColor = false;
      this.btnPickFavHosts.Click += new System.EventHandler(this.btnPickFavHosts_Click);
      // 
      // cbActiveDiscovery
      // 
      this.cbActiveDiscovery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.cbActiveDiscovery.AutoSize = true;
      this.cbActiveDiscovery.Checked = global::CDP2VISIO.Options.Default.MRUActiveDiscovery;
      this.cbActiveDiscovery.CheckState = System.Windows.Forms.CheckState.Checked;
      this.cbActiveDiscovery.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::CDP2VISIO.Options.Default, "MRUActiveDiscovery", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.cbActiveDiscovery.Location = new System.Drawing.Point(870, 147);
      this.cbActiveDiscovery.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.cbActiveDiscovery.Name = "cbActiveDiscovery";
      this.cbActiveDiscovery.Size = new System.Drawing.Size(147, 24);
      this.cbActiveDiscovery.TabIndex = 40;
      this.cbActiveDiscovery.Text = "Active discovery";
      this.toolTip1.SetToolTip(this.cbActiveDiscovery, "Controls whether newly discovered CDP neighbors\r\nwill also be included in this di" +
        "scovery process.");
      this.cbActiveDiscovery.UseVisualStyleBackColor = true;
      // 
      // cbStartScript
      // 
      this.cbStartScript.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.cbStartScript.AutoSize = true;
      this.cbStartScript.Location = new System.Drawing.Point(870, 181);
      this.cbStartScript.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.cbStartScript.Name = "cbStartScript";
      this.cbStartScript.Size = new System.Drawing.Size(199, 24);
      this.cbStartScript.TabIndex = 39;
      this.cbStartScript.Text = "Start script immediately";
      this.cbStartScript.UseVisualStyleBackColor = true;
      // 
      // btnClearDeviceList
      // 
      this.btnClearDeviceList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnClearDeviceList.BackColor = System.Drawing.Color.SteelBlue;
      this.btnClearDeviceList.ForeColor = System.Drawing.Color.White;
      this.btnClearDeviceList.Location = new System.Drawing.Point(870, 55);
      this.btnClearDeviceList.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.btnClearDeviceList.Name = "btnClearDeviceList";
      this.btnClearDeviceList.Size = new System.Drawing.Size(250, 37);
      this.btnClearDeviceList.TabIndex = 12;
      this.btnClearDeviceList.Text = "Clear list";
      this.btnClearDeviceList.UseVisualStyleBackColor = false;
      this.btnClearDeviceList.Click += new System.EventHandler(this.btnClearDeviceList_Click);
      // 
      // btnAddNetwork
      // 
      this.btnAddNetwork.BackColor = System.Drawing.Color.SteelBlue;
      this.btnAddNetwork.ForeColor = System.Drawing.Color.White;
      this.btnAddNetwork.Location = new System.Drawing.Point(221, 55);
      this.btnAddNetwork.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.btnAddNetwork.Name = "btnAddNetwork";
      this.btnAddNetwork.Size = new System.Drawing.Size(159, 37);
      this.btnAddNetwork.TabIndex = 11;
      this.btnAddNetwork.Text = "Add network";
      this.btnAddNetwork.UseVisualStyleBackColor = false;
      this.btnAddNetwork.Click += new System.EventHandler(this.btnAddNetwork_Click);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(12, 35);
      this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(185, 20);
      this.label1.TabIndex = 10;
      this.label1.Text = "Network / subnet to add :";
      // 
      // tbNetwork
      // 
      this.tbNetwork.Location = new System.Drawing.Point(15, 60);
      this.tbNetwork.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.tbNetwork.Name = "tbNetwork";
      this.tbNetwork.Size = new System.Drawing.Size(198, 26);
      this.tbNetwork.TabIndex = 9;
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(388, 35);
      this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(486, 20);
      this.label5.TabIndex = 1;
      this.label5.Text = "Device list to run discovery on (host ip;host name;Custom columns) :";
      // 
      // tb_Devices
      // 
      this.tb_Devices.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.tb_Devices.Location = new System.Drawing.Point(393, 60);
      this.tb_Devices.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.tb_Devices.Multiline = true;
      this.tb_Devices.Name = "tb_Devices";
      this.tb_Devices.ScrollBars = System.Windows.Forms.ScrollBars.Both;
      this.tb_Devices.Size = new System.Drawing.Size(466, 196);
      this.tb_Devices.TabIndex = 0;
      this.toolTip1.SetToolTip(this.tb_Devices, "Enlist the devices to be discovered. \r\nIf entering both ip address and hostname, " +
        "use the\r\nseparator character defined in Tools/Options.");
      this.tb_Devices.WordWrap = false;
      // 
      // btn_CreateInventory
      // 
      this.btn_CreateInventory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btn_CreateInventory.BackColor = System.Drawing.Color.SteelBlue;
      this.btn_CreateInventory.ForeColor = System.Drawing.Color.White;
      this.btn_CreateInventory.Location = new System.Drawing.Point(870, 214);
      this.btn_CreateInventory.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.btn_CreateInventory.Name = "btn_CreateInventory";
      this.btn_CreateInventory.Size = new System.Drawing.Size(249, 45);
      this.btn_CreateInventory.TabIndex = 8;
      this.btn_CreateInventory.Text = "Create inventory";
      this.btn_CreateInventory.UseVisualStyleBackColor = false;
      this.btn_CreateInventory.EnabledChanged += new System.EventHandler(this.btnEnabledChanged);
      this.btn_CreateInventory.Click += new System.EventHandler(this.btn_RunScript_Click);
      // 
      // cbxJumpServers
      // 
      this.cbxJumpServers.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::CDP2VISIO.Options.Default, "MRUJumpServer", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.cbxJumpServers.FormattingEnabled = true;
      this.cbxJumpServers.Location = new System.Drawing.Point(135, 183);
      this.cbxJumpServers.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.cbxJumpServers.Name = "cbxJumpServers";
      this.cbxJumpServers.Size = new System.Drawing.Size(247, 28);
      this.cbxJumpServers.TabIndex = 2;
      this.cbxJumpServers.Text = global::CDP2VISIO.Options.Default.MRUJumpServer;
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(21, 188);
      this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(103, 20);
      this.label6.TabIndex = 3;
      this.label6.Text = "Jump server :";
      // 
      // cbxProtocols
      // 
      this.cbxProtocols.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::CDP2VISIO.Options.Default, "MRUProtocol", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.cbxProtocols.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cbxProtocols.FormattingEnabled = true;
      this.cbxProtocols.Items.AddRange(new object[] {
            "SSH1",
            "SSH2",
            "SSH2_Telnet",
            "Telnet"});
      this.cbxProtocols.Location = new System.Drawing.Point(135, 225);
      this.cbxProtocols.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.cbxProtocols.Name = "cbxProtocols";
      this.cbxProtocols.Size = new System.Drawing.Size(247, 28);
      this.cbxProtocols.TabIndex = 4;
      this.cbxProtocols.Text = global::CDP2VISIO.Options.Default.MRUProtocol;
      // 
      // label7
      // 
      this.label7.AutoSize = true;
      this.label7.Location = new System.Drawing.Point(21, 229);
      this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label7.Name = "label7";
      this.label7.Size = new System.Drawing.Size(75, 20);
      this.label7.TabIndex = 5;
      this.label7.Text = "Protocol :";
      // 
      // groupBox3
      // 
      this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBox3.BackColor = System.Drawing.Color.OliveDrab;
      this.groupBox3.Controls.Add(this.btnSelectTemplateDrawing);
      this.groupBox3.Controls.Add(this.label13);
      this.groupBox3.Controls.Add(this.tbTemplateDrawing);
      this.groupBox3.Controls.Add(this.btnOutputPath);
      this.groupBox3.Controls.Add(this.label3);
      this.groupBox3.Controls.Add(this.tbOutputPath);
      this.groupBox3.Controls.Add(this.cbBindInventoryData);
      this.groupBox3.Controls.Add(this.cbBindDeviceData);
      this.groupBox3.Controls.Add(this.cbBindVLANData);
      this.groupBox3.Controls.Add(this.cbBindInterfaces);
      this.groupBox3.Controls.Add(this.label2);
      this.groupBox3.Controls.Add(this.cbxConnectorType);
      this.groupBox3.Controls.Add(this.label9);
      this.groupBox3.Controls.Add(this.btnCreateVisio);
      this.groupBox3.Controls.Add(this.cbxGraphAlg);
      this.groupBox3.Controls.Add(this.cbxDocumentSize);
      this.groupBox3.Controls.Add(this.label8);
      this.groupBox3.Location = new System.Drawing.Point(21, 571);
      this.groupBox3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.groupBox3.Name = "groupBox3";
      this.groupBox3.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.groupBox3.Size = new System.Drawing.Size(1138, 166);
      this.groupBox3.TabIndex = 36;
      this.groupBox3.TabStop = false;
      this.groupBox3.Text = "Visio Document Settings";
      this.groupBox3.Paint += new System.Windows.Forms.PaintEventHandler(this.DrawGradientBackground);
      // 
      // btnSelectTemplateDrawing
      // 
      this.btnSelectTemplateDrawing.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnSelectTemplateDrawing.BackColor = System.Drawing.Color.OliveDrab;
      this.btnSelectTemplateDrawing.ForeColor = System.Drawing.Color.White;
      this.btnSelectTemplateDrawing.Location = new System.Drawing.Point(1082, 66);
      this.btnSelectTemplateDrawing.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.btnSelectTemplateDrawing.Name = "btnSelectTemplateDrawing";
      this.btnSelectTemplateDrawing.Size = new System.Drawing.Size(38, 38);
      this.btnSelectTemplateDrawing.TabIndex = 45;
      this.btnSelectTemplateDrawing.Text = "...";
      this.btnSelectTemplateDrawing.UseVisualStyleBackColor = false;
      this.btnSelectTemplateDrawing.Click += new System.EventHandler(this.btnSelectTemplateDrawing_Click);
      // 
      // label13
      // 
      this.label13.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.label13.AutoSize = true;
      this.label13.Location = new System.Drawing.Point(566, 75);
      this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label13.Name = "label13";
      this.label13.Size = new System.Drawing.Size(138, 20);
      this.label13.TabIndex = 44;
      this.label13.Text = "Template drawing:";
      // 
      // tbTemplateDrawing
      // 
      this.tbTemplateDrawing.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.tbTemplateDrawing.Location = new System.Drawing.Point(710, 69);
      this.tbTemplateDrawing.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.tbTemplateDrawing.Name = "tbTemplateDrawing";
      this.tbTemplateDrawing.Size = new System.Drawing.Size(376, 26);
      this.tbTemplateDrawing.TabIndex = 43;
      this.toolTip1.SetToolTip(this.tbTemplateDrawing, "If selected, this document will be used as a template\r\nwhen placing shapes on the" +
        " new drawing.");
      // 
      // btnOutputPath
      // 
      this.btnOutputPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnOutputPath.BackColor = System.Drawing.Color.OliveDrab;
      this.btnOutputPath.ForeColor = System.Drawing.Color.White;
      this.btnOutputPath.Location = new System.Drawing.Point(1082, 23);
      this.btnOutputPath.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.btnOutputPath.Name = "btnOutputPath";
      this.btnOutputPath.Size = new System.Drawing.Size(38, 38);
      this.btnOutputPath.TabIndex = 42;
      this.btnOutputPath.Text = "...";
      this.btnOutputPath.UseVisualStyleBackColor = false;
      this.btnOutputPath.Click += new System.EventHandler(this.btnOutputPath_Click);
      // 
      // label3
      // 
      this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(566, 34);
      this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(134, 20);
      this.label3.TabIndex = 41;
      this.label3.Text = "Output file folder :";
      // 
      // tbOutputPath
      // 
      this.tbOutputPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.tbOutputPath.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::CDP2VISIO.Options.Default, "MRUOutputPath", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.tbOutputPath.Location = new System.Drawing.Point(710, 28);
      this.tbOutputPath.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.tbOutputPath.Name = "tbOutputPath";
      this.tbOutputPath.ReadOnly = true;
      this.tbOutputPath.Size = new System.Drawing.Size(376, 26);
      this.tbOutputPath.TabIndex = 40;
      this.tbOutputPath.Text = global::CDP2VISIO.Options.Default.MRUOutputPath;
      this.tbOutputPath.TextChanged += new System.EventHandler(this.tbOutputPath_TextChanged);
      // 
      // cbBindInventoryData
      // 
      this.cbBindInventoryData.AutoSize = true;
      this.cbBindInventoryData.Checked = true;
      this.cbBindInventoryData.CheckState = System.Windows.Forms.CheckState.Checked;
      this.cbBindInventoryData.Location = new System.Drawing.Point(320, 118);
      this.cbBindInventoryData.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.cbBindInventoryData.Name = "cbBindInventoryData";
      this.cbBindInventoryData.Size = new System.Drawing.Size(245, 24);
      this.cbBindInventoryData.TabIndex = 39;
      this.cbBindInventoryData.Text = "Bind inventory data to devices";
      this.cbBindInventoryData.UseVisualStyleBackColor = true;
      // 
      // cbBindDeviceData
      // 
      this.cbBindDeviceData.AutoSize = true;
      this.cbBindDeviceData.Checked = true;
      this.cbBindDeviceData.CheckState = System.Windows.Forms.CheckState.Checked;
      this.cbBindDeviceData.Location = new System.Drawing.Point(320, 26);
      this.cbBindDeviceData.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.cbBindDeviceData.Name = "cbBindDeviceData";
      this.cbBindDeviceData.Size = new System.Drawing.Size(227, 24);
      this.cbBindDeviceData.TabIndex = 38;
      this.cbBindDeviceData.Text = "Bind device data to devices";
      this.cbBindDeviceData.UseVisualStyleBackColor = true;
      // 
      // cbBindVLANData
      // 
      this.cbBindVLANData.AutoSize = true;
      this.cbBindVLANData.Checked = true;
      this.cbBindVLANData.CheckState = System.Windows.Forms.CheckState.Checked;
      this.cbBindVLANData.Location = new System.Drawing.Point(320, 57);
      this.cbBindVLANData.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.cbBindVLANData.Name = "cbBindVLANData";
      this.cbBindVLANData.Size = new System.Drawing.Size(224, 24);
      this.cbBindVLANData.TabIndex = 37;
      this.cbBindVLANData.Text = "Bind VLAN data to devices";
      this.cbBindVLANData.UseVisualStyleBackColor = true;
      // 
      // cbBindInterfaces
      // 
      this.cbBindInterfaces.AutoSize = true;
      this.cbBindInterfaces.Checked = true;
      this.cbBindInterfaces.CheckState = System.Windows.Forms.CheckState.Checked;
      this.cbBindInterfaces.Location = new System.Drawing.Point(320, 88);
      this.cbBindInterfaces.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.cbBindInterfaces.Name = "cbBindInterfaces";
      this.cbBindInterfaces.Size = new System.Drawing.Size(244, 24);
      this.cbBindInterfaces.TabIndex = 36;
      this.cbBindInterfaces.Text = "Bind interface data to devices";
      this.cbBindInterfaces.UseVisualStyleBackColor = true;
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(12, 117);
      this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(125, 20);
      this.label2.TabIndex = 35;
      this.label2.Text = "Connector type :";
      // 
      // cbxConnectorType
      // 
      this.cbxConnectorType.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::CDP2VISIO.Options.Default, "MRUConnType", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.cbxConnectorType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cbxConnectorType.FormattingEnabled = true;
      this.cbxConnectorType.Items.AddRange(new object[] {
            "Straight Lines",
            "Right Angle"});
      this.cbxConnectorType.Location = new System.Drawing.Point(146, 111);
      this.cbxConnectorType.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.cbxConnectorType.Name = "cbxConnectorType";
      this.cbxConnectorType.Size = new System.Drawing.Size(126, 28);
      this.cbxConnectorType.TabIndex = 34;
      this.cbxConnectorType.Text = global::CDP2VISIO.Options.Default.MRUConnType;
      // 
      // label9
      // 
      this.label9.AutoSize = true;
      this.label9.Location = new System.Drawing.Point(12, 75);
      this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label9.Name = "label9";
      this.label9.Size = new System.Drawing.Size(133, 20);
      this.label9.TabIndex = 33;
      this.label9.Text = "Graph Algorithm :";
      // 
      // btnCreateVisio
      // 
      this.btnCreateVisio.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnCreateVisio.BackColor = System.Drawing.Color.OliveDrab;
      this.btnCreateVisio.ForeColor = System.Drawing.Color.White;
      this.btnCreateVisio.Location = new System.Drawing.Point(870, 111);
      this.btnCreateVisio.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.btnCreateVisio.Name = "btnCreateVisio";
      this.btnCreateVisio.Size = new System.Drawing.Size(249, 45);
      this.btnCreateVisio.TabIndex = 10;
      this.btnCreateVisio.Text = "Create Visio Document";
      this.btnCreateVisio.UseVisualStyleBackColor = false;
      this.btnCreateVisio.EnabledChanged += new System.EventHandler(this.btnEnabledChanged);
      this.btnCreateVisio.Click += new System.EventHandler(this.btnCreateVisio_Click);
      // 
      // cbxGraphAlg
      // 
      this.cbxGraphAlg.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::CDP2VISIO.Options.Default, "GraphAlg", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.cbxGraphAlg.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cbxGraphAlg.FormattingEnabled = true;
      this.cbxGraphAlg.Items.AddRange(new object[] {
            "Hierarchical",
            "StarTopology"});
      this.cbxGraphAlg.Location = new System.Drawing.Point(146, 69);
      this.cbxGraphAlg.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.cbxGraphAlg.Name = "cbxGraphAlg";
      this.cbxGraphAlg.Size = new System.Drawing.Size(126, 28);
      this.cbxGraphAlg.TabIndex = 32;
      this.cbxGraphAlg.Text = global::CDP2VISIO.Options.Default.GraphAlg;
      // 
      // cbxDocumentSize
      // 
      this.cbxDocumentSize.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::CDP2VISIO.Options.Default, "DocSize", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.cbxDocumentSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cbxDocumentSize.FormattingEnabled = true;
      this.cbxDocumentSize.Items.AddRange(new object[] {
            "A4",
            "A3",
            "A2"});
      this.cbxDocumentSize.Location = new System.Drawing.Point(146, 28);
      this.cbxDocumentSize.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.cbxDocumentSize.Name = "cbxDocumentSize";
      this.cbxDocumentSize.Size = new System.Drawing.Size(126, 28);
      this.cbxDocumentSize.TabIndex = 9;
      this.cbxDocumentSize.Text = global::CDP2VISIO.Options.Default.DocSize;
      // 
      // label8
      // 
      this.label8.AutoSize = true;
      this.label8.Location = new System.Drawing.Point(12, 34);
      this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label8.Name = "label8";
      this.label8.Size = new System.Drawing.Size(123, 20);
      this.label8.TabIndex = 31;
      this.label8.Text = "Document size :";
      // 
      // tpDiscoveryDomain
      // 
      this.tpDiscoveryDomain.BackColor = System.Drawing.Color.DarkBlue;
      this.tpDiscoveryDomain.Controls.Add(this.label12);
      this.tpDiscoveryDomain.Controls.Add(this.splitContainer2);
      this.tpDiscoveryDomain.Location = new System.Drawing.Point(4, 29);
      this.tpDiscoveryDomain.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.tpDiscoveryDomain.Name = "tpDiscoveryDomain";
      this.tpDiscoveryDomain.Size = new System.Drawing.Size(1190, 752);
      this.tpDiscoveryDomain.TabIndex = 6;
      this.tpDiscoveryDomain.Text = "Discovery Domain Definition";
      this.tpDiscoveryDomain.Paint += new System.Windows.Forms.PaintEventHandler(this.DrawGradientBackground);
      // 
      // label12
      // 
      this.label12.AutoSize = true;
      this.label12.Location = new System.Drawing.Point(33, 40);
      this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label12.Name = "label12";
      this.label12.Size = new System.Drawing.Size(1045, 100);
      this.label12.TabIndex = 16;
      this.label12.Text = resources.GetString("label12.Text");
      // 
      // splitContainer2
      // 
      this.splitContainer2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.splitContainer2.Location = new System.Drawing.Point(27, 157);
      this.splitContainer2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.splitContainer2.Name = "splitContainer2";
      // 
      // splitContainer2.Panel1
      // 
      this.splitContainer2.Panel1.Controls.Add(this.tbIPRangesIncluded);
      this.splitContainer2.Panel1.Controls.Add(this.panel3);
      // 
      // splitContainer2.Panel2
      // 
      this.splitContainer2.Panel2.Controls.Add(this.tbIPRangesExcluded);
      this.splitContainer2.Panel2.Controls.Add(this.panel4);
      this.splitContainer2.Size = new System.Drawing.Size(1126, 558);
      this.splitContainer2.SplitterDistance = 547;
      this.splitContainer2.SplitterWidth = 6;
      this.splitContainer2.TabIndex = 0;
      // 
      // tbIPRangesIncluded
      // 
      this.tbIPRangesIncluded.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tbIPRangesIncluded.Location = new System.Drawing.Point(0, 42);
      this.tbIPRangesIncluded.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.tbIPRangesIncluded.Multiline = true;
      this.tbIPRangesIncluded.Name = "tbIPRangesIncluded";
      this.tbIPRangesIncluded.ScrollBars = System.Windows.Forms.ScrollBars.Both;
      this.tbIPRangesIncluded.Size = new System.Drawing.Size(547, 516);
      this.tbIPRangesIncluded.TabIndex = 16;
      this.toolTip1.SetToolTip(this.tbIPRangesIncluded, "Specify ip address ranges to include in discovery.\r\nEnter one network at a line u" +
        "sing the format : network/masklength\r\nExample : 192.168.19.0/24");
      this.tbIPRangesIncluded.WordWrap = false;
      // 
      // panel3
      // 
      this.panel3.BackColor = System.Drawing.Color.Transparent;
      this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.panel3.Controls.Add(this.label11);
      this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
      this.panel3.Location = new System.Drawing.Point(0, 0);
      this.panel3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.panel3.Name = "panel3";
      this.panel3.Size = new System.Drawing.Size(547, 42);
      this.panel3.TabIndex = 1;
      // 
      // label11
      // 
      this.label11.AutoSize = true;
      this.label11.Location = new System.Drawing.Point(4, 9);
      this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label11.Name = "label11";
      this.label11.Size = new System.Drawing.Size(293, 20);
      this.label11.TabIndex = 15;
      this.label11.Text = "Ip address ranges included in discovery :";
      // 
      // tbIPRangesExcluded
      // 
      this.tbIPRangesExcluded.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tbIPRangesExcluded.Location = new System.Drawing.Point(0, 42);
      this.tbIPRangesExcluded.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.tbIPRangesExcluded.Multiline = true;
      this.tbIPRangesExcluded.Name = "tbIPRangesExcluded";
      this.tbIPRangesExcluded.ScrollBars = System.Windows.Forms.ScrollBars.Both;
      this.tbIPRangesExcluded.Size = new System.Drawing.Size(573, 516);
      this.tbIPRangesExcluded.TabIndex = 17;
      this.toolTip1.SetToolTip(this.tbIPRangesExcluded, "Specify ip address ranges to exclude from discovery.\r\nEnter one network at a line" +
        " using the format : network/masklength\r\nExample : 192.168.19.0/24");
      this.tbIPRangesExcluded.WordWrap = false;
      // 
      // panel4
      // 
      this.panel4.BackColor = System.Drawing.Color.Transparent;
      this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.panel4.Controls.Add(this.label10);
      this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
      this.panel4.Location = new System.Drawing.Point(0, 0);
      this.panel4.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.panel4.Name = "panel4";
      this.panel4.Size = new System.Drawing.Size(573, 42);
      this.panel4.TabIndex = 2;
      // 
      // label10
      // 
      this.label10.AutoSize = true;
      this.label10.Location = new System.Drawing.Point(10, 9);
      this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label10.Name = "label10";
      this.label10.Size = new System.Drawing.Size(317, 20);
      this.label10.TabIndex = 14;
      this.label10.Text = "Ip address ranges excluded from discovery :";
      // 
      // tpDatabaseView
      // 
      this.tpDatabaseView.BackColor = System.Drawing.Color.SteelBlue;
      this.tpDatabaseView.Controls.Add(this.btnExportInventory);
      this.tpDatabaseView.Controls.Add(this.label4);
      this.tpDatabaseView.Controls.Add(this.dataGridView1);
      this.tpDatabaseView.Controls.Add(this.btnSaveXML);
      this.tpDatabaseView.Controls.Add(this.btnLoadXML);
      this.tpDatabaseView.Location = new System.Drawing.Point(4, 29);
      this.tpDatabaseView.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.tpDatabaseView.Name = "tpDatabaseView";
      this.tpDatabaseView.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.tpDatabaseView.Size = new System.Drawing.Size(1190, 752);
      this.tpDatabaseView.TabIndex = 0;
      this.tpDatabaseView.Text = "Inventory database";
      this.tpDatabaseView.Paint += new System.Windows.Forms.PaintEventHandler(this.DrawGradientBackground);
      // 
      // btnExportInventory
      // 
      this.btnExportInventory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnExportInventory.BackColor = System.Drawing.Color.DarkOrange;
      this.btnExportInventory.Location = new System.Drawing.Point(903, 663);
      this.btnExportInventory.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.btnExportInventory.Menu = this.cmExportButton;
      this.btnExportInventory.Name = "btnExportInventory";
      this.btnExportInventory.Size = new System.Drawing.Size(232, 45);
      this.btnExportInventory.TabIndex = 23;
      this.btnExportInventory.Text = "Export inventory database";
      this.btnExportInventory.UseVisualStyleBackColor = false;
      // 
      // cmExportButton
      // 
      this.cmExportButton.ImageScalingSize = new System.Drawing.Size(24, 24);
      this.cmExportButton.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmStandardExport,
            this.cmVisioDataExport});
      this.cmExportButton.Name = "cmSaveButton";
      this.cmExportButton.Size = new System.Drawing.Size(326, 64);
      // 
      // cmStandardExport
      // 
      this.cmStandardExport.Name = "cmStandardExport";
      this.cmStandardExport.Size = new System.Drawing.Size(325, 30);
      this.cmStandardExport.Text = "Standard Export";
      this.cmStandardExport.ToolTipText = "Exports data in a format the best fits\r\nfor further processing";
      this.cmStandardExport.Click += new System.EventHandler(this.cmStandardExport_Click);
      // 
      // cmVisioDataExport
      // 
      this.cmVisioDataExport.Name = "cmVisioDataExport";
      this.cmVisioDataExport.Size = new System.Drawing.Size(325, 30);
      this.cmVisioDataExport.Text = "Export for Visio data binding";
      this.cmVisioDataExport.ToolTipText = "Export data in a format reauired by Visio Shape Data binding.\r\nSuitable to refres" +
    "h shape data already present in a drawing.";
      this.cmVisioDataExport.Click += new System.EventHandler(this.cmVisioDataExport_Click);
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(45, 22);
      this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(88, 20);
      this.label4.TabIndex = 21;
      this.label4.Text = "Device list :";
      // 
      // dataGridView1
      // 
      this.dataGridView1.AllowUserToAddRows = false;
      this.dataGridView1.AllowUserToDeleteRows = false;
      this.dataGridView1.AllowUserToResizeRows = false;
      this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.dataGridView1.AutoGenerateColumns = false;
      this.dataGridView1.BackgroundColor = System.Drawing.Color.WhiteSmoke;
      this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.nameDataGridViewTextBoxColumn1,
            this.iPAddressDataGridViewTextBoxColumn,
            this.typeDataGridViewTextBoxColumn,
            this.Platform,
            this.SystemSerial,
            this.L3InterfaceInformation,
            this.VLANInformation,
            this.InterfaceDescriptions});
      this.dataGridView1.DataSource = this.devicesBindingSource1;
      this.dataGridView1.Location = new System.Drawing.Point(45, 51);
      this.dataGridView1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.dataGridView1.Name = "dataGridView1";
      this.dataGridView1.ReadOnly = true;
      this.dataGridView1.RowHeadersVisible = false;
      this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
      this.dataGridView1.Size = new System.Drawing.Size(1090, 585);
      this.dataGridView1.TabIndex = 19;
      // 
      // Column1
      // 
      this.Column1.DataPropertyName = "ID";
      this.Column1.HeaderText = "ID";
      this.Column1.Name = "Column1";
      this.Column1.ReadOnly = true;
      // 
      // nameDataGridViewTextBoxColumn1
      // 
      this.nameDataGridViewTextBoxColumn1.DataPropertyName = "Name";
      this.nameDataGridViewTextBoxColumn1.HeaderText = "Name";
      this.nameDataGridViewTextBoxColumn1.Name = "nameDataGridViewTextBoxColumn1";
      this.nameDataGridViewTextBoxColumn1.ReadOnly = true;
      // 
      // iPAddressDataGridViewTextBoxColumn
      // 
      this.iPAddressDataGridViewTextBoxColumn.DataPropertyName = "IP_Address";
      this.iPAddressDataGridViewTextBoxColumn.HeaderText = "IP Address";
      this.iPAddressDataGridViewTextBoxColumn.Name = "iPAddressDataGridViewTextBoxColumn";
      this.iPAddressDataGridViewTextBoxColumn.ReadOnly = true;
      // 
      // typeDataGridViewTextBoxColumn
      // 
      this.typeDataGridViewTextBoxColumn.DataPropertyName = "Type";
      this.typeDataGridViewTextBoxColumn.HeaderText = "Type";
      this.typeDataGridViewTextBoxColumn.Name = "typeDataGridViewTextBoxColumn";
      this.typeDataGridViewTextBoxColumn.ReadOnly = true;
      // 
      // Platform
      // 
      this.Platform.DataPropertyName = "Platform";
      this.Platform.HeaderText = "Platform";
      this.Platform.Name = "Platform";
      this.Platform.ReadOnly = true;
      // 
      // SystemSerial
      // 
      this.SystemSerial.DataPropertyName = "SystemSerial";
      this.SystemSerial.HeaderText = "SystemSerial";
      this.SystemSerial.Name = "SystemSerial";
      this.SystemSerial.ReadOnly = true;
      // 
      // L3InterfaceInformation
      // 
      this.L3InterfaceInformation.DataPropertyName = "L3InterfaceInformation";
      this.L3InterfaceInformation.HeaderText = "L3InterfaceInformation";
      this.L3InterfaceInformation.Name = "L3InterfaceInformation";
      this.L3InterfaceInformation.ReadOnly = true;
      // 
      // VLANInformation
      // 
      this.VLANInformation.DataPropertyName = "VLANInformation";
      this.VLANInformation.HeaderText = "VLANInformation";
      this.VLANInformation.Name = "VLANInformation";
      this.VLANInformation.ReadOnly = true;
      // 
      // InterfaceDescriptions
      // 
      this.InterfaceDescriptions.DataPropertyName = "InterfaceDescriptions";
      this.InterfaceDescriptions.HeaderText = "InterfaceDescriptions";
      this.InterfaceDescriptions.Name = "InterfaceDescriptions";
      this.InterfaceDescriptions.ReadOnly = true;
      // 
      // devicesBindingSource1
      // 
      this.devicesBindingSource1.DataMember = "Devices";
      this.devicesBindingSource1.DataSource = this.InventoryDS;
      // 
      // InventoryDS
      // 
      this.InventoryDS.DataSetName = "InventoryDS";
      this.InventoryDS.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
      // 
      // btnSaveXML
      // 
      this.btnSaveXML.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.btnSaveXML.BackColor = System.Drawing.Color.SeaGreen;
      this.btnSaveXML.Location = new System.Drawing.Point(303, 663);
      this.btnSaveXML.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.btnSaveXML.Name = "btnSaveXML";
      this.btnSaveXML.Size = new System.Drawing.Size(232, 45);
      this.btnSaveXML.TabIndex = 18;
      this.btnSaveXML.Text = "Save inventory database";
      this.btnSaveXML.UseVisualStyleBackColor = false;
      this.btnSaveXML.Click += new System.EventHandler(this.btnSaveXML_Click);
      // 
      // btnLoadXML
      // 
      this.btnLoadXML.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.btnLoadXML.BackColor = System.Drawing.Color.SteelBlue;
      this.btnLoadXML.Location = new System.Drawing.Point(45, 663);
      this.btnLoadXML.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.btnLoadXML.Name = "btnLoadXML";
      this.btnLoadXML.Size = new System.Drawing.Size(232, 45);
      this.btnLoadXML.TabIndex = 17;
      this.btnLoadXML.Text = "Load inventory database";
      this.btnLoadXML.UseVisualStyleBackColor = false;
      this.btnLoadXML.Click += new System.EventHandler(this.btnLoadXML_Click);
      // 
      // tpNeighbors
      // 
      this.tpNeighbors.BackColor = System.Drawing.Color.Indigo;
      this.tpNeighbors.Controls.Add(this.dataGridView2);
      this.tpNeighbors.Location = new System.Drawing.Point(4, 29);
      this.tpNeighbors.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.tpNeighbors.Name = "tpNeighbors";
      this.tpNeighbors.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.tpNeighbors.Size = new System.Drawing.Size(1190, 752);
      this.tpNeighbors.TabIndex = 7;
      this.tpNeighbors.Text = "Device Neighbors";
      this.tpNeighbors.Paint += new System.Windows.Forms.PaintEventHandler(this.DrawGradientBackground);
      // 
      // dataGridView2
      // 
      this.dataGridView2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.dataGridView2.AutoGenerateColumns = false;
      this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridView2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.parentIDDataGridViewTextBoxColumn,
            this.nameDataGridViewTextBoxColumn,
            this.neighborIDDataGridViewTextBoxColumn,
            this.neighbourInterfaceDataGridViewTextBoxColumn,
            this.localInterfaceDataGridViewTextBoxColumn});
      this.dataGridView2.DataSource = this.neighboursBindingSource;
      this.dataGridView2.Location = new System.Drawing.Point(22, 25);
      this.dataGridView2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.dataGridView2.Name = "dataGridView2";
      this.dataGridView2.Size = new System.Drawing.Size(1136, 689);
      this.dataGridView2.TabIndex = 0;
      // 
      // parentIDDataGridViewTextBoxColumn
      // 
      this.parentIDDataGridViewTextBoxColumn.DataPropertyName = "Parent_ID";
      this.parentIDDataGridViewTextBoxColumn.HeaderText = "Parent_ID";
      this.parentIDDataGridViewTextBoxColumn.Name = "parentIDDataGridViewTextBoxColumn";
      // 
      // nameDataGridViewTextBoxColumn
      // 
      this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
      this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
      this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
      // 
      // neighborIDDataGridViewTextBoxColumn
      // 
      this.neighborIDDataGridViewTextBoxColumn.DataPropertyName = "Neighbor_ID";
      this.neighborIDDataGridViewTextBoxColumn.HeaderText = "Neighbor_ID";
      this.neighborIDDataGridViewTextBoxColumn.Name = "neighborIDDataGridViewTextBoxColumn";
      // 
      // neighbourInterfaceDataGridViewTextBoxColumn
      // 
      this.neighbourInterfaceDataGridViewTextBoxColumn.DataPropertyName = "Neighbour_Interface";
      this.neighbourInterfaceDataGridViewTextBoxColumn.HeaderText = "Neighbour_Interface";
      this.neighbourInterfaceDataGridViewTextBoxColumn.Name = "neighbourInterfaceDataGridViewTextBoxColumn";
      // 
      // localInterfaceDataGridViewTextBoxColumn
      // 
      this.localInterfaceDataGridViewTextBoxColumn.DataPropertyName = "Local_Interface";
      this.localInterfaceDataGridViewTextBoxColumn.HeaderText = "Local_Interface";
      this.localInterfaceDataGridViewTextBoxColumn.Name = "localInterfaceDataGridViewTextBoxColumn";
      // 
      // neighboursBindingSource
      // 
      this.neighboursBindingSource.DataMember = "Neighbours";
      this.neighboursBindingSource.DataSource = this.InventoryDS;
      // 
      // tpMSAGL
      // 
      this.tpMSAGL.BackColor = System.Drawing.Color.DarkGray;
      this.tpMSAGL.Controls.Add(this.btnCreateGraph);
      this.tpMSAGL.Controls.Add(this.gViewer1);
      this.tpMSAGL.Location = new System.Drawing.Point(4, 29);
      this.tpMSAGL.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.tpMSAGL.Name = "tpMSAGL";
      this.tpMSAGL.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.tpMSAGL.Size = new System.Drawing.Size(1190, 752);
      this.tpMSAGL.TabIndex = 3;
      this.tpMSAGL.Text = "Graf pre-view";
      // 
      // btnCreateGraph
      // 
      this.btnCreateGraph.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnCreateGraph.BackColor = System.Drawing.Color.SteelBlue;
      this.btnCreateGraph.Location = new System.Drawing.Point(928, 691);
      this.btnCreateGraph.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.btnCreateGraph.Name = "btnCreateGraph";
      this.btnCreateGraph.Size = new System.Drawing.Size(249, 45);
      this.btnCreateGraph.TabIndex = 4;
      this.btnCreateGraph.Text = "Create MSAGL Graph";
      this.btnCreateGraph.UseVisualStyleBackColor = false;
      this.btnCreateGraph.Click += new System.EventHandler(this.btnCreateGraph_Click);
      // 
      // gViewer1
      // 
      this.gViewer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.gViewer1.AsyncLayout = false;
      this.gViewer1.AutoScroll = true;
      this.gViewer1.AutoSize = true;
      this.gViewer1.BackColor = System.Drawing.Color.Transparent;
      this.gViewer1.BackwardEnabled = false;
      this.gViewer1.BuildHitTree = true;
      this.gViewer1.CurrentLayoutMethod = Microsoft.Msagl.GraphViewerGdi.LayoutMethod.SugiyamaScheme;
      this.gViewer1.ForwardEnabled = false;
      this.gViewer1.Graph = null;
      this.gViewer1.LayoutAlgorithmSettingsButtonVisible = true;
      this.gViewer1.LayoutEditingEnabled = true;
      this.gViewer1.Location = new System.Drawing.Point(9, 9);
      this.gViewer1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.gViewer1.MouseHitDistance = 0.05D;
      this.gViewer1.Name = "gViewer1";
      this.gViewer1.NavigationVisible = true;
      this.gViewer1.NeedToCalculateLayout = true;
      this.gViewer1.PanButtonPressed = false;
      this.gViewer1.SaveAsImageEnabled = true;
      this.gViewer1.SaveAsMsaglEnabled = true;
      this.gViewer1.SaveButtonVisible = true;
      this.gViewer1.SaveGraphButtonVisible = true;
      this.gViewer1.SaveInVectorFormatEnabled = true;
      this.gViewer1.Size = new System.Drawing.Size(1168, 672);
      this.gViewer1.TabIndex = 6;
      this.gViewer1.ToolBarIsVisible = true;
      this.gViewer1.ZoomF = 1D;
      this.gViewer1.ZoomWindowThreshold = 0.05D;
      // 
      // devicesBindingSource
      // 
      this.devicesBindingSource.DataMember = "Devices";
      this.devicesBindingSource.DataSource = this.dataSet11BindingSource;
      // 
      // dataSet11BindingSource
      // 
      this.dataSet11BindingSource.DataSource = this.InventoryDS;
      this.dataSet11BindingSource.Position = 0;
      // 
      // openFileDialog1
      // 
      this.openFileDialog1.FileName = "openFileDialog1";
      // 
      // openFileDialog2
      // 
      this.openFileDialog2.FileName = "openFileDialog2";
      // 
      // openFileDialog3
      // 
      this.openFileDialog3.FileName = "openFileDialog3";
      // 
      // stencilsBindingSource
      // 
      this.stencilsBindingSource.DataMember = "Stencils";
      this.stencilsBindingSource.DataSource = this.StencilsDataSet;
      // 
      // CDP2VisioManager
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.SystemColors.ActiveCaption;
      this.ClientSize = new System.Drawing.Size(1248, 818);
      this.Controls.Add(this.Display);
      this.DoubleBuffered = true;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.MinimumSize = new System.Drawing.Size(1261, 848);
      this.Name = "CDP2VisioManager";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "CDP Network Discovery Module";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CDP2VisioManager_FormClosing);
      this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.PGTtoVisio_FormClosed);
      this.Load += new System.EventHandler(this.PGTtoVisio_Load);
      this.Display.ResumeLayout(false);
      this.tpInput.ResumeLayout(false);
      this.groupBox1.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.dgStencils)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.stencilsBindingSource1)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.StencilsDataSet)).EndInit();
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      this.groupBox3.ResumeLayout(false);
      this.groupBox3.PerformLayout();
      this.tpDiscoveryDomain.ResumeLayout(false);
      this.tpDiscoveryDomain.PerformLayout();
      this.splitContainer2.Panel1.ResumeLayout(false);
      this.splitContainer2.Panel1.PerformLayout();
      this.splitContainer2.Panel2.ResumeLayout(false);
      this.splitContainer2.Panel2.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
      this.splitContainer2.ResumeLayout(false);
      this.panel3.ResumeLayout(false);
      this.panel3.PerformLayout();
      this.panel4.ResumeLayout(false);
      this.panel4.PerformLayout();
      this.tpDatabaseView.ResumeLayout(false);
      this.tpDatabaseView.PerformLayout();
      this.cmExportButton.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.devicesBindingSource1)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.InventoryDS)).EndInit();
      this.tpNeighbors.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.neighboursBindingSource)).EndInit();
      this.tpMSAGL.ResumeLayout(false);
      this.tpMSAGL.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.devicesBindingSource)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.dataSet11BindingSource)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.stencilsBindingSource)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private CDPDataSet InventoryDS;
    private System.Windows.Forms.TabControl Display;
    private System.Windows.Forms.TabPage tpDatabaseView;
    private System.Windows.Forms.Button btnSaveXML;
    private System.Windows.Forms.Button btnLoadXML;
    private System.Windows.Forms.OpenFileDialog openFileDialog1;
    private System.Windows.Forms.TabPage tpMSAGL;
    private System.Windows.Forms.Button btnCreateGraph;
    private Microsoft.Msagl.GraphViewerGdi.GViewer gViewer1;
    private System.Windows.Forms.OpenFileDialog openFileDialog2;
    private System.Windows.Forms.OpenFileDialog openFileDialog3;
    private System.Windows.Forms.BindingSource neighboursBindingSource;
    private System.Windows.Forms.DataGridView dataGridView1;
    private System.Windows.Forms.BindingSource devicesBindingSource;
    private System.Windows.Forms.BindingSource dataSet11BindingSource;
    //private System.Windows.Forms.DataGridViewTextBoxColumn shinvPIDDataGridViewTextBoxColumn;
    //private System.Windows.Forms.DataGridViewTextBoxColumn stencilNameDataGridViewTextBoxColumn;
    //private System.Windows.Forms.DataGridViewTextBoxColumn masterNameUDataGridViewTextBoxColumn;
    private StencilsDS StencilsDataSet;
    private System.Windows.Forms.BindingSource stencilsBindingSource;
    private System.Windows.Forms.BindingSource stencilsBindingSource1;
    private System.Windows.Forms.BindingSource devicesBindingSource1;
    private System.Windows.Forms.TabPage tpInput;
    private System.Windows.Forms.Button btn_CreateInventory;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.ComboBox cbxProtocols;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.ComboBox cbxJumpServers;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.TextBox tb_Devices;
    private System.Windows.Forms.ComboBox cbxDocumentSize;
    private System.Windows.Forms.Button btnCreateVisio;
    private System.Windows.Forms.Button button_add_new_stencil;
    private System.Windows.Forms.DataGridView dgStencils;
    private System.Windows.Forms.Label label8;
    private System.Windows.Forms.Label label9;
    private System.Windows.Forms.ComboBox cbxGraphAlg;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.GroupBox groupBox2;
    private System.Windows.Forms.GroupBox groupBox3;
    private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
    private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
    private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
    private System.Windows.Forms.DataGridViewTextBoxColumn cDPIPDataGridViewTextBoxColumn;
    private System.Windows.Forms.DataGridViewTextBoxColumn platformDataGridViewTextBoxColumn;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Button btnClearDeviceList;
    private System.Windows.Forms.Button btnAddNetwork;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox tbNetwork;
    private System.Windows.Forms.DataGridViewTextBoxColumn showinvDataGridViewTextBoxColumn;
    private System.Windows.Forms.DataGridViewTextBoxColumn showverDataGridViewTextBoxColumn;
    private PGT.Common.MenuButton btnExportInventory;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.ComboBox cbxConnectorType;
    private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
    private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn1;
    private System.Windows.Forms.DataGridViewTextBoxColumn iPAddressDataGridViewTextBoxColumn;
    private System.Windows.Forms.DataGridViewTextBoxColumn typeDataGridViewTextBoxColumn;
    private System.Windows.Forms.DataGridViewTextBoxColumn Platform;
    private System.Windows.Forms.DataGridViewTextBoxColumn SystemSerial;
    private System.Windows.Forms.DataGridViewTextBoxColumn L3InterfaceInformation;
    private System.Windows.Forms.DataGridViewTextBoxColumn VLANInformation;
    private System.Windows.Forms.DataGridViewTextBoxColumn InterfaceDescriptions;
    private System.Windows.Forms.CheckBox cbBindDeviceData;
    private System.Windows.Forms.CheckBox cbBindVLANData;
    private System.Windows.Forms.CheckBox cbBindInterfaces;
    private System.Windows.Forms.CheckBox cbBindInventoryData;
    private System.Windows.Forms.Button btnOutputPath;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.TextBox tbOutputPath;
    private System.Windows.Forms.ToolTip toolTip1;
    private System.Windows.Forms.ContextMenuStrip cmExportButton;
    private System.Windows.Forms.ToolStripMenuItem cmStandardExport;
    private System.Windows.Forms.ToolStripMenuItem cmVisioDataExport;
    private System.Windows.Forms.Label label10;
    private System.Windows.Forms.TabPage tpDiscoveryDomain;
    private System.Windows.Forms.SplitContainer splitContainer2;
    private System.Windows.Forms.Panel panel3;
    private System.Windows.Forms.Label label11;
    private System.Windows.Forms.TextBox tbIPRangesIncluded;
    private System.Windows.Forms.TextBox tbIPRangesExcluded;
    private System.Windows.Forms.Panel panel4;
    private System.Windows.Forms.Label label12;
    private System.Windows.Forms.Button btnSelectTemplateDrawing;
    private System.Windows.Forms.Label label13;
    private System.Windows.Forms.TextBox tbTemplateDrawing;
    private System.Windows.Forms.CheckBox cbStartScript;
    private System.Windows.Forms.CheckBox cbActiveDiscovery;
    private System.Windows.Forms.TabPage tpNeighbors;
    private System.Windows.Forms.DataGridView dataGridView2;
    private System.Windows.Forms.DataGridViewTextBoxColumn parentIDDataGridViewTextBoxColumn;
    private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
    private System.Windows.Forms.DataGridViewTextBoxColumn neighborIDDataGridViewTextBoxColumn;
    private System.Windows.Forms.DataGridViewTextBoxColumn neighbourInterfaceDataGridViewTextBoxColumn;
    private System.Windows.Forms.DataGridViewTextBoxColumn localInterfaceDataGridViewTextBoxColumn;
    private System.Windows.Forms.Button btnPickFavHosts;
  }
}

