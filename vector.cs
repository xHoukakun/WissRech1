using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net.Http;
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
        private int method = 0; //methode welche genommen werden soll 0=id 1=hilbert 2 = FE
        private double eps = 0; //fehler abbruchbedingung
      
        public vector(int N, double[] b, double[] txk, double eps,int method,int iterationen)
        {
            this.N = N;
            this.b = b;
            this.txk = txk;
            this.eps = eps;
            this.method = method;
            erstelle_sol();
            erstelle_tdk();
            erstelle_trk();
            erstelle_xk1();
            erstelle_tzk();
            switch (method)
            {
                default:
                    break;
                case 0:
                    CG_method(identity, b, txk, iterationen,eps);
                    break;
                case 1:
                    CG_method(hilbert, b, txk, iterationen, eps);
                    break;
                case 2:
                    CG_method(FE, b, txk, iterationen, eps);
                    break;

            }


            for (int h = 0; h < N; h++)
            {
                sol[h] = txk[h];
                Console.WriteLine(sol[h] + " , "+txk[h]);
            }
            Console.WriteLine("Der Fehler beträgt: " + fehler2 + " für Methode; " + method);

        }
        private void erstelle_tzk()
        {
            tzk = new double[N];
            for (int i = 0; i < N; i++)
            {
                tzk[i] = 0;
            }
        }
        private void erstelle_xk1()
        {
            txk1 = new double[N];
            for (int i = 0; i < N; i++)
            {
                txk1[i] = 0;
            }
        }
        private void erstelle_tdk()
        {
            tdk = new double[N];
            for (int i = 0; i < N; i++)
            {
                tdk[i] = 0;
            }
        }
        private void erstelle_trk()
        {
            trk = new double[N];
            for (int i = 0; i < N; i++)
            {
                trk[i] = 0;
            }
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


        private void erstelle_sol()
        {
            sol = new double[N];
            for(int i=0;i<N; i++)
            {
                sol[i] = 0;
            }
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
            double sumrk = 0;
            double sumzk = 0;
            double fehler = 0;
            double sumrk2 = 0;
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
                sumrk = 0;
                sumzk = 0;
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
                sumrk2 = 0;
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
                Console.WriteLine("Vektor in der iteration bal: " + i);
                for(int j=0;j<N;j++)
                {
                    Console.WriteLine(txk[j]);
                }
            } while (i < iterator && fehler > eps);
            fehler = 0;
            for (int j = 0; j < N; j++)
            {
                fehler = fehler + trk[j] * trk[j];
            }
            fehler = Math.Sqrt(fehler);
            this.fehler2 = fehler;
        }
    
            
            
        
        private double[] identity(double[] x)
        {
            return x;
        }
        private double[] hilbert(double[] x)
        {
            double sum = 0;
            double[] y = new double[x.Length];
            for (int i = 0; i < x.Length; i++)
            {
                sum = 0;
                for (int j = 0; j < x.Length; j++)
                {
                    sum += x[j] * 1 / (i + j + 1);
                }
                y[i] = sum;
            }
            return y;
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
