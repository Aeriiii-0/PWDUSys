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

        public bool Add_Entry(Entry entry)
        {
            return dataService.Add_Entry(entry);
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

    }
}
