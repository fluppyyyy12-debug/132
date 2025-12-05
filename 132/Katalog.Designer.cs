namespace _132
{
    partial class Form3
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
            components = new System.ComponentModel.Container();
            dataGridViewProducts = new DataGridView();
            searchTextBox = new TextBox();
            filterComboBox = new ComboBox();
            sortComboBox = new ComboBox();
            btnAddProduct = new Button();
            btnDeleteProduct = new Button();
            lblProductCount = new Label();
            pictureBoxNoImage = new PictureBox();
            panelTop = new Panel();
            lblSort = new Label();
            lblFilter = new Label();
            lblSearch = new Label();
            panelBottom = new Panel();
            button1 = new Button();
            toolTip1 = new ToolTip(components);
            ((System.ComponentModel.ISupportInitialize)dataGridViewProducts).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxNoImage).BeginInit();
            panelTop.SuspendLayout();
            panelBottom.SuspendLayout();
            SuspendLayout();
            // 
            // dataGridViewProducts
            // 
            dataGridViewProducts.AllowUserToAddRows = false;
            dataGridViewProducts.AllowUserToDeleteRows = false;
            dataGridViewProducts.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridViewProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewProducts.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridViewProducts.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewProducts.Location = new Point(12, 70);
            dataGridViewProducts.MultiSelect = false;
            dataGridViewProducts.Name = "dataGridViewProducts";
            dataGridViewProducts.ReadOnly = true;
            dataGridViewProducts.RowHeadersVisible = false;
            dataGridViewProducts.RowTemplate.Height = 100;
            dataGridViewProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewProducts.Size = new Size(960, 464);
            dataGridViewProducts.TabIndex = 0;
            // 
            // searchTextBox
            // 
            searchTextBox.Location = new Point(70, 15);
            searchTextBox.Name = "searchTextBox";
            searchTextBox.PlaceholderText = "Поиск по названию, описанию, производителю...";
            searchTextBox.Size = new Size(282, 23);
            searchTextBox.TabIndex = 1;
            // 
            // filterComboBox
            // 
            filterComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            filterComboBox.FormattingEnabled = true;
            filterComboBox.Location = new Point(485, 16);
            filterComboBox.Name = "filterComboBox";
            filterComboBox.Size = new Size(200, 23);
            filterComboBox.TabIndex = 2;
            // 
            // sortComboBox
            // 
            sortComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            sortComboBox.FormattingEnabled = true;
            sortComboBox.Items.AddRange(new object[] { "Цена: по возрастанию", "Цена: по убыванию" });
            sortComboBox.Location = new Point(763, 15);
            sortComboBox.Name = "sortComboBox";
            sortComboBox.Size = new Size(200, 23);
            sortComboBox.TabIndex = 3;
            // 
            // btnAddProduct
            // 
            btnAddProduct.Font = new Font("Comic Sans MS", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            btnAddProduct.Location = new Point(20, 10);
            btnAddProduct.Name = "btnAddProduct";
            btnAddProduct.Size = new Size(120, 30);
            btnAddProduct.TabIndex = 4;
            btnAddProduct.Text = "Добавить товар";
            btnAddProduct.UseVisualStyleBackColor = true;
            btnAddProduct.Click += btnAddProduct_Click_1;
            // 
            // btnDeleteProduct
            // 
            btnDeleteProduct.BackColor = Color.Transparent;
            btnDeleteProduct.Enabled = false;
            btnDeleteProduct.Font = new Font("Comic Sans MS", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            btnDeleteProduct.Location = new Point(160, 10);
            btnDeleteProduct.Name = "btnDeleteProduct";
            btnDeleteProduct.Size = new Size(120, 30);
            btnDeleteProduct.TabIndex = 6;
            btnDeleteProduct.Text = "Удалить";
            btnDeleteProduct.UseVisualStyleBackColor = false;
            btnDeleteProduct.Click += btnDeleteProduct_Click_1;
            // 
            // lblProductCount
            // 
            lblProductCount.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblProductCount.AutoSize = true;
            lblProductCount.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblProductCount.Location = new Point(11, 537);
            lblProductCount.Name = "lblProductCount";
            lblProductCount.Size = new Size(49, 19);
            lblProductCount.TabIndex = 7;
            lblProductCount.Text = "0 из 0";
            lblProductCount.Click += lblProductCount_Click;
            // 
            // pictureBoxNoImage
            // 
            pictureBoxNoImage.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            pictureBoxNoImage.BackColor = Color.LightGray;
            pictureBoxNoImage.Location = new Point(900, 559);
            pictureBoxNoImage.Name = "pictureBoxNoImage";
            pictureBoxNoImage.Size = new Size(72, 40);
            pictureBoxNoImage.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxNoImage.TabIndex = 8;
            pictureBoxNoImage.TabStop = false;
            pictureBoxNoImage.Visible = false;
            // 
            // panelTop
            // 
            panelTop.Controls.Add(lblSort);
            panelTop.Controls.Add(lblFilter);
            panelTop.Controls.Add(lblSearch);
            panelTop.Controls.Add(searchTextBox);
            panelTop.Controls.Add(filterComboBox);
            panelTop.Controls.Add(sortComboBox);
            panelTop.Dock = DockStyle.Top;
            panelTop.Location = new Point(0, 0);
            panelTop.Name = "panelTop";
            panelTop.Size = new Size(984, 50);
            panelTop.TabIndex = 9;
            // 
            // lblSort
            // 
            lblSort.AutoSize = true;
            lblSort.Font = new Font("Comic Sans MS", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lblSort.Location = new Point(719, 17);
            lblSort.Name = "lblSort";
            lblSort.Size = new Size(38, 17);
            lblSort.TabIndex = 12;
            lblSort.Text = "Сорт:";
            // 
            // lblFilter
            // 
            lblFilter.AutoSize = true;
            lblFilter.Font = new Font("Comic Sans MS", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lblFilter.Location = new Point(425, 21);
            lblFilter.Name = "lblFilter";
            lblFilter.Size = new Size(54, 17);
            lblFilter.TabIndex = 11;
            lblFilter.Text = "Фильтр:";
            // 
            // lblSearch
            // 
            lblSearch.AutoSize = true;
            lblSearch.Font = new Font("Comic Sans MS", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lblSearch.Location = new Point(15, 18);
            lblSearch.Name = "lblSearch";
            lblSearch.Size = new Size(50, 17);
            lblSearch.TabIndex = 10;
            lblSearch.Text = "Поиск:";
            // 
            // panelBottom
            // 
            panelBottom.Controls.Add(button1);
            panelBottom.Controls.Add(btnAddProduct);
            panelBottom.Controls.Add(btnDeleteProduct);
            panelBottom.Dock = DockStyle.Bottom;
            panelBottom.Location = new Point(0, 559);
            panelBottom.Name = "panelBottom";
            panelBottom.Size = new Size(984, 50);
            panelBottom.TabIndex = 10;
            // 
            // button1
            // 
            button1.FlatStyle = FlatStyle.System;
            button1.Font = new Font("Comic Sans MS", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            button1.Location = new Point(299, 10);
            button1.Name = "button1";
            button1.Size = new Size(111, 30);
            button1.TabIndex = 7;
            button1.Text = "Заказать";
            button1.UseVisualStyleBackColor = true;
            // 
            // Form3
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(984, 609);
            Controls.Add(pictureBoxNoImage);
            Controls.Add(lblProductCount);
            Controls.Add(dataGridViewProducts);
            Controls.Add(panelTop);
            Controls.Add(panelBottom);
            MinimumSize = new Size(1000, 629);
            Name = "Form3";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Каталог товаров";
            Load += Form3_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridViewProducts).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxNoImage).EndInit();
            panelTop.ResumeLayout(false);
            panelTop.PerformLayout();
            panelBottom.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dataGridViewProducts;
        private TextBox searchTextBox;
        private ComboBox filterComboBox;
        private ComboBox sortComboBox;
        private Button btnAddProduct;
        private Button btnDeleteProduct;
        private Label lblProductCount;
        private PictureBox pictureBoxNoImage;
        private Panel panelTop;
        private Label lblSort;
        private Label lblFilter;
        private Label lblSearch;
        private Panel panelBottom;
        private ToolTip toolTip1;
        private Button button1;
    }
}