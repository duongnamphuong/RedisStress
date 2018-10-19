namespace RedisGUI1
{
    partial class spamForm
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
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnDeleteAllProduct = new System.Windows.Forms.Button();
            this.txtNumberOfProducts = new System.Windows.Forms.TextBox();
            this.lblProducts = new System.Windows.Forms.Label();
            this.btnSetup = new System.Windows.Forms.Button();
            this.btnSpam = new System.Windows.Forms.Button();
            this.txtNumberOfHeartbeats = new System.Windows.Forms.TextBox();
            this.lblHeartbeat = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(13, 24);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 0;
            this.btnConnect.Text = "&Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnDeleteAllProduct
            // 
            this.btnDeleteAllProduct.Location = new System.Drawing.Point(94, 24);
            this.btnDeleteAllProduct.Name = "btnDeleteAllProduct";
            this.btnDeleteAllProduct.Size = new System.Drawing.Size(109, 23);
            this.btnDeleteAllProduct.TabIndex = 1;
            this.btnDeleteAllProduct.Text = "Delete all Product*";
            this.btnDeleteAllProduct.UseVisualStyleBackColor = true;
            this.btnDeleteAllProduct.Click += new System.EventHandler(this.btnDeleteAllProduct_Click);
            // 
            // txtNumberOfProducts
            // 
            this.txtNumberOfProducts.Location = new System.Drawing.Point(75, 70);
            this.txtNumberOfProducts.Name = "txtNumberOfProducts";
            this.txtNumberOfProducts.Size = new System.Drawing.Size(65, 20);
            this.txtNumberOfProducts.TabIndex = 2;
            // 
            // lblProducts
            // 
            this.lblProducts.AutoSize = true;
            this.lblProducts.Location = new System.Drawing.Point(146, 73);
            this.lblProducts.Name = "lblProducts";
            this.lblProducts.Size = new System.Drawing.Size(54, 13);
            this.lblProducts.TabIndex = 3;
            this.lblProducts.Text = "product(s)";
            // 
            // btnSetup
            // 
            this.btnSetup.Location = new System.Drawing.Point(13, 68);
            this.btnSetup.Name = "btnSetup";
            this.btnSetup.Size = new System.Drawing.Size(56, 23);
            this.btnSetup.TabIndex = 4;
            this.btnSetup.Text = "&Set up";
            this.btnSetup.UseVisualStyleBackColor = true;
            this.btnSetup.Click += new System.EventHandler(this.btnInsert_Click);
            // 
            // btnSpam
            // 
            this.btnSpam.Location = new System.Drawing.Point(13, 110);
            this.btnSpam.Name = "btnSpam";
            this.btnSpam.Size = new System.Drawing.Size(56, 23);
            this.btnSpam.TabIndex = 5;
            this.btnSpam.Text = "S&pam";
            this.btnSpam.UseVisualStyleBackColor = true;
            this.btnSpam.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtNumberOfHeartbeats
            // 
            this.txtNumberOfHeartbeats.Location = new System.Drawing.Point(75, 112);
            this.txtNumberOfHeartbeats.Name = "txtNumberOfHeartbeats";
            this.txtNumberOfHeartbeats.Size = new System.Drawing.Size(65, 20);
            this.txtNumberOfHeartbeats.TabIndex = 6;
            // 
            // lblHeartbeat
            // 
            this.lblHeartbeat.AutoSize = true;
            this.lblHeartbeat.Location = new System.Drawing.Point(146, 115);
            this.lblHeartbeat.Name = "lblHeartbeat";
            this.lblHeartbeat.Size = new System.Drawing.Size(57, 13);
            this.lblHeartbeat.TabIndex = 7;
            this.lblHeartbeat.Text = "heartbeats";
            // 
            // spamForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.lblHeartbeat);
            this.Controls.Add(this.txtNumberOfHeartbeats);
            this.Controls.Add(this.btnSpam);
            this.Controls.Add(this.btnSetup);
            this.Controls.Add(this.lblProducts);
            this.Controls.Add(this.txtNumberOfProducts);
            this.Controls.Add(this.btnDeleteAllProduct);
            this.Controls.Add(this.btnConnect);
            this.Name = "spamForm";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnDeleteAllProduct;
        private System.Windows.Forms.TextBox txtNumberOfProducts;
        private System.Windows.Forms.Label lblProducts;
        private System.Windows.Forms.Button btnSetup;
        private System.Windows.Forms.Button btnSpam;
        private System.Windows.Forms.TextBox txtNumberOfHeartbeats;
        private System.Windows.Forms.Label lblHeartbeat;
    }
}

