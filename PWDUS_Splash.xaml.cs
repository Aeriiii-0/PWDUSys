using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace Social_Blade_Dashboard
{
    public partial class PWDUS_Splash : UserControl
    {
        private DispatcherTimer splashTimer;
        private DispatcherTimer rotationTimer;
        private DispatcherTimer textTimer;
        private int animationStep = 0;

        public event EventHandler SplashCompleted;

        public PWDUS_Splash()
        {
            InitializeComponent();

            // Subscribe to the Unloaded event for cleanup
            this.Unloaded += PWDUS_Splash_Unloaded;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            StartSplashSequence();
        }

        private void PWDUS_Splash_Unloaded(object sender, RoutedEventArgs e)
        {
            CleanupTimers();
        }

        private void StartSplashSequence()
        {
            try
            {
                // Start with fade in animation
                var fadeInStoryboard = (Storyboard)FindResource("FadeInStoryboard");
                if (fadeInStoryboard != null && fadeInStoryboard.Children.Count > 0)
                {
                    Storyboard.SetTarget(fadeInStoryboard.Children[0], this);
                    fadeInStoryboard.Begin();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error with fade in animation: {ex.Message}");
            }

            // Start rotating logos after a short delay
            rotationTimer = new DispatcherTimer();
            rotationTimer.Interval = TimeSpan.FromMilliseconds(500);
            rotationTimer.Tick += (s, args) =>
            {
                rotationTimer.Stop();
                StartLogoRotations();
                StartProgressAnimation();
            };
            rotationTimer.Start();

            // Set up main splash timer (5 seconds total)
            splashTimer = new DispatcherTimer();
            splashTimer.Interval = TimeSpan.FromSeconds(5);
            splashTimer.Tick += SplashTimer_Tick;
            splashTimer.Start();
        }

        private void StartLogoRotations()
        {
            try
            {
                // Create individual rotation animations for each logo with slight delays
                var logos = new[] { Logo1, Logo2, Logo3, Logo4, Logo5, Logo6, Logo7, Logo8 };

                for (int i = 0; i < logos.Length; i++)
                {
                    if (logos[i] != null)
                    {
                        try
                        {
                            var rotationStoryboard = new Storyboard();
                            var rotationAnimation = new DoubleAnimation
                            {
                                From = 0,
                                To = 360,
                                Duration = new Duration(TimeSpan.FromSeconds(2)),
                                RepeatBehavior = RepeatBehavior.Forever
                            };

                            Storyboard.SetTarget(rotationAnimation, logos[i]);
                            Storyboard.SetTargetProperty(rotationAnimation, new PropertyPath("RenderTransform.Angle"));
                            rotationStoryboard.Children.Add(rotationAnimation);

                            // Add slight delay between each logo animation
                            rotationStoryboard.BeginTime = TimeSpan.FromMilliseconds(i * 200);
                            rotationStoryboard.Begin();
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"Error animating logo {i}: {ex.Message}");
                        }
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"Logo{i + 1} is null - check XAML x:Name attributes");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in StartLogoRotations: {ex.Message}");
            }
        }

        private void StartProgressAnimation()
        {
            try
            {
                // Animate loading text changes
                textTimer = new DispatcherTimer();
                textTimer.Interval = TimeSpan.FromSeconds(1);
                textTimer.Tick += TextTimer_Tick;
                textTimer.Start();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error starting progress animation: {ex.Message}");
            }
        }

        private void TextTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (LoadingText != null)
                {
                    switch (animationStep)
                    {
                        case 0:
                            LoadingText.Text = "Initializing System...";
                            break;
                        case 1:
                            LoadingText.Text = "Loading Database...";
                            break;
                        case 2:
                            LoadingText.Text = "Connecting Services...";
                            break;
                        case 3:
                            LoadingText.Text = "Finalizing Setup...";
                            break;
                        case 4:
                            LoadingText.Text = "System Ready!";
                            textTimer?.Stop();
                            break;
                    }
                    animationStep++;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in TextTimer_Tick: {ex.Message}");
                textTimer?.Stop();
            }
        }

        private void SplashTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                splashTimer?.Stop();

                // Fade out animation before completing
                var fadeOutStoryboard = new Storyboard();
                var fadeOutAnimation = new DoubleAnimation
                {
                    From = 1,
                    To = 0,
                    Duration = new Duration(TimeSpan.FromSeconds(0.5))
                };

                Storyboard.SetTarget(fadeOutAnimation, this);
                Storyboard.SetTargetProperty(fadeOutAnimation, new PropertyPath("Opacity"));
                fadeOutStoryboard.Children.Add(fadeOutAnimation);

                fadeOutStoryboard.Completed += (s, args) =>
                {
                    // Raise the completion event
                    SplashCompleted?.Invoke(this, EventArgs.Empty);
                };

                fadeOutStoryboard.Begin();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in SplashTimer_Tick: {ex.Message}");
                // Fallback - just raise completion event
                SplashCompleted?.Invoke(this, EventArgs.Empty);
            }
        }

        // Method to manually close splash (if needed)
        public void CloseSplash()
        {
            try
            {
                CleanupTimers();
                SplashCompleted?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in CloseSplash: {ex.Message}");
            }
        }

        private void CleanupTimers()
        {
            try
            {
                if (splashTimer != null)
                {
                    if (splashTimer.IsEnabled)
                        splashTimer.Stop();
                    splashTimer = null;
                }

                if (rotationTimer != null)
                {
                    if (rotationTimer.IsEnabled)
                        rotationTimer.Stop();
                    rotationTimer = null;
                }

                if (textTimer != null)
                {
                    if (textTimer.IsEnabled)
                        textTimer.Stop();
                    textTimer = null;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error cleaning up timers: {ex.Message}");
            }
        }
    }
}