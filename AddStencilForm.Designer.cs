namespace CDP2VISIO
{
  partial class AddStencilForm
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddStencilForm));
      this.label3 = new System.Windows.Forms.Label();
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.textBox_shapename = new System.Windows.Forms.TextBox();
      this.textBox_PID = new System.Windows.Forms.TextBox();
      this.textBox_stencil_name = new System.Windows.Forms.TextBox();
      this.button2 = new System.Windows.Forms.Button();
      this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
      this.btnOK = new System.Windows.Forms.Button();
      this.btnCancel = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(25, 71);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(67, 13);
      this.label3.TabIndex = 33;
      this.label3.Text = "Shape name";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(25, 16);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(56, 13);
      this.label1.TabIndex = 31;
      this.label1.Text = "PID Name";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(25, 42);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(78, 13);
      this.label2.TabIndex = 32;
      this.label2.Text = "Visio stencil file";
      // 
      // textBox_shapename
      // 
      this.textBox_shapename.Location = new System.Drawing.Point(116, 64);
      this.textBox_shapename.Name = "textBox_shapename";
      this.textBox_shapename.Size = new System.Drawing.Size(270, 20);
      this.textBox_shapename.TabIndex = 30;
      this.textBox_shapename.Validating += new System.ComponentModel.CancelEventHandler(this.textBox_shapename_Validating);
      // 
      // textBox_PID
      // 
      this.textBox_PID.Location = new System.Drawing.Point(116, 12);
      this.textBox_PID.Name = "textBox_PID";
      this.textBox_PID.Size = new System.Drawing.Size(270, 20);
      this.textBox_PID.TabIndex = 28;
      this.textBox_PID.Validating += new System.ComponentModel.CancelEventHandler(this.textBox_PID_Validating);
      // 
      // textBox_stencil_name
      // 
      this.textBox_stencil_name.Location = new System.Drawing.Point(117, 38);
      this.textBox_stencil_name.Name = "textBox_stencil_name";
      this.textBox_stencil_name.ReadOnly = true;
      this.textBox_stencil_name.Size = new System.Drawing.Size(228, 20);
      this.textBox_stencil_name.TabIndex = 29;
      this.textBox_stencil_name.Validating += new System.ComponentModel.CancelEventHandler(this.textBox_stencil_name_Validating);
      // 
      // button2
      // 
      this.button2.BackColor = System.Drawing.Color.SteelBlue;
      this.button2.Location = new System.Drawing.Point(361, 37);
      this.button2.Name = "button2";
      this.button2.Size = new System.Drawing.Size(25, 23);
      this.button2.TabIndex = 35;
      this.button2.Text = "...";
      this.button2.UseVisualStyleBackColor = false;
      this.button2.Click += new System.EventHandler(this.button2_Click);
      // 
      // openFileDialog1
      // 
      this.openFileDialog1.FileName = "openFileDialog1";
      this.openFileDialog1.Filter = "Visio stencils|*.vss|All files|*.*";
      // 
      // btnOK
      // 
      this.btnOK.BackColor = System.Drawing.Color.SteelBlue;
      this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnOK.Location = new System.Drawing.Point(311, 97);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new System.Drawing.Size(75, 23);
      this.btnOK.TabIndex = 34;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = false;
      // 
      // btnCancel
      // 
      this.btnCancel.BackColor = System.Drawing.Color.SteelBlue;
      this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnCancel.Location = new System.Drawing.Point(28, 97);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(75, 23);
      this.btnCancel.TabIndex = 36;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = false;
      // 
      // AddStencilForm
      // 
      this.AcceptButton = this.btnOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.SystemColors.ActiveCaption;
      this.CancelButton = this.btnCancel;
      this.ClientSize = new System.Drawing.Size(419, 130);
      this.Controls.Add(this.btnCancel);
      this.Controls.Add(this.button2);
      this.Controls.Add(this.btnOK);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.textBox_shapename);
      this.Controls.Add(this.textBox_PID);
      this.Controls.Add(this.textBox_stencil_name);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "AddStencilForm";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Add new Visio stencil/shape";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AddStencilForm_FormClosing);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox textBox_shapename;
    private System.Windows.Forms.TextBox textBox_PID;
    private System.Windows.Forms.TextBox textBox_stencil_name;
    private System.Windows.Forms.Button button2;
    private System.Windows.Forms.OpenFileDialog openFileDialog1;
    private System.Windows.Forms.Button btnOK;
    private System.Windows.Forms.Button btnCancel;
  }
}