using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Assessment2_Ict638.Models;
using Android.Util;
using Android.Gms.Common;
using Android.Support.V7.App;

namespace Assessment2_Ict638
{/// <summary>
/// //
/// </summary>
    [Activity(Label = "HomeActivity")]
    public class HomeActivity : AppCompatActivity
    {
        public const string TAG = "HomeActivity";
        internal static readonly string CHANNEL_ID = "ict638assessment_notification_channel";

        RecyclerView mRecycleView;
        RecyclerView.LayoutManager mLayoutManager;
        PhotoAlbum mPhotoAlbum;
        PhotoAdapter mAdapter;
        List<Data> dList = new List<Data>();

        public bool IsPlayServicesAvailable()
        {
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (resultCode != ConnectionResult.Success)
            {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                    Log.Debug(TAG, GoogleApiAvailability.Instance.GetErrorString(resultCode));
                else
                {
                    Log.Debug(TAG, "This device is not supported");
                    Finish();
                }
                return false;
            }

            Log.Debug(TAG, "Google Play Services is available.");
            return true;
        }

        private void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                // Notification channels are new in API 26 (and not a part of the
                // support library). There is no need to create a notification
                // channel on older versions of Android.
                return;
            }

            var channelName = CHANNEL_ID;
            var channelDescription = string.Empty;
            var channel = new NotificationChannel(CHANNEL_ID, channelName, NotificationImportance.Default)
            {
                Description = channelDescription
            };

            var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            notificationManager.CreateNotificationChannel(channel);

        }

        private void putData()
        {
           /* dList.Add(new Data("Database", "This is the database description"));
            dList.Add(new Data("Programming", "This is the programming description"));
            dList.Add(new Data("GitHub", "This is the github description"));
            dList.Add(new Data("Math", "This is Math description"));
            dList.Add(new Data("MongoDB", "This is MongoDB description"));
            dList.Add(new Data("C#", "This is a C# description"));*/

            string url = "https://10.0.2.2:5001/api/Data";
            string response = APIConnect.Get(url);
            dList = JsonConvert.DeserializeObject<List<Data>>(response);

        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_home);

            if (Intent.Extras != null)
            {
                foreach (var key in Intent.Extras.KeySet())
                {
                    if (key != null)
                    {
                        var value = Intent.Extras.GetString(key);
                        Log.Debug(TAG, "Key: {0} Value: {1}", key, value);
                    }
                }
            }

            IsPlayServicesAvailable();
            CreateNotificationChannel();

            //Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            //SetSupportActionBar(toolbar);

            mPhotoAlbum = new PhotoAlbum();
            putData();
            mLayoutManager = new LinearLayoutManager(this);

            mAdapter = new PhotoAdapter(mPhotoAlbum, dList);
            mAdapter.ItemClick += MAdapter_ItemClick;

            mRecycleView = FindViewById<RecyclerView>(Resource.Id.recyclerView);
            mRecycleView.SetLayoutManager(mLayoutManager);
            mRecycleView.SetAdapter(mAdapter);


        }

        // delete button profile

        private void MAdapter_ItemClick(object sender, int e)
        {
           int photoNum = e + 1;
            Toast.MakeText(this, "This is House number " + photoNum, ToastLength.Short).Show();

            Intent i = new Intent(this, typeof(NavigationActivity));
            Bundle bundle2 = new Bundle();
            bundle2.PutInt("photoid", photoNum);
            bundle2.PutString("heading", dList[e].heading);
            bundle2.PutString("description", dList[e].description);
            bundle2.PutString("numberofroom", dList[e].numberofroom);
            bundle2.PutString("numberoftoilet", dList[e].numberoftoilet);
            bundle2.PutString("rentfee", dList[e].rentfee);
            bundle2.PutString("location", dList[e].location);
            bundle2.PutString("agencyname", dList[e].agencyname);
            

            i.PutExtra("data", bundle2);

            StartActivity(i);


        }
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            //return base.OnCreateOptionsMenu(menu);
            return true;
        }


        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.Logout:
                    {
                        Finish();
                        return true;
                    }
                case Resource.Id.EditProfile:
                    {
                        //data or user 
                        Intent newActivity = new Intent(this, typeof(ProfileActivity));
                        Bundle bundle = Intent.GetBundleExtra("user");
                        newActivity.PutExtra("user", bundle);


                        StartActivity(newActivity);
                        return true;
                    }

            }

            return base.OnOptionsItemSelected(item);
        }



    }
}