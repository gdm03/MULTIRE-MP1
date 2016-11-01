using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Diagnostics;
using MP1.model;
using System.IO;
using MP1.controller;

using ColorMine;
using ColorMine.ColorSpaces;
using System.Text.RegularExpressions;

namespace MP1
{
    public partial class MP1Form : Form
    {
        public MP1Form()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void selectedImage_Click(object sender, EventArgs e)
        {

        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            Dictionary<Color, float> histogram = new Dictionary<Color, float>();
            Dictionary<LUVClass, float> hist = new Dictionary<LUVClass, float>();
            Dictionary<int, float> normalizedHist = new Dictionary<int, float>();

            Dictionary<Color, float> currentImgHistogram = new Dictionary<Color, float>();
            Dictionary<LUVClass, float> currentImgHist = new Dictionary<LUVClass, float>();
            Dictionary<int, float> currNormalizedHist = new Dictionary<int, float>();

            // Change directory here
            String dir = @"D:\DLSU-M\Term 1 AY 2016-2017\CSC741M\MP1_files\MP1\images\";

            String[] imagePaths = Directory.GetFiles(dir, "*.jpg", SearchOption.AllDirectories);
            List<String> paths = new List<string>(); // list of other images in directory

            ComputeHistogram ch = new ComputeHistogram(); // for histogram operations
            //Quantize q = new Quantize(); 

            // .jpg only? same size only? padding
            // Filter file type
            //ofd.Filter 

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                String imgPath = ofd.FileName;

                selectedImageBox.Image = new Bitmap(imgPath);
                Bitmap img = new Bitmap(imgPath);
                
                // Selected image RGB histogram
                histogram = ch.getRGBValues(img);
                hist = ch.convertToLuv(histogram); // Convert RGB histogram to LUV histogram
                normalizedHist = ch.quantizeColors(hist); // Quantize LUV histogram to 159 colors
                /*
                foreach (Color key in histogram.Keys)
                {
                    Debug.WriteLine("Key: " + key + "Number: " + histogram[key]);
                }
                */

                // Adds all images in directory to List excluding selected image
                foreach (String s in imagePaths)
                {
                    if (!s.Equals(imgPath))
                    {
                        paths.Add(s);
                    }
                }
                
                // Current image RGB histogram
                /*
                foreach (String s in paths)
                {
                    Bitmap currImg = new Bitmap(s);
                    currentImgHistogram = ch.getRGBValues(currImg);
                    currentImgHist = ch.convertToLuv(currentImgHistogram);
                    currNormalizedHist = ch.quantizeColors(currentImgHist);
                }
                */

                /*
                float counter = 0;
                float each2 = 0;
                foreach (int n in normalizedHist.Keys)
                {
                    float each = normalizedHist[n] / imgDimensions;
                    Debug.WriteLine("Color: " + n + " Number: " + normalizedHist[n]);
                    counter += normalizedHist[n];
                    Debug.WriteLine("last: " + counter + " ::: " + each);
                    each2 += each;
                    Debug.WriteLine("each2: " + each2);
                }
                */

                /*
                foreach (Color key in normHist.Keys)
                {
                    Debug.WriteLine(key.ToString() + ": " + normHist[key]);
                    // Normalize RGB to 0-1


                    // Quantize RGB to LUV

                    // Get number of each LUV and compute for normalized Hist


                    // NH(Q) threshold
                    
                    if (normHist[key] > threshold)
                    {

                    }
                    

                    // Get histogram of each image in directory

                }
                */
                


                // Dispose old image??
            }


        }

        private void colorHistogramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new Form();
            f.ShowDialog();
        }
    }
}
