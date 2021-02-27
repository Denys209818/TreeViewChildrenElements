using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TreeViewAddElement.Entities;
using TreeViewAddElement.Models;

namespace TreeViewAddElement
{
    public partial class AddChildrenElementForm : Form
    {
        private TreeNode _parentNode { get; set; }
        private EFContext _context { get; set; }
        public AddChildrenElementForm(EFContext context, TreeNode parentNode)
        {
            InitializeComponent();
            this._context = context;
            this._parentNode = parentNode;
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.txtName.Text))
            {
                ComboBoxModel parentData = this.cbParents.SelectedItem as ComboBoxModel;

                TreeViewCategoryElement newEl = new TreeViewCategoryElement {
                    Name = this.txtName.Text,
                    UrlSlug = this.txtName.Text.ToLower().Replace(' ', '-'),
                    Parent = this._context.Categories.First(x => x.Id == parentData.Id)
                };
                this._context.Categories.Add(newEl);
                this._context.SaveChanges();
                this.DialogResult = DialogResult.OK;
            }
            else 
            {
                MessageBox.Show("Заповніть усі поля!");
            }
        }

        private void AddChildrenElementForm_Load(object sender, EventArgs e)
        {
            this.txtName.Text = "Батьківська категорія: " +this._parentNode.Text;
            var list = this._context.Categories.Select(x => new ComboBoxModel { 
                Id = x.Id,
                Name = x.Name
            });

            foreach (var item in list) 
            {
                this.cbParents.Items.Add(item);
            }
            foreach (var item in this.cbParents.Items) 
            {
                if((item as ComboBoxModel).Id == (this._parentNode.Tag as TreeViewCategoryElement).Id)
                this.cbParents.SelectedItem = item;
            }
        }
    }
}
