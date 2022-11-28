using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WissRech1
{

    public class vector
    {
        private double[] b;
        private double[] sol;
        private double[] txk;
        private double[] tdk;
        private double[] trk;
        private double[] txk1;
        private double[] tzk;
        double tak, tbk;
        double fehler2;
        private int N;
        public vector(int N)
        {
            this.N = N;
            erstelle_vectorb();
            erstelle_sol();
            ausgabe();
        }
        public vector(int N, double[] b, double[] txk, double eps)
        {
            this.N = N;
            this.b = b;
            this.txk = txk;
            erstelle_sol();
            erstelle_tdk();
            erstelle_trk();
            erstelle_xk1();
            erstelle_tzk();

            CG_method(identity, b, txk, 1, eps);
            
            for (int h = 0; h < N; h++)
            {
                sol[h]=txk[h];
                Console.WriteLine(sol[h]);
            }
            Console.WriteLine("Der Fehler beträgt: " + fehler2);

        }
        private void erstelle_tzk()
        {
            tzk = new double[N];
        }
        private void erstelle_xk1()
        {
            txk1 = new double[N];
        }
        private void erstelle_tdk()
        {
            tdk = new double[N];

        }
        private void erstelle_trk()
        {
            trk = new double[N];
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="b">Lösungvektor</param>
        /// <param name="n">Dimension</param>
        public vector(double[] b, int n)
        {
            this.b = b;
            N = n;
            erstelle_sol();
        }

        private void erstelle_vectorb()
        {
            b = new double[N];
            for (int i = 0; i < N; i++)
            {
                b[i] = 2;
            }
        }
        private void erstelle_sol()
        {
            sol = new double[N];
        }
        private void ausgabe()
        {
            for (int i = 0; i < N; i++)
            {
                Console.WriteLine(b[i]);
            }
        }
        private void CG_method(Func<double[], double[]> matrix_vec, double[] b, double[] xk, int iterator, double eps)
        {
            double fehler = 0;
            int i = 0;
            do
            {

                if (i == 0)
                {


                    txk1 = matrix_vec(xk);
                    for (int k = 0; k < N; k++)
                    {
                        trk[k] = b[k] - txk1[k];
                        tdk[k] = trk[k];
                    }
                }
                tzk = matrix_vec(tdk);
                double sumrk = 0;
                double sumzk = 0;
                fehler = 0;
                for (int j = 0; j < N; j++)
                {
                    sumzk = sumzk + tdk[j] * tzk[j];
                    sumrk = sumrk + trk[j] * trk[j];
                    fehler = fehler + trk[j] * trk[j];
                }
                fehler = Math.Sqrt(fehler);
                if (sumzk == 0)
                {
                    tak = 0;
                }
                else
                {
                    tak = sumrk / sumzk;
                }
                double sumrk2 = 0;
                for (int j = 0; j < N; j++)
                {
                    txk[j] = txk[j] + tak * tdk[j];
                    trk[j] = trk[j] - tak * tzk[j];
                    sumrk2 = sumrk2 + trk[j] * trk[j];
                }
                if (sumrk == 0)
                { tbk = 0; }
                else
                {


                    tbk = sumrk2 / sumrk;
                }

                for (int j = 0; j < N; j++)
                {
                    tdk[j] = trk[j] + tbk * tdk[j];
                }
               
                i++;
            } while (i < iterator && fehler > eps);
            this.fehler2 = fehler;
        }
    
            
            
        
        private double[] identity(double[] x)
        {
            return x;
        }
        private double[] hilbert(double[] x)
        {
            double[] y = new double[x.Length];
            for (int i = 0; i < x.Length; i++)
            {
                double sum = 0;
                for (int j = 0; j < x.Length; j++)
                {
                    sum += x[j] * 1 / (i + j + 1);
                }
                y[i] = sum;
            }
            return (y);
        }



        private double[] FE(double[] x)
        {
            double[] y = new double[x.Length];
            for (int i = 1; i < x.Length - 1; i++)
            {
                y[i] = -x[i - 1] + 2 * x[i] - x[i + 1];
            }
            y[0] = 2 * x[0] - x[1];
            y[x.Length - 1] = -x[x.Length - 2] + 2 * x[x.Length - 1];
            return y;
        }
    }
  
}
