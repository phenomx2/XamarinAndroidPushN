using Android.App;
using Android.Widget;
using Android.OS;
//google play service base
using Android.Gms.Common;
//Xamarin firebase
using Firebase.Messaging;
using Firebase.Iid;
using Android.Util;

namespace FCMClient
{
    [Activity(Label = "FCMClient", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        TextView _msgText;
        const string TAG = "MainActivity";

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            _msgText = FindViewById<TextView>(Resource.Id.msgText);
            //Handle if App is open from notification
            if (Intent.Extras != null)
            {
                foreach (var key in Intent.Extras.KeySet())
                {
                    var value = Intent.Extras.GetString(key);
                    Log.Debug(TAG, "Key: {0} Value: {1}", key, value);
                }
            }
            /*Cuando se detiene la depuracion en visual studio hay que desinstalar la aplicacion
             y volver a depurar para r3ecibir notificaciones, 
             de acuerdo a lo explicado en: 
             https://developer.xamarin.com/guides/android/application_fundamentals/notifications/remote-notifications-with-fcm/#Send_a_Message
             generar APK
             */



            IsPlayServicesAvailable();
            var logTokenButton = FindViewById<Button>(Resource.Id.logTokenButton);
            logTokenButton.Click += delegate {
                Log.Debug(TAG, "InstanceID token: " + FirebaseInstanceId.Instance.Token);
            };
            
        }


        public bool IsPlayServicesAvailable()
        {
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (resultCode != ConnectionResult.Success)
            {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                    _msgText.Text = GoogleApiAvailability.Instance.GetErrorString(resultCode);
                else
                {
                    _msgText.Text = "This device is not supported";
                    Finish();
                }
                return false;
            }
            else
            {
                _msgText.Text = "Google Play Services is available.";
                return true;
            }
        }
    }
}

