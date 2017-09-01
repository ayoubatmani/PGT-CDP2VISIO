/*
 * #########################################################################*/
/* #                                                                       #*/
/* #  This file is part of CDP2VISIO project, which is written             #*/
/* #  as a PGT plug-in to help network inventory and Visio drawing         #*/
/* #  creation based on CDP protocol discovery on Cisco IOS routers.       #*/
/* #                                                                       #*/
/* #  You may not use this file except in compliance with the license.     #*/
/* #                                                                       #*/
/* #  Copyright Laszlo Frank (c) 2014-2017                                 #*/
/* #                                                                       #*/
/* #########################################################################*/


using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.Mds;
using NetOffice.ExcelApi.Enums;
using PGT.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Excel = NetOffice.ExcelApi;
using Visio = NetOffice.VisioApi;


namespace CDP2VISIO
{
  public partial class CDP2VisioManager : Form
  {
    #region Fields
    /// <summary>
    /// The list of Visio Devices represented as Visio_Device class
    /// </summary>
    public List<Edge> edges;
    public List<Visio_Device> visio_devices_list = new List<Visio_Device>();
    /// <summary>
    /// Used to remember Button back colors
    /// </summary>
    private Hashtable ButtonColors = new Hashtable();
    /// <summary>
    /// The list of Graph edges
    /// </summary>
    int node_count = 0;

    private const string defaultShapeName = "Router";
    private const string DefaultStencilName = "PGTVisioShapes.vss";
    private bool load_db = false;
    private bool msalg = false;
    private BackgroundWorker workInProgress;
    private string workInProgressText = "Work in progress...";
    private string workInProgressCaption = "Please be patient";

    /// <summary>
    /// The file name used by this instance of CDP2VISIO engine. The unique name is generated from the ScriptingEngine EngineID
    /// when CreateInventory() is called;
    /// </summary>
    private string InventoryFileName = "";

    /// <summary>
    /// Indicates whether the inventory has been saved by user
    /// </summary>
    private bool InventorySaved = true;

    #endregion

    #region Constructors
    public CDP2VisioManager()
    {
      InitializeComponent();
      if (DebugEx.DebugLevelThreshold < DebugLevel.Full)
      {
        Display.TabPages.Remove(tpNeighbors);
        Display.TabPages.Remove(tpMSAGL);
      }
      workInProgress = new BackgroundWorker();
      workInProgress.DoWork += workInProgress_DoWork;
      workInProgress.WorkerSupportsCancellation = true;
      SetControlBackGround(this);
      cbxJumpServers.DataSource = PGT.Common.JumpServersManager.GetJumpServersAddress();
      this.Text += string.Format(" - v{0}", Assembly.GetExecutingAssembly().GetName().Version.ToString());
    }

    private void SetControlBackGround(Control rootControl)
    {
      if (rootControl != null)
      {
        foreach (Control thisControl in rootControl.Controls)
        {
          if (thisControl is System.Windows.Forms.Label || thisControl is CheckBox || /*thisControl is Panel ||*/ thisControl is SplitContainer /*||  thisControl is SplitterPanel*/)
          {
            thisControl.BackColor = System.Drawing.Color.Transparent;
            MethodInfo m = thisControl.GetType().GetMethod("SetStyle");
            if (m != null) m.Invoke(thisControl, new object[] { ControlStyles.SupportsTransparentBackColor, true });
          }
          SetControlBackGround(thisControl);
        }
      }

    }

    private void workInProgress_DoWork(object sender, DoWorkEventArgs e)
    {
      WorkInProgressAnimation L = null;
      DateTime waitStartedAt = DateTime.Now;
      while (true)
      {
        System.Threading.Thread.Sleep(100);
        if ((DateTime.Now - waitStartedAt).TotalSeconds > 2)
        {
          if (L == null)
          {
            L = new WorkInProgressAnimation(workInProgressText, workInProgressCaption);
            L.Show();
          }
        }
        System.Windows.Forms.Application.DoEvents();
        if (workInProgress.CancellationPending)
        {
          if (L != null) L.Close();
          break;
        }
      }
    }
    #endregion

    #region Private members

    private void CalculateMSALG()
    {
      InitGViewer();
      Graph tree = new Graph();
      edges = new List<Edge>(30);

      #region CalculateMSALGNodes

      var query_all_nodes = from row in InventoryDS.Devices
                            where (row.ID != 0)
                            select row;
      foreach (var node in query_all_nodes)
      {
        Node msalg_node = (Node)tree.AddNode(node.ID.ToString());
      }

      #endregion

      #region CalculateMASLGEdges
      var neighbours = from row in InventoryDS.Neighbours select row;

      foreach (var thisMeighbour in neighbours)
      {
        bool edge_check = true;
        foreach (var edge in edges)
        {
          if ((thisMeighbour.Parent_ID == edge.Source_ID) & (thisMeighbour.Neighbor_ID == edge.Target_ID)) edge_check = false;
          if ((thisMeighbour.Parent_ID == edge.Target_ID) & (thisMeighbour.Neighbor_ID == edge.Source_ID)) edge_check = false;
          if (!edge_check) break;
        }
        if (edge_check)
        {
          Edge new_edge = new Edge();
          new_edge.Source_ID = thisMeighbour.Parent_ID;
          new_edge.Target_ID = thisMeighbour.Neighbor_ID;
          new_edge.CalculateEdge(InventoryDS);
          edges.Add(new_edge);
          Microsoft.Msagl.Drawing.Edge e = tree.AddEdge(thisMeighbour.Parent_ID.ToString(), thisMeighbour.Neighbor_ID.ToString());
        }
      }
      #endregion

      #region CreateMSALGGraph

      if (cbxGraphAlg.Text == "StarTopology")
      {
        tree.LayoutAlgorithmSettings = new MdsLayoutSettings();
        gViewer1.CurrentLayoutMethod = Microsoft.Msagl.GraphViewerGdi.LayoutMethod.MDS;
      }


      gViewer1.Graph = tree;
      foreach (var ed in tree.GeometryGraph.Edges) ed.CreateSimpleEdgeCurve();
      gViewer1.Graph = tree;
      gViewer1.LayoutEngine.Layout(gViewer1, new LayoutEventArgs(gViewer1, "curves"));
      #endregion

      #region CalculateVisioDevicesCoordinates
      node_count = tree.NodeCount;
      int graph_height = (int)tree.Height;
      int graph_width = (int)tree.Width;

      var devices = from row in InventoryDS.Devices select row;

      #region Calculate Shape Shifting Parameters
      double shift_X = 0;
      double shift_Y = 0;

      if (cbxGraphAlg.Text == "StarTopology")
      {
        double lowest_x = 10000;
        double lowest_y = 10000;
        foreach (var node in devices)
        {
          if (lowest_x > tree.FindNode(node.ID.ToString()).BoundingBox.Center.X) lowest_x = tree.FindNode(node.ID.ToString()).BoundingBox.Center.X;
          if (lowest_y > tree.FindNode(node.ID.ToString()).BoundingBox.Center.Y) lowest_y = tree.FindNode(node.ID.ToString()).BoundingBox.Center.Y;
        }
        if ((0 - lowest_x) > 0) shift_X = Math.Abs(lowest_x);
        if ((0 - lowest_y) > 0) shift_Y = Math.Abs(lowest_y);
      }
      else
      {
        shift_X = 10;
        shift_Y = -500;
      }
      #endregion

      foreach (var thisDevice in devices)
      {
        double actual_x = tree.FindNode(thisDevice.ID.ToString()).BoundingBox.Center.X + shift_X;
        double actual_y = tree.FindNode(thisDevice.ID.ToString()).BoundingBox.Center.Y + shift_Y;
        Visio_Device actual_device = new Visio_Device
        {
          FullName = thisDevice.Name,
          DeviceID = thisDevice.ID,
          Label = DottedNameSpace.TLD(thisDevice.Name),
          IPAddress = thisDevice.IP_Address,
          Platform = thisDevice.Platform,
          VersionInfo = thisDevice.VersionInfo,
          Inventory = thisDevice.Inventory,
          SystemSerial = thisDevice.SystemSerial,
          VLANInformation = thisDevice.VLANInformation,
          L3InterfaceInformation = thisDevice.L3InterfaceInformation,
          DeviceType = thisDevice.Type,
          X_coord = actual_x / graph_width,
          Y_coord = actual_y / graph_height
        };

        #region Set Device Stencil Type

        if (thisDevice.Inventory != string.Empty)
        {
          var foundStencil = from stencil in StencilsDataSet.Stencils where stencil.sh_inv_PID.ToLowerInvariant() == thisDevice.Platform.ToLowerInvariant() select stencil;
          if (foundStencil.Count() > 0)
          {
            StencilsDS.StencilsRow thisStencil = foundStencil.ElementAt(0);
            actual_device.stencilName = thisStencil.stencilName;
            actual_device.masterNameU = thisStencil.masterNameU;
          }
          else
          {
            actual_device.stencilName = PGT.Common.Helper.GetWorkingDirectory() + Options.Default.VisioStencilsPath + DefaultStencilName;
            string platform = thisDevice.Platform.ToLowerInvariant();
            if (platform.IndexOf("vmware esx") >= 0) actual_device.masterNameU = "ESX";
            else
            {
              string dt = thisDevice.Type.ToLowerInvariant();
              if (dt.IndexOf("router") >= 0 && dt.IndexOf("switch") >= 0) actual_device.masterNameU = "L3Switch";
              else if (dt.IndexOf("router") >= 0) actual_device.masterNameU = "Router";
              else if (dt.IndexOf("switch") >= 0) actual_device.masterNameU = "Switch";
              else if (dt.IndexOf("trans-bridge") >= 0 && thisDevice.Platform.IndexOf("AIR") >= 0) actual_device.masterNameU = "AccessPoint";
              else if (dt.IndexOf("trans-bridge") >= 0) actual_device.masterNameU = "Bridge";
              else if (dt.IndexOf("host") >= 0) actual_device.masterNameU = "Host";
              else if (dt.IndexOf("phone") >= 0) actual_device.masterNameU = "Phone";
              else if (dt == "asa") actual_device.masterNameU = "Firewall";
              else if (dt == "vss") actual_device.masterNameU = "VSS";
              else if (dt.StartsWith("stack"))
              {
                if (dt == "stack2") actual_device.masterNameU = "Stack2";
                else if (dt == "stack3") actual_device.masterNameU = "Stack3";
                else actual_device.masterNameU = "StackN";
              }
              else actual_device.masterNameU = "Router";
            }
          }
        }

        #endregion

        visio_devices_list.Add(actual_device);
      }
      #endregion
    }

