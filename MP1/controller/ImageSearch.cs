using MP1.model;
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
 * ImageSearch i = new ImageSearch();
 * ArrayList imagePaths = i.search("test.jpg", ImageSearch.COLOR_COHERENCE);
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

        public ImageSearch(String defaultDir)
        {
            fileEntries = Directory.GetFiles(defaultDir, "*.jpg");
            defaultDirectory = defaultDir;
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
            printDictionary(tempResults);
            return orderedList(tempResults);
        }

        public ArrayList perceptualSim(String imgPath)
        {
            double threshold = 0.2;
            Dictionary<String, double> tempResults = new Dictionary<string, double>();
            PerceptualSimilarity ps = new PerceptualSimilarity();

            ComputeHistogram ch = new ComputeHistogram();
            Dictionary<LUVClass, float> luv = ch.convertToLuv(ch.getRGBValues(new Bitmap(imgPath)));
            Dictionary<int, float> queryHistogram = ch.quantizeColors(luv, 0);

            Console.WriteLine("checking matches...");
            foreach(String path in fileEntries)
            {
                Dictionary<LUVClass, float> luv2 = ch.convertToLuv(ch.getRGBValues(new Bitmap(path)));
                Dictionary<int, float> dHistogram = ch.quantizeColors(luv, 0);
                double similarity = ps.getSimilarity(queryHistogram, dHistogram);
                Console.WriteLine("percep sim: " + similarity);
                if(similarity > threshold)
                {
                    tempResults[path] = similarity;
                }
            }

            printDictionary(tempResults);
            return reversedOrderList(tempResults);
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

            printDictionary(tempResults);
            return orderedList(tempResults);
            
        }
        private ArrayList orderedList(Dictionary<String, double> tempResults)
        {
            ArrayList result = new ArrayList();
            var ordered = tempResults.OrderBy(x => x.Value);
            foreach (KeyValuePair<String, double> k in ordered)
            {
                result.Add(k.Key);
            }
            return result;
        }
        private ArrayList reversedOrderList(Dictionary<String, double> tempResults)
        {
            ArrayList result = new ArrayList();
            var ordered = tempResults.OrderByDescending(x => x.Value);
            foreach (KeyValuePair<String, double> k in ordered)
            {
                result.Add(k.Key);
            }
            return result;
        }
        private void printDictionary(Dictionary<String, double> dict)
        {
            //foreach (KeyValuePair<String, double> k in dict)
            //{
             //   Console.WriteLine("path: " + k.Key + ", similarity: " + k.Value);
            //}
        }
    }
}
