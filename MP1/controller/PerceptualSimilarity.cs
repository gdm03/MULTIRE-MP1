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
        public double[,] similarityMatrix { get; set; }

        public PerceptualSimilarity()
        {
            initializeMaxDistance();
            createMatrix();
        }

        public double getSimilarity(Dictionary<int, float> hist1, Dictionary<int, float> hist2)
        {
            double similarity = 0.0;

            /**effectively discards colors not in query **/

            foreach (KeyValuePair<int, float> nhQ in hist1)
            {
                double simExactCol = 0.0;
                double simPerCol = 0.0;

                float nhIi;
                hist2.TryGetValue(nhQ.Key, out nhIi);
                simExactCol = 1.0 - Math.Abs(nhQ.Value - nhIi) / Math.Max(nhQ.Value, nhIi);
                // get simPerCol
                foreach (KeyValuePair<int, float> nhI in hist1)
                {
                    //Console.WriteLine("sim("+nhQ.Key+ ","+nhI.Key+ "): " + similarityMatrix[nhQ.Key, nhI.Key]);
                    if (similarityMatrix[nhQ.Key, nhI.Key] != 0) // if nhIj is perceptually similar to nhQi
                    {
                        double val = (1.0 - Math.Abs(nhQ.Value - nhI.Value) / Math.Max(nhQ.Value, nhI.Value));
                        simPerCol += val * similarityMatrix[nhQ.Key, nhI.Key];
                    }
                }

                double simCol = simExactCol * (1 + simPerCol);
                double simColor = simCol * nhQ.Value;
                similarity += simColor;
            }
            return similarity;
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
        public void createMatrix()
        {
            int N = 159;
            similarityMatrix = new double[N,N];
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
            //return similarityMatrix;
        }

        private double getEuclideanDistance(LUVClass l1, LUVClass l2)
        {
            double val = Math.Sqrt(Math.Pow(l1.L - l2.L, 2) + Math.Pow(l1.u - l2.u, 2) + Math.Pow(l1.v - l2.v, 2));
            return val;
        }
    }
}
