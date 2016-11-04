using ColorMine.ColorSpaces;
using MP1.model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MP1.controller
{
    class ColorCoherence
    {
        //private int connectednessValue;
        //private int neighborhoodDistance;
        //private int nColorBuckets;
        //private float[,] quantizedImage;
        private int width;
        private int height;

        public ColorCoherence()
        {

        }

        public double getDistance(Dictionary<int, CoherenceUnit> hist1, Dictionary<int, CoherenceUnit> hist2)
        {
            double distance = 0.0;
            foreach (KeyValuePair<int, CoherenceUnit> k in hist1)
            {
                //fix for key not found
                float tempA;
                float tempB;
                CoherenceUnit dColor;
                if(hist2.TryGetValue(k.Key, out dColor))
                {
                    tempA = hist2[k.Key].aCoherentValue;
                    tempB = hist2[k.Key].bIncoherentValue;
                }
                else
                {
                    tempA = 0;
                    tempB = 0;
                }

                double val = Math.Abs(k.Value.aCoherentValue - tempA)
                            + Math.Abs(k.Value.bIncoherentValue - tempB);
                distance += val;
            }
            return distance;
        }

        public Dictionary<int, CoherenceUnit> getColorCoherenceHistogram(Bitmap img)
        {
            this.width = img.Width;
            this.height = img.Height;
            LUVClass[,] luv = getLuvMatrix(img);
            return checkCoherence(luv, img.Width, img.Height);
        }

        private LUVClass[,] getLuvMatrix(Bitmap img)
        {
            LUVClass[,] matrix = new LUVClass[img.Width, img.Height];
            for (int i = 0; i < img.Width; i++)
            {
                for (int j = 0; j < img.Height; j++)
                {
                    Color pixel = img.GetPixel(i, j);
                    var myRgb = new Rgb { R = pixel.R, G = pixel.G, B = pixel.B };
                    var myLuv = myRgb.To<Luv>();
                    matrix[i, j] = new LUVClass(myLuv.L, myLuv.U, myLuv.V);
                }
            }
            return matrix;
        }

        private LUVClass[,] blurredMatrix(LUVClass[,] luv)
        {
            LUVClass[,] result = new LUVClass[width,height];
            for(int i = 1; i < width - 1; i+=2)
            {
                for(int j = 1; j <  height - 1; j += 2)
                {
                    double aveL = 0.0;
                    double aveU = 0.0;
                    double aveV = 0.0;

                    aveL +=     luv[i - 1, j - 1].L + luv[i, j - 1].L + luv[i + 1, j - 1].L +
                                luv[i - 1, j].L + luv[i, j].L + luv[i + 1, j].L +
                                luv[i - 1, j + 1].L + luv[i, j + 1].L + luv[i + 1, j + 1].L;
                    aveU += luv[i - 1, j - 1].u + luv[i, j - 1].u + luv[i + 1, j - 1].u +
                                luv[i - 1, j].u + luv[i, j].u + luv[i + 1, j].u +
                                luv[i - 1, j + 1].u + luv[i, j + 1].u + luv[i + 1, j + 1].u;
                    aveV += luv[i - 1, j - 1].v + luv[i, j - 1].v + luv[i + 1, j - 1].v +
                                luv[i - 1, j].v + luv[i, j].v + luv[i + 1, j].v +
                                luv[i - 1, j + 1].v + luv[i, j + 1].v + luv[i + 1, j + 1].v;

                    aveL /= 9;
                    aveU /= 9;
                    aveV /= 9;

                    LUVClass myLuv = new LUVClass(aveL, aveU, aveV);
                    result[i - 1, j - 1] =myLuv; result[i, j - 1] =myLuv; result[i + 1, j - 1] =myLuv;
                    result[i - 1, j] =myLuv; result[i, j] =myLuv; result[i + 1, j] =myLuv;
                    result[i - 1, j + 1] =myLuv; result[i, j + 1] =myLuv; result[i + 1, j + 1] = myLuv;
                }
            }
            return result;
        }
        private int coherenceCounter;
        Boolean[,] marked;
        private Dictionary<int, CoherenceUnit> checkCoherence(LUVClass[,] luvMatrix, int width, int height)
        {
            Dictionary<int, CoherenceUnit> ccv = new Dictionary<int, CoherenceUnit>();
            Quantize q = new Quantize();
            int T = Convert.ToInt32(width * height * 0.01);

            marked = new Boolean[width, height];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    marked[i, j] = false;
                }
            }
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (!marked[x, y])
                    {
                        coherenceCounter = 0;
                        float a = 0; float b = 0;
                        int luvIndex = q.IndexOf(luvMatrix[x, y].L, luvMatrix[x, y].u, luvMatrix[x, y].v);
                        checkNeighbors(luvMatrix, luvIndex, x, y);
                        //Console.WriteLine("coherence: " + coherenceCounter);
                        if (coherenceCounter >= T) a += coherenceCounter;
                        else b += coherenceCounter;

                        if (ccv.ContainsKey(luvIndex))
                        {
                            ccv[luvIndex] = new CoherenceUnit(
                                    ccv[luvIndex].aCoherentValue + a,
                                    ccv[luvIndex].bIncoherentValue + b
                                );
                        }
                        else
                        {
                            ccv.Add(luvIndex, new CoherenceUnit(a, b));
                        }
                    }
                }
            }

            //normalize
            Dictionary<int, CoherenceUnit> ccvNorm = new Dictionary<int, CoherenceUnit>();
            int dim = width * height;
            foreach(KeyValuePair<int,CoherenceUnit> k in ccv)
            {
                CoherenceUnit c = new CoherenceUnit(ccv[k.Key].aCoherentValue / dim, ccv[k.Key].bIncoherentValue / dim);
                ccvNorm[k.Key] = c;
            }
            return ccvNorm;
        }

        private void checkNeighbors(LUVClass[,] luvMatrix, int luvIndex, int x, int y)
        {
            Quantize q = new Quantize();
            if (q.IndexOf(luvMatrix[x, y].L, luvMatrix[x, y].u, luvMatrix[x, y].v) == luvIndex)
            {
                coherenceCounter++;
                marked[x, y] = true;
                if (x + 1 < this.width - 1 && !marked[x + 1, y])
                {
                    checkNeighbors(luvMatrix, luvIndex, x + 1, y);
                }
                if (y + 1 < this.height - 1 && !marked[x, y + 1])
                {
                    checkNeighbors(luvMatrix, luvIndex, x, y + 1);
                }
            }

        }
    }
    class CoherenceUnit
    {
        public float aCoherentValue { get; set; }
        public float bIncoherentValue { get; set; }

        public CoherenceUnit(float a, float b)
        {
            this.aCoherentValue = a;
            this.bIncoherentValue = b;
        }
    }
}
