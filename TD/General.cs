using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

using System.Drawing.Imaging;
/*
 * General classes and settings of the game
 */
namespace TD
{
    /*
     * logging class
     */
    public class Logger
    {
        private static StreamWriter sw = null;

        public static void init()
        {
            if (sw != null) return;
            sw = new StreamWriter("log.log", true);
            sw.WriteLine("----------------------------- " + DateTime.Now + " -----------------------------");
            sw.Flush();
        }

        public static void log(string s)
        {
            if (sw == null) init();
            sw.WriteLine(s);
            sw.Flush();
        }
    }

    public class Utils
    {
        public static bool confirm(string s)
        {
            DialogResult result = MessageBox.Show(s, "Confirm dialog box", MessageBoxButtons.YesNo);
            return result == DialogResult.Yes;
        }

        public static bool confirm()
        {
            return confirm("Jste si jistí?");
        }
        
        //change cursor to gem g, if g==null change it to default
        public static void setCursor(Form form, Gem g)
        {
            if (g == null)
            {
                form.Cursor = System.Windows.Forms.Cursors.Default;
            }
            else
            {
                setCursor(form, Images.getImageGem(g.quality, g.getMainAspectNum()));
            }
        }

        private static void setCursor(Form form, Image img)
        {
            if (img == null)
            {
                form.Cursor = System.Windows.Forms.Cursors.Default;
            }
            else
            {
                Bitmap bmp = new Bitmap(img);
                form.Cursor = new Cursor(bmp.GetHicon());
            }
        }

        //changes opacity of Image
        public static Image ChangeOpacity(Image img, float opacityvalue)
        {
            Bitmap bmp = new Bitmap(img.Width, img.Height); // Determining Width and Height of Source Image
            Graphics graphics = Graphics.FromImage(bmp);
            ColorMatrix colormatrix = new ColorMatrix();
            colormatrix.Matrix33 = opacityvalue;
            ImageAttributes imgAttribute = new ImageAttributes();
            imgAttribute.SetColorMatrix(colormatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            graphics.DrawImage(img, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, imgAttribute);
            graphics.Dispose();   // Releasing all resource used by graphics 
            return (Image)bmp;
        }
    }

    public class Direction
    {
        public static int[] dx = { 0, 1, 0, -1 };
        public static int[] dy = { -1, 0, 1, 0 };
    }

    public static class Settings
    {
        public static int numberOfLevels = 8;
        public static int levelSelectNumOfCols = 2;
        public static int levelSelectNumOfRows = 4;
        public static int boardSize = 16; //number of cells both horizontal and vertical
        public static int cellSize = 33; //number of pixels in one cell
        public static int inventoryNumOfCols = 8;
        public static int inventoryNumOfRows = 4;
    }
    /*
     * uses only one istance of Random
     * so calling it in the same time doesnt return same value
     */
    public static class MyRandom
    {
        private static Random rnd = null;
        
        public static double NextDouble()
        {
            if (rnd == null) rnd = new Random();
            return rnd.NextDouble();
        }
    }

}