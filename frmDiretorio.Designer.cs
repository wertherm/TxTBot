namespace TxTBot
{
    partial class frmDiretorio
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
            this.btnDiretorio = new System.Windows.Forms.Button();
            this.txtDiretorio = new System.Windows.Forms.TextBox();
            this.lblDiretorio = new System.Windows.Forms.Label();
            this.fbdDiretorio = new System.Windows.Forms.FolderBrowserDialog();
            this.btnCompilar = new System.Windows.Forms.Button();
            this.pbArquivos = new System.Windows.Forms.ProgressBar();
            this.btnOneNote = new System.Windows.Forms.Button();

            this.SuspendLayout();

            // 
            // btnDiretorio
            // 
            this.btnDiretorio.Location = new System.Drawing.Point(456, 12);
            this.btnDiretorio.Name = "btnDiretorio";
            this.btnDiretorio.Size = new System.Drawing.Size(48, 23);
            this.btnDiretorio.TabIndex = 0;
            this.btnDiretorio.Text = "Abrir";
            this.btnDiretorio.UseVisualStyleBackColor = true;
            this.btnDiretorio.Click += new System.EventHandler(this.btnDiretorio_Click);
            // 
            // txtDiretorio
            // 
            this.txtDiretorio.Location = new System.Drawing.Point(123, 15);
            this.txtDiretorio.Name = "txtDiretorio";
            this.txtDiretorio.Size = new System.Drawing.Size(327, 20);
            this.txtDiretorio.TabIndex = 1;
            // 
            // lblDiretorio
            // 
            this.lblDiretorio.AutoSize = true;
            this.lblDiretorio.Location = new System.Drawing.Point(4, 17);
            this.lblDiretorio.Name = "lblDiretorio";
            this.lblDiretorio.Size = new System.Drawing.Size(113, 13);
            this.lblDiretorio.TabIndex = 2;
            this.lblDiretorio.Text = "Diretório dos Arquivos:";
            // 
            // btnCompilar
            // 
            this.btnCompilar.Location = new System.Drawing.Point(511, 12);
            this.btnCompilar.Name = "btnCompilar";
            this.btnCompilar.Size = new System.Drawing.Size(56, 23);
            this.btnCompilar.TabIndex = 3;
            this.btnCompilar.Text = "Compilar";
            this.btnCompilar.UseVisualStyleBackColor = true;
            this.btnCompilar.Click += new System.EventHandler(this.btnCompilar_Click);
            // 
            // pbArquivos
            // 
            this.pbArquivos.Location = new System.Drawing.Point(7, 41);
            this.pbArquivos.Name = "pbArquivos";
            this.pbArquivos.Size = new System.Drawing.Size(629, 15);
            this.pbArquivos.TabIndex = 4;
            // 
            // btnOneNote
            // 
            this.btnOneNote.Location = new System.Drawing.Point(573, 12);
            this.btnOneNote.Name = "btnOneNote";
            this.btnOneNote.Size = new System.Drawing.Size(63, 23);
            this.btnOneNote.TabIndex = 5;
            this.btnOneNote.Text = "OneNote";
            this.btnOneNote.UseVisualStyleBackColor = true;
            this.btnOneNote.Click += new System.EventHandler(this.btnOneNote_Click);
            // 
            // frmDiretorio
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(648, 64);
            this.Controls.Add(this.btnOneNote);
            this.Controls.Add(this.pbArquivos);
            this.Controls.Add(this.btnCompilar);
            this.Controls.Add(this.lblDiretorio);
            this.Controls.Add(this.txtDiretorio);
            this.Controls.Add(this.btnDiretorio);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "frmDiretorio";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Organizador TXT";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button btnDiretorio;
        private System.Windows.Forms.TextBox txtDiretorio;
        private System.Windows.Forms.Label lblDiretorio;
        private System.Windows.Forms.FolderBrowserDialog fbdDiretorio;
        private System.Windows.Forms.Button btnCompilar;
        private System.Windows.Forms.ProgressBar pbArquivos;
        private System.Windows.Forms.Button btnOneNote;
    }
}