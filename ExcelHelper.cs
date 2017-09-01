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

using NetOffice.ExcelApi.Enums;
using System.Drawing;
using Excel = NetOffice.ExcelApi;
using VB = Microsoft.VisualBasic;

namespace CDP2VISIO
{
  public static class ExcelHelper
  {
    #region Excel helper functions
    /// <summary>
    /// Convert a system color to Excel color
    /// </summary>
    /// <param name="color">The system color to convert</param>
    /// <returns>Excel color</returns>
    public static XlRgbColor ConvertSytemColorToXlColor(Color color)
    {
      int rgbColor = VB.Information.RGB(color.R, color.G, color.B);
      XlRgbColor xlColor = (XlRgbColor)rgbColor;
      return xlColor;
    }

    /// <summary>
    /// Converts an Excel column ordinal number to column letter
    /// </summary>
    /// <param name="i">Column ordinal number</param>
    /// <returns></returns>
    public static string GetColumnLetter(int i)
    {
      if (i < 26)
      {
        return ((char)(((int)'A') + i)).ToString();
      }
      else
      {
        int j = i / 26 - 1;
        int k = i % 26;
        return GetColumnLetter(j) + GetColumnLetter(k);
      }
    }

    /// <summary>
    /// Returns the specified range object
    /// </summary>
    /// <param name="Sheet">Excel Worksheet reference</param>
    /// <param name="FirstColumn">Range's first columns's identifier, in case of a number this is a 1 based index.</param>
    /// <param name="FirstRow">Range's first row's identifier, in case of a number this is a 1 based index.</param>
    /// <param name="LastColumn">Range's last column's identifier, in case of a number this is a 1 based index.</param>
    /// <param name="LastRow">Range's last row's identifier, in case of a number this is a 1 based index.</param>
    /// <returns></returns>
    public static Excel.Range GetRange(ref Excel._Worksheet Sheet, int FirstColumn, int FirstRow, int LastColumn, int LastRow)
    {
      return Sheet.get_Range(string.Format("{0}{1}", GetColumnLetter(FirstColumn - 1), FirstRow), string.Format("{0}{1}", GetColumnLetter(LastColumn - 1), LastRow));
    }
    public static Excel.Range GetRange(ref Excel._Worksheet Sheet, char FirstColumn, int FirstRow, char LastColumn, int LastRow)
    {
      return Sheet.get_Range(string.Format("{0}{1}", FirstColumn, FirstRow), string.Format("{0}{1}", LastColumn, LastRow));
    }
    public static Excel.Range GetRange(ref Excel._Worksheet Sheet, string FirstColumn, int FirstRow, string LastColumn, int LastRow)
    {
      return Sheet.get_Range(string.Format("{0}{1}", FirstColumn, FirstRow), string.Format("{0}{1}", LastColumn, LastRow));
    }
    #endregion

  }
}
