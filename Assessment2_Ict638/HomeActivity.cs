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

namespace Assessment2_Ict638
{/// <summary>
/// //
/// </summary>
    [Activity(Label = "HomeActivity")]
    public class HomeActivity : Activity
    {
        RecyclerView mRecycleView;
        RecyclerView.LayoutManager mLayoutManager;
        PhotoAlbum mPhotoAlbum;
        PhotoAdapter mAdapter;
        List<Data> dList = new List<Data>();
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

            mPhotoAlbum = new PhotoAlbum();
            putData();
            mLayoutManager = new LinearLayoutManager(this);

            mAdapter = new PhotoAdapter(mPhotoAlbum, dList);
            mAdapter.ItemClick += MAdapter_ItemClick;

            mRecycleView = FindViewById<RecyclerView>(Resource.Id.recyclerView);
            mRecycleView.SetLayoutManager(mLayoutManager);
            mRecycleView.SetAdapter(mAdapter);

            Button btnuserprofile = FindViewById<Button>(Resource.Id.btnuserprofile);
            btnuserprofile.Click += Btnuserprofile_Click; ;

        }

        private void Btnuserprofile_Click(object sender, EventArgs e)
        {
            Intent newActivity = new Intent(this, typeof(ProfileActivity));
            Bundle bundle = Intent.GetBundleExtra("data");
            newActivity.PutExtra("data", bundle);
           

            StartActivity(newActivity);
        }

        private void MAdapter_ItemClick(object sender, int e)
        {
           int photoNum = e + 1;
            Toast.MakeText(this, "This is photo number " + photoNum, ToastLength.Short).Show();

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


    }
}