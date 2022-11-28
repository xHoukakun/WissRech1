using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WissRech1
{
    internal class Program
    {
        
        static void Main(string[] args)
        {
           
            int N = 10;
            double[] b = new double[N];
            double[] txk = new double[N];
            for(int i = 0; i < N; i++)
            {
                b[i] = 1;
                txk[i] = Math.Pow(-1,i) *10000;
            }
            vector mein_vector = new vector(N, b, txk, 0.01);

            Console.ReadKey();
            

        }
    }
}
