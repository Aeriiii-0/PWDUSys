using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Common_Class
{
    public class Entry
    {
        [JsonProperty("Case ID")]
        public int caseId { get; set; }

        [JsonProperty("FirstName")]
        public string firstName { get; set; }

        [JsonProperty("MiddleName")]
        public string middleName { get; set; }
        
        [JsonProperty("LastName")]
        public string lastName { get; set; }

        [JsonProperty("ExtensionName")]
        public string extensionName { get; set; }

        [JsonProperty("Gender")]
        public string gender { get; set; }
        [JsonProperty("Birthday")]
        public string birthday { get; set; }
        [JsonProperty("Age")]
        public int age { get; set; }
        [JsonProperty("Address")]
        public string address { get; set; }
        [JsonProperty("Contact Number")]
        public string phone { get; set; }
        [JsonProperty("Barangay")]
        public string barangay { get; set; }
        [JsonProperty("Criminal Case")]
        public string criminalCase { get; set; }
        [JsonProperty("Offense Committed")]
        public string offenseCommitted { get; set; }
        [JsonProperty("Court Number")]
        public string courtNumber { get; set; }
        [JsonProperty("Status")]
        public string status { get; set; }
        [JsonProperty("PhotoUrl")]
        public string photoUrl { get; set; }
        [JsonProperty("Date Admitted")]
        public string dateAdmitted { get; set; }
        [JsonProperty("Date Graduated")]
        public string dateGraduated { get; set; }
        public string description { get; set; }

        public Entry(){}
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
