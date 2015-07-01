using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTask1
{
    class FuncSelector
    {
        private List<double> FuncParams;

        public FuncSelector(){}

        //Не уверен, сработает ли возврат функции, учитывая что параметры лежат в этом классе.
        public Func<double, double> FuncReturner(int num, List<double> fpars)
        {
            FuncParams = fpars;
            if (num == 0) return Polynom;
            else return Exponent;
        }

        private double Exponent(double x)
        {
            return Math.Abs(FuncParams[0]) * Math.Exp(x) + FuncParams[1];
        }

        private double Polynom(double x)
        {
            double res=0;
            for (int i = 0; i < FuncParams.Count; i++)
                res += FuncParams[i] * Math.Pow(x, i);
            return res;
        }
    }
}
