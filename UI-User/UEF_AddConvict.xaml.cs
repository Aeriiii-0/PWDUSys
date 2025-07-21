using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Input;

namespace Social_Blade_Dashboard
{
    public class ConvictData
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string ExtensionName { get; set; }
        public string Gender { get; set; }
        public string BirthMonth { get; set; }
        public string BirthDay { get; set; }
        public string BirthYear { get; set; }
        public string ContactNumber { get; set; }
        public string Barangay { get; set; }
        public string Address { get; set; }
        public string CriminalCase { get; set; }
        public string OffenseCommitted { get; set; }
        public string Description { get; set; }
        public string CourtNumber { get; set; }
        public string CommittingCourt { get; set; }
        public string AdmittedMonth { get; set; }
        public string AdmittedDay { get; set; }
        public string AdmittedYear { get; set; }
        public string StatusOfCase { get; set; }
        public string GraduatedMonth { get; set; }
        public string GraduatedDay { get; set; }
        public string GraduatedYear { get; set; }
    }
    public partial class UEF_AddConvict : UserControl
    {
        public UEF_AddConvict()
        {
            InitializeComponent();
        }


        private void MaleButton_Click(object sender, RoutedEventArgs e)
        {
            MaleButton.Tag = "Selected";
            FemaleButton.Tag = null;
        }

        private void FemaleButton_Click(object sender, RoutedEventArgs e)
        {
            FemaleButton.Tag = "Selected";
            MaleButton.Tag = null;
        }

        private void Offense1Button_Click(object sender, RoutedEventArgs e)
        {
            Offense1Button.Tag = "Selected";
            Offense2Button.Tag = null;
        }

        private void Offense2Button_Click(object sender, RoutedEventArgs e)
        {
            Offense2Button.Tag = "Selected";
            Offense1Button.Tag = null;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {

            var convictData = new ConvictData
            {
                LastName = LastNameTextBox.Text,
                FirstName = FirstNameTextBox.Text,
                MiddleName = MiddleNameTextBox.Text,
                ExtensionName = ExtensionComboBox.SelectedItem != null ?
                    ((ComboBoxItem)ExtensionComboBox.SelectedItem).Content.ToString() : "",

                Gender = GetSelectedGender(),

                BirthMonth = GetComboBoxValue(BirthMonthComboBox),
                BirthDay = GetComboBoxValue(BirthDayComboBox),
                BirthYear = BirthYearTextBox.Text,

                ContactNumber = ContactNumberTextBox.Text,
                Barangay = BarangayComboBox.SelectedItem != null ?
                    ((ComboBoxItem)BarangayComboBox.SelectedItem).Content.ToString() : "",
                Address = AddressTextBox.Text,
                CriminalCase = CriminalCaseTextBox.Text,

                OffenseCommitted = GetSelectedOffense(),
                Description = DescriptionTextBox.Text,
                CourtNumber = CourtNumberTextBox.Text,
                CommittingCourt = CommittingCourtComboBox.Text,

                AdmittedMonth = GetComboBoxValue(AdmittedMonthComboBox),
                AdmittedDay = GetComboBoxValue(AdmittedDayComboBox),
                AdmittedYear = AdmittedYearTextBox.Text,

                StatusOfCase = StatusOfCaseComboBox.SelectedItem != null ?
                    ((ComboBoxItem)StatusOfCaseComboBox.SelectedItem).Content.ToString() : "",


                GraduatedMonth = GetComboBoxValue(MonthGraduatedComboBox),
                GraduatedDay = GetComboBoxValue(DayGraduatedComboBox),
                GraduatedYear = YearGraduatedTextBox.Text
            };

            string message = $"Collected Data:\n" +
                           $"Name: {convictData.LastName}, {convictData.FirstName} {convictData.MiddleName} {convictData.ExtensionName}\n" +
                           $"Gender: {convictData.Gender}\n" +
                           $"Birthday: {convictData.BirthMonth}/{convictData.BirthDay}/{convictData.BirthYear}\n" +
                           $"Contact: {convictData.ContactNumber}\n" +
                           $"Barangay: {convictData.Barangay}\n" +
                           $"Address: {convictData.Address}\n" +
                           $"Criminal Case: {convictData.CriminalCase}\n" +
                           $"Offense: {convictData.OffenseCommitted}\n" +
                           $"Description: {convictData.Description}\n" +
                           $"Court Number: {convictData.CourtNumber}\n" +
                           $"Committing Court: {convictData.CommittingCourt}\n" +
                           $"Date Admitted: {convictData.AdmittedMonth}/{convictData.AdmittedDay}/{convictData.AdmittedYear}\n" +
                           $"Status: {convictData.StatusOfCase}\n" +
                           $"Date Graduated: {convictData.GraduatedMonth}/{convictData.GraduatedDay}/{convictData.GraduatedYear}";

            MessageBox.Show(message, "Convict Data Collected", MessageBoxButton.OK, MessageBoxImage.Information);


            ClearAllFields();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            ClearAllFields();
        }

        private string GetSelectedGender()
        {
            if (MaleButton.Tag != null && MaleButton.Tag.ToString() == "Selected")
                return "Male";
            else if (FemaleButton.Tag != null && FemaleButton.Tag.ToString() == "Selected")
                return "Female";
            else
                return "";
        }

        private string GetSelectedOffense()
        {
            if (Offense1Button.Tag != null && Offense1Button.Tag.ToString() == "Selected")
                return "Offense 1";
            else if (Offense2Button.Tag != null && Offense2Button.Tag.ToString() == "Selected")
                return "Offense 2";
            else
                return "";
        }

        private string GetComboBoxValue(ComboBox comboBox)
        {
            if (comboBox.SelectedItem != null)
            {
                var selectedItem = (ComboBoxItem)comboBox.SelectedItem;

                if (selectedItem.Tag != null)
                    return selectedItem.Tag.ToString();
                else
                    return selectedItem.Content.ToString();
            }
            return "";
        }

        private byte[] _photoData;
        private string _photoFileName;

        private void PhotoUpload_Click(object sender, MouseButtonEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Title = "Select Photo",
                Filter = "Image files (*.png;*.jpg;*.jpeg;*.gif;*.bmp)|*.png;*.jpg;*.jpeg;*.gif;*.bmp|All files (*.*)|*.*",
                FilterIndex = 1
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    
                    _photoData = File.ReadAllBytes(openFileDialog.FileName);
                    _photoFileName = Path.GetFileName(openFileDialog.FileName);

                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.StreamSource = new MemoryStream(_photoData);
                    bitmap.EndInit();

                    UploadedPhotoImage.Source = bitmap;
                    UploadedPhotoImage.Visibility = Visibility.Visible;
                    DefaultPhotoIcon.Visibility = Visibility.Collapsed;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading image: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        
        public byte[] PhotoData => _photoData;
        public string PhotoFileName => _photoFileName;
        public bool HasPhoto => _photoData != null;

        private void ClearAllFields()
        {
            //txbs
            LastNameTextBox.Clear();
            FirstNameTextBox.Clear();
            MiddleNameTextBox.Clear();
            BirthYearTextBox.Clear();
            ContactNumberTextBox.Clear();
            AddressTextBox.Clear();
            CriminalCaseTextBox.Clear();
            DescriptionTextBox.Clear();
            CourtNumberTextBox.Clear();
            AdmittedYearTextBox.Clear();
            YearGraduatedTextBox.Clear();

            ExtensionComboBox.SelectedIndex = -1;
            BirthMonthComboBox.SelectedIndex = 0;
            BirthDayComboBox.SelectedIndex = 0;
            BarangayComboBox.SelectedIndex = -1;
            AdmittedMonthComboBox.SelectedIndex = 0;
            AdmittedDayComboBox.SelectedIndex = 0;
            StatusOfCaseComboBox.SelectedIndex = -1;
            MonthGraduatedComboBox.SelectedIndex = 0;
            DayGraduatedComboBox.SelectedIndex = 0;

            CommittingCourtComboBox.Text = "";
            CommittingCourtComboBox.SelectedIndex = -1;

            MaleButton.Tag = null;
            FemaleButton.Tag = null;

            Offense1Button.Tag = null;
            Offense2Button.Tag = null;
        }
    }



}