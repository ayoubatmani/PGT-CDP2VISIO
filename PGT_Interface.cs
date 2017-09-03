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


using PGT.Common;
using PGT.ExtensionInterfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;


namespace CDP2VISIO
{
  public class PGT_Interface : ICustomActionHandler
  {
    #region Fields
    private CDPDataSet local_dataset;
    private PGTDataSet.ScriptSettingRow ScriptSettings;
    /// <summary>
    /// The name of the inventory file that is to be created
    /// </summary>
    private string InventoryFileName;
    /// <summary>
    /// Indicates, that the inventory file has already been pre-provisioned with configuration details
    /// when the script started.
    /// </summary>
    private bool InventoryPreProvisioned = false;
    /// <summary>
    /// Controls whether newly discovered CDP neighbors will also be added to script and therefore be discovered, too
    /// </summary>
    private bool AllowRecursion = true;
    #endregion

    public void Initialize(IScriptExecutorBase Executor)
    {
      DebugEx.WriteLine("Initializing PGTNetworkDiscovery CustomActionHandler... ", DebugLevel.Informational);
      local_dataset = new CDPDataSet();
      local_dataset.Clear();
      ScriptSettings = SettingsManager.GetCurrentScriptSettings();
      Guid engineID = Executor.EngineID();
      // search an existing inventory file and load. used later for checking domain boundary
      InventoryFileName = string.Format("{0}{1}{2}.xml", Helper.GetWorkingDirectory(), Options.Default.InventoryDBDirectory, engineID.ToString());
      DebugEx.WriteLine(string.Format("Checking for pre-existing inventory file {0}... ", InventoryFileName), DebugLevel.Informational);
      if (File.Exists(InventoryFileName))
      {
        try
        {
          local_dataset.ReadXml(InventoryFileName);
          InventoryPreProvisioned = true;
          AllowRecursion = local_dataset.Parameters.First()?.AllowRecursion ?? Options.Default.MRUActiveDiscovery;
          DebugEx.WriteLine(string.Format("Inventory file loaded successfully. Active discovery is {0}.", AllowRecursion ? "enabled" : "disabled"), DebugLevel.Informational);
        }
        catch (Exception Ex)
        {
          DebugEx.WriteLine(string.Format("Error loading inventory file : {0}", Ex.InnerExceptionsMessage()));
        }
      }
      else
      {
        AllowRecursion = Options.Default.MRUActiveDiscovery;
        DebugEx.WriteLine(string.Format("File {0} does not exist. Active discovery is {1}.", InventoryFileName, AllowRecursion ? "enabled" : "disabled"), DebugLevel.Informational);
      }
    }