    internal void InternalCreateVisioDoc(object state)
    {
      try
      {
        #region Initialize Visio data binding
        bool bindDeviceData = false;
        Visio.IVDataRecordset deviceRecordset = null;
        bool bindVLANData = false;
        Visio.IVDataRecordset vlanRecordset = null;
        bool bindInterfaceData = false;
        Visio.IVDataRecordset ifRecordset = null;
        bool bindInventpryData = false;
        Visio.IVDataRecordset invRecordset = null;
        this.Invoke(new Action(() =>
        {
          bindDeviceData = cbBindDeviceData.Checked;
          bindVLANData = cbBindVLANData.Checked;
          bindInterfaceData = cbBindInterfaces.Checked;
          bindInventpryData = cbBindInventoryData.Checked;
        }));
        DateTime now = DateTime.Now;
        string inventoryExcelFile = string.Format("{7}\\VisioShapeData_{0}{1}{2}_{3}{4}{5}{6}.xlsx", now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, now.Millisecond, tbOutputPath.Text);
        //try
        //{
        //  File.Create(inventoryExcelFile);
        //  File.Delete(inventoryExcelFile);
        //}
        //catch (Exception Ex)
        //{
        //  MessageBox.Show(string.Format("Can't continue operation due to an error occurred while creating the file {0}. The error is : {1}", inventoryExcelFile, Ex.Message), "IO Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //  return;
        //}
        string visioFileName = string.Format("{7}\\VisioDrawing_{0}{1}{2}_{3}{4}{5}{6}.vsdx", now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, now.Millisecond, tbOutputPath.Text);
        //try
        //{
        //  File.Create(visioFileName);
        //  File.Delete(visioFileName);
        //}
        //catch (Exception Ex)
        //{
        //  MessageBox.Show(string.Format("Can't continue operation due to an error occurred while creating the file {0}. The error is : {1}", visioFileName, Ex.Message), "IO Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //  return;
        //}
        #endregion

        #region Create Visio Document
        Visio.Application VApp = new Visio.Application();
        // open template drawing if selected
        Visio.IVDocument _templateDrawing = !string.IsNullOrEmpty(tbTemplateDrawing.Text) && File.Exists(tbTemplateDrawing.Text) ? VApp.Documents.Open(tbTemplateDrawing.Text) : null;
        // create new drawing sheet
        string templateName = "";
        string docSize = "A4";
        cbxDocumentSize.Invoke(new MethodInvoker(delegate () { docSize = cbxDocumentSize.Text; }));
        switch (docSize)
        {
          case "A4": templateName += Options.Default.VisioA4Template; break;
          case "A3": templateName += Options.Default.VisioA3Template; break;
          case "A2": templateName += Options.Default.VisioA2Template; break;
        }
        if (!File.Exists(templateName))
        {
          templateName = System.IO.Path.GetTempFileName();
          switch (docSize)
          {
            case "A4": File.WriteAllBytes(templateName, Resources.Resources.VisioA4Template); break;
            case "A3": File.WriteAllBytes(templateName, Resources.Resources.VisioA3Template); break;
            case "A2": File.WriteAllBytes(templateName, Resources.Resources.VisioA2Template); break;
          }
        }
        Visio.IVDocument newDocument = VApp.Documents.Add(templateName);
        #endregion

        #region Create inventory Excel file and link the data
        if (bindDeviceData || bindVLANData || bindInterfaceData || bindInventpryData)
        {

          #region Create inventory Excel file
          GenerateExcelInventory(false, false, inventoryExcelFile);
          #endregion

          DebugEx.WriteLine("Linking VISIO data to inventory file " + inventoryExcelFile, DebugLevel.Informational);
          string connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;User ID=Admin;Data Source={0};" +
            "Mode=Read;Extended Properties=\"HDR = YES; IMEX = 1; MaxScanRows = 0; Excel 12.0;\";Jet OLEDB:System database=\"\";" +
            "Jet OLEDB:Registry Path=\"\";Jet OLEDB:Engine Type=37;Jet OLEDB:Database Locking Mode=0;Jet OLEDB:Global Partial Bulk Ops=2;" +
            "Jet OLEDB:Global Bulk Transactions=1;Jet OLEDB:New Database Password=\"\";Jet OLEDB:Create System Database=False;" +
            "Jet OLEDB:Encrypt Database=False;Jet OLEDB:Don&apos;t Copy Locale on Compact=False;Jet OLEDB:Compact Without Replica Repair=False;" +
            "Jet OLEDB:SFP=False;Jet OLEDB:Support Complex Data=False;Jet OLEDB:Bypass UserInfo Validation=False;Jet OLEDB:Limited DB Caching=False;" +
            "Jet OLEDB:Bypass ChoiceField Validation=False;", inventoryExcelFile);
          if (bindDeviceData)
          {
            string command = "SELECT * FROM [Devices$]";
            deviceRecordset = VApp.ActiveDocument.DataRecordsets.Add(connectionString, command, 0, "Device");
          }
          if (bindVLANData)
          {
            string command = "SELECT * FROM [VLAN Information$]";
            vlanRecordset = VApp.ActiveDocument.DataRecordsets.Add(connectionString, command, 0, "VLAN");
          }
          if (bindInterfaceData)
          {
            string command = "SELECT * FROM [Interfaces$]";
            ifRecordset = VApp.ActiveDocument.DataRecordsets.Add(connectionString, command, 0, "Interfaces");
          }
          if (bindInventpryData)
          {
            string command = "SELECT * FROM [Inventory$]";
            invRecordset = VApp.ActiveDocument.DataRecordsets.Add(connectionString, command, 0, "Inventory");
          }
        }
        #endregion

        #region SetShapePropertes

        int devicenumber = node_count;
        foreach (var member in visio_devices_list)
        {
          try
          {
            DebugEx.WriteLine(string.Format("Pacing shape {0}", member.FullName), DebugLevel.Informational);
            var devices = from row in InventoryDS.Devices.AsEnumerable() where (row.Name == member.FullName) select row;
            if (devices.Count() == 1)
            {
              Visio.IVPage _templtePage = _templateDrawing?.Pages.First();
              Visio.IVShape _templateShape = _templtePage?.Shapes.FirstOrDefault(shape => shape.Text == member.Label);
              if (_templateShape != null)
              {
                // update member coordinates to match the found shape's
                double X_coord = _templateShape.Cells("PinX").ResultIU;
                double Y_coord = _templateShape.Cells("PinY").ResultIU;
                double Sheet_Width = _templtePage.PageSheet.get_CellsU("PageWidth").ResultIU - 3;
                double Sheet_Height = _templtePage.PageSheet.get_CellsU("PageHeight").ResultIU - 3;
                member.X_coord = (X_coord - 2) / Sheet_Width;
                member.Y_coord = (Y_coord - 2) / Sheet_Height;
              }
              member.CreateShape(VApp, newDocument);
              member.SetShapeProperties(devicenumber, devices.ElementAt(0).VersionInfo);
              if (bindDeviceData)
              {
                int deviceDataRowID = deviceRecordset.GetDataRowIDs(string.Format("ID='{0}'", member.DeviceID))[0];
                member.LinkShapeToData(deviceRecordset.ID, deviceDataRowID);
              }
              if (bindVLANData)
              {
                int vlanDataRowID = vlanRecordset.GetDataRowIDs(string.Format("ID='{0}'", member.DeviceID))[0];
                member.LinkShapeToData(vlanRecordset.ID, vlanDataRowID);
              }
              if (bindInterfaceData)
              {
                int ifDataRowID = ifRecordset.GetDataRowIDs(string.Format("ID='{0}'", member.DeviceID))[0];
                member.LinkShapeToData(ifRecordset.ID, ifDataRowID);
              }
              if (bindInventpryData)
              {
                int invDataRowID = invRecordset.GetDataRowIDs(string.Format("ID='{0}'", member.DeviceID))[0];
                member.LinkShapeToData(invRecordset.ID, invDataRowID);
              }

            }
            else MessageBox.Show(string.Format("The drawing may be incomplete as multiple shapes found with the same name : {0}", member.FullName), "Error creating Visio document", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          }
          catch (Exception Ex)
          {
            DebugEx.WriteLine("CDP2VISIO Error while setting shape properties : " + Ex.InnerExceptionsMessage());
          }
        }
        _templateDrawing?.Close();

        #endregion

        #region Connect shapes
        string connType = "Straight Lines";
        this.Invoke(new Action(delegate () { connType = cbxConnectorType.Text; }));
        foreach (var edge in edges)
        {
          try
          {
            Visio_Device device1 = visio_devices_list.Find(s => s.FullName == edge.SourceName && s.Visio_shape != null);
            Visio_Device device2 = visio_devices_list.Find(s => s.FullName == edge.TargetName && s.Visio_shape != null);
            DebugEx.WriteLine(string.Format("Connecting {0} to {1}", device1?.FullName, device2?.FullName), DebugLevel.Informational);

            VisioInterface.ConnectWithDynamicGlueAndConnector(devicenumber, device1?.Visio_shape, device2?.Visio_shape, edge.ConnectionText, edge.ConnectionConfig, edge.line_color, edge.line_weight, edge.TextSize, connType);
          }
          catch (Exception Ex)
          {
            DebugEx.WriteLine("CDP2VISIO Error while connecting shapes : " + Ex.InnerExceptionsMessage());
          }
        }

        #endregion

        #region BrigtoFrontallDevices
        foreach (var member in visio_devices_list)
        {
          try
          {
            DebugEx.WriteLine(string.Format("Bringing shape {0} to front", member.FullName), DebugLevel.Informational);
            VisioInterface.BrigtoFront(member.Visio_shape);
          }
          catch (Exception Ex)
          {
            DebugEx.WriteLine("CDP2VISIO Error in BrigtoFrontAllDevices : " + Ex.InnerExceptionsMessage());
          }
        }
        #endregion

        #region Save Visio Doc
        VApp.ActiveDocument.SaveAs(visioFileName);
        //VApp.ActiveDocument.Close(); //close visio doc
        #endregion

        MessageBox.Show("Finished creating Visio document", "Processing finished", MessageBoxButtons.OK, MessageBoxIcon.Information);
      }
      catch (Exception Ex)
      {
        string msg = string.Format("Unfortunately an unexpected exception occurred while processing data : {0}", Ex.Message);
        msg += Ex.InnerException?.Message ?? "";
        MessageBox.Show(msg, "Error creating Visio document", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }
    private void InitGViewer()
    {
      Point curLocation = gViewer1.Location;
      System.Drawing.Size curSize = new System.Drawing.Size(gViewer1.Size.Width, gViewer1.Size.Height);
      this.tpMSAGL.Controls.Remove(this.gViewer1);
      gViewer1 = new Microsoft.Msagl.GraphViewerGdi.GViewer();
      gViewer1.Location = curLocation;
      gViewer1.Size = curSize;
      gViewer1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;
      this.gViewer1.AsyncLayout = false;
      this.gViewer1.AutoScroll = true;
      this.gViewer1.BackColor = System.Drawing.Color.Transparent;
      this.gViewer1.BackwardEnabled = false;
      this.gViewer1.BuildHitTree = true;
      this.gViewer1.CurrentLayoutMethod = Microsoft.Msagl.GraphViewerGdi.LayoutMethod.SugiyamaScheme;
      this.gViewer1.ForwardEnabled = false;
      this.gViewer1.Graph = null;
      this.gViewer1.LayoutAlgorithmSettingsButtonVisible = true;
      this.gViewer1.LayoutEditingEnabled = true;
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
      this.gViewer1.TabIndex = 6;
      this.gViewer1.ToolBarIsVisible = true;
      this.gViewer1.ZoomF = 1D;
      this.gViewer1.ZoomFraction = 0.5D;
      this.gViewer1.ZoomWindowThreshold = 0.05D;
      this.tpMSAGL.Controls.Add(this.gViewer1);
    }

    /// <summary>
    /// Checks whether default stencil is present and creates it from resource if not
    /// </summary>
    private void InitStencils()
    {
      bool FileParsed = false;
      string StencilsDBFileName = PGT.Common.Helper.GetWorkingDirectory() + Options.Default.VisioStencilsPath + Options.Default.StencilsDBName;
      if (File.Exists(StencilsDBFileName))
      {
        try
        {
          StencilsDataSet.ReadXml(StencilsDBFileName);
          #region Check Default Settings
          var query_settings = from row in StencilsDataSet.Stencils
                               select row;
          bool add_default = true;
          foreach (var member in query_settings)
          {
            if (member.sh_inv_PID == "3750G")
            {
              if (!File.Exists(member.stencilName))
              {
                // The stencil does not exist, lets create it from embedded resource
                try
                {
                  member.stencilName = PGT.Common.Helper.GetWorkingDirectory() + Options.Default.VisioStencilsPath + DefaultStencilName;
                  if (!Directory.Exists(System.IO.Path.GetDirectoryName(member.stencilName))) Directory.CreateDirectory(System.IO.Path.GetDirectoryName(member.stencilName));
                  File.WriteAllBytes(member.stencilName, Resources.Resources.PGTVisioShapes);
                  StencilsDataSet.WriteXml(StencilsDBFileName);
                  add_default = false;
                }
                catch (Exception Ex)
                {
                  MessageBox.Show(string.Format("Could not find or create stencil : {0}. Error : {1}", member.stencilName, Ex.Message), "Visio stencil missing", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
              }
              else add_default = false;
            }
          }

          if (add_default)
          {
            StencilsDS.StencilsRow new_row = StencilsDataSet.Stencils.NewStencilsRow();
            string StencilFileName = PGT.Common.Helper.GetWorkingDirectory() + Options.Default.VisioStencilsPath + DefaultStencilName;
            // It is better to always overwrite to ensure the correct stencil is used when version is changed
            //if (!File.Exists(StencilFileName))
            //{
              // The stencil does not exist, lets create it from embedded resource
              if (!Directory.Exists(System.IO.Path.GetDirectoryName(StencilFileName))) Directory.CreateDirectory(System.IO.Path.GetDirectoryName(StencilFileName));
              File.WriteAllBytes(StencilFileName, Resources.Resources.PGTVisioShapes);
            //}
            new_row.sh_inv_PID = "3750G";
            new_row.stencilName = StencilFileName;
            new_row.masterNameU = "Workgroup switch";
            StencilsDataSet.Stencils.AddStencilsRow(new_row);
            StencilsDataSet.WriteXml(StencilsDBFileName);
          }
          #endregion
          FileParsed = true;
        }
        catch (System.Xml.XmlException)
        {
          File.Delete(StencilsDBFileName);
        }
        catch
        {
        }
      }
      if (!FileParsed)
      {
        #region Add Default Settings
        string StencilFileName = PGT.Common.Helper.GetWorkingDirectory() + Options.Default.VisioStencilsPath + DefaultStencilName;
        if (!File.Exists(StencilFileName))
        {
          // The stencil does not exist, lets create it from embedded resource
          if (!Directory.Exists(System.IO.Path.GetDirectoryName(StencilFileName))) Directory.CreateDirectory(System.IO.Path.GetDirectoryName(StencilFileName));
          File.WriteAllBytes(StencilFileName, Resources.Resources.PGTVisioShapes);
        }
        StencilsDS.StencilsRow new_row = StencilsDataSet.Stencils.NewStencilsRow();
        new_row.sh_inv_PID = "3750G";
        new_row.stencilName = StencilFileName;
        new_row.masterNameU = "Workgroup switch";
        StencilsDataSet.Stencils.AddStencilsRow(new_row);
        if (!Directory.Exists(System.IO.Path.GetDirectoryName(StencilsDBFileName))) Directory.CreateDirectory(System.IO.Path.GetDirectoryName(StencilsDBFileName));
        StencilsDataSet.WriteXml(StencilsDBFileName);
        #endregion
      }
      dgStencils.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
    }

    /// <summary>
    /// Sets up a new ScriptingForm to start collecting inventory information. Starts the script.
    /// </summary>
    private void CreateInventory()
    {
      if (!string.IsNullOrEmpty(cbxProtocols.Text))
      {
        if (ValidateNetworkDefinitions(tbIPRangesIncluded.Lines) && ValidateNetworkDefinitions(tbIPRangesExcluded.Lines))
        {
          btn_CreateInventory.Enabled = false;
          CDP2VISIO.Options.Default.Save();
          // Create a new, empty scripting form and get the reference for it
          PGT.PGTScriptManager thisManager = PGT.ScriptingFormManager.OpenNewScriptingForm();
          // Signal the script manager, that we are about to add multiple entries
          thisManager.BeginAddingEntries();
          try
          {
            IPAddress ip;
            bool isValidIP = false;
            string sep = PGT.Common.SettingsManager.GetCurrentScriptSettings().CSVSeparator;
            int extraHeaderCount = tb_Devices.Lines.Count() > 0 ? tb_Devices.Lines.Max(s => s.Split(sep.ToCharArray()).Length) - 2 : 0;
            extraHeaderCount.Times((int index) => { thisManager?.AddHeader(string.Format("Custom{0}", index + 1)); });
            // iterate through each devices
            foreach (string device in tb_Devices.Lines)
            {
              if (device == "") continue; // skip empty lines
              string hostName = "";
              string deviceIP = device;

              string[] ipAndhostName = device.Split(sep.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
              if (ipAndhostName.Length >= 2)
              {
                deviceIP = ipAndhostName[0];
                hostName = ipAndhostName[1];
              }
              isValidIP = IPAddress.TryParse(deviceIP, out ip);
              if (isValidIP | !isValidIP & !string.IsNullOrEmpty(cbxJumpServers.Text))
              {
                string thisLine = "";
                // construct the script line as CSV line (like one single in a CSV script files
                if (isValidIP) thisLine = string.Format("1{0}" + cbxJumpServers.Text + "{0}Cisco{0}" + deviceIP + "{0}" + hostName + "{0}" + cbxProtocols.Text + "{0}{0}yes{0}{0}{0}CDPtoVISIO{0}{0}", sep);
                else thisLine = string.Format("1{0}" + cbxJumpServers.Text + "{0}Cisco{0}" + "{0}" + deviceIP + "" + "{0}" + cbxProtocols.Text + "{0}{0}yes{0}{0}{0}CDPtoVISIO{0}{0}", sep);
                // append custom fields
                thisLine += (sep + string.Join(sep, ipAndhostName.SkipWhile((string text, int index) => index < 2)));
                // add the generated line to the script
                thisManager.AddEntry(thisLine, true);
              }
              else MessageBox.Show(string.Format("The name <{0}> is either not a valid IP address or as a hostname is not valid without a jump server", device), "Input error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
          }
          finally
          {
            // Signal the script manager, that we finished adding entries
            thisManager.EndAddingEntries();
          }
          // let us know when the script starts
          thisManager.OnScriptStarted += OnScriptStarted;
          // let the script manager know what to call once the script finished
          thisManager.OnScriptFinished += OnScriptFinished;
          // also let it know what to call if the user stops (aborts) the script manually
          thisManager.OnScriptAborted += OnScriptAborted;
          // and finally start executing the script
          if (cbStartScript.Checked) thisManager.ExecuteScript(null, false);
        }
      }
      else MessageBox.Show("Please select a connection protocol", "Selection error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }

    /// <summary>
    /// Generates VISIO drawing based on actual Inventory DataSet
    /// </summary>
    private void GenerateVisioDocument()
    {
      if (!string.IsNullOrEmpty(cbxDocumentSize.Text) & !string.IsNullOrEmpty(cbxGraphAlg.Text))
      {
        try
        {
          btnCreateVisio.Enabled = false;
          workInProgress.RunWorkerAsync();
          try
          {
            visio_devices_list.Clear();
            if (InventoryDS.Devices.Count > 0)
            {
              CalculateMSALG();
              Thread thGenDoc = new Thread(InternalCreateVisioDoc);
              thGenDoc.Name = "CDP2Visio::CreateVisioDocument";
              thGenDoc.Start();
            }
            else MessageBox.Show("Hey dude, there is no data to present. Please create or load inventory first.", "User error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          }
          finally
          {
            btnCreateVisio.Enabled = true;
            workInProgress.CancelAsync();
          }
        }
        catch (Exception Ex)
        {
          MessageBox.Show(string.Format("Could not create Visio drawing because : {0}", Ex.Message), "Error creating document", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }
      else MessageBox.Show("Please select both document size and graph algorithm", "Selection error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }

    /// <summary>
    /// Exports the Inventory DataSet to Excel
    /// </summary>
    /// <param name="ShowExcel"></param>
    /// <param name="OutputFileName"></param>
    private void GenerateExcelInventory(bool ShowExcel = true, bool SplitLines = true, string OutputFileName = "")
    {
      WorkInProgress wpg = new WorkInProgress("Exporting inventory", "Processing data, please wait...");
      wpg.Delay = 200;
      wpg.SupportCancellation = false;
      wpg.Run();
      Excel.Application xApplication = null;
      try
      {
        #region Export Inventory to Excel
        xApplication = new Excel.Application();
        Excel._Workbook xWorkbook = xApplication.Workbooks.Add();

        #region Create VLAN Information Sheet
        Excel._Worksheet xSheet = (Excel._Worksheet)xWorkbook.ActiveSheet;
        xSheet.Name = "VLAN Information";
        wpg.Text = "Processing VLAN data, please wait...";
        List<object[]> results = new List<object[]>();
        int ColumnCount = 8;
        InventoryDS.Devices.Cast<CDPDataSet.DevicesRow>().ToList().ForEach(dev =>
        {
          if (SplitLines)
          {
            dev.VLANInformation.SplitByLine().ToList().ForEach(VLANLine =>
            {
              string[] vInfo = VLANLine.SplitBySemicolon(StringSplitOptions.None);
              results.Add(new object[]
              {
                dev.ID,
                dev.Name,
                dev.IP_Address,
                vInfo[0],
                vInfo[1],
                vInfo[2],
                vInfo[3],
                vInfo[4]
              });
            });
          }
          else
          {
            var displayLines = dev.VLANInformation.SplitByLine().ToList().Select((string line, int index) =>
            {
              string[] fields = line.SplitBySemicolon(StringSplitOptions.None);
              if (string.IsNullOrEmpty(fields[2])) return string.Format("VLAN{0} ({1})", fields[0], fields[1]);
              else if (dev.Type.ToLowerInvariant() == "asa")
              {
                if (fields[0] == "routed") return string.Format("{0} ({1}) : {2}, {3}", fields[0], fields[1], fields[2], fields[4]);
                return string.Format("VLAN{0} ({1}) : {2}, {3}", fields[0], fields[1], fields[2], fields[4]);
              }
              else return string.Format("Interface vLAN {0} ({1}) : {2}, {3}", fields[0], fields[1], fields[2], fields[4]);
            })./*OrderBy(v => int.Parse(v.SplitBySpace()[2]) .ToString("D4")).*/Cast<object>().ToList();
            displayLines.Insert(0, dev.ID);
            results.Add(displayLines.ToArray());
            ColumnCount = Math.Max(ColumnCount, displayLines.Count);
          }
        });
        if (SplitLines)
        {
          results.Insert(0, new string[] { "ID", "Name", "IP Address", "VLAN ID", "VLAN / IF Name", "IF IP Address", "Netmask", "VLAN Network" });
        }
        else
        {
          ColumnCount++; // Count with ID field, too
          string vHeader = "ID";
          (ColumnCount - 1).Times(() => vHeader += ";" + "VLAN Data");
          results.Insert(0, vHeader.SplitBySemicolon());
        }
        object[,] exportData = new object[results.Count, ColumnCount];
        for (int i = 0; i < results.Count; i++)
          for (int j = 0; j < results[i].Length; j++) exportData[i, j] = results[i][j];
        Excel.Range r = ExcelHelper.GetRange(ref xSheet, 1, 1, ColumnCount, results.Count);
        r.set_Value(XlRangeValueDataType.xlRangeValueDefault, exportData);
        // AutoFit
        xSheet.Cells.Columns.AutoFit();
        // Set Autofilter on first row
        Excel.Range filtertRow = ExcelHelper.GetRange(ref xSheet, 1, 1, exportData.GetLength(1), 1);
        filtertRow.Cells.AutoFilter(1, Missing.Value, XlAutoFilterOperator.xlFilterValues, Missing.Value, true);
        // Set Header row color
        Excel.Range firstRow = ExcelHelper.GetRange(ref xSheet, 1, 1, exportData.GetLength(1), 1);
        firstRow.Font.Color = ExcelHelper.ConvertSytemColorToXlColor(System.Drawing.Color.White);
        firstRow.Cells.Interior.Color = ExcelHelper.ConvertSytemColorToXlColor(System.Drawing.Color.Purple);

        #endregion

        #region Create Inventory Sheet
        xSheet = (Excel._Worksheet)xWorkbook.Worksheets.Add();
        xSheet.Name = "Inventory";
        wpg.Text = "Processing device inventory data, please wait...";
        results.Clear();
        ColumnCount = 7;
        if (SplitLines)
        {
          var q = from device in InventoryDS.Devices select device;
          q.ToList().ForEach(d =>
          {
            var inv = Visio_Device.InventoryInformation(d.Inventory);
            foreach (var item in inv)
            {
              var l = item.ToList();
              l.Insert(0, d.IP_Address);
              l.Insert(0, d.Name);
              l.Insert(0, d.ID.ToString());
              results.Add(l.ToArray());
            }
          });
        }
        else
        {
          foreach (CDPDataSet.DevicesRow thisDevice in InventoryDS.Devices)
          {
            var invLines = Visio_Device.InventoryInformation(thisDevice.Inventory).Select((string[] thisPID) =>
            {
              return string.Format("{0} {1} {2} {3}", thisPID[0], thisPID[1], thisPID[2], thisPID[3]);
            }).Cast<object>().ToList();
            invLines.Insert(0, thisDevice.ID);
            results.Add(invLines.ToArray());
            ColumnCount = Math.Max(ColumnCount, invLines.Count);
          }
        }
        if (SplitLines)
        {
          results.Insert(0, new string[] { "ID", "Name", "IPAddress", "Model", "PartName", "Description", "Serial" });
        }
        else
        {
          ColumnCount++; // Count with ID field, too
          string vHeader = "ID";
          (ColumnCount - 1).Times(() => vHeader += ";" + "Part");
          results.Insert(0, vHeader.SplitBySemicolon());
        }
        exportData = new object[results.Count, ColumnCount];
        for (int i = 0; i < results.Count; i++)
          for (int j = 0; j < results[i].Length; j++) exportData[i, j] = results[i][j];
        r = ExcelHelper.GetRange(ref xSheet, 1, 1, ColumnCount, results.Count);
        r.set_Value(XlRangeValueDataType.xlRangeValueDefault, exportData);
        // AutoFit
        xSheet.Cells.Columns.AutoFit();
        // Set Autofilter on first row
        filtertRow = ExcelHelper.GetRange(ref xSheet, 1, 1, exportData.GetLength(1), 1);
        filtertRow.Cells.AutoFilter(1, Missing.Value, XlAutoFilterOperator.xlFilterValues, Missing.Value, true);
        // Set Header row color
        firstRow = ExcelHelper.GetRange(ref xSheet, 1, 1, exportData.GetLength(1), 1);
        firstRow.Font.Color = ExcelHelper.ConvertSytemColorToXlColor(System.Drawing.Color.White);
        firstRow.Cells.Interior.Color = ExcelHelper.ConvertSytemColorToXlColor(System.Drawing.Color.Purple);

        #endregion

        #region Create Interfaces Sheet
        xSheet = (Excel._Worksheet)xWorkbook.Worksheets.Add();
        xSheet.Name = "Interfaces";
        wpg.Text = "Processing interface data, please wait...";
        results.Clear();
        ColumnCount = 6;
        if (SplitLines)
        {
          var q = from device in InventoryDS.Devices
                  join iface in InventoryDS.Interfaces on device.ID equals iface.ID
                  select new { device, iface };
          q.ToList().ForEach(o =>
          {
            results.Add(new object[]
            {
              o.device.ID,
              o.device.Name,
              o.device.IP_Address,
              o.iface.Name,
              o.iface.Status,
              o.iface.Description
            });
          });
        }
        else
        {
          foreach (CDPDataSet.DevicesRow thisDevice in InventoryDS.Devices)
          {
            var ifLines = InventoryDS.Interfaces.Where(i => i.ID == thisDevice.ID).ToList().Select((CDPDataSet.InterfacesRow row, int index) =>
            {
              return string.Format("{0} ({1}) : {2}", row.Name, row.Description, row.Status);
            }).Cast<object>().ToList();
            ifLines.Insert(0, thisDevice.ID);
            results.Add(ifLines.ToArray());
            ColumnCount = Math.Max(ColumnCount, ifLines.Count);
          }
        }
        if (SplitLines)
        {
          results.Insert(0, new string[] { "ID", "Name", "IPAddress", "InterfaceName", "Status", "Description" });
        }
        else
        {
          ColumnCount++; // Count with ID field, too
          string vHeader = "ID";
          (ColumnCount - 1).Times(() => vHeader += ";" + "Interface");
          results.Insert(0, vHeader.SplitBySemicolon());
        }
        exportData = new object[results.Count, ColumnCount];
        for (int i = 0; i < results.Count; i++)
          for (int j = 0; j < results[i].Length; j++) exportData[i, j] = results[i][j];
        r = ExcelHelper.GetRange(ref xSheet, 1, 1, ColumnCount, results.Count);
        r.set_Value(XlRangeValueDataType.xlRangeValueDefault, exportData);
        // AutoFit
        xSheet.Cells.Columns.AutoFit();
        // Set Autofilter on first row
        filtertRow = ExcelHelper.GetRange(ref xSheet, 1, 1, exportData.GetLength(1), 1);
        filtertRow.Cells.AutoFilter(1, Missing.Value, XlAutoFilterOperator.xlFilterValues, Missing.Value, true);
        // Set Header row color
        firstRow = ExcelHelper.GetRange(ref xSheet, 1, 1, exportData.GetLength(1), 1);
        firstRow.Font.Color = ExcelHelper.ConvertSytemColorToXlColor(System.Drawing.Color.White);
        firstRow.Cells.Interior.Color = ExcelHelper.ConvertSytemColorToXlColor(System.Drawing.Color.Purple);

        #endregion

        #region Not in use - Create VersionInfo Sheet - Not in use
        //xSheet = (Excel._Worksheet)xWorkbook.Worksheets.Add();
        //xSheet.Name = "Version Information";
        //results.Clear();
        //results.Add(new string[] { "Device ID", "Name", "IP Address", "Version Info" });
        //InventoryDS.Devices.Cast<CDPDataSet.DevicesRow>().ToList().ForEach(dev =>
        //{
        //  if (SplitLines)
        //  {
        //    dev.VersionInfo.SplitByLine().ToList().ForEach(vLine =>
        //    {
        //      results.Add(new object[]
        //      {
        //        dev.ID,
        //        dev.Name,
        //        dev.IP_Address,
        //        vLine
        //      });
        //    });
        //  }
        //  else
        //  {
        //    results.Add(new object[]
        //    {
        //        dev.ID,
        //        dev.Name,
        //        dev.IP_Address,
        //        dev.VersionInfo
        //    });
        //  }
        //});
        //exportData = new object[results.Count, InventoryDS.Devices.Columns.Count];
        //for (int i = 0; i < results.Count; i++)
        //  for (int j = 0; j < results[i].Length; j++) exportData[i, j] = results[i][j];
        //r = ExcelHelper.GetRange(ref xSheet, 1, 1, exportData.GetLength(1), exportData.GetLength(0) - 1);
        //r.set_Value(XlRangeValueDataType.xlRangeValueDefault, exportData);
        //// AutoFit
        //xSheet.Cells.Columns.AutoFit();
        //// Set Autofilter on first row
        //filtertRow = ExcelHelper.GetRange(ref xSheet, 1, 1, exportData.GetLength(1), 1);
        //filtertRow.Cells.AutoFilter(1, Missing.Value, XlAutoFilterOperator.xlFilterValues, Missing.Value, true);
        //// Set Header row color
        //firstRow = ExcelHelper.GetRange(ref xSheet, 1, 1, exportData.GetLength(1), 1);
        //firstRow.Font.Color = ExcelHelper.ConvertSytemColorToXlColor(System.Drawing.Color.White);
        //firstRow.Cells.Interior.Color = ExcelHelper.ConvertSytemColorToXlColor(System.Drawing.Color.Purple);
        #endregion

        #region Create Devices Worksheet
        xSheet = (Excel._Worksheet)xWorkbook.Worksheets.Add();
        xSheet.Name = "Devices";
        wpg.Text = "Processing device data, please wait...";
        results.Clear();
        string[] header = new string[] { "ID", "Name", "IP Address", "Type", "Platform", "IOS Version", "Serial#" };
        results.Add(header);
        InventoryDS.Devices.Cast<CDPDataSet.DevicesRow>().ToList().ForEach(d =>
        {
          results.Add(new object[]
          {
            d.ID,
            d.Name,
            d.IP_Address,
            d.Type,
            d.Platform,
            Visio_Device.IOSVersion(d.VersionInfo),
            d.SystemSerial
          });
        });
        exportData = new object[results.Count, header.Length];
        for (int i = 0; i < results.Count; i++)
          for (int j = 0; j < results[i].Length; j++) exportData[i, j] = results[i][j];
        r = ExcelHelper.GetRange(ref xSheet, 1, 1, exportData.GetLength(1), exportData.GetLength(0));
        r.set_Value(XlRangeValueDataType.xlRangeValueDefault, exportData);
        // AutoFit
        xSheet.Cells.Columns.AutoFit();
        // Set Header row color
        firstRow = ExcelHelper.GetRange(ref xSheet, 1, 1, exportData.GetLength(1), 1);
        firstRow.Font.Color = ExcelHelper.ConvertSytemColorToXlColor(System.Drawing.Color.White);
        firstRow.Cells.Interior.Color = ExcelHelper.ConvertSytemColorToXlColor(System.Drawing.Color.Purple);

        #endregion

        #endregion

        if (!string.IsNullOrEmpty(OutputFileName))
        {
          xApplication.ActiveWorkbook.SaveAs(OutputFileName);
        }
      }
      finally
      {
        wpg.Cancel();
      }
      if (xApplication != null)
      {
        if (ShowExcel)
        {
          xApplication.WindowState = XlWindowState.xlMaximized;
          xApplication.Visible = true;
        }
        else
        {
          xApplication.ActiveWorkbook.Close();
          xApplication.Dispose();
          xApplication = null;
        }
      }

    }

    private bool ValidateNetworkDefinitions(string[] Lines, bool DisplayError = true)
    {
      bool result = true;
      foreach (string thisLine in Lines)
      {
        if (string.IsNullOrEmpty(thisLine)) continue;
        string[] words = thisLine.Split("/".ToCharArray(), StringSplitOptions.None);
        if (words.Length == 1)
        {
          IPAddress ip;
          if (!IPAddress.TryParse(words[0], out ip))
          {
            result = false;
            if (DisplayError) MessageBox.Show(string.Format("The ip network definition {0} is invalid, please correct", thisLine), "Input error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            break;
          }
        }
        else if (words.Length == 2)
        {
          IPAddress ip;
          if (!IPAddress.TryParse(words[0], out ip))
          {
            result = false;
            if (DisplayError) MessageBox.Show(string.Format("The ip network definition {0} is invalid, please correct", thisLine), "Input error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            break;
          }
          int maskLen = -1;
          if (!int.TryParse(words[1], out maskLen) || !maskLen.Between(2, 32, true))
          {
            result = false;
            if (DisplayError) MessageBox.Show(string.Format("The ip network definition {0} is invalid, please correct", thisLine), "Input error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            break;
          }
        }
      }
      return result;
    }

    #endregion

    #region Event handlers
    private void OnScriptStarted(object sender, EventArgs e)
    {
      if (IsHandleCreated && InvokeRequired) Invoke(new MethodInvoker(delegate () { OnScriptStarted(sender, e); }));
      else
      {
        DebugEx.WriteLine("CDP discovery script has just started", DebugLevel.Informational);
        InventorySaved = false;
        PGT.PGTScriptManager thisManager = null;
        if (sender is PGT.ScriptExecutor) thisManager = PGT.ScriptingFormManager.GetScriptManager(sender as PGT.ScriptExecutor);
        else if (sender is PGT.ScriptingForm) thisManager = (sender as PGT.ScriptingForm).ScriptManager;
        if (thisManager != null)
        {
          Guid engineID = thisManager.GetExecutorEngineID;
          InventoryFileName = string.Format("{0}{1}{2}.xml", Helper.GetWorkingDirectory(), Options.Default.InventoryDBDirectory, engineID.ToString());
          // Now create a new inventory xml file and write domain boundary table
          // File has a unique name constructed after the unique ID of the executor engine that is associated to the newly opened scripting form
          // so that the CDP2VISIO CustomActionHAndler can find and load this file
          #region Create a pre-provisioned Inventory xml database with boundary definition and parameters then save it to file
          CDPDataSet newDS = new CDPDataSet();
          foreach (string thisAddressDefinition in tbIPRangesIncluded.Lines)
          {
            CDPDataSet.DomainBoundaryRow newBoundaryRow = newDS.DomainBoundary.NewDomainBoundaryRow();
            newBoundaryRow.IP_Address = thisAddressDefinition;
            newBoundaryRow.Action = BoundaryAddressAction.Include.ToString();
            newDS.DomainBoundary.AddDomainBoundaryRow(newBoundaryRow);
          }
          foreach (string thisAddressDefinition in tbIPRangesExcluded.Lines)
          {
            CDPDataSet.DomainBoundaryRow newBoundaryRow = newDS.DomainBoundary.NewDomainBoundaryRow();
            newBoundaryRow.IP_Address = thisAddressDefinition;
            newBoundaryRow.Action = BoundaryAddressAction.Exclude.ToString();
            newDS.DomainBoundary.AddDomainBoundaryRow(newBoundaryRow);
          }
          newDS.Parameters.AddParametersRow(cbActiveDiscovery.Checked);
          if (!Directory.Exists(Path.GetDirectoryName(InventoryFileName))) Directory.CreateDirectory(Path.GetDirectoryName(InventoryFileName));
          newDS.WriteXml(InventoryFileName);
          DebugEx.WriteLine(string.Format("Inventory file {0} created successfully", InventoryFileName));
          #endregion
        }
      }
    }

    private void OnScriptAborted(object sender, EventArgs e)
    {
      if (IsHandleCreated && InvokeRequired) Invoke(new MethodInvoker(delegate () { OnScriptAborted(sender, e); }));
      else
      {
        btn_CreateInventory.Enabled = true;
        PGT.PGTScriptManager thisManager = null;
        if (sender is PGT.ScriptExecutor) thisManager = PGT.ScriptingFormManager.GetScriptManager(sender as PGT.ScriptExecutor);
        else if (sender is PGT.ScriptingForm) thisManager = (sender as PGT.ScriptingForm).ScriptManager;
        if (thisManager != null)
        {
          thisManager.CloseForm(false);
          if (!thisManager.IsScriptSaved()) MessageBox.Show("Script has been aborted, inventory not created", "Script abort", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
      }
    }

    private void OnScriptFinished(object sender, EventArgs e)
    {
      if (IsHandleCreated && InvokeRequired) Invoke(new MethodInvoker(delegate () { OnScriptFinished(sender, e); }));
      else
      {
        btn_CreateInventory.Enabled = true;
        InventoryDS.Clear();
        try
        {
          bool file_inuse = false;
          if (File.Exists(InventoryFileName))
          {
            do
            {
              var LockedBy = PGT.Common.FileUtil.WhoIsLocking(InventoryFileName);
              if (LockedBy.Count > 0)
              {
                string ProcessList = string.Join(";", LockedBy.Select(p => p.ProcessName));
                if (MessageBox.Show(string.Format("The inventory file {0} is being locked by processes : {1}. Do you want to retry ?", InventoryFileName, ProcessList), "Could not create inventory file", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.No)
                  break;
              }
              else
              {
                File.OpenRead(this.InventoryFileName);
                file_inuse = false;
              }
            }
            while (file_inuse);
            if (!file_inuse)
            {
              InventoryDS.ReadXml(this.InventoryFileName);
              PGT.PGTScriptManager thisManager = null;
              if (sender is PGT.ScriptExecutor) thisManager = PGT.ScriptingFormManager.GetScriptManager(sender as PGT.ScriptExecutor);
              else if (sender is PGT.ScriptingForm) thisManager = (sender as PGT.ScriptingForm).ScriptManager;
              if (thisManager != null) thisManager.SetScriptSaved();
              MessageBox.Show("Script has been successfully finished and inventory has been loaded. Click on Create Visio Document to show the results", "Script success", MessageBoxButtons.OK, MessageBoxIcon.Information);
              InventorySaved = false;
            }
            else MessageBox.Show(string.Format("Could not load inventory file : {0}", InventoryFileName), "Script error", MessageBoxButtons.OK, MessageBoxIcon.Information);
          }
          else
          {
            MessageBox.Show(string.Format("Could not find inventory file : {0}. The file might have been removed or deleted.", InventoryFileName), "Script error", MessageBoxButtons.OK, MessageBoxIcon.Information);
          }
        }
        catch (Exception Ex)
        {
          MessageBox.Show(string.Format("That's a pity but could not load inventory data because : {0}", Ex.Message), "Error loading inventory", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }
    }

    private void btnLoadXML_Click(object sender, EventArgs e) //OPEN XML DB
    {
      openFileDialog1.Filter = "CDP Inventory database|*.xml|All Files|*.*";
      if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
      {
        InventoryDS.Clear();
        string xml_filename = openFileDialog1.FileName;
        try
        {
          InventoryDS.ReadXml(xml_filename);
          InventorySaved = true;
          tb_Devices.Clear();
          string sep = PGT.Common.SettingsManager.GetCurrentScriptSettings().CSVSeparator;
          foreach (var device in InventoryDS.Devices)
          {
            IPAddress ip;
            // omit nocdpip_xy devices, they are there for the drawing only
            if (IPAddress.TryParse(device.IP_Address, out ip)) tb_Devices.AppendText(device.IP_Address + sep + device.Name + Environment.NewLine);
          }
          foreach (var addressdefinition in InventoryDS.DomainBoundary)
          {
            if (addressdefinition.Action == BoundaryAddressAction.Include.ToString()) tbIPRangesIncluded.AppendText(addressdefinition.IP_Address + Environment.NewLine);
            else if (addressdefinition.Action == BoundaryAddressAction.Exclude.ToString()) tbIPRangesExcluded.AppendText(addressdefinition.IP_Address + Environment.NewLine);
          }
          cbActiveDiscovery.Checked = InventoryDS.Parameters.First()?.AllowRecursion ?? Options.Default.MRUActiveDiscovery;
          CalculateMSALG();
          tbOutputPath.ReadOnly = false;
          tbOutputPath.Text = Path.GetDirectoryName(openFileDialog1.FileName);
          tbOutputPath.ReadOnly = true;
        }
        catch { }
        load_db = true;
      }
    }

    private void btnSaveXML_Click(object sender, EventArgs e) // SAVE TO XML DB
    {
      SaveFileDialog sfd = new SaveFileDialog();
      sfd.Filter = "CDP Inventory database|*.xml|All Files|*.*";
      if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
      {
        string xml_filename = sfd.FileName;
        InventoryDS.WriteXml(xml_filename);
        // don't need the original inventory file anymore
        if (File.Exists(InventoryFileName)) File.Delete(InventoryFileName);
        InventorySaved = true;
      }
    }

    private void btnCreateVisio_Click(object sender, EventArgs e)
    {
      GenerateVisioDocument();
    }

    private void btnCreateGraph_Click(object sender, EventArgs e)//CalculateMSALG
    {

      if (load_db)
      {
        CalculateMSALG();
        msalg = true;
      }
      else
      {
        MessageBox.Show("Load XML DB first!");
      }
    }

    private void PGTtoVisio_Load(object sender, EventArgs e)
    {
      InitStencils();
      if (Options.Default.MRUDeviceList != null && Options.Default.MRUDeviceList.Count > 0) tb_Devices.Lines = Options.Default.MRUDeviceList.OfType<string>().ToArray();
      if (string.IsNullOrEmpty(Options.Default.MRUOutputPath) || !Directory.Exists(tbOutputPath.Text))
      {
        tbOutputPath.ReadOnly = false;
        tbOutputPath.Text = RelativePath.GetAbsolutePathToExecutablePath(PGT.Common.Properties.Settings.Default.WorkingDirectory);
        tbOutputPath.ReadOnly = true;
      }
    }

    private void button_add_new_stencil_Click(object sender, EventArgs e)
    {
      AddStencilForm aSF = new AddStencilForm();
      if (aSF.ShowDialog() == System.Windows.Forms.DialogResult.OK)
      {
        bool exists = StencilsDataSet.Stencils.Any(p => p.sh_inv_PID == aSF.PIDName);
        if (!exists)
        {
          StencilsDS.StencilsRow new_row = StencilsDataSet.Stencils.NewStencilsRow();
          new_row.sh_inv_PID = aSF.PIDName;
          new_row.stencilName = aSF.StencilName;
          new_row.masterNameU = aSF.ShapeName;
          StencilsDataSet.Stencils.AddStencilsRow(new_row);
          string StencilsDBFileName = PGT.Common.Helper.GetWorkingDirectory() + Options.Default.VisioStencilsPath + Options.Default.StencilsDBName;
          if (!Directory.Exists(System.IO.Path.GetDirectoryName(StencilsDBFileName))) Directory.CreateDirectory(System.IO.Path.GetDirectoryName(StencilsDBFileName));
          StencilsDataSet.WriteXml(StencilsDBFileName);
        }
        else
        {
          MessageBox.Show("PID already exists!");
        }
      }
    }

    private void PGTtoVisio_FormClosed(object sender, FormClosedEventArgs e)
    {
      try
      {
        if (tb_Devices.Lines.Length > 0)
        {
          Options.Default.MRUDeviceList = new StringCollection();
          Options.Default.MRUDeviceList.AddRange(tb_Devices.Lines);
        }
        Options.Default.Save();
        string StencilsDBFileName = PGT.Common.Helper.GetWorkingDirectory() + Options.Default.VisioStencilsPath + Options.Default.StencilsDBName;
        if (!Directory.Exists(System.IO.Path.GetDirectoryName(StencilsDBFileName))) Directory.CreateDirectory(System.IO.Path.GetDirectoryName(StencilsDBFileName));
        StencilsDataSet.WriteXml(StencilsDBFileName);

        if (File.Exists(InventoryFileName)) File.Delete(InventoryFileName);
      }
      catch (Exception Ex)
      {
        DebugEx.WriteLine("Error closing CDP2VIsio manager : " + Ex.InnerExceptionsMessage());
      }
    }

    private void btn_RunScript_Click(object sender, EventArgs e)
    {
      CreateInventory();
    }

    private void btnEnabledChanged(object sender, EventArgs e)
    {
      try
      {
        Button btn = sender as Button;
        if (btn.Enabled)
        {
          btn.BackColor = (System.Drawing.Color)ButtonColors[btn];
        }
        else
        {
          if (!ButtonColors.ContainsKey(btn)) ButtonColors.Add(btn, btn.BackColor);
          btn.BackColor = System.Drawing.Color.DarkGray;
        }
      }
      catch { }
    }

    private void btnAddNetwork_Click(object sender, EventArgs e)
    {
      try
      {
        string[] addressAndMask = tbNetwork.Text.Split("/\\".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        if (addressAndMask.Length == 2)
        {
          int maskLength = -1;
          if (int.TryParse(addressAndMask[1], out maskLength))
          {
            if (maskLength >= 16 && maskLength <= 30)
            {
              workInProgress.RunWorkerAsync();
              try
              {
                string[] hosts = PGT.Common.IPOperations.GetHostAddresses(addressAndMask[0], maskLength);
                tb_Devices.AppendText(string.Join(Environment.NewLine, hosts));
              }
              finally
              {
                workInProgress.CancelAsync();
              }
            }
            else MessageBox.Show("Weird subnet mask. It should be between 16 and 30", "Address input error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          }
          else MessageBox.Show("Weird address mask. It should be an integer value between 8 and 30", "Address input error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        else MessageBox.Show("Address formatting error. It should look like 192.168.1.0/24", "Address input error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      catch (Exception Ex)
      {
        MessageBox.Show("An error occurred : " + Ex.Message, "Error calculating IP addresses", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
    }

    private void btnClearDeviceList_Click(object sender, EventArgs e)
    {
      tb_Devices.Clear();
    }

    private void DrawGradientBackground(object sender, PaintEventArgs e)
    {
      try
      {
        Rectangle rect = (sender as Control).ClientRectangle;
        using (Brush br = new LinearGradientBrush(rect, System.Drawing.Color.White, (sender as Control).BackColor, 90))
        {
          e.Graphics.FillRectangle(br, rect);
        }
      }
      catch (Exception Ex)
      {
        DebugEx.WriteLine("Error in DrawGradientBackground : " + Ex.InnerExceptionsMessage());
      }
    }

    private void btnOutputPath_Click(object sender, EventArgs e)
    {
      FolderBrowserDialog fbd = new FolderBrowserDialog();
      if (fbd.ShowDialog() == DialogResult.OK)
      {
        tbOutputPath.ReadOnly = false;
        tbOutputPath.Text = fbd.SelectedPath;
        tbOutputPath.ReadOnly = true;
      }
    }
    private void btnSelectTemplateDrawing_Click(object sender, EventArgs e)
    {
      OpenFileDialog ofd = new OpenFileDialog();
      ofd.Filter = "Visio Drawing|*.vsdx;*.vsd";
      ofd.CheckFileExists = true;
      if (ofd.ShowDialog() == DialogResult.OK)
      {
        tbTemplateDrawing.Text = ofd.FileName;
      }
    }

    private void tbOutputPath_TextChanged(object sender, EventArgs e)
    {
      toolTip1.SetToolTip(tbOutputPath, tbOutputPath.Text);
    }

    private void cmStandardExport_Click(object sender, EventArgs e)
    {
      SaveFileDialog sfd = new SaveFileDialog();
      sfd.Title = "Export to Excel in standard format";
      sfd.Filter = "Excel Workbook|*.xlsx|All Files|*.*";
      if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
      {
        try
        {
          if (File.Exists(sfd.FileName)) File.Delete(sfd.FileName);
          GenerateExcelInventory(false, true, sfd.FileName);
          MessageBox.Show("Inventory was exported successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception Ex)
        {
          MessageBox.Show("Unfortunately something bad happened while exporting inventory : " + Ex.Message, "Error exporting inventory", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }
    }

    private void cmVisioDataExport_Click(object sender, EventArgs e)
    {
      SaveFileDialog sfd = new SaveFileDialog();
      sfd.Title = "Export to Excel for Visio data binding";
      sfd.Filter = "Excel Workbook|*.xlsx|All Files|*.*";
      if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
      {
        try
        {
          if (File.Exists(sfd.FileName)) File.Delete(sfd.FileName);
          GenerateExcelInventory(false, false, sfd.FileName);
          MessageBox.Show("Inventory was exported successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception Ex)
        {
          MessageBox.Show("Unfortunately something bad happened while exporting inventory : " + Ex.Message, "Error exporting inventory", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }
    }

    private void CDP2VisioManager_FormClosing(object sender, FormClosingEventArgs e)
    {
      e.Cancel = !InventorySaved && MessageBox.Show("Inventory not saved, if you close the form information will be lost ! Are you sure you want to close ?", "Inventory not saved", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No;
    }

    private void btnPickFavHosts_Click(object sender, EventArgs e)
    {
      var pickedHosts = HostsManager.ShowHostPicker(true);
      if (pickedHosts.Count() > 0)
      {
        string sep = PGT.Common.SettingsManager.GetCurrentScriptSettings().CSVSeparator;
        foreach (var thisHost in pickedHosts)
        {
          string hostEntry = string.Join(sep, new string[] { thisHost.DeviceIP, thisHost.DeviceName, thisHost.FolderName }) + Environment.NewLine;
          tb_Devices.AppendText(hostEntry);
        }
      }
    }
    #endregion
  }

  public enum BoundaryAddressAction { Include, Exclude };
}
