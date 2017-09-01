using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CDP2VISIO
{
  public partial class AddStencilForm : Form
  {
    public AddStencilForm()
    {
      InitializeComponent();
    }
    public string PIDName { get { return textBox_PID.Text; } }
    public string StencilName { get { return textBox_stencil_name.Text; } }
    public string ShapeName { get { return textBox_shapename.Text; } }
    private void textBox_PID_Validating(object sender, CancelEventArgs e)
    {
      if (DialogResult == System.Windows.Forms.DialogResult.OK)
      {
        e.Cancel = string.IsNullOrEmpty(textBox_PID.Text);
        if (e.Cancel) MessageBox.Show("PID name is required", "Value missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
      }
    }
    private void textBox_stencil_name_Validating(object sender, CancelEventArgs e)
    {
      if (DialogResult == System.Windows.Forms.DialogResult.OK)
      {
        e.Cancel = string.IsNullOrEmpty(textBox_stencil_name.Text);
        if (e.Cancel) MessageBox.Show("Stencil name is required", "Value missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
      }
    }
    private void textBox_shapename_Validating(object sender, CancelEventArgs e)
    {
      if (DialogResult == System.Windows.Forms.DialogResult.OK)
      {
        e.Cancel = string.IsNullOrEmpty(textBox_shapename.Text);
        if (e.Cancel) MessageBox.Show("Shape name is required", "Value missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
      }
    }
    private void button2_Click(object sender, EventArgs e)
    {
      if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK) textBox_stencil_name.Text = openFileDialog1.FileName;
    }

    private void AddStencilForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (DialogResult == System.Windows.Forms.DialogResult.OK)
      {
        e.Cancel = !ValidateChildren();
      }
    }
  }
}
