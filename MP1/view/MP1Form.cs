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
        
        List<String> similarImagesPaths = new List<string>();

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

            Dictionary<Color, float> rgbHistogram = new Dictionary<Color, float>();
            Dictionary<LUVClass, float> luvHistogram = new Dictionary<LUVClass, float>();
            Dictionary<int, float> quantizedHistogram = new Dictionary<int, float>();
            Dictionary<int, float> normalizedHistogram = new Dictionary<int, float>();

            float[] hist1 = new float[159];
            float[] hist2 = new float[159];
            
            // Change directory here
            String dir = @"D:\DLSU-M\Term 1 AY 2016-2017\CSC741M\MP1_files\MP1\images\";

            String[] imagePaths = Directory.GetFiles(dir, "*.jpg", SearchOption.AllDirectories);
            List<String> paths = new List<string>(); // list of other images in directory

            int imgDimensions = 0;
            int currImgDimensions = 0;

            float threshold = 0;
            float sim = 0;
            //float simThreshold = 0.24F;
            float simThreshold = 0.1F;

            ComputeHistogram ch = new ComputeHistogram(); // for histogram operations

            // .jpg only? same size only? padding
            // Filter file type
            //ofd.Filter 

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                String imgPath = ofd.FileName;

                selectedImageBox.Image = new Bitmap(imgPath);
                Bitmap img = new Bitmap(imgPath);
                
                // Selected image RGB histogram
                rgbHistogram = ch.getRGBValues(img);
                imgDimensions = ch.getImgDimensions(img);
                luvHistogram = ch.convertToLuv(rgbHistogram); // Convert RGB histogram to LUV histogram
                quantizedHistogram = ch.quantizeColors(luvHistogram, imgDimensions); // Quantize LUV histogram to 159 colors

                float counter = 0;
                /*
                foreach (int x in quantizedHistogram.Keys)
                {
                    //Debug.WriteLine("Color: " + x + " Number: " + quantizedHistogram[x]);
                    counter += quantizedHistogram[x];
                }
                */
                //Debug.WriteLine(counter);
                //counter = 0;
                foreach (int x in quantizedHistogram.Keys)
                {
                    normalizedHistogram.Add(x, quantizedHistogram[x] / imgDimensions);
                    //counter += quantizedHistogram[x] / imgDimensions;
                }

                //Debug.WriteLine(counter);

                for (int i = 0; i < 159; i++)
                {
                    normalizedHistogram.TryGetValue(i, out hist1[i]);
                }

                // Test 1 image
                
                /*
                String s = @"D:\DLSU-M\Term 1 AY 2016-2017\CSC741M\MP1_files\MP1\images\114.jpg";

                Dictionary<Color, float> currRgbHistogram = new Dictionary<Color, float>();
                Dictionary<LUVClass, float> currLuvHistogram = new Dictionary<LUVClass, float>();
                Dictionary<int, float> currQuantizedHistogram = new Dictionary<int, float>();
                Dictionary<int, float> currNormalizedHistogram = new Dictionary<int, float>();
                Bitmap currImg = new Bitmap(s);
                currImgDimensions = ch.getImgDimensions(currImg);
                currRgbHistogram = ch.getRGBValues(currImg);
                currLuvHistogram = ch.convertToLuv(currRgbHistogram);
                currQuantizedHistogram = ch.quantizeColors(currLuvHistogram, currImgDimensions);

                counter = 0;

                foreach (int x in currQuantizedHistogram.Keys)
                {
                    currNormalizedHistogram.Add(x, currQuantizedHistogram[x] / currImgDimensions);
                    //counter += currQuantizedHistogram[x] / currImgDimensions;
                    //Debug.WriteLine("Color: " + x + " Number: " + currQuantizedHistogram[x]);
                }

                for (int i = 0; i < 159; i++)
                {
                    currNormalizedHistogram.TryGetValue(i, out hist2[i]);
                }
                Debug.WriteLine("Similarity: " + ch.computeSimilarity(hist1, hist2, threshold));

                // End test
                */
                
                // Adds all images in directory to List excluding selected image
                foreach (String s in imagePaths)
                {
                    if (!s.Equals(imgPath))
                    {
                        paths.Add(s);
                    }
                }
                
                // Loop for currentImg
                
                foreach (String s in paths)
                {
                    Dictionary<Color, float> currRgbHistogram = new Dictionary<Color, float>();
                    Dictionary<LUVClass, float> currLuvHistogram = new Dictionary<LUVClass, float>();
                    Dictionary<int, float> currQuantizedHistogram = new Dictionary<int, float>();
                    Dictionary<int, float> currNormalizedHistogram = new Dictionary<int, float>();
                    Bitmap currImg = new Bitmap(s);
                    currImgDimensions = ch.getImgDimensions(currImg);
                    currRgbHistogram = ch.getRGBValues(currImg);
                    currLuvHistogram = ch.convertToLuv(currRgbHistogram);
                    currQuantizedHistogram = ch.quantizeColors(currLuvHistogram, currImgDimensions);

                    counter = 0;
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
                        //Debug.WriteLine("Path: " + s + "] || Similarity: " + sim);
                        similarImagesPaths.Add(s);
                    }
                }

                List<int> bottomlist = new List<int>();
                //counter = 0;
                int c = 0;
                foreach (String s in similarImagesPaths)
                {
                    Debug.WriteLine(s);
                    //int topmargin = 40;
                    PictureBox pc = new PictureBox();
                    Image imgTest = new Bitmap(s);
                    pc.Image = imgTest;
                    pc.Size = imgTest.Size;
                    //pc.Top = topmargin;
                    //pc.Left = 200;
                    if (c == 0)
                    {
                        bottomlist.Add(pc.Bottom + 8);
                        pc.Top = 8;
                    }
                        
                    else
                    {
                        bottomlist.Add(pc.Bottom + bottomlist[c - 1] + 8);
                        pc.Top = bottomlist[c - 1] + 8;
                    }
                    c++;
                    panel1.Controls.Add(pc);
                }
                // Dispose old image??
            }


        }

        private void colorHistogramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new Form();
            f.ShowDialog();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            /*
            PictureBox pc = new PictureBox();
            pc.Image = new Bitmap(@"D:\DLSU-M\Term 1 AY 2016-2017\CSC741M\MP1_files\MP1\images\114.jpg");
            pc.Top = 10;
            panel1.Controls.Add(pc);
            */
            
        }
    }
}
