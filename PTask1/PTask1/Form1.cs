using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PTask1
{
    public partial class Form1 : Form
    {
        private Label Title;
        private Label Q1;
        private Label Q2;
        private CheckedListBox fselector;
        private Label QEps;
        private Label Ans;
        private TextBox par_getter;

        public Form1()
        {
            InitializeComponent();
            this.Size = new Size(640, 480);
            this.Text = "Минимизация одномерной функции: метод деления пополам";
        }
    }
}
