using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/**
 * Use (only) this class to use the algorithms
 * Simply call search function (query image, algo number)
 * search function returns an ArrayList of image paths;
 * 
 * ImageSearch is = new ImageSearch();
 * ArrayList imagePaths = is.search("test.jpg", ImageSearch.COLOR_COHERENCE);
 * */

namespace MP1.controller
{
    class ImageSearch
    {
        public static readonly int COLOR_COHERENCE = 0;
        public static readonly int PERCEPTUAL_SIM = 1;
        public static readonly int COLOR_DIFF_HISTOGRAM = 2;

        private String defaultDirectory = "";
        private String[] fileEntries;

        public ImageSearch()
        {
            fileEntries = Directory.GetFiles(defaultDirectory, "*.jpg");
        }

        public ArrayList search(String imgPath, int algo)
        {
            switch (algo)
            {
                case 0: return colorCoherence(imgPath);
                case 1: return perceptualSim(imgPath);
                case 2: return colorDiffHistogram(imgPath);
                default: break;
            }
            return new ArrayList();
        }

        public void setDirectory(String dir)
        {
            defaultDirectory = dir;
            fileEntries = Directory.GetFiles(defaultDirectory, "*.jpg");
        }
        public ArrayList colorCoherence(String imgPath)
        {
            double threshold = 0.2;
            Dictionary<String, double> tempResults = new Dictionary<string, double>();

            ColorCoherence ccv = new ColorCoherence();
            Dictionary<int, CoherenceUnit> queryHistogram = ccv.getColorCoherenceHistogram(new Bitmap(imgPath));
            foreach(String path in fileEntries)
            {
                Dictionary<int, CoherenceUnit> dHistogram = ccv.getColorCoherenceHistogram(new Bitmap(path));
                double similarity = ccv.getSimilarity(queryHistogram, dHistogram);
                if (similarity > threshold)
                {
                    tempResults[path] = similarity;
                }

            }

            return orderedList(tempResults);
        }

        public ArrayList perceptualSim(String imgPath)
        {
            ArrayList result = new ArrayList();
            Bitmap img = new Bitmap(imgPath);

            ComputeHistogram ch = new ComputeHistogram();


            return result;
        }

        public ArrayList colorDiffHistogram(String imgPath)
        {
            double threshold = 0.2;
            ColorDifferenceHistogram cdh = new ColorDifferenceHistogram();
            Dictionary<String, double> tempResults = new Dictionary<string, double>();

            double[] queryHistogram = cdh.createHistogram(new Bitmap(imgPath));
            foreach (String path in fileEntries)
            {
                double[] dHistogram = cdh.createHistogram(new Bitmap(path));
                double similarity = cdh.getSimilarity(queryHistogram, dHistogram);
                if(similarity > threshold)
                {
                    tempResults[path] = similarity;
                }
            }

            return orderedList(tempResults);
            
        }
        private ArrayList orderedList(Dictionary<String, double> tempResults)
        {
            ArrayList result = new ArrayList();
            var ordered = tempResults.OrderByDescending(x => x.Value);
            foreach (KeyValuePair<String, double> k in ordered)
            {
                result.Add(k.Key);
            }
            return result;
        }
    }
}
