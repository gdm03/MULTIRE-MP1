﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MP1.controller
{
    class CenteringCH
    {
        Dictionary<int, float> quantizedHistogram = new Dictionary<int, float>();
        Dictionary<int, float> normalizedHistogram = new Dictionary<int, float>();

        float[] hist1 = new float[159];
        float[] hist2 = new float[159];
        String[] imagePaths;
        List<String> paths = new List<string>(); // list of other images in directory
        
        int imgDimensions = 0;
        int currImgDimensions = 0;

        float threshold = 0.0F;
        float sim = 0;
        float simThreshold = 0.9F;
        float centering = 0.3F;

        List<float> centeredSim = new List<float>();
        List<float> nonCenteredSim = new List<float>();
        //float centeredSim = 0;
        //float nonCenteredSim = 0;

        ComputeHistogram ch = new ComputeHistogram(); // for histogram operations
        List<String> similarImagesPaths = new List<string>();
        
        // Change directory here
        public CenteringCH()
        {
            String dir = @"D:\DLSU-M\Term 1 AY 2016-2017\CSC741M\MP1_files\MP1\images\";
            imagePaths = Directory.GetFiles(dir, "*.jpg", SearchOption.AllDirectories);
        }

        public float returnAveSim()
        {
            /*
            foreach(float f in centeredSim)
            {
                Debug.WriteLine("Centered: " + f + " Non-centered: " + nonCenteredSim);
            }
            */
            Debug.WriteLine("Centered: " + centeredSim.Sum() + " Non-centered: " + nonCenteredSim.Sum());
            float test = centeredSim.Sum() + nonCenteredSim.Sum();

            return 1.0F;
            //return (centeredSim + nonCenteredSim) / 2;
        }

        public List<String> returnRelevantImages(Bitmap image, bool center)
        {
            Bitmap img = new Bitmap(image);
            //bool center = true;
            CenteringRefinement cr = new CenteringRefinement(img, centering);
            imgDimensions = cr.getDimensions(center);

            quantizedHistogram = ch.getCRHistogram(img, center, centering);

            foreach (int x in quantizedHistogram.Keys)
            {
                normalizedHistogram.Add(x, quantizedHistogram[x] / imgDimensions);
            }

            for (int i = 0; i < 159; i++)
            {
                normalizedHistogram.TryGetValue(i, out hist1[i]);
            }
            
            foreach (String s in imagePaths)
            {
                paths.Add(s);
            }

            // Loop for currentImg
            foreach (String s in paths)
            {
                Dictionary<int, float> currQuantizedHistogram = new Dictionary<int, float>();
                Dictionary<int, float> currNormalizedHistogram = new Dictionary<int, float>();
                Bitmap currImg = new Bitmap(s);

                // CH with Centering Refinement
                CenteringRefinement cr2 = new CenteringRefinement(currImg, centering);
                //bool cent = false;

                currImgDimensions = cr2.getDimensions(center);
                currQuantizedHistogram = ch.getCRHistogram(img, center, centering);

                sim = 0;

                foreach (int x in currQuantizedHistogram.Keys)
                {
                    currNormalizedHistogram.Add(x, currQuantizedHistogram[x] / currImgDimensions);
                }

                for (int i = 0; i < 159; i++)
                {
                    currNormalizedHistogram.TryGetValue(i, out hist2[i]);
                }

                sim = ch.computeSimilarity(hist1, hist2, threshold);
                if (sim > simThreshold)
                {
                    similarImagesPaths.Add(s);
                }

                //Debug.WriteLine(sim);
                if (center)
                    centeredSim.Add(sim);
                else
                    nonCenteredSim.Add(sim);
            }

            return similarImagesPaths;
        }
    }
}