    public void Terminate()
    {
      DebugEx.WriteLine("Terminating PGTNetworkDiscovery CustomActionHandler and saving Inventory information", DebugLevel.Informational);
      bool RetryWrite = true;
      while (RetryWrite)
      {
        try
        {
          if (!Directory.Exists(Path.GetDirectoryName(InventoryFileName))) Directory.CreateDirectory(Path.GetDirectoryName(InventoryFileName));
          var LockedBy = PGT.Common.FileUtil.WhoIsLocking(InventoryFileName);
          if (LockedBy.Count > 0)
          {
            string ProcessList = string.Join(";", LockedBy.Select(p => p.ProcessName));
            RetryWrite = MessageBox.Show(string.Format("The file {0} is being used by processes : {1}. Do you want to retry ?", InventoryFileName, ProcessList), "Could not create inventory file", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes;
          }
          else
          {
            local_dataset.WriteXml(InventoryFileName);
            RetryWrite = false;
            if (!InventoryPreProvisioned && MessageBox.Show("Do you want to save the inventory to a specific file ?\r\nIf you choose No, the temporary inventory file will be retained.", "Save As", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
              SaveFileDialog sfd = new SaveFileDialog();
              sfd.Filter = "CDP Inventory database|*.xml|All Files|*.*";
              if (sfd.ShowDialog() == DialogResult.OK)
              {
                try
                {
                  local_dataset.WriteXml(sfd.FileName);
                  MessageBox.Show("Inventory saved successfully", "Operation completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                  try
                  {
                    // as the inventory file was not pre-provisioned and hence the CDP2Visio handler will not be searching for it
                    // we can safely delete the file
                    File.Delete(InventoryFileName);
                  }
                  catch (Exception Ex)
                  {
                    string msg = string.Format("Error while removing temporary inventory file : {0}", InventoryFileName, Ex.Message);
                    DebugEx.WriteLine(Ex.InnerExceptionsMessage());
                  }
                }
                catch (Exception Ex)
                {
                  string msg = string.Format("Unfortunately an unexpected error occurred while saving inventory to file {0}. Error is : {1}", sfd.FileName, Ex.InnerExceptionsMessage());
                  DebugEx.WriteLine(msg);
                  MessageBox.Show(msg, "Could not create inventory file", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                }
              }
            }
          }
        }
        catch (Exception Ex)
        {
          string msg = string.Format("Unfortunately an unexpected error occurred while writing inventory to file {0}. Error is : {1}. Do you want to retry ?", InventoryFileName, Ex.InnerExceptionsMessage());
          DebugEx.WriteLine(msg);
          RetryWrite = MessageBox.Show(msg, "Could not create inventory file", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes;
        }
      }
    }

    public bool LoggingRequired()
    {
      return false;
    }

    public bool DoCustomAction(IScriptExecutorBase Executor, DeviceConnectionInfo ConnectionInfo, out string ActionResult, out bool ConnectionDropped, out bool BreakExecution)
    {
      bool result = false;
      ActionResult = "Custom action not implemented";
      ConnectionDropped = false;
      BreakExecution = false;
      IScriptableSession st = (IScriptableSession)Executor.Session;
      int DeviceID = 0;

      if (ConnectionInfo.CustomActionID == "PGTNetworkDiscovery")
      {
        if (ConnectionInfo.VendorName.ToLowerInvariant() == "cisco")
        {
          #region Cisco
          ActionResult = "Error processing PGTNetworkDiscovery for Cisco";
          string show_ver = string.Empty;
          string device_type = "router";
          string actual_hostname = string.Empty;
          string show_inventory = string.Empty;
          string system_serial = string.Empty;

          #region SHOW VER, Virtual switch, Stacked switch

          Executor.ShowActivity("Retrieving device information...");
          show_ver = st.ExecCommand("sh ver");
          actual_hostname = st.GetHostName();

          string virtualswitch = st.ExecCommand("sh switch virtual");
          bool isVSS = false;
          try
          {
            isVSS = virtualswitch.SplitByLine().First().Split(':')[1].ToLowerInvariant().Trim() == "virtual switch";
          }
          catch (Exception Ex)
          {
            DebugEx.WriteLine(String.Format("CDP2VISIO : error parsing \"sh virtual switch\" output : {0}", Ex.InnerExceptionsMessage()), DebugLevel.Debug);
          }
          int stackCount = 0;
          string stackedswitches = st.ExecCommand("sh switch");
          try
          {
            if (stackedswitches.ToLowerInvariant().StartsWith("switch/stack"))
              stackCount = stackedswitches.SplitByLine().Count(l => l.SplitBySpace()[0].Trim('*').IsInt());
          }
          catch (Exception Ex)
          {
            DebugEx.WriteLine(String.Format("CDP2VISIO : error parsing \"sh switch\" output : {0}", Ex.InnerExceptionsMessage()), DebugLevel.Debug);
          }
          #endregion

          try
          {
            #region Identify device
            show_inventory = st.ExecCommand("show inventory");
            // Indicates that we are connected to an ASA, using later for VLAN discovery
            bool isASA = show_inventory.IndexOf("Adaptive Security Appliance") >= 0;
            // some switches doe no support the "show inventory" command
            bool exec_error = show_inventory.ToLowerInvariant().Contains("invalid input detected") || ScriptSettings.FailedCommandPattern.SplitBySemicolon().Any(w => show_inventory.IndexOf(w) >= 0);
            if (exec_error)
            {
              DebugEx.WriteLine(String.Format("CDP2VISIO : switch does not support \"sh inventory\" command, parsing version information"), DebugLevel.Debug);
              // try to parse sh_version to get system serial numbers      
              try
              {
                system_serial = string.Join(",", show_ver.SplitByLine().Where(l => l.StartsWith("System serial number")).Select(l => l.Split(':')[1].Trim()));
              }
              catch (Exception Ex)
              {
                DebugEx.WriteLine(String.Format("CDP2VISIO : error searching serial number in \"sh version\" output : {0}", Ex.InnerExceptionsMessage()), DebugLevel.Debug);
              }
            }
            else
            {
              // This should return system serial most of the time
              try
              {
                if (stackCount > 0)
                {
                  // if stackCount > 0 the switch supported the "show switch" command. Probably also understands "show module"
                  string modules = st.ExecCommand("show module");
                  // some switches who support the "show switch" command may still do not understand "show modules"
                  exec_error = modules.ToLowerInvariant().Contains("invalid input detected") || ScriptSettings.FailedCommandPattern.SplitBySemicolon().Any(w => modules.IndexOf(w) >= 0);
                  if (exec_error)
                  {
                    DebugEx.WriteLine(String.Format("CDP2VISIO : switch does not support \"sh module\" command, parsing version information"), DebugLevel.Debug);
                    // try to parse sh_version to get system serial numbers
                    system_serial = string.Join(",", show_ver.SplitByLine().Where(l => l.StartsWith("System serial number")).Select(l => l.Split(':')[1].Trim()));
                  }
                  else
                  {
                    // select lines starting with a number. These are assumed the be the switches in stack
                    var switchList = modules.SplitByLine().Where(l => l.SplitBySpace()[0].Trim('*').IsInt());
                    // each line contains the serial number in th 4th column
                    system_serial = string.Join(",", switchList.Select(m => m.SplitBySpace()[3]));
                  }
                }
                else system_serial = show_inventory.SplitByLine().First(l => l.StartsWith("PID:")).Split(',')[2].Split(':')[1].Trim();
              }
              catch (Exception Ex)
              {
                system_serial = "parsing error";
                DebugEx.WriteLine(string.Format("Error parsing serial number : {0}", Ex.InnerExceptionsMessage()), DebugLevel.Error);
              }
            }
            #endregion

            #region Add New Device to DB with inlist
            // Two devices considered identical if :
            // - have the same hostname, or
            // - have the same IPAddress
            CDPDataSet.DevicesRow device_row = local_dataset.Devices.FirstOrDefault(d =>
              d.IP_Address.SplitBySemicolon().Any(thisIP => thisIP == ConnectionInfo.DeviceIP) ||
              d.Name.SplitBySemicolon().Any(thisName => DottedNameSpace.CompateTLD(thisName, ConnectionInfo.HostName))
            );
            if (device_row == null) //If NOT found in the DB have to ADD as new
            {
              device_row = local_dataset.Devices.NewDevicesRow();
              device_row.SystemSerial = system_serial;
              device_row.IP_Address = ConnectionInfo.DeviceIP;
              device_row.Name = actual_hostname;
              device_row.VersionInfo = show_ver;
              device_row.Type = isASA ? "ASA" : isVSS ? "VSS" : stackCount > 1 ? string.Format("Stack{0}", stackCount) : device_type;
              device_row.Inventory = show_inventory;
              local_dataset.Devices.AddDevicesRow(device_row);
              DeviceID = device_row.ID;
            }
            else //IF found in the existing DB have to update!
            {
              device_row.VersionInfo = show_ver;
              device_row.Inventory = show_inventory;
              device_row.SystemSerial = system_serial;
              if (isASA) device_row.Type = "ASA";
              else if (isVSS) device_row.Type = "VSS";
              else if (stackCount > 1) device_row.Type = string.Format("Stack{0}", stackCount);
            }

            #endregion

            #region SHOW CDP NEIGHBOUR
            if (!isASA)
            {
              Executor.ShowActivity("Checking for CDP neighbors...");
              string cdpResult = st.ExecCommand("sh cdp neighbors detail");

              CDPParser thisParser;
              if (show_ver.IndexOf("NX-OS") >= 0)
              {
                thisParser = new NXOS_CDPParser(Executor, ConnectionInfo, AllowRecursion);
                thisParser.ProcessCDPResult(cdpResult, device_row, local_dataset);
              }
              else
              {
                thisParser = new IOS_CDPParser(Executor, ConnectionInfo, AllowRecursion);
                thisParser.ProcessCDPResult(cdpResult, device_row, local_dataset);
              }
            }

            #endregion

            #region Collect interface configuration details for CDP connected interfaces
            Executor.ShowActivity("Collecting CDP connected interface information...");
            var query_local_interfaces = from device in local_dataset.Devices
                                         where (device.ID == device_row.ID)
                                         join neigh in local_dataset.Neighbours on device.ID equals neigh.Parent_ID
                                         select new
                                         {
                                           local_int = neigh.Local_Interface,
                                           ID = device.ID,
                                         };
            foreach (var thisInterface in query_local_interfaces)
            {
              CDP2VISIO.CDPDataSet.InterfacesRow interface_row = null;
              interface_row = local_dataset.Interfaces.NewInterfacesRow();


              string command = "sh run interface " + thisInterface.local_int;


              string commandResult = st.ExecCommand(command);
              commandResult = commandResult.ToLower();

              string[] lines_in_commandresult = commandResult.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
              string interface_config = string.Empty;
              bool addtoconfig = false;
              foreach (var line in lines_in_commandresult)
              {
                if (line.IndexOf(RevConvInt(thisInterface.local_int)) >= 0) addtoconfig = true;
                if (addtoconfig) interface_config = interface_config + line + "\r\n";
              }

              interface_row.ID = thisInterface.ID;
              interface_row.Name = thisInterface.local_int;
              interface_row.Running_config = interface_config;


              local_dataset.Interfaces.AddInterfacesRow(interface_row);
            }

            #endregion

            #region Collect overall interface status information
            if (Options.Default.DisplayConnected && !isASA)
            {
              Executor.ShowActivity("Checking interface status...");
              string[] ifDesc = st.ExecCommand("show interface description").SplitByLine();
              foreach (string thisIfDescription in ifDesc.SkipWhile((string s, int i) => i < 1))
              {
                string[] ifDescrWords = thisIfDescription.SplitBySpace();
                string IFName = Common.ConvInt(ifDescrWords[0]);
                if (!IFName.StartsWith("vl")) // Don't care vlan interfaces here
                {
                  string IFStatus = string.Format("{0}/{1}", ifDescrWords[1], ifDescrWords[2]);
                  string IFDescription = string.Join(" ", ifDescrWords.SkipWhile((string s, int i) => i < 3));
                  var foundIF = from thisIF in local_dataset.Interfaces where thisIF.ID == device_row.ID && thisIF.Name == IFName select thisIF;
                  if (foundIF.Count() == 1)
                  {
                    // Update existing IF data
                    foundIF.ElementAt(0).Status = IFStatus;
                    foundIF.ElementAt(0).Description = IFDescription;
                  }
                  else
                  {
                    // Add as new IF
                    CDPDataSet.InterfacesRow thisIF = local_dataset.Interfaces.NewInterfacesRow();
                    thisIF.ID = device_row.ID;
                    thisIF.Name = IFName;
                    thisIF.Status = IFStatus;
                    thisIF.Description = IFDescription;
                    local_dataset.Interfaces.AddInterfacesRow(thisIF);
                  }
                }
              }

            }
            #endregion

            #region Collect VLAN interface information
            if (isASA)
            {
              Executor.ShowActivity("Gathering VLAN information...");
              // Contains the L3 information : interface names, interface ip, network, mask
              List<string> VLANInfo = new List<string>();
              string asaIFs = st.ExecCommand("sh int ip brief");
              device_row.L3InterfaceInformation = asaIFs;
              foreach (string thisInterface in asaIFs.SplitByLine().SkipWhile(l => l.StartsWith("Interface")))
              {
                string[] ifWords = thisInterface.SplitBySpace();
                string asaIFName = ifWords[0];
                string nameIF = "";
                string secLevel = "";
                string vlanIPAddr = "";
                string vlanNetMask = "";
                string netaddress = "";
                int maskLength = 0;
                int vlanID = 0;
                string thisIFConfig = st.ExecCommand(string.Format("sh run int {0}", asaIFName));
                foreach (string thisConfigLine in thisIFConfig.SplitByLine().Select(s => s.Trim()))
                {
                  if (thisConfigLine.StartsWith("vlan")) int.TryParse(thisConfigLine.SplitBySpace()[1], out vlanID);
                  else if (thisConfigLine.StartsWith("nameif")) nameIF = thisConfigLine.SplitBySpace()[1];
                  else if (thisConfigLine.StartsWith("security-level")) secLevel = thisConfigLine.SplitBySpace()[1];
                  else if (thisConfigLine.StartsWith("ip address"))
                  {
                    string[] lineWords = thisConfigLine.SplitBySpace();
                    vlanIPAddr = lineWords[2];
                    vlanNetMask = lineWords[3];
                    maskLength = IPOperations.GetMaskLength(vlanNetMask);
                    netaddress = IPOperations.GetNetworkAddress(vlanIPAddr, maskLength);
                  }
                }
                string networkAddressPrint = "";
                if (maskLength > 0) networkAddressPrint = string.Format("{0}/{1}", netaddress, maskLength);
                string reportedIFName = string.Format("{0} name: {1} security-level: {2}", asaIFName, nameIF, secLevel);
                VLANInfo.Add(string.Join(";", new string[] { vlanID == 0 ? "routed" : vlanID.ToString(), reportedIFName, vlanIPAddr, vlanNetMask, networkAddressPrint }));

                #region Add ASA interface to inventory database
                string IFStatus = "n/a";
                if (ifWords.Length == 6) IFStatus = string.Format("{0}/{1}", ifWords[4], ifWords[5]);
                else if (ifWords.Length == 7) IFStatus = string.Format("{0} {1}/{2}", ifWords[4], ifWords[5], ifWords[6]);
                var foundIF = from thisIF in local_dataset.Interfaces where thisIF.ID == device_row.ID && thisIF.Name == asaIFName select thisIF;
                if (foundIF.Count() == 1)
                {
                  // Update existing IF data
                  foundIF.ElementAt(0).Status = IFStatus;
                  foundIF.ElementAt(0).Description = reportedIFName;
                }
                else
                {
                  // Add as new IF
                  CDPDataSet.InterfacesRow thisIF = local_dataset.Interfaces.NewInterfacesRow();
                  thisIF.ID = device_row.ID;
                  thisIF.Name = asaIFName;
                  thisIF.Status = IFStatus;
                  thisIF.Description = reportedIFName;
                  local_dataset.Interfaces.AddInterfacesRow(thisIF);
                }
                #endregion
              }
              device_row.VLANInformation = string.Join(Environment.NewLine, VLANInfo);
            }
            else
            {
              Executor.ShowActivity("Gathering VLAN interface information...");
              // TODO : for routers, also include "sh ip int brief"
              string vlanIFs = st.ExecCommand("sh ip int brief | i [Vv]lan");
              device_row.L3InterfaceInformation = vlanIFs;

              #region Collect network details for VLANs
              // Contains the list of VLAN interface names
              List<string> VLANInterfaces = Regex.Matches(vlanIFs, "^Vlan(\\d+)", RegexOptions.Multiline).Cast<Match>().Select(m => m.Value.ToLowerInvariant()).ToList();
              // Contains the L3 information : interface names, interface ip, network, mask
              List<string> VLANInfo = new List<string>();
              string vlans = st.ExecCommand("sh vlan");
              bool addLineToOutput = false;
              foreach (string line in vlans.SplitByLine())
              {
                if (line.StartsWith("---"))
                {
                  addLineToOutput = !addLineToOutput;
                  if (!addLineToOutput) break;
                }
                else if (addLineToOutput)
                {
                  string[] words = line.SplitBySpace();
                  int vlanID = -1;
                  if (int.TryParse(words[0], out vlanID))
                  {
                    string vlanName = words[1];
                    string vlanIPAddr = "";
                    string vlanNetMask = "";
                    string netaddress = "";
                    int maskLength = 0;
                    // Check if current VLAN has a corresponding VLAN interface definition
                    if (VLANInterfaces.Contains(string.Format("vlan{0}", vlanID)))
                    {
                      string vlanIntConfig = st.ExecCommand(string.Format("sh run int vlan{0}", vlanID));
                      string ipAddressLine = vlanIntConfig.SplitByLine().FirstOrDefault(l => l.Trim().StartsWith("ip address"));
                      if (ipAddressLine != null)
                      {
                        string[] addr = ipAddressLine.SplitBySpace();
                        vlanIPAddr = addr[2];
                        vlanNetMask = addr[3];
                        maskLength = IPOperations.GetMaskLength(vlanNetMask);
                        netaddress = IPOperations.GetNetworkAddress(vlanIPAddr, maskLength);
                      }
                      else
                      {
                        ipAddressLine = vlanIntConfig.SplitByLine().FirstOrDefault(l => l.Trim().StartsWith("no ip address"));
                        if (ipAddressLine != null)
                        {
                          vlanIPAddr = "no ip address";
                        }
                      }
                    }
                    string networkAddressPrint = "";
                    if (maskLength > 0) networkAddressPrint = string.Format("{0}/{1}", netaddress, maskLength);
                    VLANInfo.Add(string.Join(";", new string[] { vlanID.ToString(), vlanName, vlanIPAddr, vlanNetMask, networkAddressPrint }));
                  }

                }
              }
              device_row.VLANInformation = string.Join(Environment.NewLine, VLANInfo);
              #endregion
            }
            #endregion

            result = true;
            ActionResult = "Discovery information processing finished successfully.";
            if (system_serial == "") ActionResult += "Warning : Could not identify system serial number.";
          }
          catch (Exception Ex)
          {
            ActionResult = string.Format("Unexpected processing error : {0} {1}", Ex.Message, Ex.InnerException?.Message);
          }
          #endregion
        }
        else if (ConnectionInfo.VendorName.ToLowerInvariant() == "junos")
        {
          #region JunOS
          //
          // http://www.juniper.net/documentation/en_US/junos11.1/topics/task/configuration/802-1x-lldp-cli.html
          //
          ActionResult = "Error processing PGTNetworkDiscovery for JunOS";
          string show_ver = string.Empty;
          string device_type = "router";
          string actual_hostname = string.Empty;
          string show_inventory = string.Empty;
          string system_serial = string.Empty;
          int stackCount = 0;

          #region Identify device
          show_inventory = st.ExecCommand("show chassis hardware");
          // Indicates that we are connected to an ASA, using later for VLAN discovery
          bool isASA = show_inventory.IndexOf("Adaptive Security Appliance") >= 0;
          // some switches doe no support the "show inventory" command
          bool exec_error = show_inventory.ToLowerInvariant().Contains("invalid input detected") || ScriptSettings.FailedCommandPattern.SplitBySemicolon().Any(w => show_inventory.IndexOf(w) >= 0);
          if (exec_error)
          {
            DebugEx.WriteLine(String.Format("CDP2VISIO : switch does not support \"sh inventory\" command, parsing version information"), DebugLevel.Debug);
            // try to parse sh_version to get system serial numbers      
            try
            {
              system_serial = string.Join(",", show_ver.SplitByLine().Where(l => l.StartsWith("System serial number")).Select(l => l.Split(':')[1].Trim()));
            }
            catch (Exception Ex)
            {
              DebugEx.WriteLine(String.Format("CDP2VISIO : error searching serial number in \"sh version\" output : {0}", Ex.InnerExceptionsMessage()), DebugLevel.Debug);
            }
          }
          else
          {
            // This should return system serial most of the time
            try
            {
              if (stackCount > 0)
              {
                // if stackCount > 0 the switch supported the "show switch" command. Probably also understands "show module"
                string modules = st.ExecCommand("show module");
                // some switches who support the "show switch" command may still do not understand "show modules"
                exec_error = modules.ToLowerInvariant().Contains("invalid input detected") || ScriptSettings.FailedCommandPattern.SplitBySemicolon().Any(w => modules.IndexOf(w) >= 0);
                if (exec_error)
                {
                  DebugEx.WriteLine(String.Format("CDP2VISIO : switch does not support \"sh module\" command, parsing version information"), DebugLevel.Debug);
                  // try to parse sh_version to get system serial numbers
                  system_serial = string.Join(",", show_ver.SplitByLine().Where(l => l.StartsWith("System serial number")).Select(l => l.Split(':')[1].Trim()));
                }
                else
                {
                  // select lines starting with a number. These are assumed the be the switches in stack
                  var switchList = modules.SplitByLine().Where(l => l.SplitBySpace()[0].Trim('*').IsInt());
                  // each line contains the serial number in th 4th column
                  system_serial = string.Join(",", switchList.Select(m => m.SplitBySpace()[3]));
                }
              }
              else system_serial = show_inventory.SplitByLine().First(l => l.StartsWith("PID:")).Split(',')[2].Split(':')[1].Trim();
            }
            catch (Exception Ex)
            {
              system_serial = "parsing error";
              DebugEx.WriteLine(string.Format("Error parsing serial number : {0}", Ex.InnerExceptionsMessage()), DebugLevel.Error);
            }
          }
          #endregion


          #endregion
        }
      }
      return result;
    }

    public string[] HandledCustomActions()
    {
      return new string[] { "PGTNetworkDiscovery" };
    }

    public void HostUnreachable(IScriptExecutorBase Executor, DeviceConnectionInfo ConnectionInfo)
    {

    }

    public string ConvInt(string input_interface)
    {
      input_interface = input_interface.ToLower();
      string conv_int = input_interface;

      if (conv_int.IndexOf("fastethernet") >= 0) conv_int = conv_int.Replace("fastethernet", "fa");
      if (conv_int.IndexOf("tengigabitethernet") >= 0) conv_int = conv_int.Replace("tengigabitethernet", "tengi");
      if (conv_int.IndexOf("gigabitethernet") >= 0) conv_int = conv_int.Replace("gigabitethernet", "gi");
      if (conv_int.IndexOf("ethernet") >= 0) conv_int = conv_int.Replace("ethernet", "eth");
      if (conv_int.IndexOf("loopback") >= 0) conv_int = conv_int.Replace("loopback", "lo");
      return conv_int;
    }

    public string RevConvInt(string input_interface)
    {
      input_interface = input_interface.ToLower();
      string conv_int = input_interface;

      if (conv_int.IndexOf("fa") >= 0) conv_int = conv_int.Replace("fa", "fastethernet");
      else if (conv_int.IndexOf("tengi") >= 0) conv_int = conv_int.Replace("tengi", "tengigabitethernet");
      else if (conv_int.IndexOf("gi") >= 0) conv_int = conv_int.Replace("gi", "gigabitethernet");
      else if (conv_int.IndexOf("eth") >= 0) conv_int = conv_int.Replace("eth", "ethernet");
      else if (conv_int.IndexOf("lo") >= 0) conv_int = conv_int.Replace("lo", "loopback");
      return conv_int;
    }
  }

  public class CDPtoVISIOManager : ICustomMenuHandler
  {
    private Form AppMainForm;
    #region ICustomMenuHandler Members

    public ToolStripMenuItem GetMenu()
    {
      ToolStripMenuItem tsmMainMenu = new ToolStripMenuItem();

      #region Menu definition
      tsmMainMenu.Image = Resources.Resources.CreateSchema_8259_32;
      tsmMainMenu.ImageTransparentColor = System.Drawing.Color.Black;
      tsmMainMenu.Name = "CDP2VISIOManager.tsmMainMenu";
      tsmMainMenu.Text = "CDP Network Discovery";
      tsmMainMenu.Click += tsmMainMenu_Click;
      #endregion
      return tsmMainMenu;
    }

    void tsmMainMenu_Click(object sender, EventArgs e)
    {
      CDP2VisioManager frm = new CDP2VisioManager();
      frm.MdiParent = AppMainForm;
      frm.Show();
    }

    public void SetMainForm(Form mainForm)
    {
      this.AppMainForm = mainForm;
    }

    #endregion
  }



}