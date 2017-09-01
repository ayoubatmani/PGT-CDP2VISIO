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



namespace CDP2VISIO
{
  public static class Common
  {
    #region IntConverter Function
    /// <summary>
    /// Converts long interface names to short, like FastEthernet to Fa
    /// </summary>
    /// <param name="input_interface">Long interface name</param>
    /// <returns>The short name for the interface</returns>
    public static string ConvInt(string input_interface)
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

    /// <summary>
    /// Converts short interface names to long one, like Fa to FastEthernet
    /// </summary>
    /// <param name="input_interface">Short interface name</param>
    /// <returns>The long name for the interface</returns>
    public static string RevConvInt(string input_interface)
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
    #endregion
  }
}
