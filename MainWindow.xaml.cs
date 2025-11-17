using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EdgeLight.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private OverlayWindow _overlay;

        public MainWindow()
        {
            InitializeComponent();

            _overlay = new OverlayWindow();
            _overlay.Show();

            ApplySettings();
        }

        private void ApplySettings()
        {
            if (_overlay == null)
                return;

            // Light on/off
            if (LightEnabledCheckBox != null && LightEnabledCheckBox.IsChecked == false)
            {
                _overlay.Hide();
                return;
            }

            if (!_overlay.IsVisible)
            {
                _overlay.Show();
            }

            // Color slider: 0..100 -> 0..1
            double colorT = 0.2;
            if (ColorSlider != null)
                colorT = ColorSlider.Value / 100.0;

            // Intensity slider: 0..100 -> 0..1
            double intensity = 0.7;
            if (IntensitySlider != null)
                intensity = IntensitySlider.Value / 100.0;

            // Crispness slider: 0..100
            // We interpret: left = blurred, right = crisp.
            // So blurAmount = 1 - crispnessNormalized.
            double blurAmount = 0.5;
            if (CrispnessSlider != null)
            {
                double crispness = CrispnessSlider.Value / 100.0; // 0..1
                blurAmount = 1.0 - crispness; // 0 = crisp, 1 = max blur
            }

            // Interpolate color between cool and warm white
            Color coolWhite = Color.FromRgb(255, 255, 255);
            Color warmWhite = Color.FromRgb(255, 245, 230);

            Color baseColor = LerpColor(coolWhite, warmWhite, colorT);

            _overlay.UpdateLight(baseColor, intensity);
            _overlay.UpdateBlur(blurAmount);
        }

        private static Color LerpColor(Color a, Color b, double t)
        {
            if (t < 0) t = 0;
            if (t > 1) t = 1;

            byte r = (byte)(a.R + (b.R - a.R) * t);
            byte g = (byte)(a.G + (b.G - a.G) * t);
            byte bC = (byte)(a.B + (b.B - a.B) * t);

            return Color.FromRgb(r, g, bC);
        }

        private void ColorSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ApplySettings();
        }

        private void IntensitySlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ApplySettings();
        }

        private void CrispnessSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ApplySettings();
        }

        private void LightEnabledChanged(object sender, RoutedEventArgs e)
        {
            ApplySettings();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            _overlay?.Close();
            Application.Current.Shutdown();
        }
    }
}