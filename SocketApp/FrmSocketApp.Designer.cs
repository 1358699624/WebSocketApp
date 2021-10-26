namespace SocketApp
{
    partial class FrmSocketApp
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.richtextquery = new System.Windows.Forms.RichTextBox();
            this.tichtextsend = new System.Windows.Forms.RichTextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // richtextquery
            // 
            this.richtextquery.Location = new System.Drawing.Point(12, 26);
            this.richtextquery.Name = "richtextquery";
            this.richtextquery.Size = new System.Drawing.Size(535, 194);
            this.richtextquery.TabIndex = 0;
            this.richtextquery.Text = "";
            // 
            // tichtextsend
            // 
            this.tichtextsend.Location = new System.Drawing.Point(12, 254);
            this.tichtextsend.Name = "tichtextsend";
            this.tichtextsend.Size = new System.Drawing.Size(535, 170);
            this.tichtextsend.TabIndex = 0;
            this.tichtextsend.Text = "";
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(438, 382);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(98, 27);
            this.btnSend.TabIndex = 1;
            this.btnSend.Text = "发送";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // FrmSocketApp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(563, 435);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.richtextquery);
            this.Controls.Add(this.tichtextsend);
            this.Name = "FrmSocketApp";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.FrmSocketApp_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox richtextquery;
        private System.Windows.Forms.RichTextBox tichtextsend;
        private System.Windows.Forms.Button btnSend;
    }
}

