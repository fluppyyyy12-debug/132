namespace _132
{
    partial class Avtoriz
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            RegisteredUsername = new TextBox();
            label3 = new Label();
            RegisteredPassword = new TextBox();
            button1 = new Button();
            button2 = new Button();
            linkLabel1 = new LinkLabel();
            captchaPictureBox = new PictureBox();
            captchaTextBox = new TextBox();
            labelCaptcha = new Label();
            refreshCaptchaButton = new Button();
            logoPictureBox = new PictureBox();
            label2 = new Label();
            ((System.ComponentModel.ISupportInitialize)captchaPictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)logoPictureBox).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Comic Sans MS", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 204);
            label1.Location = new Point(336, 28);
            label1.Name = "label1";
            label1.Size = new Size(76, 30);
            label1.TabIndex = 0;
            label1.Text = "Логин";
            label1.Click += label1_Click;
            // 
            // RegisteredUsername
            // 
            RegisteredUsername.Location = new Point(311, 61);
            RegisteredUsername.Name = "RegisteredUsername";
            RegisteredUsername.Size = new Size(124, 23);
            RegisteredUsername.TabIndex = 2;
            RegisteredUsername.TextChanged += RegisteredUsername_TextChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Comic Sans MS", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 204);
            label3.Location = new Point(328, 89);
            label3.Name = "label3";
            label3.Size = new Size(91, 30);
            label3.TabIndex = 4;
            label3.Text = "Пароль";
            label3.Click += label3_Click;
            // 
            // RegisteredPassword
            // 
            RegisteredPassword.Location = new Point(311, 122);
            RegisteredPassword.Name = "RegisteredPassword";
            RegisteredPassword.Size = new Size(124, 23);
            RegisteredPassword.TabIndex = 5;
            RegisteredPassword.TextChanged += textBox2_TextChanged;
            // 
            // button1
            // 
            button1.BackColor = Color.Transparent;
            button1.Font = new Font("Comic Sans MS", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            button1.ForeColor = Color.Black;
            button1.Location = new Point(337, 160);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 6;
            button1.Text = "Войти";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Font = new Font("Comic Sans MS", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            button2.Location = new Point(311, 189);
            button2.Name = "button2";
            button2.Size = new Size(123, 23);
            button2.TabIndex = 7;
            button2.Text = "Войти как гость";
            button2.UseVisualStyleBackColor = true;
            // 
            // linkLabel1
            // 
            linkLabel1.AutoSize = true;
            linkLabel1.Font = new Font("Comic Sans MS", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            linkLabel1.Location = new Point(322, 215);
            linkLabel1.Name = "linkLabel1";
            linkLabel1.Size = new Size(105, 23);
            linkLabel1.TabIndex = 8;
            linkLabel1.TabStop = true;
            linkLabel1.Text = "Регистрация";
            linkLabel1.LinkClicked += linkLabel1_LinkClicked;
            // 
            // captchaPictureBox
            // 
            captchaPictureBox.Location = new Point(328, 272);
            captchaPictureBox.Name = "captchaPictureBox";
            captchaPictureBox.Size = new Size(100, 50);
            captchaPictureBox.TabIndex = 9;
            captchaPictureBox.TabStop = false;
            // 
            // captchaTextBox
            // 
            captchaTextBox.Location = new Point(328, 328);
            captchaTextBox.MaxLength = 4;
            captchaTextBox.Name = "captchaTextBox";
            captchaTextBox.Size = new Size(100, 23);
            captchaTextBox.TabIndex = 10;
            // 
            // labelCaptcha
            // 
            labelCaptcha.AutoSize = true;
            labelCaptcha.Font = new Font("Comic Sans MS", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            labelCaptcha.Location = new Point(322, 354);
            labelCaptcha.Name = "labelCaptcha";
            labelCaptcha.Size = new Size(112, 17);
            labelCaptcha.TabIndex = 10;
            labelCaptcha.Text = "Введите CAPTCHA";
            labelCaptcha.Visible = false;
            // 
            // refreshCaptchaButton
            // 
            refreshCaptchaButton.Font = new Font("Comic Sans MS", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            refreshCaptchaButton.ForeColor = Color.Black;
            refreshCaptchaButton.Location = new Point(343, 382);
            refreshCaptchaButton.Name = "refreshCaptchaButton";
            refreshCaptchaButton.Size = new Size(75, 23);
            refreshCaptchaButton.TabIndex = 12;
            refreshCaptchaButton.Text = "Обновить";
            refreshCaptchaButton.UseVisualStyleBackColor = true;
            // 
            // logoPictureBox
            // 
            logoPictureBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            logoPictureBox.ErrorImage = null;
            logoPictureBox.Image = Properties.Resources.logo;
            logoPictureBox.InitialImage = null;
            logoPictureBox.Location = new Point(12, 12);
            logoPictureBox.Name = "logoPictureBox";
            logoPictureBox.Size = new Size(215, 145);
            logoPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            logoPictureBox.TabIndex = 13;
            logoPictureBox.TabStop = false;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Comic Sans MS", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label2.Location = new Point(32, 160);
            label2.Name = "label2";
            label2.Size = new Size(182, 29);
            label2.TabIndex = 14;
            label2.Text = "ООО «Посуда»  ";
            label2.Click += label2_Click;
            // 
            // Avtoriz
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(800, 450);
            Controls.Add(label2);
            Controls.Add(logoPictureBox);
            Controls.Add(refreshCaptchaButton);
            Controls.Add(labelCaptcha);
            Controls.Add(captchaTextBox);
            Controls.Add(captchaPictureBox);
            Controls.Add(linkLabel1);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(RegisteredPassword);
            Controls.Add(label3);
            Controls.Add(RegisteredUsername);
            Controls.Add(label1);
            Name = "Avtoriz";
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)captchaPictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)logoPictureBox).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox RegisteredUsername;
        private Label label3;
        private TextBox RegisteredPassword;
        private Button button1;
        private Button button2;
        private LinkLabel linkLabel1;
        private PictureBox captchaPictureBox;
        private TextBox captchaTextBox;
        private Label labelCaptcha;
        private Button refreshCaptchaButton;
        private PictureBox logoPictureBox;
        private Label label2;
    }
}
