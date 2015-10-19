using System;
using System.Drawing;
using System.Windows.Forms;

namespace MiControl
{
    /// <summary>
    /// Class for calculating average screen color
    /// </summary>
    public class ScreenColor
    {
        /// <summary>
        /// Gets or sets the size of the grid (in pixels) to capture
        /// the average color of the screen.
        /// 
        /// Default set to '100'.
        /// </summary>
        public int Gridsize = 100;

        /// <summary>
        /// Gets or sets the <see cref="Screen"/> for which to capture the color.
        /// 
        /// Default set to 'Screen.PrimaryScreen'
        /// </summary>
        public Screen CaptureScreen = Screen.PrimaryScreen;

        /// <summary>
        /// Returns the amount of pixels being captured each run by the Ambuino
        /// </summary>
        public int PixelAmount
        {
            get { return (CaptureScreen.Bounds.Height / Gridsize) * (CaptureScreen.Bounds.Width / Gridsize); }
        }

        /// <summary>
        /// Gets or sets the method for calculating the average color value
        /// of the set CaptureScreen.
        /// 
        /// Default is set to 'CaptureMethod.Ambuino'.
        /// </summary>
        public CaptureMethod Method = CaptureMethod.Ambuino;

        /// <summary>
        /// Determines if the output Color should be smoothed by
        /// averaging it with the last found Color.
        /// 
        /// Default is set to 'true'.
        /// </summary>
        public bool Smoothing = true;

        // Private bitmap for storing the screenshot.
        Bitmap screen;
        // Private graphics object for screen capturing
        Graphics graphics;
        // Private variable for storing the last found color for
        // smoothing purposes
        Color lastcolor;


        /// <summary>
        /// Creates a new instance of the Ambuino class. Color of
        /// the screen can be retrieved using the 'AverageColor' method.
        /// The method for determining the average color can be set
        /// with the 'Method' property.
        /// </summary>
        public ScreenColor() 
        {
        }


        /// <summary>
        /// Returns the average Color of the set CaptureScreen. Uses one
        /// of two methods to determine average color.
        /// </summary>
        /// <returns></returns>
        public Color AverageColor()
        {
            // Temporary variable for the found color.
            Color color;

            // Create an array to store the retrieved colors based on 
            // selected Screen and Gridsize.
            Color[] pixels = new Color[PixelAmount];

            // Capture the screen and store the required pixels in the array
            using (screen = new Bitmap(CaptureScreen.Bounds.Width, CaptureScreen.Bounds.Height)) {
                // Create a graphics object to capture the screen
                graphics = Graphics.FromImage(screen);
                graphics.CopyFromScreen(CaptureScreen.Bounds.X, CaptureScreen.Bounds.Y,
                    0, 0, CaptureScreen.Bounds.Size, CopyPixelOperation.SourceCopy);

                // Fill the array with the required pixels
                int pixelcounter = 0;
                for (int x = Gridsize; x < CaptureScreen.Bounds.Width; x += Gridsize) {
                    for (int y = Gridsize; y < CaptureScreen.Bounds.Height; y += Gridsize) {
                        pixels[pixelcounter] = screen.GetPixel(x, y);
                        pixelcounter++;
                    }
                }

                // Tames the hungry, hungry RAM hippo
                screen.Dispose();
                graphics.Dispose();
            }

            // Determine average color based on selected CaptureMethod
            color = (Method == CaptureMethod.Ambuino) ? AverageAmbuino(pixels) : AverageRGB(pixels);

            // Apply smoothing if set
            if (Smoothing) {
                color = Color.FromArgb(
                    (color.R + lastcolor.R) / 2,
                    (color.G + lastcolor.G) / 2,
                    (color.B + lastcolor.B) / 2
                    );
                lastcolor = color;
            }

            return color;
        }

        /// <summary>
        /// Private method for calculation the average color of a given
        /// Color[] array.
        /// </summary>
        /// <param name="pixels">The Color[] array to average.</param>
        /// <returns>Returns a <see cref="Color"/> object.</returns>
        private Color AverageRGB(Color[] pixels)
        {
            int r = 0;
            int g = 0;
            int b = 0;

            foreach(Color color in pixels) {
                r += color.R;
                g += color.G;
                b += color.B;
            }

            r /= PixelAmount; g /= PixelAmount; b /= PixelAmount;

            return Color.FromArgb(r, g, b);
        }

        /// <summary>
        /// Private method to calculate the average color of a given
        /// Color[] array using the HSL color model to boost the
        /// Saturation and the Luminosity of the color.
        /// </summary>
        /// <param name="pixels"></param>
        /// <returns></returns>
        private Color AverageAmbuino(Color[] pixels)
        {
            // Calculate average RGB and convert to HSL
            HSLColor hsl = new HSLColor(AverageRGB(pixels));

            // Bump up the Saturation (force hard to 0.5)
            // 16 * (x-0.5)^5 + 0.5
            hsl.Saturation = 16.0 * Math.Pow(hsl.Saturation - 0.5, 5.0) + 0.5;

            // Bump up the Lighting (force to 0.5)
            // 4 * (x-0.5)^3 + 0.5
            hsl.Luminosity = 4.0 * Math.Pow(hsl.Luminosity - 0.5, 3.0) + 0.5;

            return (Color)hsl;
        }
    }

    public enum CaptureMethod {
        AverageRGB,
        Ambuino
    }
}
