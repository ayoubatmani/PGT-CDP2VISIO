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
using System.Diagnostics;
using System.Linq;

namespace CDP2VISIO
{
  public class CDPParser
  {
    private IScriptExecutorBase _Executor;
    private DeviceConnectionInfo _ConnectionInfo;
    private bool _AllowRecursion;
    private const string nocdpip = "no cdp ip";

    public CDPParser(IScriptExecutorBase Executor, DeviceConnectionInfo ConnectionInfo, bool AllowRecursion)
    {
      _Executor = Executor;
      _ConnectionInfo = ConnectionInfo;
      _AllowRecursion = AllowRecursion;
    }

    public virtual void ProcessCDPResult(string command_result, CDPDataSet.DevicesRow parent_device, CDPDataSet ds)
    {
      if (command_result.IndexOf("CDP is not enabled") >= 0)
      {
        parent_device.CDP_status = false;
      }
      else
      {
        string[] cdp_lines = command_result.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        CDPDataSet.DevicesRow current_device = null; // As going through the CDP output lines, this is the actual neighbor
        string currentDeviceName = "";
        string currentDeviceIP = "";
        CDPDataSet.NeighboursRow new_neigbor = null; // This is the neighbor created for current_device as a neighbor of parent_device
        PGTDataSet.ScriptSettingRow ScriptSettings = SettingsManager.GetCurrentScriptSettings();
        // As the ip address must be unique for each device, adding a device with ip address of nocdpip constant would fail for the second time
        // To overcome this issue, we do indexing for devices with no ip address (such as VMware ESX hosts)
        int nocdpip_index = 0;
        foreach (string line in cdp_lines)
        {
          DebugEx.WriteLine(String.Format("CDP2VISIO : parsing cdp neighbor row [ {0} ]", line), DebugLevel.Full);
          try
          {
            bool isVMWareESX = false;

            #region Check for DeviceID line and set currentDeviceName accordingly
            if (line.IndexOf(DeviceID()) >= 0)
            {
              try
              {
                if (new_neigbor != null) ds.Neighbours.AddNeighboursRow(new_neigbor);
              }
              catch (Exception Ex)
              {
                // Depending on discovery list, under special circumstances it can happen that we try to add a new neighbor row 
                // with the same connection parameters (same parent, neighbor and interfaces) that will violate unique key constraint
                DebugEx.WriteLine(String.Format("CDP2VISIO : Error storing neighbor row : {0}", Ex.InnerExceptionsMessage()), DebugLevel.Warning);
              }
              new_neigbor = null;
              string[] words = line.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
              currentDeviceName = words[2].Trim(); ;
              currentDeviceIP = "";
              DebugEx.WriteLine(String.Format("CDP2VISIO : CDPParser found a new neighbor : {0}", currentDeviceName), DebugLevel.Informational);
            }
            #endregion

            if (currentDeviceName == "") continue;

            #region Check for IPAddress line and set currentDeviceIP accordingly
            if (line.IndexOf(IPAddress()) >= 0)
            {
              string[] words_in_line = line.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
              currentDeviceIP = words_in_line[2].Trim();
              if (currentDeviceIP == "")
              {
                DebugEx.WriteLine(String.Format("CDP2VISIO : cdp is not reporting ip address for neighbor {0}", currentDeviceName), DebugLevel.Debug);
                currentDeviceIP = string.Format("{0}_{1}", nocdpip, nocdpip_index);
                nocdpip_index++;
              }
            }
            #endregion

            #region Check whether the Platform is VMware ESX
            if (line.Trim().StartsWith("Platform:"))
            {
              string[] words = line.SplitByComma();
              // words[0] should be PlatForm, words[1] is Capabilities
              isVMWareESX = words[0].ToLowerInvariant().IndexOf("vmware esx") >= 0;
              if (isVMWareESX)
              {
                DebugEx.WriteLine(String.Format("CDP2VISIO : neighbor {0} is an ESX host", currentDeviceName), DebugLevel.Debug);
                currentDeviceIP = string.Format("{0}_{1}", nocdpip, nocdpip_index);
                nocdpip_index++;
              }
            }
            #endregion

            if (currentDeviceIP == "") continue;

            #region Add current device as new device or select existing device from Devices table
            // At this point we can identify the current device(cdp neighbor) by name and ip. This is also a new neighbor
            // Two devices considered identical if :
            // - have the same hostname, or
            // - have the same IPAddress
            string currentDeviceHostName = DottedNameSpace.TLD(currentDeviceName);
            current_device = ds.Devices.FirstOrDefault(d =>
              currentDeviceIP != nocdpip && d.IP_Address.SplitBySemicolon().Any(thisIP => thisIP == currentDeviceIP) ||
              d.Name.SplitBySemicolon().Any(thisName => DottedNameSpace.CompateTLD(thisName, currentDeviceHostName))
            );
            if (current_device == null)
            {
              DebugEx.WriteLine(String.Format("CDP2VISIO : neighbor {0} is a new device, adding to devices data table", currentDeviceName), DebugLevel.Informational);
              // This device is not yet known. Add to device list and also to list of devices in script
              current_device = ds.Devices.NewDevicesRow();
              //if (current_device.Name != currentDeviceName) current_device.Name += ";" + currentDeviceName;
              //else 
              current_device.Name = currentDeviceName;
              if (current_device.IsIP_AddressNull() || current_device.IP_Address == "") current_device.IP_Address = currentDeviceIP;
              else if (current_device.IP_Address != currentDeviceIP) current_device.IP_Address += ";" + currentDeviceIP;
              ds.Devices.AddDevicesRow(current_device);
              // Add a new entry for discovered device if has a valid neighbor ip, Recursion is allowed
              // and this ip is not defined as discovery boundary
              if (_AllowRecursion && !currentDeviceIP.StartsWith(nocdpip))
              {
                var includedAddresses = (from addressdefinition in ds.DomainBoundary where addressdefinition.Action == BoundaryAddressAction.Include.ToString() select addressdefinition.IP_Address)?.ToList();
                var excludedAddresses = (from addressdefinition in ds.DomainBoundary where addressdefinition.Action == BoundaryAddressAction.Exclude.ToString() select addressdefinition.IP_Address)?.ToList();
                if (IPOperations.IsIPAddressInNetworks(currentDeviceIP, includedAddresses, true) && !IPOperations.IsIPAddressInNetworks(currentDeviceIP, excludedAddresses, false))
                {
                  string[] newScriptLine = _ConnectionInfo.ScriptLine.Split(ScriptSettings.CSVSeparator.ToCharArray());
                  newScriptLine[2] = currentDeviceIP;
                  newScriptLine[3] = currentDeviceName;
                  _Executor.AddScriptEntry(string.Join(ScriptSettings.CSVSeparator, newScriptLine));
                  string msg = string.Format("Added device <{0}> to discovery list.", currentDeviceIP);
                  DebugEx.WriteLine(msg, DebugLevel.Informational);
                  _Executor.ShowActivity(msg);
                }
                else
                {
                  string msg = string.Format("Not adding device <{0}> to discovery list because it is either explicitly excluded or not included in discovery domain", currentDeviceIP);
                  DebugEx.WriteLine(msg, DebugLevel.Full);
                  _Executor.ShowActivity(msg);
                }
              }
              else
              {
                string msg = "Not adding device a new neighbor to discovery list because Active Discovery is not allowed or the neighbor does not have a valid ip address detected.";
                DebugEx.WriteLine(msg, DebugLevel.Full);
              }
            }
            else DebugEx.WriteLine(String.Format("CDP2VISIO : neighbor {0} is already a known device", currentDeviceName), DebugLevel.Full);
            #endregion

            if (current_device == null) continue;

            #region Create Neighbor for parent_device <-> current_device
            if (new_neigbor == null)
            {
              new_neigbor = ds.Neighbours.NewNeighboursRow();
              new_neigbor.Neighbor_ID = current_device.ID;
              new_neigbor.Parent_ID = parent_device.ID;
              new_neigbor.Name = current_device.Name;
              DebugEx.WriteLine(String.Format("CDP2VISIO : new neighbor {0} added for device {1}", currentDeviceName, parent_device.Name), DebugLevel.Full);
            }
            #endregion

            #region  Get Platform/Interfaces info and update the neighbor

            if (line.Trim().StartsWith("Platform") && new_neigbor != null)
            {
              string[] words = line.SplitByComma();
              // words[0] should be PlatForm, words[1] is Capabilities
              string[] platformWords = words[0].SplitBySpace();
              string[] capabilities = words[1].SplitBySpace();
              current_device.Platform = string.Join(" ", platformWords.SkipWhile((string l, int i) => i < 1));
              if (current_device.Type != "VSS" && current_device.Type != "ASA" && !current_device.Type.StartsWith("Stack")) current_device.Type = string.Join(";", capabilities.SkipWhile((s, i) => i < 1));
              DebugEx.WriteLine(String.Format("CDP2VISIO : Platform of neighbor {0} identified as {1}. Device type was set to {2}", currentDeviceName, current_device.Platform, current_device.Type), DebugLevel.Full);
            }

            if (line.IndexOf("Interface") >= 0 && new_neigbor != null)
            {
              string[] words_in_line = line.SplitBySpace();
              int words_length = words_in_line.Length;
              string neighbour_interfaces = words_in_line[words_length - 1];
              neighbour_interfaces = Common.ConvInt(neighbour_interfaces);

              string local_interfaces = words_in_line[1].Replace(",", "");
              local_interfaces = Common.ConvInt(local_interfaces);

              new_neigbor.Neighbour_Interface = neighbour_interfaces;
              new_neigbor.Local_Interface = local_interfaces;
              DebugEx.WriteLine(String.Format("CDP2VISIO : connected interface added {0}::{1} connects to  {2}::{3}", currentDeviceName, local_interfaces, parent_device.Name, neighbour_interfaces), DebugLevel.Full);
            }
            #endregion
          }
          catch (Exception Ex)
          {
            DebugEx.WriteLine(String.Format("CDP2VISIO : Error while parsing cdp output line [{0}]. Error is : {1}", line, Ex.InnerExceptionsMessage()));
          }
        }
        if (new_neigbor != null) ds.Neighbours.AddNeighboursRow(new_neigbor);
      }
    }

    public virtual string DeviceID()
    {
      throw new NotImplementedException();
    }

    public virtual string IPAddress()
    {
      throw new NotImplementedException();
    }
  }

  public class IOS_CDPParser : CDPParser
  {
    public IOS_CDPParser(IScriptExecutorBase Executor, DeviceConnectionInfo ConnectionInfo, bool AllowRecursion) : base(Executor, ConnectionInfo, AllowRecursion) { }

    public override string DeviceID()
    {
      return "Device ID:";
    }
    public override string IPAddress()
    {
      return "IP address:";
    }
  }

  public class NXOS_CDPParser : CDPParser
  {
    public NXOS_CDPParser(IScriptExecutorBase Executor, DeviceConnectionInfo ConnectionInfo, bool AllowRecursion) : base(Executor, ConnectionInfo, AllowRecursion) { }
    public override string DeviceID()
    {
      return "System Name:";
    }
    public override string IPAddress()
    {
      return "address:";
    }
  }
}
