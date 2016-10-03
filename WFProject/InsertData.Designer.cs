namespace WFProject
{
    partial class frInsertData
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
            this.lblTable = new System.Windows.Forms.Label();
            this.cmbTable = new System.Windows.Forms.ComboBox();
            this.dgvData = new System.Windows.Forms.DataGridView();
            this.btnInsert = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.btnRamdomInsert = new System.Windows.Forms.Button();
            this.txtColumn2 = new System.Windows.Forms.TextBox();
            this.lblColumn2 = new System.Windows.Forms.Label();
            this.txtColumn1 = new System.Windows.Forms.TextBox();
            this.lblColumn1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTable
            // 
            this.lblTable.AutoSize = true;
            this.lblTable.Location = new System.Drawing.Point(5, 15);
            this.lblTable.Name = "lblTable";
            this.lblTable.Size = new System.Drawing.Size(65, 12);
            this.lblTable.TabIndex = 0;
            this.lblTable.Text = "请选择表：";
            // 
            // cmbTable
            // 
            this.cmbTable.FormattingEnabled = true;
            this.cmbTable.Location = new System.Drawing.Point(76, 12);
            this.cmbTable.Name = "cmbTable";
            this.cmbTable.Size = new System.Drawing.Size(121, 20);
            this.cmbTable.TabIndex = 1;
            this.cmbTable.SelectedIndexChanged += new System.EventHandler(this.cmbTable_SelectedIndexChanged);
            // 
            // dgvData
            // 
            this.dgvData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvData.Location = new System.Drawing.Point(12, 38);
            this.dgvData.Name = "dgvData";
            this.dgvData.RowTemplate.Height = 23;
            this.dgvData.Size = new System.Drawing.Size(754, 349);
            this.dgvData.TabIndex = 2;
            // 
            // btnInsert
            // 
            this.btnInsert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnInsert.Location = new System.Drawing.Point(347, 19);
            this.btnInsert.Name = "btnInsert";
            this.btnInsert.Size = new System.Drawing.Size(49, 23);
            this.btnInsert.TabIndex = 3;
            this.btnInsert.Text = "录入";
            this.btnInsert.UseVisualStyleBackColor = true;
            this.btnInsert.Click += new System.EventHandler(this.btnInsert_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.btnRamdomInsert);
            this.groupBox1.Controls.Add(this.txtColumn2);
            this.groupBox1.Controls.Add(this.btnInsert);
            this.groupBox1.Controls.Add(this.lblColumn2);
            this.groupBox1.Controls.Add(this.txtColumn1);
            this.groupBox1.Controls.Add(this.lblColumn1);
            this.groupBox1.Location = new System.Drawing.Point(12, 393);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(754, 69);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "添加数据";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(419, 18);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(125, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "单CDB随机插入数据";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnRamdomInsert
            // 
            this.btnRamdomInsert.Location = new System.Drawing.Point(569, 18);
            this.btnRamdomInsert.Name = "btnRamdomInsert";
            this.btnRamdomInsert.Size = new System.Drawing.Size(120, 23);
            this.btnRamdomInsert.TabIndex = 4;
            this.btnRamdomInsert.Text = "双CDB随机插入数据";
            this.btnRamdomInsert.UseVisualStyleBackColor = true;
            this.btnRamdomInsert.Click += new System.EventHandler(this.btnRamdomInsert_Click);
            // 
            // txtColumn2
            // 
            this.txtColumn2.Location = new System.Drawing.Point(241, 20);
            this.txtColumn2.Name = "txtColumn2";
            this.txtColumn2.Size = new System.Drawing.Size(100, 21);
            this.txtColumn2.TabIndex = 3;
            this.txtColumn2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtColumn2_KeyPress);
            // 
            // lblColumn2
            // 
            this.lblColumn2.AutoSize = true;
            this.lblColumn2.Location = new System.Drawing.Point(203, 23);
            this.lblColumn2.Name = "lblColumn2";
            this.lblColumn2.Size = new System.Drawing.Size(23, 12);
            this.lblColumn2.TabIndex = 2;
            this.lblColumn2.Text = "列2";
            // 
            // txtColumn1
            // 
            this.txtColumn1.Location = new System.Drawing.Point(85, 20);
            this.txtColumn1.Name = "txtColumn1";
            this.txtColumn1.Size = new System.Drawing.Size(100, 21);
            this.txtColumn1.TabIndex = 1;
            this.txtColumn1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtColumn1_KeyPress);
            // 
            // lblColumn1
            // 
            this.lblColumn1.AutoSize = true;
            this.lblColumn1.Location = new System.Drawing.Point(39, 24);
            this.lblColumn1.Name = "lblColumn1";
            this.lblColumn1.Size = new System.Drawing.Size(23, 12);
            this.lblColumn1.TabIndex = 0;
            this.lblColumn1.Text = "列1";
            // 
            // frInsertData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(778, 474);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.dgvData);
            this.Controls.Add(this.cmbTable);
            this.Controls.Add(this.lblTable);
            this.Name = "frInsertData";
            this.Text = "录入数据";
            this.Load += new System.EventHandler(this.frInsertData_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTable;
        private System.Windows.Forms.ComboBox cmbTable;
        private System.Windows.Forms.DataGridView dgvData;
        private System.Windows.Forms.Button btnInsert;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtColumn2;
        private System.Windows.Forms.Label lblColumn2;
        private System.Windows.Forms.TextBox txtColumn1;
        private System.Windows.Forms.Label lblColumn1;
        private System.Windows.Forms.Button btnRamdomInsert;
        private System.Windows.Forms.Button button1;
    }
}