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

        public Dictionary<int, float> getHistogram(Bitmap img)
        {
            Dictionary<int, float> histogram = new Dictionary<int, float>();
            Dictionary<Color, float> rgbHistogram = new Dictionary<Color, float>();
            Dictionary<LUVClass, float> luvHistogram = new Dictionary<LUVClass, float>();

            rgbHistogram = getRGBValues(img);
            int imgDimensions = getImgDimensions(img);
            luvHistogram = convertToLuv(rgbHistogram); // Convert RGB histogram to LUV histogram
            histogram = quantizeColors(luvHistogram, imgDimensions);

            return histogram;
        }

        public Dictionary<int, float> getCRHistogram(Bitmap img, bool isCenter)
        {
            CenteringRefinement cr = new CenteringRefinement(img);
            int imgDimensions = cr.getDimensions(isCenter);

            return quantizeColors(convertToLuv(cr.getRgbHistogram(isCenter)), imgDimensions);
        }

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

        public Dictionary<Color, float> getRGBValues(Bitmap img, int width_disp, int height_disp, bool center)
        {
            Dictionary<Color, float> nonCenterHist = new Dictionary<Color, float>();
            Dictionary<Color, float> centerHist = new Dictionary<Color, float>();

            
            for (int i = 0; i < img.Width; i++)
            {
                for (int j = 0; j < img.Height; j++)
                {
                    if ((i < width_disp || i >= img.Width - width_disp) && (j < height_disp || j >= img.Height - height_disp))
                    {
                        Color pixel = img.GetPixel(i, j);

                        if (nonCenterHist.ContainsKey(pixel))
                        {
                            nonCenterHist[pixel] = nonCenterHist[pixel] + 1;
                        }
                        else
                        {
                            nonCenterHist.Add(pixel, 1);
                        }
                    }

                    else
                    {
                        Color pixel = img.GetPixel(i, j);

                        if (centerHist.ContainsKey(pixel))
                        {
                            centerHist[pixel] = centerHist[pixel] + 1;
                        }
                        else
                        {
                            centerHist.Add(pixel, 1);
                        }
                    }
                }
            }

            if (!center)
                return nonCenterHist;
            else
                return centerHist;
        }
        

        public Dictionary<LUVClass, float> convertToLuv(Dictionary<Color,float> histogram)
        {
            Dictionary<LUVClass, float> hist = new Dictionary<LUVClass, float>();

            foreach (Color key in histogram.Keys)
            {
                var myRgb = new Rgb { R = key.R, G = key.G, B = key.B };
                var myLuv = myRgb.To<Luv>();
                
                hist.Add(new LUVClass(myLuv.L, myLuv.U, myLuv.V), histogram[key]);
            }

            return hist;
        }

        public Dictionary<int, float> quantizeColors(Dictionary<LUVClass,float> histogram, int imgDimensions)
        {
            Dictionary<int, float> normalizedHist = new Dictionary<int, float>();
            Quantize q = new Quantize();
            foreach (LUVClass luv in histogram.Keys)
            {
                int luvIndex = q.IndexOf(luv.L, luv.u, luv.v);

                if (normalizedHist.ContainsKey(luvIndex))
                {
                    normalizedHist[luvIndex] = normalizedHist[luvIndex] + histogram[luv];
                }
                else
                {
                    normalizedHist.Add(luvIndex, histogram[luv]);
                }
            }

            return normalizedHist;
        }

        public float computeSimilarity(float[] hist1, float[] hist2, float threshold)
        {
            float sim = 0;
            float included = 0;

            for (int i = 0; i < 159; i++)
            {
                if (hist1[i] > threshold && hist1[i] != 0 || hist2[i] != 0)
                    included++;
            }

            for (int i = 0; i < 159; i++)
            {
                float num = Math.Abs(hist1[i] - hist2[i]); // Numerator
                float denum = Math.Abs(Math.Max(hist1[i], hist2[i])); // Denominator

                if (num != 0 || denum != 0)
                {
                    float c = 1 - (num / denum);
                    sim += c;
                }
            }

            sim /= included;
            return sim;
        }
    }
}
