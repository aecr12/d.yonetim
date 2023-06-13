using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;

using Google.Apis.Auth.OAuth2;
namespace diyabetik.dao.Concrete
{
    public class FirebaseAdmin
    {

        // Firebase admin bağlantısı, bu bağlantı firebase üzerinde işlemler yapılmasını sağlayacak
        private static FirebaseApp app;
        private static readonly object lockObject = new object();

        string jsonPath = "../firebase-adminsdk.json";

        public FirebaseAdmin()
        {
            InitializeFirebaseApp();
        }

        public FirebaseApp GetFirebaseApp()
        {
            if (app == null)
            {
                lock (lockObject)
                {
                    if (app == null)
                    {
                        InitializeFirebaseApp();
                    }
                }
            }
            return app;
        }

        private void InitializeFirebaseApp()
        {
            if (app != null)
            {
                return; // FirebaseApp zaten oluşturulduysa işlemi tekrarlamayın
            }

            var credential = GoogleCredential.FromFile(jsonPath);
            var options = new AppOptions
            {
                Credential = credential,
            };
            app = FirebaseApp.Create(options);
        }
        
        
    }
}