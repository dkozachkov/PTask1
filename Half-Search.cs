using System;

namespace PTask1
{
	class HalfSearch
	{
        private double a;
        private double b;
        private Func<double,double> f;
        private double eps;

        public HalfSearch(Func<double, double> fu = EqualFunc, double a = -1, double b = 1, double eps = 0.01)
        {
            this.a = a;
            this.b = b;
            f = fu; //предполагается, что функция корректная, то есть унимодальная.
            this.eps = eps;
        }

        public double MinPointSearch()
        {
            double x2 = 0.5 * (b + a);
            do
            {
                double x1 = 0.25 * (b + a);
                double x3 = 0.75 * (b + a);
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
        /// Стандартная функция - "вернуть х". На случай, если другую не предоставят.
        /// </summary>
        /// <param name="inp"></param>
        /// <returns></returns>
        private double EqualFunc(double inp)
        {return inp;}
	}
}