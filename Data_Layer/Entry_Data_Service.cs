using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Common_Class;
using Google.Cloud.Storage.V1;
using Firebase.Storage;
using Google.Cloud.Firestore;

namespace Data_Layer
{
    public class Entry_DataService
    {
        FirestoreDb db;
        private readonly string bucketName = "pwsd-7f264.firebasestorage.app"; // Your bucket name
        private readonly StorageClient storageClient;
        private string tempPhotoUrl = null; // Store the temporary photo URL
        private string tempPhotoFileName = null;
        public Entry_DataService()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "pwsd-7f264-firebase-adminsdk-fbsvc-93a2654d90.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
            db = FirestoreDb.Create("pwsd-7f264");
            storageClient = StorageClient.Create();
        }

        static List<Entry> CacheEntries = new List<Entry>();

        //user Methods

        public async Task<string> AuthenticateUserAsync(string username, string password)
        {
            try
            {
                // Query Firestore collection 'Users'
                Google.Cloud.Firestore.Query usersQuery = db.Collection("Users")
                                      .WhereEqualTo("usermail", username)
                                     .WhereEqualTo("password", password);

                QuerySnapshot snapshot = await usersQuery.GetSnapshotAsync();

                if (snapshot.Documents.Count == 0)
                {
                    // No user found with given username & password
                    return null;
                }

                // Retrieve the first matched user document
                DocumentSnapshot userDoc = snapshot.Documents[0];

                // Retrieve the Role field (case-sensitive in Firestore)
                string role = userDoc.ContainsField("Role") ? userDoc.GetValue<string>("Role") : null;

                return role;  // Could be "user", "admin", etc.
            }
            catch (Exception ex)
            {
                return null;
                throw new ApplicationException($"Error authenticating user: {ex.Message}");
             
            }
        }


        //Entry
        public async Task UploadInBackgroundAsync(string filePath, string fileName)
        {
            try
            {
                // Update UI to show upload status (optional)
              
                // Perform the actual upload
                tempPhotoUrl = await UploadPhotoDirectlyAsync(filePath,  fileName);
                tempPhotoFileName = fileName;

      
                if (tempPhotoUrl != null)
                {
                
                    //may need to add input validation later on 
                }
            }
            catch (Exception ex)
            {
              
                tempPhotoUrl = null;
                tempPhotoFileName = null;
            }
        }

         public async Task<string> UploadPhotoDirectlyAsync(string filePath,  string fileName)
        {
            try
            {
                int caseId = 0;
                if (!File.Exists(filePath))
                {
                    throw new ApplicationException("Selected photo file not found: " + filePath);
                    return null;
                }

                string finalPath = $"TempPhotos/CaseId{caseId}/{fileName}";

                // Get the content type based on file extension
                string contentType = GetContentType(Path.GetExtension(filePath).ToLower());

                using (var fileStream = File.OpenRead(filePath))
                {
                    // Upload with proper content type
                    await storageClient.UploadObjectAsync(bucketName, finalPath, contentType, fileStream);
                }

                string finalUrl = GetFileUrl(finalPath);
                Console.WriteLine($"Photo '{fileName}' uploaded successfully: {finalUrl}");
                return finalUrl;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error uploading photo: {ex.Message}");
                return null;
            }
        }


     

        private string GetFileUrl(string objectPath)
        {
            return $"https://firebasestorage.googleapis.com/v0/b/{bucketName}/o/{Uri.EscapeDataString(objectPath)}?alt=media";
        }

        public async Task<string> MovePhotoToFinalAsync(string tempFileName, int caseId, string finalFileName)
        {
            try
            {
                string tempPath = $"TempPhotos/CaseId0/{tempFileName}";
                string finalPath = $"Photos/CaseId{caseId}/{finalFileName}";

                Console.WriteLine($"Copying {tempPath} to {finalPath}...");

                // Simple copy - content type will be preserved from original upload
                await storageClient.CopyObjectAsync(bucketName, tempPath, bucketName, finalPath);

                // Delete the temp file
                await storageClient.DeleteObjectAsync(bucketName, tempPath);

                string finalUrl = GetFileUrl(finalPath);
                Console.WriteLine($"Photo moved from {tempPath} to {finalPath}");
                return finalUrl;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error moving photo: {ex.Message}");
                return null;
            }
        }

        // Helper method to get content type based on file extension
        private string GetContentType(string fileExtension)
        {
            return fileExtension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".bmp" => "image/bmp",
                ".webp" => "image/webp",
                ".tiff" or ".tif" => "image/tiff",
                _ => "application/octet-stream" // fallback
            };
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
        private async Task<int> GetUniqueCaseIdAsync()
        {
            try
            {
                DocumentReference caseNumRef = db.Collection("caseNumber").Document("caseNumber");

                // Atomically increment the latest case number
                await caseNumRef.UpdateAsync("latestCaseNumber", FieldValue.Increment(1));

                // Fetch the updated value
                DocumentSnapshot snapshot = await caseNumRef.GetSnapshotAsync();
                int nextCaseId = Convert.ToInt32(snapshot.GetValue<long>("latestCaseNumber"));

                return nextCaseId;
            }
            catch (Exception ex)
            {
                return -1;
              throw new ApplicationException($"Error getting next case ID: {ex.Message}");
               
            }
        }
      

        public async void Add_Entry_Helper(Entry entry)
        {   // Get unique case ID
            int newCaseId = await GetUniqueCaseIdAsync();
            if (newCaseId == -1) return;

         
            string photoDownloadUrl = null;

            string finalPhotoName = $"Case_{newCaseId}_{entry.lastName}_{DateTime.Now:yyyyMMdd_HHmmss}{Path.GetExtension(tempPhotoFileName)}";
            string finalPhotoUrl = await MovePhotoToFinalAsync(tempPhotoFileName, newCaseId, finalPhotoName);
            

            
            string docName = "CaseId" + newCaseId;
            DocumentReference col1 = db.Collection("PWUDS").Document(docName);
            Dictionary<string, object> data = new Dictionary<string, object>
            {
                { "CaseId", newCaseId },
                { "FirstName", entry.firstName},
                { "MiddleName", entry.middleName },
                { "LastName", entry.lastName },
                { "ExtensionName", entry.extensionName },
                {"Gender", entry.gender },
                { "Birthday", entry.birthday },
                { "Age", entry.age },
                { "Address", entry.address },
                { "Conctact Number", entry.phone },
                { "Barangay", entry.barangay },
                { "Criminal Case", entry.criminalCase },
                { "Offense Committed", entry.offenseCommitted },
                { "Court Number", entry.courtNumber },
                { "Status", entry.status },
                { "PhotoUrl", entry.photoUrl },
                {"Date Admitted",entry.dateAdmitted },
                {"Date Graduated", entry.dateGraduated }// Store the download URL here
            };

            // --- Await the Firestore SetAsync operation ---
            await col1.SetAsync(data);
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
