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
using System.Collections.Generic;
using System.Linq;

namespace CDP2VISIO
{
  #region Edge class definition
  public class Edge
  {
    public int Source_ID;
    public int Target_ID;
    public string SourceName;
    public string TargetName;
    public string ConnectionText;
    private List<string> Source_Interfaces;
    private List<string> Target_Interafecs;
    public string ConnectionConfig;
    public string line_color;
    public string line_weight;
    public int TextSize;



    public void CalculateEdge(CDPDataSet ds)
    {
      Source_Interfaces = new List<string>();
      Target_Interafecs = new List<string>();
      CalculateEdgeText(ds);
      CalculateEdgeRuningConfig(ds);
      CalculateNames(ds);
      CalculateWeight(ds);
    }

    #region Edge Sourec/Target Name
    private void CalculateNames(CDPDataSet ds)
    {
      var query_source_name = from row in ds.Devices
                              where (row.ID == Source_ID)
                              select (row);
      foreach (var mem in query_source_name)
      {
        SourceName = mem.Name;
      }

      var query_target_name = from row in ds.Devices
                              where (row.ID == Target_ID)
                              select (row);
      foreach (var mem in query_target_name)
      {
        TargetName = mem.Name;
      }

    }
    #endregion

    #region Edge Text/Color Calculation
    private void CalculateEdgeText(CDPDataSet ds)
    {

      var edges= from row in ds.Neighbours where ((row.Parent_ID == Source_ID) & (row.Neighbor_ID == Target_ID)) select row;
      bool check = true;
      if (edges.Count() > 1)
      {
        ConnectionText = "Multiple Connection";
        line_color = "RGB(255,0,0)";
        check = false;
      }

      foreach (var thisEdge in edges)
      {
        string source_name = ds.Devices.FindByID(Source_ID).Name;
        string target_name = thisEdge.Name;
        Source_Interfaces.Add(thisEdge.Local_Interface);
        Target_Interafecs.Add(thisEdge.Neighbour_Interface);
        if (check)
        {
          ConnectionText = string.Format("{0} - {1}\r\n{2} - {3}", DottedNameSpace.TLD(source_name), thisEdge.Local_Interface, DottedNameSpace.TLD(target_name), thisEdge.Neighbour_Interface);
          line_color = "RGB(0,128,0)";
        }

      }

    }
    #endregion

    #region Edge Config Calculation

    private void CalculateEdgeRuningConfig(CDPDataSet ds)
    {
      string source_interf_config = string.Empty;
      string target_interf_config = string.Empty;
      #region source_int_config
      var query_source_int_config = from row in ds.Devices
                                    join interf in ds.Interfaces on row.ID equals interf.ID
                                    where (row.ID == Source_ID)
                                    select new
                                    {
                                      name = row.Name,
                                      int_name = interf.Name,
                                      inter_conf = interf.Running_config
                                    };
      foreach (var mem in query_source_int_config)
      {
        if (Source_Interfaces.Contains(mem.int_name))
        {
          source_interf_config = source_interf_config + mem.name + ": \r\n";
          source_interf_config = source_interf_config + mem.inter_conf + "\r\n";
        }
      }
      #endregion
      #region target_int_config
      var query_target_int_config = from row in ds.Devices
                                    join interf in ds.Interfaces on row.ID equals interf.ID
                                    where (row.ID == Target_ID)
                                    select new
                                    {
                                      name = row.Name,
                                      int_name = interf.Name,
                                      inter_conf = interf.Running_config
                                    };
      foreach (var mem in query_target_int_config)
      {
        if (Target_Interafecs.Contains(mem.int_name))
        {
          target_interf_config = target_interf_config + mem.name + ": \r\n";
          target_interf_config = target_interf_config + mem.inter_conf + "\r\n";
        }
      }
      #endregion

      ConnectionConfig = source_interf_config + "\r\n" + target_interf_config;
    }
    #endregion

    #region Calculate eWeight&TextSize
    private void CalculateWeight(CDPDataSet ds)
    {
      if (ds.Devices.Count() < 5)
      {
        if (ConnectionText.IndexOf("Multiple Connection") >= 0)
        {
          line_weight = "5pt";
          TextSize = 10;
        }
        else
        {
          line_weight = "2pt";
          TextSize = 8;
        }
      }
      else
      {
        if (ConnectionText.IndexOf("Multiple Connection") >= 0)
        {
          line_weight = "1pt";
          TextSize = 6;
        }
        else
        {
          line_weight = "1pt";
          TextSize = 5;
        }
      }
    }
    #endregion
  }

  #endregion

}
