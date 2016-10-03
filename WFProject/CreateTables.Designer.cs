namespace WFProject
{
    partial class frCreateTables
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
            this.lbTableName = new System.Windows.Forms.Label();
            this.txtTableName = new System.Windows.Forms.TextBox();
            this.lblColumn = new System.Windows.Forms.Label();
            this.txtColumn1 = new System.Windows.Forms.TextBox();
            this.btnCreateTable = new System.Windows.Forms.Button();
            this.txtColumn2 = new System.Windows.Forms.TextBox();
            this.txtColumn0 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lbTableName
            // 
            this.lbTableName.AutoSize = true;
            this.lbTableName.Location = new System.Drawing.Point(36, 39);
            this.lbTableName.Name = "lbTableName";
            this.lbTableName.Size = new System.Drawing.Size(41, 12);
            this.lbTableName.TabIndex = 0;
            this.lbTableName.Text = "表名：";
            // 
            // txtTableName
            // 
            this.txtTableName.Location = new System.Drawing.Point(83, 36);
            this.txtTableName.Name = "txtTableName";
            this.txtTableName.Size = new System.Drawing.Size(168, 21);
            this.txtTableName.TabIndex = 1;
            this.txtTableName.Text = "test";
            // 
            // lblColumn
            // 
            this.lblColumn.AutoSize = true;
            this.lblColumn.Location = new System.Drawing.Point(36, 71);
            this.lblColumn.Name = "lblColumn";
            this.lblColumn.Size = new System.Drawing.Size(41, 12);
            this.lblColumn.TabIndex = 2;
            this.lblColumn.Text = "列名：";
            // 
            // txtColumn1
            // 
            this.txtColumn1.Location = new System.Drawing.Point(83, 100);
            this.txtColumn1.Name = "txtColumn1";
            this.txtColumn1.Size = new System.Drawing.Size(168, 21);
            this.txtColumn1.TabIndex = 3;
            this.txtColumn1.Text = "A";
            // 
            // btnCreateTable
            // 
            this.btnCreateTable.Location = new System.Drawing.Point(240, 171);
            this.btnCreateTable.Name = "btnCreateTable";
            this.btnCreateTable.Size = new System.Drawing.Size(75, 23);
            this.btnCreateTable.TabIndex = 6;
            this.btnCreateTable.Text = "创建表";
            this.btnCreateTable.UseVisualStyleBackColor = true;
            this.btnCreateTable.Click += new System.EventHandler(this.btnCreateTable_Click);
            // 
            // txtColumn2
            // 
            this.txtColumn2.Location = new System.Drawing.Point(83, 136);
            this.txtColumn2.Name = "txtColumn2";
            this.txtColumn2.Size = new System.Drawing.Size(168, 21);
            this.txtColumn2.TabIndex = 5;
            this.txtColumn2.Text = "B";
            // 
            // txtColumn0
            // 
            this.txtColumn0.Location = new System.Drawing.Point(83, 68);
            this.txtColumn0.Name = "txtColumn0";
            this.txtColumn0.Size = new System.Drawing.Size(168, 21);
            this.txtColumn0.TabIndex = 7;
            this.txtColumn0.Text = "id";
            // 
            // frCreateTables
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(327, 206);
            this.Controls.Add(this.txtColumn0);
            this.Controls.Add(this.txtColumn2);
            this.Controls.Add(this.btnCreateTable);
            this.Controls.Add(this.txtColumn1);
            this.Controls.Add(this.lblColumn);
            this.Controls.Add(this.txtTableName);
            this.Controls.Add(this.lbTableName);
            this.Name = "frCreateTables";
            this.Text = "创建表";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbTableName;
        private System.Windows.Forms.TextBox txtTableName;
        private System.Windows.Forms.Label lblColumn;
        private System.Windows.Forms.TextBox txtColumn1;
        private System.Windows.Forms.Button btnCreateTable;
        private System.Windows.Forms.TextBox txtColumn2;
        private System.Windows.Forms.TextBox txtColumn0;
    }
}