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
        public PerceptualSimilarity()
        {

        }

        public double[,] createMatrix(Bitmap img)
        {
            //histogram
            ComputeHistogram ch = new ComputeHistogram();
            Dictionary<LUVClass, float> luv = ch.convertToLuv(ch.getRGBValues(img));
            Dictionary<int, float> histogram = ch.quantizeColors(luv, 0);
            double[,] similarityMatrix = new double[histogram.Count, histogram.Count];
            Quantize q = new Quantize();
            int i = 0;
            foreach(int index1 in histogram.Keys)
            {
                int j = 0;
                foreach(int index2 in histogram.Keys)
                {
                    LUVClass luv1 = q.getLUVfromIndex(index1);
                    LUVClass luv2 = q.getLUVfromIndex(index2);
                    double distance = getEuclideanDistance(luv1, luv2);
                    similarityMatrix[i, j] = distance;
                    j++;
                }
                i++;
            }
            return similarityMatrix;
        }

        private double getEuclideanDistance(LUVClass l1, LUVClass l2)
        {
            return Math.Sqrt(Math.Pow(l1.L - l2.L, 2) + Math.Pow(l1.u - l2.u, 2) + Math.Pow(l1.v - l2.v, 2));
        }
    }
}
