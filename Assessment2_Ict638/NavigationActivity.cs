﻿using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Assessment2_Ict638
{
   public class Agency
    {
        public string agencyname { get; set; }
        public string agencyemail { get; set; }
        public string agencyphonenumber { get; set; }
        public string agencylocation { get; set; }

    }




    [Activity(Label = "NavigationActivity")]
    public class NavigationActivity : Activity, BottomNavigationView.IOnNavigationItemSelectedListener
    {
        [System.Obsolete]

        List<Data> hList;
        List<Agency> agencies;
        List<Agency> hagency;



        public bool OnNavigationItemSelected(IMenuItem item)
        {
            FrameLayout navFragContainer = FindViewById<FrameLayout>(Resource.Id.navFragContainer);
            FragmentTransaction transaction;
            Bundle data = Intent.GetBundleExtra("data");

            
            switch (item.ItemId)
            {
                case Resource.Id.menu1:

                    HousedetailFragment sFrag = new HousedetailFragment(hList, hagency);
                    sFrag.getph(data.GetInt("photoid"));
                    transaction = FragmentManager.BeginTransaction();
                    transaction.Replace(Resource.Id.navFragContainer, sFrag);
                    transaction.Commit();

                    return true;

                case Resource.Id.menu2:

                    navFragContainer.RemoveAllViewsInLayout();

                  
                    AgencydetailFragment aFrag = new AgencydetailFragment(agencies, data.GetString("uname"), data.GetString("uphone"));

                    transaction = FragmentManager.BeginTransaction();
                    transaction.Replace(Resource.Id.navFragContainer, aFrag);
                    transaction.Commit();

                    return true;

            }
            return false;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.activity_navigation);

            Bundle data = Intent.GetBundleExtra("data");

            BottomNavigationView navigationView = FindViewById<BottomNavigationView>(Resource.Id.TopNavBar);
            navigationView.SetOnNavigationItemSelectedListener(this);

            FragmentTransaction transaction = FragmentManager.BeginTransaction();

            hList = JsonConvert.DeserializeObject<List<Data>>(Intent.GetStringExtra("ListItem"));


            bool staus = false;
            string url = "https://10.0.2.2:5001/api/Agencies";
            string response = APIConnect.Get(url);
            agencies = JsonConvert.DeserializeObject<List<Agency>>(response);




            for (int i = 0; i < agencies.Count; i++)
            {
                if (agencies[i].agencyname == hList[0].agencyname)
                {
                     hagency.Add(agencies[i]);
                }
            }
            


            HousedetailFragment sFrag = new HousedetailFragment(hList, hagency);
            sFrag.getph(data.GetInt("photoid"));





            navigationView.SelectedItemId = Resource.Id.menu1;


 // sFrag. PutExtra("data", data);
            // heading = "House name : " + data.GetString("heading");
            // numberofroom = data.GetString("numberofroom");
            //numberoftoilet = data.GetString("numberoftoilet");
            // rentfee = data.GetString("rentfee");
            // location = data.GetString("location");
            // agencyname = "Agency name : " + data.GetString("agencyname");
            // description = "Description " + data.GetString("description");
        }


    }
}