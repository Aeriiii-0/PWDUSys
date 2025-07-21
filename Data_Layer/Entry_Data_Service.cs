using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Common_Class;
using Firebase.Storage;
using Google.Cloud.Firestore;

namespace Data_Layer
{
    public class Entry_DataService
    {
        FirestoreDb db;

        public Entry_DataService()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "pwsd-7f264-firebase-adminsdk-fbsvc-93a2654d90.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
            db = FirestoreDb.Create("pwsd-7f264");
        }

        static List<Entry> CacheEntries = new List<Entry>();

        public bool Add_Entry(Entry entry)
        {
            try
            {
                Add_Entry_Helper(entry);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async void Add_Entry_Helper(Entry entry)
        {
            string docName = "CaseId" + entry.caseId;
            string photoDownloadUrl = null;


            // --- Photo Upload Logic ---
            string actualFileNameForStorage = entry.lastName + entry.firstName + " picture"; // Use the actual file name
            photoDownloadUrl = await UploadPhotoAsync(entry.photoUrl, actualFileNameForStorage, entry.caseId.ToString());

            /*if (photoDownloadUrl == null)
            {
                MessageBox.Show("Photo upload failed. Document not added.", "Upload Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Stop if photo upload fails
            }*/

            // --- Firestore Data Preparation (includes PhotoUrl) ---
            DocumentReference col1 = db.Collection("PWUDS").Document(docName);
            Dictionary<string, object> data = new Dictionary<string, object>
            {
                { "CaseId", entry.caseId },
                { "FirstName", entry.firstName},
                { "MiddleName", entry.middleName },
                { "LastName", entry.lastName },
                { "ExtensionName", entry.extensionName },
                {"Gender", entry.gender },
                { "Birthday", entry.birthday },
                { "Age", entry.age },
                { "Address", entry.address },
                { "Phone", entry.phone },
                { "Baranggay", entry.barangay },
                { "Criminal Case", entry.criminalCase },
                { "Offense Committed", entry.offenseCommitted },
                { "Court Number", entry.courtNumber },
                { "Status", entry.status },
                { "PhotoUrl", entry.photoUrl }, // Store the download URL here
            };

            // --- Await the Firestore SetAsync operation ---
            await col1.SetAsync(data);
        }


        public async Task<string> UploadPhotoAsync(string filePath, string fileName, string caseId)
        {
            // IMPORTANT: Using your identified correct Firebase Storage bucket URL format
            var storage = new FirebaseStorage("pwsd-7f264.firebasestorage.app");

            try
            {
                if (!System.IO.File.Exists(filePath))
                {
                    return "Selected photo file not found: ";
                }

                using (var stream = System.IO.File.Open(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                {
                    var task = storage
                        .Child("Photos") // Matches your "Photos/" folder in Firebase Storage
                        .Child("CaseId" + caseId) // Subfolder for each case, e.g., CaseId69
                        .Child(fileName) // The actual file name
                        .PutAsync(stream);

                    task.Progress.ProgressChanged += (s, e) =>
                    {
                        Console.WriteLine($"Upload Progress for {fileName}: {e.Percentage}%");
                    };

                    var downloadUrl = await task;
                    Console.WriteLine($"Photo '{fileName}' uploaded successfully to: {downloadUrl}");
                    return downloadUrl;
                }
            }
            catch (Firebase.Storage.FirebaseStorageException fsex)
            {
                return $"Firebase Storage Error: {fsex.Message}\nURL: {fsex.RequestUrl}\nResponse: {fsex.ResponseData}";
            }
            catch (Exception ex)
            {
                return $"An unexpected error occurred during photo upload: {ex.Message}";
            }
        }

        public bool Update_Entry(Entry entry)
        {
            try
            {
                return Update_Entry_Helper(entry).Result; // Await Task result synchronously (not ideal, but works if needed)
            }
            catch (Exception e)
            {
                // Log error here if needed
                return false;
            }
        }

        public async Task<bool> Update_Entry_Helper(Entry entry)
        {
            try
            {
                string docName = "CaseId" + entry.caseId;
                DocumentReference docRef = db.Collection("PWUDS").Document(docName);
                Dictionary<string, object> data = new Dictionary<string, object>
                {
                    { "CaseId", entry.caseId },
                    { "FirstName", entry.firstName },
                    { "MiddleName", entry.middleName },
                    { "LastName", entry.lastName },
                    { "ExtensionName", entry.extensionName },
                    { "Gender", entry.gender },
                    { "Birthday", entry.birthday },
                    { "Age", entry.age },
                    { "Address", entry.address },
                    { "Phone", entry.phone },
                    { "Baranggay", entry.barangay },
                    { "Criminal Case", entry.criminalCase },
                    { "Offense Committed", entry.offenseCommitted },
                    { "Court Number", entry.courtNumber },
                    { "Status", entry.status },
                    { "PhotoUrl", entry.photoUrl }
                };

                await docRef.SetAsync(data, SetOptions.Overwrite); // Overwrites the whole document
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public Entry Get_Entry_By_CaseID(int caseID)
        {
            return Get_Entry_By_CaseId_Helper(caseID).Result;
        }

        public async Task<Entry> Get_Entry_By_CaseId_Helper(int caseId)
        {
            try
            {
                string docName = "CaseId" + caseId;
                DocumentReference docRef = db.Collection("PWUDS").Document(docName);

                DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

                if (snapshot.Exists)
                {
                    Entry entry = new Entry
                    (
                        snapshot.GetValue<int>("CaseId"),
                        snapshot.GetValue<string>("FirstName"),
                        snapshot.GetValue<string>("MiddleName"),
                        snapshot.GetValue<string>("LastName"),
                        snapshot.GetValue<string>("ExtensionName"),
                        snapshot.GetValue<string>("Gender"),
                        snapshot.GetValue<string>("Birthday"),
                        snapshot.GetValue<int>("Age"),
                        snapshot.GetValue<string>("Address"),
                        snapshot.GetValue<string>("Phone"),
                        snapshot.GetValue<string>("Baranggay"),
                        snapshot.GetValue<string>("Criminal Case"),
                        snapshot.GetValue<string>("Offense Committed"),
                        snapshot.GetValue<string>("Court Number"),
                        snapshot.GetValue<string>("Status"),
                        snapshot.GetValue<string>("PhotoUrl")
                    );

                    return entry;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
