namespace NUnit.Gui.Views
{
	partial class CategoryView
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.selectedCategoriesGroup = new System.Windows.Forms.GroupBox();
			this.selectedCatagories = new System.Windows.Forms.ListBox();
			this.excludeCategories = new System.Windows.Forms.CheckBox();
			this.actions = new System.Windows.Forms.TableLayoutPanel();
			this.addCategory = new System.Windows.Forms.Button();
			this.removeCategory = new System.Windows.Forms.Button();
			this.availableCategoriesGroup = new System.Windows.Forms.GroupBox();
			this.availableCatagories = new System.Windows.Forms.ListBox();
			this.selectedCategoriesGroup.SuspendLayout();
			this.actions.SuspendLayout();
			this.availableCategoriesGroup.SuspendLayout();
			this.SuspendLayout();
			// 
			// selectedCategoriesGroup
			// 
			this.selectedCategoriesGroup.Controls.Add(this.selectedCatagories);
			this.selectedCategoriesGroup.Controls.Add(this.excludeCategories);
			this.selectedCategoriesGroup.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.selectedCategoriesGroup.Location = new System.Drawing.Point(0, 295);
			this.selectedCategoriesGroup.Name = "selectedCategoriesGroup";
			this.selectedCategoriesGroup.Padding = new System.Windows.Forms.Padding(5);
			this.selectedCategoriesGroup.Size = new System.Drawing.Size(308, 123);
			this.selectedCategoriesGroup.TabIndex = 0;
			this.selectedCategoriesGroup.TabStop = false;
			this.selectedCategoriesGroup.Text = "Selected Categories";
			// 
			// selectedCatagories
			// 
			this.selectedCatagories.Dock = System.Windows.Forms.DockStyle.Fill;
			this.selectedCatagories.FormattingEnabled = true;
			this.selectedCatagories.Location = new System.Drawing.Point(5, 18);
			this.selectedCatagories.Name = "selectedCatagories";
			this.selectedCatagories.Size = new System.Drawing.Size(298, 83);
			this.selectedCatagories.TabIndex = 1;
			// 
			// excludeCategories
			// 
			this.excludeCategories.AutoSize = true;
			this.excludeCategories.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.excludeCategories.Location = new System.Drawing.Point(5, 101);
			this.excludeCategories.Name = "excludeCategories";
			this.excludeCategories.Size = new System.Drawing.Size(298, 17);
			this.excludeCategories.TabIndex = 0;
			this.excludeCategories.Text = "Exclude these categories";
			this.excludeCategories.UseVisualStyleBackColor = true;
			// 
			// actions
			// 
			this.actions.ColumnCount = 4;
			this.actions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.actions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
			this.actions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
			this.actions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.actions.Controls.Add(this.addCategory, 1, 0);
			this.actions.Controls.Add(this.removeCategory, 2, 0);
			this.actions.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.actions.Location = new System.Drawing.Point(0, 250);
			this.actions.Name = "actions";
			this.actions.Padding = new System.Windows.Forms.Padding(0, 7, 0, 7);
			this.actions.RowCount = 1;
			this.actions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.actions.Size = new System.Drawing.Size(308, 45);
			this.actions.TabIndex = 1;
			// 
			// addCategory
			// 
			this.addCategory.Dock = System.Windows.Forms.DockStyle.Fill;
			this.addCategory.Location = new System.Drawing.Point(57, 10);
			this.addCategory.Name = "addCategory";
			this.addCategory.Size = new System.Drawing.Size(94, 25);
			this.addCategory.TabIndex = 0;
			this.addCategory.Text = "Add";
			this.addCategory.UseVisualStyleBackColor = true;
			// 
			// removeCategory
			// 
			this.removeCategory.Dock = System.Windows.Forms.DockStyle.Fill;
			this.removeCategory.Location = new System.Drawing.Point(157, 10);
			this.removeCategory.Name = "removeCategory";
			this.removeCategory.Size = new System.Drawing.Size(94, 25);
			this.removeCategory.TabIndex = 1;
			this.removeCategory.Text = "Remove";
			this.removeCategory.UseVisualStyleBackColor = true;
			// 
			// availableCategoriesGroup
			// 
			this.availableCategoriesGroup.Controls.Add(this.availableCatagories);
			this.availableCategoriesGroup.Dock = System.Windows.Forms.DockStyle.Fill;
			this.availableCategoriesGroup.Location = new System.Drawing.Point(0, 0);
			this.availableCategoriesGroup.Name = "availableCategoriesGroup";
			this.availableCategoriesGroup.Padding = new System.Windows.Forms.Padding(5);
			this.availableCategoriesGroup.Size = new System.Drawing.Size(308, 250);
			this.availableCategoriesGroup.TabIndex = 2;
			this.availableCategoriesGroup.TabStop = false;
			this.availableCategoriesGroup.Text = "Available Categories";
			// 
			// availableCatagories
			// 
			this.availableCatagories.Dock = System.Windows.Forms.DockStyle.Fill;
			this.availableCatagories.FormattingEnabled = true;
			this.availableCatagories.Location = new System.Drawing.Point(5, 18);
			this.availableCatagories.Name = "availableCatagories";
			this.availableCatagories.Size = new System.Drawing.Size(298, 227);
			this.availableCatagories.TabIndex = 0;
			// 
			// CategoriesView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.availableCategoriesGroup);
			this.Controls.Add(this.actions);
			this.Controls.Add(this.selectedCategoriesGroup);
			this.Name = "CategoriesView";
			this.Size = new System.Drawing.Size(308, 418);
			this.selectedCategoriesGroup.ResumeLayout(false);
			this.selectedCategoriesGroup.PerformLayout();
			this.actions.ResumeLayout(false);
			this.availableCategoriesGroup.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox selectedCategoriesGroup;
		private System.Windows.Forms.CheckBox excludeCategories;
		private System.Windows.Forms.TableLayoutPanel actions;
		private System.Windows.Forms.Button addCategory;
		private System.Windows.Forms.Button removeCategory;
		private System.Windows.Forms.GroupBox availableCategoriesGroup;
		private System.Windows.Forms.ListBox availableCatagories;
		private System.Windows.Forms.ListBox selectedCatagories;
	}
}
