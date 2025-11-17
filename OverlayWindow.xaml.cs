using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EdgeLight.App
{
    /// <summary>
    /// Interaction logic for OverlayWindow.xaml
    /// </summary>
    public partial class OverlayWindow : Window
    {
        public OverlayWindow()
        {
            InitializeComponent();
            Loaded += OverlayWindow_Loaded;
        }

        private void OverlayWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var screenWidth = SystemParameters.PrimaryScreenWidth;
            var screenHeight = SystemParameters.PrimaryScreenHeight;

            Width = screenWidth;
            Height = screenHeight;
            Left = 0;
            Top = 0;
        }

        /// <summary>
        /// Update the light frame based on base color and intensity (0..1).
        /// </summary>
        public void UpdateLight(Color baseColor, double intensity)
        {
            intensity = Math.Clamp(intensity, 0.0, 1.0);

            byte a = (byte)(intensity * 255);
            var finalColor = Color.FromArgb(a, baseColor.R, baseColor.G, baseColor.B);

            LightFrame.BorderBrush = new SolidColorBrush(finalColor);
        }

        /// <summary>
        /// Update blur softness based on a normalized value (0..1).
        /// 0 = no blur (crisp), 1 = maximum blur.
        /// </summary>
        public void UpdateBlur(double blurAmount)
        {
            blurAmount = Math.Clamp(blurAmount, 0.0, 1.0);

            const double maxRadius = 40.0; // tweak max blur if you want
            double radius = blurAmount * maxRadius;

            if (LightBlurEffect != null)
            {
                LightBlurEffect.Radius = radius;
            }
        }
    }
}
