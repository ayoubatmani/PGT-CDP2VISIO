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
using System;
using System.Diagnostics;
using System.Linq;
using Visio = NetOffice.VisioApi;

namespace CDP2VISIO
{
  /// <summary>
  /// Static class encapsulating basic VISIO functionality for handling Shapes and Connectors
  /// </summary>
  public static class VisioInterface
  {
    public static void ConnectWithDynamicGlueAndConnector(int DeviceNumber, Visio.IVShape shapeFrom, Visio.IVShape shapeTo, string EdgeText, string EdgeIntConfigs, string Edgecolor, string EdgeWeight, int TextSize, string RouteType)
    {
      if (shapeFrom == null || shapeTo == null)
      {
        DebugEx.WriteLine("Can't connect a null shape !");
        return;
      }

      const string BASIC_FLOWCHART_STENCIL = "Basic Flowchart Shapes (US units).vss";
      const string DYNAMIC_CONNECTOR_MASTER = "Dynamic Connector";
      const string MESSAGE_NOT_SAME_PAGE = "Both the shapes are not on the same page.";

      Visio.Application visioApplication;
      Visio.IVDocument stencil;
      Visio.IVMaster masterInStencil;
      Visio.IVShape connector;
      Visio.IVCell beginX;
      Visio.IVCell endX;

      // Get the Application object from the shape.
      visioApplication = (Visio.Application)shapeFrom.Application;
      try
      {
        // Verify that the shapes are on the same page
        if (shapeFrom.ContainingPage != null && shapeTo.ContainingPage != null &&
          shapeFrom.ContainingPage == shapeTo.ContainingPage)
        {
          #region Set ConnectorShapeProperties

          #region InitiateConnector
          // Access the Basic Flowchart Shapes stencil from the
          // Documents collection of the application.
          stencil = visioApplication.Documents.OpenEx(BASIC_FLOWCHART_STENCIL, (short)VisOpenSaveArgs.visOpenDocked);

          // Get the dynamic connector master on the stencil by its
          // universal name.
          masterInStencil = stencil.Masters.get_ItemU(DYNAMIC_CONNECTOR_MASTER);


          // Drop the dynamic connector on the active page.
          connector = visioApplication.ActivePage.Drop(masterInStencil, 0, 0);

          #endregion

          #region SetconnectorToSraightLine
          //Set dynamic cable is a straight line, the key is to know FormulaU this property 16 shows a straight line 
          // Straight Line : 16
          // Right Angle : 1
          string rType = "16";
          if (RouteType == "Right Angle") rType = "1";
          else if (RouteType == "Straight Lines") rType = "16";
          // https://msdn.microsoft.com/en-us/library/office/aa221304(v=office.11).aspx
          connector.get_CellsSRC((short)VisSectionIndices.visSectionObject, (short)VisRowIndices.visRowShapeLayout, (short)VisCellIndices.visSLORouteStyle).FormulaU = rType;
          #endregion

          #region SetConnector_Tiptext
          string connector_Tiptex = string.Empty;
          connector_Tiptex = EdgeIntConfigs;

          //Set the Tiptext
          connector.get_CellsSRC((short)VisSectionIndices.visSectionObject, (short)VisRowIndices.visRowMisc, (short)VisCellIndices.visComment).FormulaU = "\"" + connector_Tiptex.Replace("\"", "") + "\"";

          #endregion

          #region SetConnectorText
          connector.Text = string.Empty;
          connector.Text = EdgeText;
          Visio.IVCharacters shapeText;
          shapeText = connector.Characters;
          shapeText.set_CharProps((short)VisCellIndices.visCharacterSize, (short)TextSize);
          #endregion

          #region SetConnectorColor
          try
          {
            //Set the connection line to "RED" color
            Visio.IVCell thisCell = connector.CellsSRC((short)VisSectionIndices.visSectionObject, (short)VisRowIndices.visRowLine, (short)VisCellIndices.visLineColor);
            if (thisCell != null) thisCell.Formula = Edgecolor;
            else DebugEx.WriteLine("CDP2VISIO : Couldn't set connector color, Cell is null", DebugLevel.Warning);
          }
          catch (Exception Ex)
          {
            DebugEx.WriteLine(string.Format("CDP2VISIO : Couldn't set connector color : {0} {1}", Ex.InnerExceptionsMessage(), Ex.InnerException?.Message), DebugLevel.Warning);
          }
          #endregion

          #region SetConnectorWeight
          try
          {
            Visio.IVCell thisCell = connector.get_CellsU("LineWeight");
            if (thisCell != null) thisCell.FormulaU = EdgeWeight;
            else DebugEx.WriteLine("CDP2VISIO : Couldn't set connector weight, Cell is null", DebugLevel.Warning);
          }
          catch (Exception Ex)
          {
            DebugEx.WriteLine("CDP2VISIO : Couldn't set connector weight : " + Ex.InnerExceptionsMessage(), DebugLevel.Warning);
          }
          #endregion

          #region SetConnectorSource&TargetShape
          try
          {
            // Connect the begin point of the dynamic connector to the
            // PinX cell of the first 2-D shape.
            beginX = connector.CellsSRC(
              (short)VisSectionIndices.visSectionObject,
              (short)VisRowIndices.visRowXForm1D,
              (short)VisCellIndices.vis1DBeginX);

            if (beginX != null)
            {
              beginX.GlueTo(shapeFrom.get_CellsSRC(
                (short)VisSectionIndices.visSectionObject,
                (short)VisRowIndices.visRowXFormOut,
                (short)VisCellIndices.visXFormPinX));
            }
            else DebugEx.WriteLine("CDP2VISIO : Couldn't glue connector origin", DebugLevel.Warning);
            // Connect the end point of the dynamic connector to the
            // PinX cell of the second 2-D shape.
            endX = connector.get_CellsSRC(
              (short)VisSectionIndices.visSectionObject,
              (short)VisRowIndices.visRowXForm1D,
              (short)VisCellIndices.vis1DEndX);
            if (endX != null)
            {
              endX.GlueTo(shapeTo.get_CellsSRC(
                (short)VisSectionIndices.visSectionObject,
                (short)VisRowIndices.visRowXFormOut,
                (short)VisCellIndices.visXFormPinX));
              DebugEx.WriteLine(string.Format("Glued {0} to {1}", shapeFrom.Name, shapeTo.Name), DebugLevel.Informational);
            }
            else DebugEx.WriteLine("CDP2VISIO : Couldn't glue connector end", DebugLevel.Warning);
          }
          catch (Exception Ex)
          {
            DebugEx.WriteLine("CDP2VISIO : Couldn't glue connector : " + Ex.InnerExceptionsMessage(), DebugLevel.Warning);
          }
          #endregion

          #endregion
        }
        else
        {
          // Processing cannot continue because the shapes are not on 
          // the same page.
          System.Diagnostics.DebugEx.WriteLine(MESSAGE_NOT_SAME_PAGE);
        }
      }

      catch (Exception Ex)
      {
        System.Diagnostics.DebugEx.WriteLine(Ex.InnerExceptionsMessage());
        throw;
      }
    }
    //CONNECT TO SHAPES
    public static Visio.IVShape CreateVisioShape(Visio.Application vApp, Visio.IVDocument drawingDocuemnt, double X_in, double Y_in, string stencilName, string masterNameU)
    {
      // Find the stencil in the Documents collection by name.
      Visio.IVDocuments  visioDocuments = vApp.Documents;
      Visio.IVShape droppedShape = null;
      Visio.IVMaster masterInStencil;

      Visio.IVPage page = drawingDocuemnt.Pages.First();

      //get width and height of the sheet
      double Sheet_Width = page.PageSheet.get_CellsU("PageWidth").ResultIU;
      double Sheet_Height = page.PageSheet.get_CellsU("PageHeight").ResultIU;
      Sheet_Width = Sheet_Width - 3;
      Sheet_Height = Sheet_Height - 3;

      Visio.IVDocument stencil;
      try
      {
        stencil = visioDocuments[stencilName];
      }
      catch (System.Runtime.InteropServices.COMException)
      {
        // The stencil is not in the collection; open it as a 
        // docked stencil.
        stencil = visioDocuments.OpenEx(stencilName, (short)VisOpenSaveArgs.visOpenDocked);
      }

      // Get a master from the stencil by its universal name.
      try
      {
        masterInStencil = stencil.Masters.get_ItemU(masterNameU);
        double actual_xposition = (Sheet_Width * X_in) + 2;
        double actual_yposition = (Sheet_Height * Y_in) + 2;
        droppedShape = page.Drop(masterInStencil, actual_xposition, actual_yposition);
      }
      catch (Exception Ex)
      {
        System.Windows.Forms.MessageBox.Show(string.Format("Cannot create Visio shape. Error is : {0} {1}", Ex.Message, Ex.InnerException?.Message), "Visio interface error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Hand);
      }
      return droppedShape;
    }

    public static void BrigtoFront(Visio.IVShape input_shape)
    {
      input_shape?.BringToFront();
    }
  }
}
