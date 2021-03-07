using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CandleStick
{
    public partial class Setting : Form
    {
        public Action OnSubmit;
        public Setting()
        {
            InitializeComponent();
            Console.WriteLine("Setting start");
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            OnSubmit.Invoke();
            Hide();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void PicFore_Click(object sender, EventArgs e)
        {
            if(dlgColor.ShowDialog() == DialogResult.OK)
            {
                picFore.BackColor = dlgColor.Color;
            }
        }

        private void PicBack_Click(object sender, EventArgs e)
        {
            if (dlgColor.ShowDialog() == DialogResult.OK)
            {
                picBack.BackColor = dlgColor.Color;
            }
        }

        private void PicUpInSide_Click(object sender, EventArgs e)
        {
            if (dlgColor.ShowDialog() == DialogResult.OK)
            {
                picUpInSide.BackColor = dlgColor.Color;
            }
        }

        private void PicUpBorder_Click(object sender, EventArgs e)
        {
            if (dlgColor.ShowDialog() == DialogResult.OK)
            {
                picUpBorder.BackColor = dlgColor.Color;
            }
        }

        private void PicDownInside_Click(object sender, EventArgs e)
        {
            if (dlgColor.ShowDialog() == DialogResult.OK)
            {
                picDownInside.BackColor = dlgColor.Color;
            }
        }

        private void PicDownBorder_Click(object sender, EventArgs e)
        {
            if (dlgColor.ShowDialog() == DialogResult.OK)
            {
                picDownBorder.BackColor = dlgColor.Color;
            }
        }

        private void picRecent_Click(object sender, EventArgs e)
        {
            if (dlgColor.ShowDialog() == DialogResult.OK)
            {
                picRecent.BackColor = dlgColor.Color;
            }
        }
    }
}
