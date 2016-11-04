using MP1.controller;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MP1
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MP1Form());

            /*
            Console.WriteLine("*WARNING, Set directory to correct folder in your system..");
            //String directory = "C:/Users/retxh/Desktop/MP1/test";
            String directory = "C:/images";
            ImageSearch imagesearch = new ImageSearch(directory);

            //String queryPath = "C:/Users/retxh/Desktop/MP1/test/0.jpg";
            String queryPath = "C:/images/0.jpg";
            //test algos
            ArrayList results = imagesearch.search(queryPath, ImageSearch.COLOR_DIFF_HISTOGRAM);
            for (int i = 0; i < Convert.ToInt32(results.Count * 0.1); i++)
            {
                Console.WriteLine(results[i]);
            }
            */

        }
    }
}
