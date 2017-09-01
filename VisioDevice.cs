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


using NetOffice.VisioApi.Enums;
using PGT.Common;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Visio = NetOffice.VisioApi;

namespace CDP2VISIO
{
  /// <summary>
  /// Wrapper class around Visio.IVShape class
  /// </summary>
  public class Visio_Device
  {
    /// <summary>
    /// The full name of Cisco device as discovered by CDP
    /// </summary>
    public string FullName;

    /// <summary>
    /// The ID of the device in the Devices data table
    /// </summary>
    public int DeviceID { get; set; }

    /// <summary>
    /// Display label of the Visio Shape
    /// </summary>
    public string Label { get; set; }
    /// <summary>
    /// IP addresses discovered for the device
    /// </summary>
    public string IPAddress { get; set; }
    /// <summary>
    /// This is the device type as reported by CDP
    /// </summary>
    public string DeviceType { get; set; }
    /// <summary>
    /// Device platform reported by CDP
    /// </summary>
    public string Platform { get; set; }
    /// <summary>
    /// The output of sh inventory command
    /// </summary>
    public string Inventory { get; set; }
    /// <summary>
    /// Output of sh version command
    /// </summary>
    public string VersionInfo { get; set; }
    /// <summary>
    /// System serial number taken from inventory or version information
    /// </summary>
    public string SystemSerial { get; set; }
    public string VLANInformation { get; set; }
    public string L3InterfaceInformation { get; set; }
    public double X_coord { get; set; }
    public double Y_coord { get; set; }
    public List<Visio_Device> neighbours_list { get; set; }
    public Visio.IVShape Visio_shape { get; private set; }
    public string stencilName { get; set; }
    public string masterNameU { get; set; }
    public void CreateShape(Visio.Application VApp, Visio.IVDocument drawingDocument)
    {
      Visio_shape = VisioInterface.CreateVisioShape(VApp, drawingDocument, X_coord, Y_coord, stencilName, masterNameU);
      if (Visio_shape == null) DebugEx.WriteLine(string.Format("Error creating Visio shape for {0}", FullName));
    }

    public void LinkShapeToData(int datarowSetID, int datarowID)
    {
      Visio_shape?.LinkToData(datarowSetID, datarowID, false);
    }

    public void SetShapeProperties(int DeviceNumber, string VersionInfo)
    {
      if (Visio_shape != null)
      {
        bool set_size = true;
        short text_size = 0;
        int shape_width = 0;
        int shape_height = 0;

        #region Calculate font size
        if (DeviceNumber <= 2)
        {
          text_size = 10;
          shape_width = 1;
          shape_height = 1;
        }
        else
        {
          set_size = false;
          text_size = 8;
        }
        #endregion

        #region Set shape text
        Visio_shape.Text = Label;
        Visio.IVCharacters shapeText;
        shapeText = Visio_shape.Characters;
        shapeText.set_CharProps((short)VisCellIndices.visCharacterSize, text_size);
        #endregion

        //Set the device text to Red if it is Inlist
        //if (inlist == "inlist") shapeText.set_CharProps((short)VisCellIndices.visCharacterColor, 2);

        #region Set shape width
        if (set_size)
        {
          Visio_shape.get_CellsSRC(
             (short)VisSectionIndices.visSectionObject,
             (short)VisRowIndices.visRowXFormIn,
             (short)VisCellIndices.visXFormWidth).ResultIU = shape_width; //shape Width

          Visio_shape.get_CellsSRC(
                   (short)VisSectionIndices.visSectionObject,
                   (short)VisRowIndices.visRowXFormIn,
                   (short)VisCellIndices.visXFormHeight).ResultIU = shape_height; //shape Height
        }
        #endregion

        #region  Set the ScreenTip text

        string ToolTipText = string.Format("{0} ({1})\r\n{2}\r\n{3}", Platform, SystemSerial, IOSVersion(), ""/*L3InterfaceInformation*/).Replace("\"", "");
        Visio_shape.get_CellsSRC((short)VisSectionIndices.visSectionObject,
        (short)VisRowIndices.visRowMisc,
        (short)VisCellIndices.visComment).FormulaU = string.Format("\"{0}\"", ToolTipText);
        #endregion
      }
    }

    /// <summary>
    /// Returns the IOS Version information for this instance
    /// </summary>
    /// <returns></returns>
    public string IOSVersion()
    {
      return IOSVersion(VersionInfo);
    }

    /// <summary>
    /// Parse the text returned by the "sh version" command to get the IOS version part of it
    /// </summary>
    /// <param name="RawVersion"></param>
    /// <returns></returns>
    public static string IOSVersion(string RawVersion)
    {
      string _IOSName = " n/a ";
      string _IOSVersion = " n/a ";
      var _versions = RawVersion.SplitByLine().Where(l => l.Trim().StartsWith("Cisco")).Select(l => l.Trim());
      if (_versions.Count() > 0)
      {
        var CiscoVersionInfo = _versions.ElementAt(0).SplitByComma().Select(l => l.Trim()).ToList();
        _IOSName = CiscoVersionInfo.Count() > 0 ? CiscoVersionInfo[0] : " n/c ";
        _IOSVersion = CiscoVersionInfo.Count() > 2 ? CiscoVersionInfo[2] : " n/c ";
      }
      return string.Format("{0}, {1}", _IOSName, _IOSVersion);
    }

    /// <summary>
    /// Returns the InventoryInformation for this instance
    /// </summary>
    /// <returns></returns>
    public List<string[]> InventoryInformation()
    {
      return InventoryInformation(this.Inventory);
    }

    /// <summary>
    /// Parse the text returned by "sh inventory" command to get the list of items
    /// </summary>
    /// <param name="RawInventory"></param>
    /// <returns></returns>
    public static List<string[]> InventoryInformation(string RawInventory)
    {
      List<string[]> result = new List<string[]>();
      try
      {
        string[] PIDLines = RawInventory.SplitByLine().Where(l => l.StartsWith("PID:")).ToArray();
        string[] NameLines = RawInventory.SplitByLine().Where(l => l.StartsWith("NAME:")).ToArray();
        bool NamesMatched = PIDLines.Length == NameLines.Length;
        int PIDCounter = 0;
        foreach (string thisPID in PIDLines)
        {
          string[] PIDWords = thisPID.Split(',').Select(s => s.Trim()).ToArray();
          if (PIDWords.Length == 3)
          {
            string[] aPID = PIDWords[0].Split(':').Select(s => s.Trim(" \"".ToCharArray())).ToArray();
            string[] aSN = PIDWords[2].Split(':').Select(s => s.Trim(" \"".ToCharArray())).ToArray();
            if (aPID[1] == "") aPID[1] = "System";
            if (aPID.Length == 2 && aSN.Length == 2)
            {
              string ModelNumber = aPID[1];
              string Serial = aSN[1];
              string PIDName = "";
              string PIDDescription = "";
              if (NamesMatched)
              {
                string[] PIDNameWords = NameLines[PIDCounter].SplitByComma();
                PIDName = (PIDNameWords[0].Split(':')[1]).Trim(" \"".ToCharArray()); // Don't need the NAME: tag
                PIDDescription = (PIDNameWords[1].Split(':')[1]).Trim(" \"".ToCharArray()); // Don't need the DESCR: tag
              }
              result.Add(new string[]
              {
                ModelNumber,
                PIDName,
                PIDDescription,
                Serial
              });
            }
          }
          PIDCounter++;
        }
      }
      catch
      {

      }
      return result;
    }

  }

}
