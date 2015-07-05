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
        private Label FShow;
        private Label QEps;
        private Label Ans;
        private TextBox par_getter;
        private TextBox[] basepars;
        private RadioButton rb1;
        private RadioButton rb2;
        private Button Execute;
        private List<double[]> iterPoints;

        private Button Draw;
        private PictureBox img;
        private Func<double, double> f;

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
            par_getter.MouseLeave += SetFunc;

            FShow = new Label();
            FShow.Tag = "Функция: ";
            FShow.Text = "Функция: ";
            Controls.Add(FShow);
            FShow.Size = Q1.Size;
            FShow.BorderStyle = BorderStyle.FixedSingle;
            FShow.Padding = new Padding(5, 5, 5, 5);
            FShow.Left = 10;
            FShow.Top = par_getter.Bottom + 5;


            QEps = new Label();
            Controls.Add(QEps);
            QEps.Text = "Введите границы промежутка и точность";
            QEps.Size = Q1.Size;
            QEps.Left = 10;
            QEps.BorderStyle = BorderStyle.FixedSingle;
            QEps.Padding = new Padding(5, 5, 5, 5);
            QEps.Top = FShow.Bottom + 5;

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
            Ans.Tag = "Answer: ";
            Ans.Text = "Answer";
            Ans.Left = Execute.Right + 10;
            Ans.Width = QEps.Right - Ans.Left;
            Ans.Height = Execute.Height;
            Ans.TextAlign = ContentAlignment.MiddleCenter;

            img = new PictureBox();
            Controls.Add(img);
            img.BorderStyle = BorderStyle.FixedSingle;
            img.Left = Q1.Right + 20;
            img.Width = this.Right;
            img.Top = Q1.Top;
            img.Height = this.Height - 30;

            Draw = new Button();
            Controls.Add(Draw);
            Draw.BringToFront();
            Draw.Text = "Draw";
            Draw.Padding = new Padding(5, 5, 5, 5);
            Draw.AutoSize = true;
            Draw.Left = img.Left + img.Width / 2 - Draw.Width / 2;
            Draw.Top = img.Bottom+5;
            Draw.Enabled = false;
            Draw.MouseClick += SetGraphics;
            this.Size = new Size(590, 360);
            this.Text = "Минимизация одномерной функции: метод деления пополам";
        }

        private void TextClear(object o, EventArgs ea)
        {
            TextBox tb = (TextBox)o;
            tb.Text = "";
            tb.MouseClick -= TextClear;
            o = tb;
        }

        private void SetFunc(object o, EventArgs ea)
        {
            try 
            {
                string[] pars = par_getter.Text.Split(' ');
                if (rb1.Checked)
                {
                    string str = pars[0];
                    for (int i = 1; i < pars.Length; i++)
                    {
                        double t=Convert.ToDouble(pars[i]);
                        if (t != 0)
                        {
                            str = str + (Math.Sign(t) == 1 ? "+" : "-");
                            t = Math.Abs(t);
                            str = str + (t == 1 ? "" : "" + t) + "*x" + (i == 1 ? "" : "^" + i);
                        }
                    }
                    FShow.Text =(string)FShow.Tag+str;
                }
                else if (rb2.Checked)
                {
                    double t=Convert.ToDouble(pars[0]);
                    double t1 = Convert.ToDouble(pars[1]);
                    if (t <= 0)
                        return;
                    string str = (t==1?"":t+"*")+"exp("+(t1<0?"-":"")+(Math.Abs(t1)==1?"":""+Math.Abs(t1))+"*x)";
                    FShow.Text = (string)FShow.Tag + str;
                }
                else return;
            }
            catch { return; }
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
                f = new FuncSelector().FuncReturner(type, fpars);
                double a = Convert.ToDouble(basepars[0].Text);
                double b = Convert.ToDouble(basepars[1].Text);
                double eps = Convert.ToDouble(basepars[2].Text);
                HalfSearch hs = new HalfSearch(f, a, b, eps);
                double res=hs.MinPointSearch();
                iterPoints = hs.IterationPoints();
                Ans.Text = (string)Ans.Tag + "x=" + res;
                Draw.Enabled = true;
                Draw.Visible = true;
                Draw.BringToFront();
                img.SendToBack();
            }
            catch { return; }
        }

        private void SetGraphics(object o, EventArgs ea)
        {
            Draw.Enabled = false;
            Draw.Visible = false;
            Draw.SendToBack();
            img.BringToFront();
            double a = Convert.ToDouble(basepars[0].Text);
            double b = Convert.ToDouble(basepars[1].Text);
            double eps = Convert.ToDouble(basepars[2].Text);
            double fmax=Math.Max(f(a),f(b));
            double fmin=f(iterPoints[iterPoints.Count-1][1]);

            Graphics g = img.CreateGraphics();
            g.Clear(this.BackColor);
            int zerox, zeroy;
            double abs_dist_y, abs_dist_x;  //max distances
            //Отрисовка осей.
            if (fmin >= 0)
            {
                g.DrawLine(new Pen(Color.Black), 20, 250, 280, 250);
                zeroy = 250;
                abs_dist_y = fmax;
            }
            else if (fmax <= 0)
            {
                zeroy = 20;
                g.DrawLine(new Pen(Color.Black), 20, 30, 280, 30);
                abs_dist_y = -fmin;
            }
            else
            {
                //fmin<0<fmax
                zeroy=30 +(int)Math.Round(220 * fmax / (fmax - fmin));
                g.DrawLine(new Pen(Color.Black), 20, zeroy, 280,zeroy);
                abs_dist_y = fmax-fmin;
            }
            if (a >= 0)
            {
                zerox = 30;
                g.DrawLine(new Pen(Color.Black), 30, 20, 30, 260);
                abs_dist_x = b;
            }
            else if (b <= 0)
            {
                zerox = 270;
                g.DrawLine(new Pen(Color.Black), 270, 20, 270, 260);
                abs_dist_x = -a;
            }
            else
            {
                zerox = (int)Math.Round(30 - a / (b - a) * 240);
                g.DrawLine(new Pen(Color.Black), zerox, 20, zerox, 260);
                abs_dist_x = b - a;
            }
            g.DrawEllipse(new Pen(Color.Black),zerox-2,zeroy-2,4,4);
            g.DrawString("0", new System.Drawing.Font("Arial", 8), new SolidBrush(Color.Black), new Point(zerox - 9, zeroy));
            g.DrawLines(new Pen(Color.Black), new Point[] { new Point(zerox - 3, 25), new Point(zerox, 20), new Point(zerox + 3, 25) });
            g.DrawLines(new Pen(Color.Black), new Point[] { new Point(275, zeroy - 3), new Point(280, zeroy), new Point(275, zeroy + 3) });
            //Разметка осей.
            int t = zerox - 24;
            double k1 = StepSize(a, b);
            int m = 1;
            while (t >= 20)
            {
                g.DrawLine(new Pen(Color.Black), t, zeroy - 2, t, zeroy + 2);
                g.DrawString("-"+k1*m, new System.Drawing.Font("Arial", 8), new SolidBrush(Color.Black), new Point(t - 9, zeroy));
                t = t - 24;
                m++;
            }
            t = zerox + 24;
            m = 1;
            while(t<=270)
            {
                g.DrawLine(new Pen(Color.Black), t, zeroy - 2, t, zeroy + 2);
                g.DrawString(""+k1*m, new System.Drawing.Font("Arial", 8), new SolidBrush(Color.Black), new Point(t - 9, zeroy));
                t = t + 24;
                m++;
            }
            t = zeroy - 22;
            m = 1;
            double k2 = StepSize(fmin, fmax);
            while (t >= 20)
            {
                g.DrawLine(new Pen(Color.Black), zerox - 2, t, zerox + 2, t);
                g.DrawString("" + k2 * m, new System.Drawing.Font("Arial", 8), new SolidBrush(Color.Black), new Point(zerox - 22, t - 5));
                t = t - 22;
                m++;
            }
            t = zeroy + 22;
            m = 1;
            while (t <= 250)
            {
                g.DrawLine(new Pen(Color.Black), zerox - 2, t, zerox + 2, t);
                g.DrawString("-" + k2 * m, new System.Drawing.Font("Arial", 8), new SolidBrush(Color.Black), new Point(zerox - 22, t - 5));
                t = t + 22;
                m++;
            }
            int p_a = PixelFromCoord("x", a, k1);
            int p_b = PixelFromCoord("x", b, k1);
            int p_fa=PixelFromCoord("y",f(a),k2);

            for (int i = p_a+1; i <= p_b; i++)
            {
                double newx=CoordForPixel("x",i,k1);
                double newf = f(newx);
                int p_f2 = PixelFromCoord("y", newf, k2);
                g.DrawLine(new Pen(Color.DarkGray),zerox+i-1,zeroy-p_fa,zerox+i,zeroy-p_f2);
                p_fa = p_f2;
            }
            //double[][] x_y=new double[2][];
            //x_y[0]=new double[(int)step];
            //x_y[1]=new double[x_y[0].Length];
            //List<Point>pts=new List<Point>();
            //for(double i=a;i<=b;i+=step)
            //{
            //    Point pt=new Point((int)Math.Round(i),(int)Math.Round(f(i)));
            //}
            
        }

        private double CoordForPixel(string dir, int pix, double step)
        {
            //pix - переводимое значение в пикселах
            //step - длина шага (в х-размерности)
            //pix_step - длина шага (в пикселах)
            double pix_step=1;
            if (dir == "x")
                pix_step = 24;
            else if (dir == "y")
                pix_step = 22;
            else return 0;
            return pix*step / pix_step;
            //Возвращает смещение в х-размерах от 0.
        }

        private int PixelFromCoord(string dir, double x,double step)
        {
            //x - переводимое значение
            //step - длина шага (в х-размерности)
            //pix_step - длина шага (в пикселах)
            double pix_step = 1;
            if (dir == "x")
                pix_step = 24;
            else if (dir == "y")
                pix_step = 22;
            else return 0;
            return (int)Math.Round(x * pix_step/step);
            //Возвращает смещение в пикселах (от 0)
        }

        private double StepSize(double a, double b)
        {
            //Задача: вычислить адекватную разметку 10-шаговой сетки. 
            //double M = b - a;
            double M = Math.Max(Math.Abs(a), Math.Abs(b));
            bool side = false;
            if (M < b - a)  //с разных сторон оси
            {
                side = true;
                //M = b - a;
            }
            int r = 0;
            bool flag = true;
            while (M < 0.5)
            {
                //Example:0.0052=>0.052|-1=>0.52|-2=>5.2|-3
                M *= 10;
                r--;
                flag = false;
            }
            if (flag)
            {
                while (M >= 50)
                {
                    //Example:1050=>105|1=>10.5|2=>1.05|3
                    M /= 10;
                    r++;
                }
            }
            int lim = (int)Math.Ceiling(M);
            if (side)  //с разных сторон оси
            {
                int k1 = (int)(10*Math.Abs(a) / lim);
                int k2 = (int)(10*b / lim);
                lim = (int)Math.Ceiling(M * (1 + Math.Max(k1,k2)/10));
            }
            return lim * Math.Pow(10, r - 1);
        }
    }
}
