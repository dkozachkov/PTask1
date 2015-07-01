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
        private Label Q1;
        private Label Q2;
        private GroupBox fselector;
        private Label QEps;
        private Label Ans;
        private TextBox par_getter;
        private TextBox[] basepars;
        private RadioButton rb1;
        private RadioButton rb2;
        private Button Execute;
        private List<double[]> iterPoints;

        public Form1()
        {
            InitializeComponent();

            Q1 = new Label();
            Controls.Add(Q1);
            Q1.Text = "Выберите тип минимизируемой функции:";
            Q1.Top = 10;
            Q1.Left = 10;
            Q1.BorderStyle = BorderStyle.FixedSingle;
            Q1.AutoSize = true;
            Q1.Padding = new Padding(5, 5, 5, 5);

            fselector = new GroupBox();
            Controls.Add(fselector);
            fselector.Top = Q1.Bottom + 5;
            fselector.Left = 10;

            rb1 = new RadioButton();
            rb1.Text = "Polynom";
            rb1.Location = new Point(fselector.Left + 5, 10/*fselector.Top + 5*/);
            rb1.AutoSize = true;

            rb2 = new RadioButton();
            //Controls.Add(rb2);
            rb2.Text = "Exponent";
            rb2.Location = new Point(fselector.Left + 5, rb1.Bottom + 5);
            rb2.AutoSize = true;

            
            fselector.Controls.Add(rb1);
            fselector.Controls.Add(rb2);
            fselector.Width = Math.Max(rb1.Right, rb2.Right) + 5;
            fselector.Height = rb2.Bottom + 5;

            Q2 = new Label();
            Controls.Add(Q2);
            Q2.Text = "Введите параметры выбранной функции\n(через пробел):";
            Q2.BorderStyle = BorderStyle.FixedSingle;
            Q2.Height = Q1.Height + 15;
            Q2.Width = Q1.Width;
            Q2.Padding = new Padding(5, 5, 5, 5);
            Q2.Left = 10;
            Q2.Top = fselector.Bottom + 10;

            par_getter = new TextBox();
            Controls.Add(par_getter);
            par_getter.Left = 10;
            par_getter.Top = Q2.Bottom + 5;
            par_getter.Width = Q2.Width;

            QEps = new Label();
            Controls.Add(QEps);
            QEps.Text = "Введите границы промежутка и точность";
            QEps.Size = Q1.Size;
            QEps.Left = 10;
            QEps.BorderStyle = BorderStyle.FixedSingle;
            QEps.Padding = new Padding(5, 5, 5, 5);
            QEps.Top = par_getter.Bottom + 5;

            basepars = new TextBox[3];
            basepars[0] = new TextBox();
            Controls.Add(basepars[0]);
            basepars[1] = new TextBox();
            Controls.Add(basepars[1]);
            basepars[2] = new TextBox();
            Controls.Add(basepars[2]);
            foreach (TextBox tb in basepars)
            {
                tb.Width = QEps.Width / 3 - 8;
                tb.Top = QEps.Bottom + 5;
                tb.MouseClick += TextClear;
            }
            basepars[0].Left = 10;
            basepars[1].Left = basepars[0].Right + 12;
            basepars[2].Left = QEps.Right - basepars[2].Width;
            basepars[0].Text = "a";
            basepars[1].Text = "b";
            basepars[2].Text = "c";

            Execute = new Button();
            Controls.Add(Execute);
            Execute.Left = 5;
            Execute.Text = "Execute";
            Execute.Top = basepars[0].Bottom + 15;
            Execute.AutoSize = true;
            Execute.MouseClick += ProcessMethod;
            Execute.Padding = new Padding(5, 5, 5, 5);

            Ans = new Label();
            Controls.Add(Ans);
            //Ans.Visible = false;
            Ans.Top = Execute.Top;
            Ans.BorderStyle = BorderStyle.FixedSingle;
            Ans.Text = "Answer";
            Ans.Left = Execute.Right + 10;
            Ans.Width = QEps.Right - Ans.Left;
            Ans.Height = Execute.Height;
            Ans.TextAlign = ContentAlignment.MiddleCenter;

            this.Size = new Size(640, 330);
            this.Text = "Минимизация одномерной функции: метод деления пополам";
        }

        private void TextClear(object o, EventArgs ea)
        {
            TextBox tb = (TextBox)o;
            tb.Text = "";
            o = tb;
        }

        private void ProcessMethod(object o, EventArgs ea)
        {
            try 
            {
                int type;
                if (rb1.Checked) type = 0;
                else if (rb2.Checked) type = 1;
                else return;
                string[] pars = par_getter.Text.Split(' ');
                List<double> fpars=new List<double>();
                foreach (string s in pars)
                    fpars.Add(Convert.ToDouble(s));
                double a = Convert.ToDouble(basepars[0].Text);
                double b = Convert.ToDouble(basepars[1].Text);
                double eps = Convert.ToDouble(basepars[2].Text);
                HalfSearch hs = new HalfSearch(new FuncSelector().FuncReturner(type, fpars), a, b, eps);
                double res=hs.MinPointSearch();
                Ans.Text = Ans.Text + ": x=" + res;
            }
            catch { return; }

        }
    }
}
