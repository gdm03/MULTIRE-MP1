using MP1.model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MP1.controller
{
    class PerceptualSimilarity
    {
        public double p = 0.2;
        public double dMax;
        public double tColor;

        public PerceptualSimilarity()
        {
            initializeMaxDistance();
        }

        private void initializeMaxDistance()
        {
            int N = 159;
            Quantize q = new Quantize();
            double max = 0;
            for (int index1 = 0; index1 < N; index1++)
            {
                for (int index2 = 0; index2 < N; index2++)
                {
                    LUVClass luv1 = q.getLUVfromIndex(index1);
                    LUVClass luv2 = q.getLUVfromIndex(index2);
                    double distance = getEuclideanDistance(luv1, luv2);
                    if (distance > max) max = distance;
                }
            }
            dMax = max;
            tColor = p * dMax;
        }
        public double[,] createMatrix(Bitmap img)
        {
            //histogram
            //ComputeHistogram ch = new ComputeHistogram();
            //Dictionary<LUVClass, float> luv = ch.convertToLuv(ch.getRGBValues(img));
            //Dictionary<int, float> histogram = ch.quantizeColors(luv, 0);
            int N = 159;
            double[,] similarityMatrix = new double[N,N];
            Quantize q = new Quantize();

            for (int index1 = 0; index1 < N; index1++)
            {
                for (int index2 = 0; index2 < N; index2++)
                {
                    LUVClass luv1 = q.getLUVfromIndex(index1);
                    LUVClass luv2 = q.getLUVfromIndex(index2);
                    double distance = getEuclideanDistance(luv1, luv2);
                    if(distance > tColor)
                    {
                        similarityMatrix[index1, index2] = 0;
                    }
                    else
                    {
                        similarityMatrix[index1, index2] = 1 - distance / tColor;
                    }
                   
                }
            }
            return similarityMatrix;
        }

        private double getEuclideanDistance(LUVClass l1, LUVClass l2)
        {
            return Math.Sqrt(Math.Pow(l1.L - l2.L, 2) + Math.Pow(l1.u - l2.u, 2) + Math.Pow(l1.v - l2.v, 2));
        }
    }
}
