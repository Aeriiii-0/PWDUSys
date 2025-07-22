using Microsoft.Win32;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Business_Layer;
using Common_Class;

namespace Social_Blade_Dashboard
{
 
    public partial class UEF_AddConvict : UserControl
    {
        Entry_Business_Logic businessLogic;
        public UEF_AddConvict()
        {
            InitializeComponent();
            businessLogic = new Entry_Business_Logic();
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
        private int CalculateAge(string birthYear)
{
    if (int.TryParse(birthYear, out int year))
    {
        return DateTime.Now.Year - year;
    }
    return 0;
}
        //pwede lipat later sa bl
        private bool ValidateConvictForm()
        {
            if (string.IsNullOrWhiteSpace(LastNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(FirstNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(ContactNumberTextBox.Text) ||
                string.IsNullOrWhiteSpace(AddressTextBox.Text) ||
                string.IsNullOrWhiteSpace(CriminalCaseTextBox.Text))
            {
                MessageBox.Show("Please fill in all required fields (First Name, Last Name, Contact, Address, Criminal Case).",
                                "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (GetSelectedGender() == "")
            {
                MessageBox.Show("Please select a gender.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!HasPhoto)
            {
                MessageBox.Show("Please upload a photo.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var entry = new Entry
            {
                caseId = 0, // Assuming caseId is auto-generated or handled by the database
                firstName = FirstNameTextBox.Text,
                middleName = MiddleNameTextBox.Text,
                lastName = LastNameTextBox.Text,
                extensionName = ExtensionComboBox.SelectedItem != null
                 ? ((ComboBoxItem)ExtensionComboBox.SelectedItem).Content.ToString()
                 : "",
                gender = GetSelectedGender(),
                birthday = $"{GetComboBoxValue(BirthMonthComboBox)}/{GetComboBoxValue(BirthDayComboBox)}/{BirthYearTextBox.Text}",
                age = CalculateAge(BirthYearTextBox.Text), // Create a helper if needed
                address = AddressTextBox.Text,
                phone = ContactNumberTextBox.Text,
                barangay = BarangayComboBox.SelectedItem != null
                 ? ((ComboBoxItem)BarangayComboBox.SelectedItem).Content.ToString()
                 : "",
                criminalCase = CriminalCaseTextBox.Text,
                offenseCommitted = GetSelectedOffense(),
                courtNumber = CourtNumberTextBox.Text,
                status = StatusOfCaseComboBox.SelectedItem != null
                 ? ((ComboBoxItem)StatusOfCaseComboBox.SelectedItem).Content.ToString()
                 : "",
                photoUrl = _photoFileName // Temp filename to be finalized by Add_Entry_Helper
            };
            if (ValidateConvictForm()==false)
            {
                return;
            }



            businessLogic.Add_Entry(entry);
        
            string message = $"Collected Data:\n" +
                           $"Name: {entry.lastName}, {entry.firstName} {entry.middleName} {entry.extensionName}\n" +
                           $"Gender: {entry.gender}\n" +
                           $"Birthday: {entry.birthday}\n" +
                           $"Contact: {entry.phone}\n" +
                           $"Barangay: {entry.barangay}\n" +
                           $"Address: {entry.address}\n" +
                           $"Criminal Case: {entry.criminalCase}\n" +
                           $"Offense: {entry.offenseCommitted}\n" +
                           $"Description: {entry.description}\n" +
                           $"Court Number: {entry.courtNumber}\n" +
                           $"Committing Court: {entry.courtNumber}\n" +
                           $"Date Admitted: {entry.dateAdmitted}\n" +
                           $"Status: {entry.status}\n" +
                           $"Date Graduated: {entry.dateGraduated}";

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

        private async void PhotoUpload_Click(object sender, MouseButtonEventArgs e)
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
                    string selectedFile = openFileDialog.FileName;
                    _photoFileName = Path.GetFileName(selectedFile);

                    // Load image on background thread to avoid freezing
                    var imageData = await Task.Run(() => File.ReadAllBytes(selectedFile));
                    _photoData = imageData;

                    // Update UI with the loaded image
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.StreamSource = new MemoryStream(_photoData);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad; // Ensures the stream is closed
                    bitmap.EndInit();
                    bitmap.Freeze(); // Allow cross-thread access

                    UploadedPhotoImage.Source = bitmap;
                    UploadedPhotoImage.Visibility = Visibility.Visible;
                    DefaultPhotoIcon.Visibility = Visibility.Collapsed;

                    // Start upload in the background (fire-and-forget)
                    string uniqueFileName = $"{Path.GetFileNameWithoutExtension(selectedFile)}_{DateTime.Now:yyyyMMdd_HHmmss}{Path.GetExtension(selectedFile)}";
                    _ = businessLogic.UploadInBackgroundAsync(selectedFile, uniqueFileName);
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