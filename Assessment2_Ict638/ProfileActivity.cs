using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assessment2_Ict638.Models;
using Android.Util;
using Android.Gms.Maps;

namespace Assessment2_Ict638
{
    [Activity(Label = "ProfileActivity")]
    public class ProfileActivity : Activity, IOnMapReadyCallback
    {
        int userid;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Create your application here
            SetContentView(Resource.Layout.activity_userprofile);
            Bundle bundle = Intent.GetBundleExtra("data");
            userid = bundle.GetInt("id");

            EditText et = FindViewById<EditText>(Resource.Id.Pname);
            EditText et2 = FindViewById<EditText>(Resource.Id.PPhonenumber);
            TextView tv1 = FindViewById<TextView>(Resource.Id.Pemail);
            EditText et3 = FindViewById<EditText>(Resource.Id.PCountry);
            TextView tv3 = FindViewById<TextView>(Resource.Id.Pusername);
            TextView tv4 = FindViewById<TextView>(Resource.Id.Ppassword);

            et.Hint = bundle.GetString("name");
            tv3.Text = bundle.GetString("username");
            tv4.Text = bundle.GetString("password");
            et2.Hint = bundle.GetString("phonenumber");
            et3.Hint = bundle.GetString("country");
            tv1.Text = bundle.GetString("email");


            // Delete the logout and current location
            Button btnPModify = FindViewById<Button>(Resource.Id.btnPModify);
            

            btnPModify.Click += BtnPModify_Click;


           

            var mapFragment = (MapFragment)FragmentManager.FindFragmentById(Resource.Id.PMapFrgContainer);
            mapFragment.GetMapAsync(this);


        }

        public static MapFragment newInstance()
        {
            var mapFrag = MapFragment.NewInstance();
            ChildFragmentManager.BeginTransaction()
                                    .Add(Resource.Id.PMapFrgContainer, mapFrag, "map_fragment")
                                    .Commit();
        }

        private void BtnPModify_Click(object sender, EventArgs e)
        {
            EditText et = FindViewById<EditText>(Resource.Id.Pname);
            EditText et2 = FindViewById<EditText>(Resource.Id.PPhonenumber);
            TextView tv1 = FindViewById<TextView>(Resource.Id.Pemail);
            EditText et3 = FindViewById<EditText>(Resource.Id.PCountry);
            TextView tv3 = FindViewById<TextView>(Resource.Id.Pusername);
            TextView tv4 = FindViewById<TextView>(Resource.Id.Ppassword);



            User u = new User();
            u.name = et.Text;
            u.username = tv3.Text;
            u.password = tv4.Text;
            u.email = tv1.Text;
            u.country = et3.Text;
            u.phonenumber = et2.Text;


            ModifyUser(u, userid);
        }
        public string getQuotedString(string str)
        {
            return "\"" + str + "\"";
        }

        public void ModifyUser(User user, int Userid)
        {
            string url = "https://10.0.2.2:5001/api/Users" + "/" + Userid;

            string json =
                "{" +
                getQuotedString("id")+":"+Userid+","+
                getQuotedString("name") + ":" + getQuotedString(user.name) + "," +
                getQuotedString("username") + ":" + getQuotedString(user.username) + "," +
                getQuotedString("password") + ":" + getQuotedString(user.password) + "," +
                getQuotedString("phonenumber") + ":" + getQuotedString(user.phonenumber) + "," +
                getQuotedString("country") + ":" + getQuotedString(user.country) + "," +
                getQuotedString("email") + ":" + getQuotedString(user.email) +
                "}";

            if (APIConnect.Put(url, json))
            {
                Toast.MakeText(this, "User successfully Modified!", ToastLength.Long).Show();
            }
        }




        //May delete the logout in profile

        private void BtnPLogout_Click(object sender, EventArgs e)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(this);

            builder.SetTitle("Logout?");
            builder.SetMessage("Are you sure you want to log out of the app?\n(Go to the Login page after the logout.)");
            builder.SetPositiveButton("OK", (c, ev) =>
            {

                Intent LoginActivity = new Intent(this, typeof(LoginActivity));
                StartActivity(LoginActivity);
                FinishAffinity();

            });
            builder.SetNegativeButton("Cancel", (c, ev) =>
            {
                builder.Dispose();  
            });
            builder.Create().Show();
        }

        public void OnMapReady(GoogleMap googleMap)
        {
            throw new NotImplementedException();
        }
    }
}