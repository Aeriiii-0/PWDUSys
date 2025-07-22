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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Social_Blade_Dashboard
{
    /// <summary>
    /// Interaction logic for SplashLoading.xaml
    /// </summary>
    public partial class SplashLoading : Window
    {
        private int currentProgress = 0;
        private string[] loadingMessages = {
            "Initializing system...",
            "Loading Database resources...",
            "Loading PDEA resources...",
            "Loading PNP resources...",
            "Loading DILG resources...",
            "Loading City Binan resources...",
            "Finalizing setup...",
            "Ready!" };

        public SplashLoading()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await StartLoadingSequence();
        }

        private async Task StartLoadingSequence()
        {
            await AnimateLogo(6, "Dot6", "Scale6", "Shadow6", "Transform6", 20);
            await AnimateLogo(1, "Dot1", "Scale1", "Shadow1", "Transform1", 40);  // Bagong Pilipinas
            await AnimateLogo(4, "Dot4", "Scale4", "Shadow4", "Transform4", 60);  // PDEA
            await AnimateLogo(5, "Dot5", "Scale5", "Shadow5", "Transform5", 80);  // PNP
            await AnimateLogo(3, "Dot3", "Scale3", "Shadow3", "Transform3", 90);  // DILG
            await AnimateLogo(2, "Dot2", "Scale2", "Shadow2", "Transform2", 100);  // City Binan

            await UpdateProgress(100, "Ready!");

            // Add delay to ensure animations complete and provide better UX
            await Task.Delay(1000);

            // Navigate to main application
            await NavigateToMainApplicationAsync();
        }

        private async Task AnimateLogo(int logoNumber, string logoName, string scaleName, string shadowName, string transformName, int progressPercent)
        {
            if (logoNumber - 1 < loadingMessages.Length)
            {
                LoadingText.Text = loadingMessages[logoNumber];
            }

            var logo = FindName(logoName) as FrameworkElement;
            var scaleTransform = FindName(scaleName) as ScaleTransform;
            var shadow = FindName(shadowName) as FrameworkElement;
            var translateTransform = FindName(transformName) as TranslateTransform;

            if (logo != null && scaleTransform != null)
            {
                var duration = TimeSpan.FromMilliseconds(500);
                var waveEasing = new SineEase { EasingMode = EasingMode.EaseOut };
                var fadeEasing = new QuadraticEase { EasingMode = EasingMode.EaseOut };

                var opacityAnimation = new DoubleAnimation
                {
                    From = 0,
                    To = 1,
                    Duration = duration,
                    EasingFunction = fadeEasing
                };

                if (translateTransform != null)
                {
                    var translateYAnimation = new DoubleAnimation
                    {
                        From = 40,
                        To = 0,
                        Duration = duration,
                        EasingFunction = waveEasing
                    };
                    translateTransform.BeginAnimation(TranslateTransform.YProperty, translateYAnimation);
                }

                var scaleXAnimation = new DoubleAnimation
                {
                    From = 0.7,
                    To = 1,
                    Duration = duration,
                    EasingFunction = waveEasing
                };

                var scaleYAnimation = new DoubleAnimation
                {
                    From = 0.8,
                    To = 1,
                    Duration = duration,
                    EasingFunction = waveEasing
                };

                logo.BeginAnimation(UIElement.OpacityProperty, opacityAnimation);
                scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleXAnimation);
                scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleYAnimation);

                if (shadow != null)
                {
                    var shadowOpacityAnimation = new DoubleAnimation
                    {
                        From = 0,
                        To = 0.4,
                        Duration = duration,
                        EasingFunction = fadeEasing
                    };
                    shadow.BeginAnimation(UIElement.OpacityProperty, shadowOpacityAnimation);

                    var shadowTransform = shadow.RenderTransform as TranslateTransform;
                    if (shadowTransform != null)
                    {
                        var shadowTranslateAnimation = new DoubleAnimation
                        {
                            From = 32,
                            To = 0,
                            Duration = TimeSpan.FromMilliseconds(500),
                            EasingFunction = waveEasing
                        };
                        shadowTransform.BeginAnimation(TranslateTransform.YProperty, shadowTranslateAnimation);
                    }
                }

                await UpdateProgress(progressPercent, loadingMessages[logoNumber]);
                await Task.Delay(300);
            }
        }

        private async Task UpdateProgress(int percentage, string message)
        {
            currentProgress = percentage;

            var progressAnimation = new DoubleAnimation
            {
                From = ProgressBarFill.Width,
                To = 400 * (percentage / 100.0),
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new SineEase { EasingMode = EasingMode.EaseOut }
            };

            ProgressBarFill.BeginAnimation(FrameworkElement.WidthProperty, progressAnimation);

            var textFade = new DoubleAnimation
            {
                From = 0.7,
                To = 1,
                Duration = TimeSpan.FromMilliseconds(100),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
            };

            ProgressText.Text = $"{percentage}%";
            LoadingText.Text = message;

            ProgressText.BeginAnimation(UIElement.OpacityProperty, textFade);
            LoadingText.BeginAnimation(UIElement.OpacityProperty, textFade);

            await Task.Delay(75);
        }

        private async Task NavigateToMainApplicationAsync()
        {
            // Ensure we're on the UI thread for window operations
            await Dispatcher.BeginInvoke(new Action(() =>
            {
                try
                {
                    // Create and show the login window
                    userLogin userLoginWindow = new userLogin();
                    userLoginWindow.Show();

                    // Close the splash screen
                    this.Close();

                    // Optional: Show completion message
                    MessageBox.Show("Loading Complete! Ready to proceed to main application.",
                                  "Loading Complete",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    // Handle any errors during navigation
                    MessageBox.Show($"Error loading main application: {ex.Message}",
                                  "Error",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Error);
                }
            }));
        }
    }
}