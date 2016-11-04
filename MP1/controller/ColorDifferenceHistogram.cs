using System;
using ColorMine.ColorSpaces;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MP1.controller
{
    class ColorDifferenceHistogram
    {

        public double[] createHistogram(Bitmap img)
        {

            int wid = img.Width;
            int hei = img.Height;

            int lnum = 10;
            int anum = 3;
            int bnum = 3;

            int cnum = lnum * anum * bnum;
            int onum = 18;

            double[] hist = new double[cnum + onum];
            int[,] ori = new int[wid, hei];
            int[,] colors = new int[wid, hei];

            LABClass[,] labMatrix = convertToLab(img);
            quantizeColors(ref labMatrix, out colors, lnum, anum, bnum, wid, hei);
            getOrientations(ref labMatrix, out ori, onum, wid, hei);

            int D = 1;

            finalizeHistogram(ref colors, ref ori, ref labMatrix, out hist, wid, hei, cnum, onum, D);
            return hist;

        }

        public double getSimilarity(double[] hist1, double[] hist2)
        {
            double M = 108;
            double distance = 0.0;
            for(int i = 0; i < M; i++)
            {
                double uT = 0.0;
                for(int j = 0; j < M; j++)
                {
                    uT += hist1[j] / M;
                }

                double uQ = 0.0;
                for (int j = 0; j < M; j++)
                {
                    uQ += hist2[j] / M;
                }

                double val = Math.Abs(hist1[i] - hist2[i]) / Math.Abs(hist1[i] + uT) + Math.Abs(hist2[i] + uQ);
                distance += val;
            }
            return distance;
        }
        private LABClass[,] convertToLab(Bitmap img)
        {
            LABClass[,] labMatrix = new LABClass[img.Width, img.Height];
            for(int i = 0; i < img.Width; i++)
            {
                for (int j = 0; j < img.Height; j++)
                {
                    Color rgb = img.GetPixel(i, j);
                    var myRgb = new Rgb { R = rgb.R, G = rgb.G, B = rgb.B };
                    var myLuv = myRgb.To<Lab>();
                    labMatrix[i, j] = new LABClass(myLuv.L, myLuv.A, myLuv.B);
                }
            }
            return labMatrix;   
        }

        private void quantizeColors(ref LABClass[,] Lab, out int[,] img, int colnum1, int colnum2, int colnum3, int wid, int hei)
        {
            img = new int[wid, hei];


            int L = 0, a = 0, b = 0;

            for (int i = 0; i < wid; i++)
            {
                for (int j = 0; j < hei; j++)
                {
                    L = Convert.ToInt32(Lab[i, j].L * colnum1 / 100.0);

                    if (L >= (colnum1 - 1))
                    {
                        L = colnum1 - 1;
                    }
                    if (L < 0)
                    {
                        L = 0;
                    }

                    a = Convert.ToInt32((Lab[i, j].a + 127) * colnum2 / 254.0);
                    if (a >= (colnum2 - 1))
                    {
                        a = colnum2 - 1;
                    }
                    if (a < 0)
                    {
                        a = 0;
                    }

                    b = Convert.ToInt32((Lab[i, j].b + 127) * colnum3 / 254.0);
                    if (b >= (colnum3 - 1))
                    {
                        b = colnum3 - 1;
                    }
                    if (b < 0)
                    {
                        b = 0;
                    }

                    //-------------------------------------------
                    img[i, j] = (colnum3 * colnum2) * L + colnum3 * a + b;

                }
            }
        }

        private void transformMatrix(ref LABClass[,] arr, out double[,,] Lab, int wid, int hei)
        {
            Lab = new double[3, wid, hei];

            for (int i = 0; i < wid; i++)
            {
                for (int j = 0; j < hei; j++)
                {
                    Lab[0, i, j] = arr[i, j].L;

                    Lab[1, i, j] = arr[i, j].a;

                    Lab[2, i, j] = arr[i, j].b;

                    //--------------------------
                    Lab[1, i, j] = arr[i, j].a + 127.0;

                    if (Lab[1, i, j] >= 254.0)
                    {
                        Lab[1, i, j] = 254.0 - 1.0;
                    }
                    if (Lab[1, i, j] < 0)
                    {
                        Lab[1, i, j] = 0;
                    }

                    Lab[2, i, j] = arr[i, j].b + 127.0;

                    if (Lab[2, i, j] >= 254.0)
                    {
                        Lab[2, i, j] = 254.0 - 1.0;
                    }
                    if (Lab[2, i, j] < 0)
                    {
                        Lab[2, i, j] = 0;
                    }
                }
            }
        }

        private void getOrientations(ref LABClass[,] LAB, out int[,] ori, int num, int wid, int hei)
        {
            double[,,] Lab = new double[3, wid, hei];

            transformMatrix(ref LAB, out Lab, wid, hei);

            ori = new int[wid, hei];

            double gxx = 0.0, gyy = 0.0, gxy = 0.0;

            double rh = 0.0, gh = 0.0, bh = 0.0;
            double rv = 0.0, gv = 0.0, bv = 0.0;

            double theta = 0.0;

            for (int i = 1; i <= wid - 2; i++)
            {
                for (int j = 1; j <= hei - 2; j++)
                {


                    rh = (Lab[0, i - 1, j + 1] + 2 * Lab[0, i, j + 1] + Lab[0, i + 1, j + 1]) - (Lab[0, i - 1, j - 1] + 2 * Lab[0, i, j - 1] + Lab[0, i + 1, j - 1]);
                    gh = (Lab[1, i - 1, j + 1] + 2 * Lab[1, i, j + 1] + Lab[1, i + 1, j + 1]) - (Lab[1, i - 1, j - 1] + 2 * Lab[1, i, j - 1] + Lab[1, i + 1, j - 1]);
                    bh = (Lab[2, i - 1, j + 1] + 2 * Lab[2, i, j + 1] + Lab[2, i + 1, j + 1]) - (Lab[2, i - 1, j - 1] + 2 * Lab[2, i, j - 1] + Lab[2, i + 1, j - 1]);

                    rv = (Lab[0, i + 1, j - 1] + 2 * Lab[0, i + 1, j] + Lab[0, i + 1, j + 1]) - (Lab[0, i - 1, j - 1] + 2 * Lab[0, i - 1, j] + Lab[0, i - 1, j + 1]);
                    gv = (Lab[1, i + 1, j - 1] + 2 * Lab[1, i + 1, j] + Lab[1, i + 1, j + 1]) - (Lab[1, i - 1, j - 1] + 2 * Lab[1, i - 1, j] + Lab[1, i - 1, j + 1]);
                    bv = (Lab[2, i + 1, j - 1] + 2 * Lab[2, i + 1, j] + Lab[2, i + 1, j + 1]) - (Lab[2, i - 1, j - 1] + 2 * Lab[2, i - 1, j] + Lab[2, i - 1, j + 1]);

                    gxx = rh * rh + gh * gh + bh * bh;
                    gyy = rv * rv + gv * gv + bv * bv;
                    gxy = rh * rv + gh * gv + bh * bv;

                    theta = Math.Round(Math.Atan(2.0 * gxy / (gxx - gyy + 0.00001)) / 2.0, 4);

                    double G1 = 0.0;
                    double G2 = 0.0;

                    G1 = Math.Sqrt(0.5 * ((gxx + gyy) + (gxx - gyy) * Math.Cos(2.0 * theta) + 2.0 * gxy * Math.Sin(2.0 * theta)));
                    G2 = Math.Sqrt(0.5 * ((gxx + gyy) + (gxx - gyy) * Math.Cos(2.0 * (theta + (Math.PI / 2.0))) + 2.0 * gxy * Math.Sin(2.0 * (theta + (Math.PI / 2.0)))));

                    double dir = 0;


                    if (Math.Max(G1, G2) == G1)
                    {
                        dir = 90.0 + theta * 180.0 / Math.PI;
                        ori[i, j] = Convert.ToInt32(dir * num / 360.0);
                    }
                    else
                    {
                        dir = 180.0 + (theta + Math.PI / 2.0) * 180.0 / Math.PI;

                        ori[i, j] = Convert.ToInt32(dir * num / 360.0);
                    }

                    if (ori[i, j] >= num - 1) ori[i, j] = num - 1;

                }
            }
        }

        public void finalizeHistogram(ref int[,] ColorX, ref int[,] ori, ref LABClass[,] LAB, out double[] hist, int wid, int hei, int CSA, int CSB, int D)
        {
            double[,,] Arr = new double[3, wid, hei];

            transformMatrix(ref LAB, out Arr, wid, hei);
      
            double[] Matrix = new double[CSA + CSB];

            hist = new double[CSA + CSB];
            

            int i, j;
            

            for (i = 0; i <= wid - 1; i++)
            {
                for (j = 0; j <= hei - D - 1; j++)
                {
                    double value = 0.0;

                    if (ori[i, j + D] == ori[i, j])
                    {
                        value = Math.Sqrt(Math.Pow(Arr[0, i, j + D] - Arr[0, i, j], 2) + Math.Pow(Arr[1, i, j + D] - Arr[1, i, j], 2) + Math.Pow(Arr[2, i, j + D] - Arr[2, i, j], 2));

                        Matrix[ColorX[i, j]] += value;

                    }
                    if (ColorX[i, j + D] == ColorX[i, j])
                    {
                        value = Math.Sqrt(Math.Pow(Arr[0, i, j + D] - Arr[0, i, j], 2) + Math.Pow(Arr[1, i, j + D] - Arr[1, i, j], 2) + Math.Pow(Arr[2, i, j + D] - Arr[2, i, j], 2));

                        Matrix[ori[i, j] + CSA] += value;
                    }
                }
            }

            for (i = 0; i <= wid - D - 1; i++)
            {
                for (j = 0; j <= hei - 1; j++)
                {
                    double value = 0.0;

                    if (ori[i + D, j] == ori[i, j])
                    {
                        value = Math.Sqrt(Math.Pow(Arr[0, i + D, j] - Arr[0, i, j], 2) + Math.Pow(Arr[1, i + D, j] - Arr[1, i, j], 2) + Math.Pow(Arr[2, i + D, j] - Arr[2, i, j], 2));

                        Matrix[ColorX[i, j]] += value;

                    }
                    if (ColorX[i + D, j] == ColorX[i, j])
                    {
                        value = Math.Sqrt(Math.Pow(Arr[0, i + D, j] - Arr[0, i, j], 2) + Math.Pow(Arr[1, i + D, j] - Arr[1, i, j], 2) + Math.Pow(Arr[2, i + D, j] - Arr[2, i, j], 2));

                        Matrix[ori[i, j] + CSA] += value;

                    }
                }
            }

            for (i = 0; i <= wid - D - 1; i++)
            {
                for (j = 0; j <= hei - D - 1; j++)
                {
                    double value = 0.0;

                    if (ori[i + D, j + D] == ori[i, j])
                    {
                        value = Math.Sqrt(Math.Pow(Arr[0, i + D, j + D] - Arr[0, i, j], 2) + Math.Pow(Arr[1, i + D, j + D] - Arr[1, i, j], 2) + Math.Pow(Arr[2, i + D, j + D] - Arr[2, i, j], 2));

                        Matrix[ColorX[i, j]] += value;

                    }
                    if (ColorX[i + D, j + D] == ColorX[i, j])
                    {
                        value = Math.Sqrt(Math.Pow(Arr[0, i + D, j + D] - Arr[0, i, j], 2) + Math.Pow(Arr[1, i + D, j + D] - Arr[1, i, j], 2) + Math.Pow(Arr[2, i + D, j + D] - Arr[2, i, j], 2));

                        Matrix[ori[i, j] + CSA] += value;

                    }
                }
            }

            for (i = D; i <= wid - 1; i++)
            {
                for (j = 0; j <= hei - D - 1; j++)
                {
                    double value = 0.0;

                    if (ori[i - D, j + D] == ori[i, j])
                    {
                        value = Math.Sqrt(Math.Pow(Arr[0, i - D, j + D] - Arr[0, i, j], 2) + Math.Pow(Arr[1, i - D, j + D] - Arr[1, i, j], 2) + Math.Pow(Arr[2, i - D, j + D] - Arr[2, i, j], 2));
                        Matrix[ColorX[i, j]] += value;

                    }
                    if (ColorX[i - D, j + D] == ColorX[i, j])
                    {
                        value = Math.Sqrt(Math.Pow(Arr[0, i - D, j + D] - Arr[0, i, j], 2) + Math.Pow(Arr[1, i - D, j + D] - Arr[1, i, j], 2) + Math.Pow(Arr[2, i - D, j + D] - Arr[2, i, j], 2));
                        Matrix[ori[i, j] + CSA] += value;

                    }
                }
            }

            for (i = 0; i < CSA + CSB; i++)
            {

                hist[i] = (Matrix[i]) / 4.0;

            }

        }


    }
    class LABClass
    {
        public double L { get; set; }
        public double a { get; set; }
        public double b { get; set; }

        public LABClass(double L, double a, double b)
        {
            this.L = L;
            this.a = a;
            this.b = b;
        }
    }
}
