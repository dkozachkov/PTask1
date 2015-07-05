using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTask1
{
    class HalfSearch
    {
        private double a;
        private double b;
        private Func<double, double> f;
        private double eps;
        private List<double[]> points;

        public HalfSearch(Func<double, double> fu = null, double a = -1, double b = 1, double eps = 0.01)
        {
            this.a = a;
            this.b = b;
            f = fu; //��������������, ��� ������� ����������, �� ���� ������������.
            if (fu == null) f = EqualFunc;
            this.eps = eps;
            points = new List<double[]>();
        }

        /// <summary>
        /// ���������� ����� ��� ���� ����������� ��������.
        /// </summary>
        /// <returns></returns>
        public List<double[]> IterationPoints()
        { return points; }

        public double MinPointSearch()
        {
            double x2 = a + 0.5 * (b - a);
            do
            {
                double x1 = a + 0.25 * (b - a);
                double x3 = a + 0.75 * (b - a);
                points.Add(new double[] { x1, x2, x3 });
                if (f(x1) < f(x2))
                {
                    b = x2;
                    x2 = x1;
                    continue;
                }
                if (f(x2) < f(x3))
                {
                    a = x1;
                    b = x3;
                    continue;
                }
                a = x2;
                x2 = x3;
            } while (b - a > eps);
            return x2;
        }

        /// <summary>
        /// ����������� �������: f(x)=x. ������������ ��� ���������� ��� ���������������� ���������������� �������.
        /// </summary>
        /// <param name="inp"></param>
        /// <returns></returns>
        private double EqualFunc(double inp)
        { return inp; }
    }
}