using ColorMine.ColorSpaces;
using MP1.model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MP1.controller
{
    class ComputeHistogram
    {
        public int getImgDimensions (Bitmap img) { return img.Width * img.Height; }

        public Dictionary<Color, float> getRGBValues(Bitmap img)
        {
            Dictionary<Color, float> histogram = new Dictionary<Color, float>();

            for (int i = 0; i < img.Width; i++)
            {
                for (int j = 0; j < img.Height; j++)
                {
                    Color pixel = img.GetPixel(i, j);

                    if (histogram.ContainsKey(pixel))
                    {
                        histogram[pixel] = histogram[pixel] + 1;
                    }
                    else
                    {
                        histogram.Add(pixel, 1);
                    }
                }
            }
            return histogram;
        }

        public Dictionary<LUVClass, float> convertToLuv(Dictionary<Color,float> histogram)
        {
            //float counter = 0;
            Dictionary<LUVClass, float> hist = new Dictionary<LUVClass, float>();

            foreach (Color key in histogram.Keys)
            {
                var myRgb = new Rgb { R = key.R, G = key.G, B = key.B };
                var myLuv = myRgb.To<Luv>();

                //Debug.WriteLine(myLuv.L + " " + myLuv.U + " " + myLuv.V);
                hist.Add(new LUVClass(myLuv.L, myLuv.U, myLuv.V), histogram[key]);
                //counter += histogram[key];
            }
            //Debug.WriteLine("LUV Counter: " + counter);
            return hist;
        }

        public Dictionary<int, float> quantizeColors(Dictionary<LUVClass,float> histogram, int imgDimensions)
        {
            Dictionary<int, float> normalizedHist = new Dictionary<int, float>();
            Quantize q = new Quantize();

            //float counter = 0;

            foreach (LUVClass luv in histogram.Keys)
            {
                //Debug.WriteLine(luv.ToString() + " [Key]: " + histogram[luv]);
                int luvIndex = q.IndexOf(luv.L, luv.u, luv.v);

                if (normalizedHist.ContainsKey(luvIndex))
                {
                    //normalizedHist[luvIndex] = normalizedHist[luvIndex] + 1;
                    normalizedHist[luvIndex] = normalizedHist[luvIndex] + histogram[luv];
                }
                else
                {
                    //normalizedHist.Add(luvIndex, histogram[luv]);
                    normalizedHist.Add(luvIndex, histogram[luv]);
                }
                //counter += histogram[luv];
                //Debug.WriteLine(counter);
            }

            return normalizedHist;
        }

        public float computeSimilarity(float[] hist1, float[] hist2, float threshold)
        {
            float sim = 0;
            float included = 0;

            for (int i = 0; i < 159; i++)
            {
                //Debug.WriteLine("Color: " + i + " Value: " + hist1[i] + " || " + hist2[i]);

                if (hist1[i] > threshold && hist1[i] != 0 || hist2[i] != 0)
                    included++;

            }
            for (int i = 0; i < 159; i++)
            {
                float num = Math.Abs(hist1[i] - hist2[i]); // Numerator
                float denum = Math.Abs(Math.Max(hist1[i], hist2[i])); // Denominator

                if (num != 0 || denum != 0)
                {

                    //Debug.WriteLine(num + "/" + denum);
                    float c = 1 - (num / denum);
                    sim += c;
                }
            }

            sim /= included;
            return sim;
        }
    }
}
