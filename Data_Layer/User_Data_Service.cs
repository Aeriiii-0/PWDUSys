using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Layer
{
    public class User_Data_Service
    {
        FirestoreDb db;

        public User_Data_Service()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "pwsd-7f264-firebase-adminsdk-fbsvc-93a2654d90.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
            db = FirestoreDb.Create("pwsd-7f264");
        }


        public string AuthenticateUserAsync(string username, string password)
        {
            return AuthenticateUserAsync_Helper(username, password).Result;  
        }



        async Task<string> AuthenticateUserAsync_Helper(string username, string password)
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
    }
}
