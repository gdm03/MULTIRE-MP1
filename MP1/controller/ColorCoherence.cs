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
        private int connectednessValue;
        private int neighborhoodDistance;
        private int nColorBuckets;
        private float[,] quantizedImage;
        private int width;
        private int height;

        public ColorCoherence(int connectedness, int neighboringPixelDistance, int numColors)
        {
            this.connectednessValue = connectedness;
            this.neighborhoodDistance = neighboringPixelDistance;
        }

        public static double getSimilarity(Dictionary<int, CoherenceUnit> hist1, Dictionary<int, CoherenceUnit> hist2)
        {
            return 0;
        }

        public Dictionary<int,CoherenceUnit> getColorCoherenceHistogram(Bitmap img)
        {
            ComputeHistogram ch = new ComputeHistogram();
            Dictionary<LUVClass, float> luv = ch.convertToLuv(ch.getRGBValues(img));
            Dictionary<int, float> histogram = ch.quantizeColors(luv,0);
            this.width = img.Width;
            this.height = img.Height;
            return checkCoherence(histogram, luv, img.Width, img.Height);
        }

        private int coherenceCounter;
        Boolean[,] marked;
        private Dictionary<int, CoherenceUnit> checkCoherence(Dictionary<int, float> histogram, Dictionary<LUVClass, float> luvHist, int width, int height)
        {
            Dictionary<int, CoherenceUnit> ccv = new Dictionary<int, CoherenceUnit>();
            Quantize q = new Quantize();
            int T = Convert.ToInt32(width * height * 0.01);

            marked = new Boolean[width, height];
            for (int i = 0; i < width; i++) {
                for (int j = 0; j < height; j++)
                {
                    marked[i, j] = false;
                }
            }
            LUVPair[,] luvMatrix = convertToLuvMatrix(luvHist, width, height);
            for(int x = 0; x < width; x++)
            {
                for(int y = 0; y<height; y++)
                {
                    if (!marked[x, y])
                    {
                        coherenceCounter = 0;
                        float a = 0; float b = 0;
                        int luvIndex = q.IndexOf(luvMatrix[x,y].luv.L, luvMatrix[x, y].luv.u, luvMatrix[x, y].luv.v);
                        checkNeighbors(luvMatrix, luvIndex, x, y);

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
                            ccv.Add(luvIndex, new CoherenceUnit(a,b));
                        }
                    }
                }
            }
            return ccv;
        }

        private void checkNeighbors(LUVPair[,] luvMatrix, int luvIndex, int x, int y)
        {
            Quantize q = new Quantize();
            if (q.IndexOf(luvMatrix[x, y].luv.L, luvMatrix[x, y].luv.u, luvMatrix[x, y].luv.v) == luvIndex)
            {
                coherenceCounter++;
                marked[x, y] = true;
                if (x + 1 < this.width && !marked[x + 1, y])
                {
                    checkNeighbors(luvMatrix, luvIndex, x + 1, y);
                }
                if (y + 1 < this.height && !marked[x, y + 1])
                {
                    checkNeighbors(luvMatrix, luvIndex, x, y + 1);
                }
            }
            
        }

        private LUVPair[,] convertToLuvMatrix(Dictionary<LUVClass,float> luvHist,int width, int height)
        {
            LUVPair[,] luvMatrix = new LUVPair[width, height];
            int counter = 0;
            foreach (KeyValuePair<LUVClass, float> entry in luvHist)
            {
                int x = counter / height;
                int y = counter % height;
                luvMatrix[x, y] = new LUVPair(entry.Key, entry.Value);
            }
            return luvMatrix;
        }

    }
    class LUVPair
    {
        public LUVClass luv { get; set; }
        public float value { get; set; }
        public LUVPair(LUVClass l, float f)
        {
            luv = l;
            value = f;
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
