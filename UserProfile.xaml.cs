using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Social_Blade_Dashboard
{
    /// <summary>
    /// Interaction logic for UserProfile.xaml
    /// </summary>
    public partial class UserProfile : UserControl
    {
        // ✅ Field you were missing
        private DateTime _currentDisplayDate;

        public UserProfile()
        {
            InitializeComponent();
            _currentDisplayDate = DateTime.Now;
            Loaded += UserProfile_Loaded;
        }

        private void UserProfile_Loaded(object sender, RoutedEventArgs e)
        {
            GenerateCalendar();
        }

        #region Calendar Methods

        private void GenerateCalendar()
        {
            MonthYearTextBlock.Text = _currentDisplayDate.ToString("MMMM yyyy", CultureInfo.InvariantCulture);
            CalendarDaysGrid.Children.Clear();

            DateTime first = new DateTime(_currentDisplayDate.Year, _currentDisplayDate.Month, 1);
            int daysInMonth = DateTime.DaysInMonth(_currentDisplayDate.Year, _currentDisplayDate.Month);
            int startDayOfWeek = (int)first.DayOfWeek;
            DateTime today = DateTime.Today;

            for (int i = 0; i < 42; i++)
            {
                var border = new Border();
                var text = new TextBlock
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };

                if (i < startDayOfWeek)
                {
                    // previous-month days
                    var prev = first.AddMonths(-1);
                    int dayNum = DateTime.DaysInMonth(prev.Year, prev.Month) - startDayOfWeek + i + 1;
                    text.Text = dayNum.ToString();
                    text.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#CCC"));
                }
                else if (i < startDayOfWeek + daysInMonth)
                {
                    // current-month days
                    int dayNum = i - startDayOfWeek + 1;
                    DateTime current = new DateTime(_currentDisplayDate.Year, _currentDisplayDate.Month, dayNum);
                    text.Text = dayNum.ToString();
                    text.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#333"));

                    if (current.Date == today)
                    {
                        border.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E6F7F1"));
                        border.CornerRadius = new CornerRadius(5);
                        text.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2D5A41"));
                        text.FontWeight = FontWeights.Bold;
                    }
                }
                else
                {
                    // next-month days
                    int dayNum = i - startDayOfWeek - daysInMonth + 1;
                    text.Text = dayNum.ToString();
                    text.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#CCC"));
                }

                border.Child = text;
                CalendarDaysGrid.Children.Add(border);
            }
        }

        private void PreviousMonthButton_Click(object sender, RoutedEventArgs e)
        {
            _currentDisplayDate = _currentDisplayDate.AddMonths(-1);
            GenerateCalendar();
        }

        private void NextMonthButton_Click(object sender, RoutedEventArgs e)
        {
            _currentDisplayDate = _currentDisplayDate.AddMonths(1);
            GenerateCalendar();
        }

        #endregion
    }
}
