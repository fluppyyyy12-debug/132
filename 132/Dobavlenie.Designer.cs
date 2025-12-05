using System;
using System.Drawing;
using System.Windows.Forms;

namespace _132
{
    partial class Dobavlenie
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
            lblProductId = new Label();
            pbProductImage = new PictureBox();
            btnLoadImage = new Button();
            btnRemoveImage = new Button();
            lblImageInfo = new Label();
            lblName = new Label();
            txtName = new TextBox();
            lblCategory = new Label();
            cmbCategory = new ComboBox();
            lblManufacturer = new Label();
            txtManufacturer = new TextBox();
            lblStock = new Label();
            txtStock = new TextBox();
            lblMinStock = new Label();
            txtMinimumStock = new TextBox();
            lblUnit = new Label();
            cmbUnit = new ComboBox();
            lblSupplier = new Label();
            cmbSupplier = new ComboBox();
            lblPrice = new Label();
            txtPrice = new TextBox();
            lblDescription = new Label();
            txtDescription = new RichTextBox();
            btnSave = new Button();
            btnCancel = new Button();
            ((System.ComponentModel.ISupportInitialize)pbProductImage).BeginInit();
            SuspendLayout();
            // 
            // lblProductId
            // 
            lblProductId.AutoSize = true;
            lblProductId.Font = new Font("Comic Sans MS", 12F, FontStyle.Bold, GraphicsUnit.Point, 204);
            lblProductId.Location = new Point(12, 9);
            lblProductId.Name = "lblProductId";
            lblProductId.Size = new Size(112, 23);
            lblProductId.TabIndex = 0;
            lblProductId.Text = "Новый товар";
            // 
            // pbProductImage
            // 
            pbProductImage.BorderStyle = BorderStyle.FixedSingle;
            pbProductImage.Location = new Point(12, 43);
            pbProductImage.Name = "pbProductImage";
            pbProductImage.Size = new Size(300, 200);
            pbProductImage.SizeMode = PictureBoxSizeMode.Zoom;
            pbProductImage.TabIndex = 1;
            pbProductImage.TabStop = false;
            // 
            // btnLoadImage
            // 
            btnLoadImage.Font = new Font("Comic Sans MS", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            btnLoadImage.Location = new Point(12, 249);
            btnLoadImage.Name = "btnLoadImage";
            btnLoadImage.Size = new Size(147, 30);
            btnLoadImage.TabIndex = 2;
            btnLoadImage.Text = "Загрузить изображение";
            btnLoadImage.UseVisualStyleBackColor = true;
            btnLoadImage.Click += btnLoadImage_Click;
            // 
            // btnRemoveImage
            // 
            btnRemoveImage.Font = new Font("Comic Sans MS", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            btnRemoveImage.Location = new Point(165, 249);
            btnRemoveImage.Name = "btnRemoveImage";
            btnRemoveImage.Size = new Size(147, 30);
            btnRemoveImage.TabIndex = 3;
            btnRemoveImage.Text = "Удалить изображение";
            btnRemoveImage.UseVisualStyleBackColor = true;
            btnRemoveImage.Click += btnRemoveImage_Click;
            // 
            // lblImageInfo
            // 
            lblImageInfo.AutoSize = true;
            lblImageInfo.Font = new Font("Comic Sans MS", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lblImageInfo.ForeColor = Color.Gray;
            lblImageInfo.Location = new Point(12, 282);
            lblImageInfo.Name = "lblImageInfo";
            lblImageInfo.Size = new Size(150, 17);
            lblImageInfo.TabIndex = 4;
            lblImageInfo.Text = "Изображение не выбрано";
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.Font = new Font("Comic Sans MS", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lblName.Location = new Point(330, 43);
            lblName.Name = "lblName";
            lblName.Size = new Size(93, 17);
            lblName.TabIndex = 5;
            lblName.Text = "Наименование";
            // 
            // txtName
            // 
            txtName.Location = new Point(330, 61);
            txtName.Name = "txtName";
            txtName.Size = new Size(300, 23);
            txtName.TabIndex = 6;
            // 
            // lblCategory
            // 
            lblCategory.AutoSize = true;
            lblCategory.Font = new Font("Comic Sans MS", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lblCategory.Location = new Point(330, 97);
            lblCategory.Name = "lblCategory";
            lblCategory.Size = new Size(67, 17);
            lblCategory.TabIndex = 7;
            lblCategory.Text = "Категория";
            // 
            // cmbCategory
            // 
            cmbCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCategory.FormattingEnabled = true;
            cmbCategory.Location = new Point(330, 115);
            cmbCategory.Name = "cmbCategory";
            cmbCategory.Size = new Size(300, 23);
            cmbCategory.TabIndex = 8;
            // 
            // lblManufacturer
            // 
            lblManufacturer.AutoSize = true;
            lblManufacturer.Font = new Font("Comic Sans MS", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lblManufacturer.Location = new Point(330, 151);
            lblManufacturer.Name = "lblManufacturer";
            lblManufacturer.Size = new Size(99, 17);
            lblManufacturer.TabIndex = 9;
            lblManufacturer.Text = "Производитель";
            // 
            // txtManufacturer
            // 
            txtManufacturer.Location = new Point(330, 169);
            txtManufacturer.Name = "txtManufacturer";
            txtManufacturer.Size = new Size(300, 23);
            txtManufacturer.TabIndex = 10;
            // 
            // lblStock
            // 
            lblStock.AutoSize = true;
            lblStock.Font = new Font("Comic Sans MS", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lblStock.Location = new Point(330, 205);
            lblStock.Name = "lblStock";
            lblStock.Size = new Size(136, 17);
            lblStock.TabIndex = 11;
            lblStock.Text = "Количество на складе";
            // 
            // txtStock
            // 
            txtStock.Location = new Point(330, 223);
            txtStock.Name = "txtStock";
            txtStock.Size = new Size(140, 23);
            txtStock.TabIndex = 12;
            txtStock.Text = "0";
            txtStock.KeyPress += txtStock_KeyPress;
            // 
            // lblMinStock
            // 
            lblMinStock.AutoSize = true;
            lblMinStock.Font = new Font("Comic Sans MS", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lblMinStock.Location = new Point(490, 205);
            lblMinStock.Name = "lblMinStock";
            lblMinStock.Size = new Size(160, 17);
            lblMinStock.TabIndex = 13;
            lblMinStock.Text = "Минимальное количество";
            // 
            // txtMinimumStock
            // 
            txtMinimumStock.Location = new Point(490, 223);
            txtMinimumStock.Name = "txtMinimumStock";
            txtMinimumStock.Size = new Size(140, 23);
            txtMinimumStock.TabIndex = 14;
            txtMinimumStock.Text = "0";
            txtMinimumStock.KeyPress += txtMinimumStock_KeyPress;
            // 
            // lblUnit
            // 
            lblUnit.AutoSize = true;
            lblUnit.Font = new Font("Comic Sans MS", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lblUnit.Location = new Point(330, 259);
            lblUnit.Name = "lblUnit";
            lblUnit.Size = new Size(124, 17);
            lblUnit.TabIndex = 15;
            lblUnit.Text = "Единица измерения";
            // 
            // cmbUnit
            // 
            cmbUnit.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbUnit.FormattingEnabled = true;
            cmbUnit.Location = new Point(330, 277);
            cmbUnit.Name = "cmbUnit";
            cmbUnit.Size = new Size(300, 23);
            cmbUnit.TabIndex = 16;
            // 
            // lblSupplier
            // 
            lblSupplier.AutoSize = true;
            lblSupplier.Font = new Font("Comic Sans MS", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lblSupplier.Location = new Point(330, 313);
            lblSupplier.Name = "lblSupplier";
            lblSupplier.Size = new Size(74, 17);
            lblSupplier.TabIndex = 17;
            lblSupplier.Text = "Поставщик";
            // 
            // cmbSupplier
            // 
            cmbSupplier.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSupplier.FormattingEnabled = true;
            cmbSupplier.Location = new Point(330, 331);
            cmbSupplier.Name = "cmbSupplier";
            cmbSupplier.Size = new Size(300, 23);
            cmbSupplier.TabIndex = 18;
            // 
            // lblPrice
            // 
            lblPrice.AutoSize = true;
            lblPrice.Font = new Font("Comic Sans MS", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lblPrice.Location = new Point(330, 367);
            lblPrice.Name = "lblPrice";
            lblPrice.Size = new Size(138, 17);
            lblPrice.TabIndex = 19;
            lblPrice.Text = "Стоимость за единицу";
            // 
            // txtPrice
            // 
            txtPrice.Location = new Point(330, 385);
            txtPrice.Name = "txtPrice";
            txtPrice.Size = new Size(300, 23);
            txtPrice.TabIndex = 20;
            txtPrice.Text = "0,00";
            txtPrice.TextChanged += TxtPrice_TextChanged;
            txtPrice.KeyPress += txtPrice_KeyPress;
            // 
            // lblDescription
            // 
            lblDescription.AutoSize = true;
            lblDescription.Font = new Font("Comic Sans MS", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lblDescription.Location = new Point(12, 310);
            lblDescription.Name = "lblDescription";
            lblDescription.Size = new Size(132, 17);
            lblDescription.TabIndex = 21;
            lblDescription.Text = "Подробное описание";
            // 
            // txtDescription
            // 
            txtDescription.Location = new Point(12, 328);
            txtDescription.Name = "txtDescription";
            txtDescription.Size = new Size(300, 120);
            txtDescription.TabIndex = 22;
            txtDescription.Text = "";
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.Transparent;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Comic Sans MS", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 204);
            btnSave.ForeColor = Color.Black;
            btnSave.Location = new Point(330, 440);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(140, 40);
            btnSave.TabIndex = 23;
            btnSave.Text = "Добавить товар";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.BackColor = Color.Transparent;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Font = new Font("Comic Sans MS", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 204);
            btnCancel.ForeColor = SystemColors.ActiveCaptionText;
            btnCancel.Location = new Point(490, 440);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(140, 40);
            btnCancel.TabIndex = 24;
            btnCancel.Text = "Отмена";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += btnCancel_Click;
            // 
            // Dobavlenie
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(650, 500);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(txtDescription);
            Controls.Add(lblDescription);
            Controls.Add(txtPrice);
            Controls.Add(lblPrice);
            Controls.Add(cmbSupplier);
            Controls.Add(lblSupplier);
            Controls.Add(cmbUnit);
            Controls.Add(lblUnit);
            Controls.Add(txtMinimumStock);
            Controls.Add(lblMinStock);
            Controls.Add(txtStock);
            Controls.Add(lblStock);
            Controls.Add(txtManufacturer);
            Controls.Add(lblManufacturer);
            Controls.Add(cmbCategory);
            Controls.Add(lblCategory);
            Controls.Add(txtName);
            Controls.Add(lblName);
            Controls.Add(lblImageInfo);
            Controls.Add(btnRemoveImage);
            Controls.Add(btnLoadImage);
            Controls.Add(pbProductImage);
            Controls.Add(lblProductId);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Dobavlenie";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Добавление товара";
            FormClosed += Form4_FormClosed;
            Load += Form4_Load;
            ((System.ComponentModel.ISupportInitialize)pbProductImage).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblProductId;
        private PictureBox pbProductImage;
        private Button btnLoadImage;
        private Button btnRemoveImage;
        private Label lblImageInfo;
        private Label lblName;
        private TextBox txtName;
        private Label lblCategory;
        private ComboBox cmbCategory;
        private Label lblManufacturer;
        private TextBox txtManufacturer;
        private Label lblStock;
        private TextBox txtStock;
        private Label lblMinStock;
        private TextBox txtMinimumStock;
        private Label lblUnit;
        private ComboBox cmbUnit;
        private Label lblSupplier;
        private ComboBox cmbSupplier;
        private Label lblPrice;
        private TextBox txtPrice;
        private Label lblDescription;
        private RichTextBox txtDescription;
        private Button btnSave;
        private Button btnCancel;
    }
}