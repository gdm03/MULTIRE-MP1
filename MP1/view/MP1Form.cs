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

        private String currImgPath;
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            // Change directory here
            String dir = @"C:\Users\retxh\Desktop\MP1\images";
            String[] imagePaths = Directory.GetFiles(dir, "*.jpg", SearchOption.AllDirectories);
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                String imgPath = ofd.FileName;
                currImgPath = imgPath;
                selectedImageBox.Image = new Bitmap(imgPath);
                Bitmap img = new Bitmap(imgPath);
            }
        }

        private void displayImages(List<String> paths)
        {
            List<int> bottomlist = new List<int>();
            int c = 0;

            foreach (String s in paths)
            {
                //Debug.WriteLine(s);
                PictureBox pc = new PictureBox();
                Image imgTest = new Bitmap(s);
                pc.Image = imgTest;
                pc.Size = imgTest.Size;
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
        }

        private void colorHistogramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new Form();
            f.ShowDialog();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            NormalCH nch = new NormalCH();
            Bitmap img = new Bitmap(selectedImageBox.Image);
            displayImages(nch.returnRelevantImages(img));
        }

        private void chcenterbutton_Click(object sender, EventArgs e)
        {
            CenteringCH cch = new CenteringCH();
            Bitmap img = new Bitmap(selectedImageBox.Image);
            displayImages(cch.returnRelevantImages(img));
        }

        private String directory = @"C:\Users\retxh\Desktop\MP1\test";
        private void button1_Click_1(object sender, EventArgs e)
        {
            //cdh here
            ImageSearch imagesearch = new ImageSearch(directory);
            displayImages(imagesearch.search(currImgPath, ImageSearch.COLOR_DIFF_HISTOGRAM));
            
        }

        private void perceptualbutton_Click(object sender, EventArgs e)
        {
            //percep
            ImageSearch imagesearch = new ImageSearch(directory);
            displayImages(imagesearch.search(currImgPath, ImageSearch.PERCEPTUAL_SIM));
        }

        private void colorcoherencebutton_Click(object sender, EventArgs e)
        {
            //colorcoherence
            ImageSearch imagesearch = new ImageSearch(directory);
            displayImages(imagesearch.search(currImgPath, ImageSearch.COLOR_COHERENCE));
        }
    }
}
