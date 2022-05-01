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

namespace Trello_Desktop
{
    public partial class NewProyectForm : DevExpress.XtraEditors.XtraForm
    {
        public NewProyectForm()
        {
            InitializeComponent();
        }

        private void layoutControlItem5_Click(object sender, EventArgs e)
        {
            
        }

        private void buttonEdit2_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                buttonEdit2.Text = openFileDialog1.FileName;

            }
        }
    }
}