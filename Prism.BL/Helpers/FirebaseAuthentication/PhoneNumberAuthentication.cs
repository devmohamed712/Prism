using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prism.BL.Configrations.FirebaseAuthentication
{
    public class PhoneNumberAuthentication
    {
        static PhoneNumberAuthentication()
        {
            string firebaseFilePath = AppDomain.CurrentDomain.BaseDirectory + "/FirebaseAuthentication/" + "prism-45b52-331179b4a4cc.json";
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile(firebaseFilePath),
            });
        }

        public async Task<UserRecord> PhoneNumberAuth(string uid)
        {
            UserRecord userRecord = await FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance.GetUserAsync(uid);
            return userRecord;
        }
    }
}
