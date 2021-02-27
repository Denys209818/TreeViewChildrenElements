using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TreeViewAddElement.Entities;
using TreeViewAddElement.Services;

namespace TreeViewAddElement
{
    public partial class MainForm : Form
    {
        private EFContext _context { get; set; }
        public MainForm()
        {
            InitializeComponent();
            this._context = new EFContext();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            DbSeeder.SeedAll(this._context);
            FillParentsTreeView();
        }

        private void FillParentsTreeView() 
        {
            
            this.tvCategories.Nodes.Clear();

            var list = this._context.Categories.Where(x => x.ParentId == null).Select(x => new TreeNode { 
                Name = x.Id.ToString(),
                Text = x.Name,
                Tag = x
            }).ToList();

            foreach (var item in list) 
            {
                item.Nodes.Add("");
                this.tvCategories.Nodes.Add(item);
            }
        }

        private void tvCategories_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Nodes[0].Text == "") 
            {
                e.Node.Nodes.Clear();
                TreeNode parent = e.Node;
               
                var children = this._context.Categories
                    .Where(x =>x.ParentId == (parent.Tag as TreeViewCategoryElement).Id).Select(x => new TreeNode { 
                        Name = x.Id.ToString(),
                        Text = x.Name,
                        Tag = x,
                        
                    }).ToList();
                if (children.Count() > 0)
                {
                    foreach (var item in children)
                    {
                        item.Nodes.Add("");

                        parent.Nodes.Add(item);
                    }
                }
                else 
                {
                    parent.Nodes.Add("");
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (this.tvCategories.SelectedNode != null)
            {
                AddChildrenElementForm form = new AddChildrenElementForm(this._context, this.tvCategories.SelectedNode);
                if (form.ShowDialog() == DialogResult.OK) 
                {
                    FillParentsTreeView();
                }
            }
            else 
            {
                MessageBox.Show("Оберіть батьківський елемент!");
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.txtSearch.Text)) 
            {
                //  Переініціалізовує колекцію
                FillParentsTreeView();
                //  Витягує елементи з БД де Назва категорії дорівнює тексту в текстовому полі 
                //  (не враховуючи регітр)
                var coll = this._context.Categories.Where(x => x.Name.ToUpper()
                .Contains(this.txtSearch.Text.ToUpper())).ToList();

                foreach (var item in coll) 
                {
                    //  Масив, що означає шлях ідентифікаторів. кожен елемент масиву це рівень дерева,
                    //  який містить інформацію про дочірній елемент який необхідно розкрити, щоб знайти підходящий елемент.
                    List<int> id = new List<int>();
                    var el = item;
                    //  Поки батьківський елемент, що знайдений у БД не поверне нуль записуємо номера батьківських
                    //  елементів (тобто формуємо шлях)
                    while (el != null) 
                    {
                        id.Add(el.Id);
                        el = el.Parent;
                    }
                    //  Перевартається масив, щоб самий перший елемент це був кореневий елемент 
                    id.Reverse();
                    //  Повертається батьківські ноди для подальшого заповнення їх дочірніми елементами
                    var col = this.tvCategories.Nodes;
                    //  Цикл по номерах колекції id. Проходить кожен рівень дерева від батьківського елемента 
                    //  до дочірнього. Використовує при цьому колекцію ідентифікаторів де 0 елемент - перший рівень дерева
                    //  1 - другий рівень і тд.
                    for (int i = 0; i < id.Count; i++) 
                    {
                        //  Прохід по нодах, щоб визначити який ідентифікатор співпадає з іменем TreeNode
                        foreach (var element in col) 
                        {
                            var node = element as TreeNode;
                            //  Якщо числа співпали то це той елемент який потрібно розкрити, щоб знайти потрібний елемент
                            if ((node.Tag as TreeViewCategoryElement).Id == id[i]) 
                            {
                                //  Якщо ідентифікатор ноди співпадає з i(Рівень) елементом колекції то він розкривається
                                node.Expand();
                                //  Передається колекція дочірніх елементів для того щоб далі продовжити розкривати
                                //  підходящі елементи поки колекція ідентифікатор (шлях) не закінчиться.
                                //  І також формуються підкатегорії для даного батьківського елементу
                                col = node.Nodes;
                            }
                        }
                    }
                }
            }
        }
    }
}
