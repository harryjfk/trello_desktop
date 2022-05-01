using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraBars;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Trello_Desktop
{
    public partial class RibbonForm1 : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public RibbonForm1()
        {
            InitializeComponent();
          
        }


        private Data data = new Data();

        private void bbiNew_ItemClick(object sender, ItemClickEventArgs e)
        {
            var t = new NewProyectForm();
            if(t.ShowDialog()==System.Windows.Forms.DialogResult.OK)
            {
                var s = new Project();
                if (t.buttonEdit2.Text.Trim() != "")
                    s.loadFromFile(t.buttonEdit2.Text);
                s.URL = t.textEdit2.Text;
                s.Name = t.textEdit1.Text;
                data.List.Add(s);
            }
        }

        private void RibbonForm1_Load(object sender, EventArgs e)
        {

            if (File.Exists(this.getFile()))
            {
                var f = new FileStream(this.getFile(), FileMode.Open);
                LoadFromStream(f);
                f.Close();

                foreach(var i in data.List)
                {
                   var t=  treeList1.AppendNode(null,null,i);
                   t.SetValue("Nombre", i.Name);
                }

            }
        }

        private void RibbonForm1_FormClosed(object sender, FormClosedEventArgs e)
        {
            var f = new FileStream(this.getFile(), FileMode.Create);
            SaveToStream(f);
            f.Close();
        }
        private String getFile()
        {
            return Path.GetDirectoryName(Application.ExecutablePath) + @"\data.bin";
        }
        public void LoadFromStream(Stream stream)
        {
            IFormatter formatter = new BinaryFormatter();
            data = (Data)formatter.Deserialize(stream);

        }
        public void SaveToStream(Stream stream)
        {

            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, data);
        }

        private object selected;
        private void setProyect(Project project)
        {
           /// project.getFromJson();
            treeList2.FocusedNodeChanged -= treeList2_FocusedNodeChanged;
              //  project.getFromJson();
                treeList2.Nodes.Clear();
                foreach (var cat in project.Categories)
                {
                    var t = treeList2.AppendNode(null, null, cat);
                    t.SetValue("Nombre", cat.Name.Replace('"', ' ').Trim());
                }
          //  }
           
            setSelected(project);
            treeList2.FocusedNodeChanged += treeList2_FocusedNodeChanged;
            //https://trello.com/b/RmEP39yV.json
        }
        private void setCategory(Categories categories)
        {
            setSelected(categories);

         
            treeList5.FocusedNodeChanged -= treeList5_FocusedNodeChanged;
            treeList5.Nodes.Clear();
            foreach (var card in categories.Cards)
            {
                
                    var t = treeList5.AppendNode(null, null, card);
                    t.SetValue("Nombre", card.Name.Replace('"', ' ').Trim());
                  
               
            }
           
            treeList5.FocusedNodeChanged += treeList5_FocusedNodeChanged;

        }
        private void setSelected(object select)
        {
            selected = select;
            propertyGridControl1.SelectedObject = select;
            propertyGridControl1.RetrieveFields();

        }
        private void treeList1_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            setProyect((Project)e.Node.Tag);
        }

        private void treeList2_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            if(e.Node!=null)
            setCategory((Categories)e.Node.Tag);
        }

        private void treeList1_MouseClick(object sender, MouseEventArgs e)
        {
          //  setProyect((Project)treeList1.Fo.Tag);
        }

        private void setCard(Cards card)
        {
            setSelected(card);
            var date = ((Project)treeList1.FocusedNode.Tag).Current;

            treeList3.FocusedNodeChanged -= treeList3_FocusedNodeChanged;
            treeList3.Nodes.Clear();
            foreach (var checklist in card.CheckList)
                foreach (var items in checklist.Items)
                {
                    var t = treeList3.AppendNode(null, null, items);
                    t.SetValue("Nombre", items.Name.Replace('"', ' ').Trim());
                    t.SetValue("Nuevo", items.Datetime > date);
                    t.SetValue("Completado", items.Completed);
                }

            treeList3.FocusedNodeChanged += treeList3_FocusedNodeChanged;

            treeList4.FocusedNodeChanged -= treeList4_FocusedNodeChanged;
            treeList4.Nodes.Clear();
            foreach (var comment in card.Comments)
               
                {
                    var t = treeList4.AppendNode(null, null, comment);
                    t.SetValue("Nombre", comment.Text.Replace('"', ' ').Trim());
                    t.SetValue("Nuevo", comment.Datetime > date);
                }
            treeList4.FocusedNodeChanged += treeList4_FocusedNodeChanged;


        }
        private void setCheckItem(CheckListItem categories)
        {
            setSelected(categories);

        }
        
             private void setComment(Comment categories)
        {
            setSelected(categories);


        }
        private void treeList3_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            setCheckItem((CheckListItem)e.Node.Tag);
        }

        private void bbiRefresh_ItemClick(object sender, ItemClickEventArgs e)
        {
            ((Project)treeList1.FocusedNode.Tag).loadFromUrl();

        }

        private void treeList5_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            setCard((Cards)e.Node.Tag);
        }

        private void treeList4_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            if(e.Node!=null)
            setComment((Comment)e.Node.Tag);
        }

        private void bbiEdit_ItemClick(object sender, ItemClickEventArgs e)
        {

        }
    }
}