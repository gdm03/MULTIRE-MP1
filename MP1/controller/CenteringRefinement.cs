using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MP1.controller
{
    class CenteringRefinement
    {
        ComputeHistogram ch = new ComputeHistogram();

        //Dictionary<Color, float> rgbCenterHist = new Dictionary<Color, float>();
        //Dictionary<Color, float> rgbNonCenterHist = new Dictionary<Color, float>();
        Dictionary<Color, float> rgbHistogram = new Dictionary<Color, float>();
        Dictionary<int, float> centerHist = new Dictionary<int, float>();
        Dictionary<int, float> nonCenterHist = new Dictionary<int, float>();

        int startIndex = 0;
        int endIndex = 0;

        Bitmap img;
        ImageCenter ic;

        public CenteringRefinement(Bitmap img, float centering)
        {
            this.img = img;  
            ic = new ImageCenter(img, centering);
        }
        
        public Dictionary<Color,float> getRgbHistogram(bool center)
        {
            startIndex = ic.getWidthDisplacement();
            endIndex = ic.getHeightDisplacement();

            rgbHistogram = ch.getRGBValues(img, startIndex, endIndex, center);
            return rgbHistogram;
        }

        public int getDimensions(bool center)
        {
            int centerDimensions = ic.c_width * ic.c_height;
            if (center)
                return centerDimensions;
            return (ic.old_width * ic.old_height) - centerDimensions;

        }
    }

    class Factor
    {
        public int factor_width { get; set; }
        public int factor_height { get; set; }

        public Factor(int x, int y)
        {
            this.factor_width = x;
            this.factor_height = y;
        }
    }

    class ImageCenter
    {
        // Center width and height
        public int c_width { get; set; }
        public int c_height { get; set; }

        // Original image properties
        public int old_width { get; set; }
        public int old_height { get; set; }

        public float centering { get; set; }

        public ImageCenter(Bitmap img, float centering)
        {
            old_width = img.Width;
            old_height = img.Height;
            this.centering = centering;
            
            c_width = (int)Math.Ceiling(old_width * centering);
            c_height = (int)Math.Ceiling(old_height * centering);
        }

        public int getWidthDisplacement()
        {
            return (old_width - c_width) / 2;
        }

        public int getHeightDisplacement()
        {
            return (old_height - c_height) / 2;
        }
    }
}
