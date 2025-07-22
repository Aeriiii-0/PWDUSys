using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data_Layer;
namespace Business_Layer
{
    public class User_Business_Logic
    {
        User_Data_Service dataService;

        public User_Business_Logic()
        {
            dataService = new User_Data_Service();
        }

        public string AuthenticateUserAsync(string usermail,string password)
        {
            return dataService.AuthenticateUserAsync(usermail,password);
        }



    }
}
