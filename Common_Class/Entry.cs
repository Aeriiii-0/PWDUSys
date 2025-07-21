using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Common_Class
{
    public class Entry
    {
        public int caseId { get; set; }
        public string firstName { get; set; }
        public string middleName { get; set; }
        public string lastName { get; set; }
        public string extensionName { get; set; }
        public string gender { get; set; }
        public string birthday { get; set; }
        public int age { get; set; }
        public string address { get; set; }
        public string phone { get; set; }
        public string barangay { get; set; }
        public string criminalCase { get; set; }
        public string offenseCommitted { get; set; }
        public string courtNumber { get; set; }
        public string status { get; set; }
        public string photoUrl { get; set; }

        public Entry(int caseId, string firstName, string middleName, string lastName, string extensionName, string gender, string birthday, int age, string address, string phone, string barangay, string criminalCase, string offenseCommitted, string courtNumber, string status, string photoUrl)
        {
            this.caseId = caseId;
            this.firstName = firstName;
            this.middleName = middleName;
            this.lastName = lastName;
            this.extensionName = extensionName;
            this.gender = gender;
            this.birthday = birthday;
            this.age = age;
            this.address = address;
            this.phone = phone;
            this.barangay = barangay;
            this.criminalCase = criminalCase;
            this.offenseCommitted = offenseCommitted;
            this.courtNumber = courtNumber;
            this.status = status;
            this.photoUrl = photoUrl;
        }
    }
}
