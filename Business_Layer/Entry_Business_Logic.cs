using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common_Class;
using Data_Layer;

namespace Business_Layer
{
    public class Entry_Business_Logic
    {
        Entry_DataService dataService = new Entry_DataService();

        public Entry_Business_Logic()
        {
        }

        public async Task<string> AuthenticateUserAsync(string username, string password)
        {
            return await dataService.AuthenticateUserAsync(username, password);
        }


        public bool Add_Entry(Entry entry)
        {
            return dataService.Add_Entry(entry);
        }

        public async Task UploadInBackgroundAsync(string filePath, string fileName)
        {
             await dataService.UploadInBackgroundAsync(filePath, fileName);
        }


     
        public bool Update_Entry(Entry entry)
        {
            return dataService.Update_Entry(entry);
        }

        //use caseID to search
        public Entry SearchEntry(int caseId)
        {
            return dataService.Get_Entry_By_CaseID(caseId);
        }


        public async Task<List<Entry>> GetAllEntriesByBarangay(string barangay)
        {
            return await Query.TestGetFilteredRecords(barangay);
        }

    }
}
