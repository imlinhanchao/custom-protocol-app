namespace ProtocolApp
{
    partial class Form1
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.text_id = new System.Windows.Forms.TextBox();
            this.text_key = new System.Windows.Forms.TextBox();
            this.text_query = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "Id:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(196, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 18);
            this.label2.TabIndex = 2;
            this.label2.Text = "Key:";
            // 
            // text_id
            // 
            this.text_id.Location = new System.Drawing.Point(57, 7);
            this.text_id.Name = "text_id";
            this.text_id.Size = new System.Drawing.Size(133, 28);
            this.text_id.TabIndex = 5;
            // 
            // text_key
            // 
            this.text_key.Location = new System.Drawing.Point(247, 6);
            this.text_key.Name = "text_key";
            this.text_key.Size = new System.Drawing.Size(144, 28);
            this.text_key.TabIndex = 6;
            // 
            // text_query
            // 
            this.text_query.Location = new System.Drawing.Point(18, 45);
            this.text_query.Multiline = true;
            this.text_query.Name = "text_query";
            this.text_query.Size = new System.Drawing.Size(373, 380);
            this.text_query.TabIndex = 7;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 450);
            this.Controls.Add(this.text_query);
            this.Controls.Add(this.text_key);
            this.Controls.Add(this.text_id);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox text_id;
        private System.Windows.Forms.TextBox text_key;
        private System.Windows.Forms.TextBox text_query;
    }
}

