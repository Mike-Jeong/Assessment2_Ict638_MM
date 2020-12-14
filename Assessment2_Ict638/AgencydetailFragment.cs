using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Assessment2_Ict638
{
    public class AgencydetailFragment : Fragment, IOnMapReadyCallback
    {
        GoogleMap gMap;
        List<Data> houses;
        List<Agency> agencies;
        string uname;
        string uphone;
        string hlocation;
        

     
        public AgencydetailFragment(List<Agency> A, List<Data> H, string username, string userphone, string location)
        {

            houses = H;
            agencies = A;
            uname = username;
            uphone = userphone;
            hlocation = location;


        }

       


        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View v = inflater.Inflate(Resource.Layout.fragment_agencydetail, container, false);

            TextView Name = v.FindViewById<TextView>(Resource.Id.Aname);
            TextView Phonenumber = v.FindViewById<TextView>(Resource.Id.APhonenumber);
            TextView AEmail = v.FindViewById<TextView>(Resource.Id.Aemail);
            TextView Alocation = v.FindViewById<TextView>(Resource.Id.Alocation);

            Name.Text = agencies[0].agencyname;
            Phonenumber.Text = agencies[0].agencyphonenumber;
            AEmail.Text = agencies[0].agencyemail;
            Alocation.Text = agencies[0].agencylocation;


            var mapFrag = MapFragment.NewInstance();
            ChildFragmentManager.BeginTransaction()
                .Add(Resource.Id.mapAFrgContainer, mapFrag, "map")
                .Commit();

            mapFrag.GetMapAsync(this);


            Button btnShare = v.FindViewById<Button>(Resource.Id.btnAShare);
            Button btnSMS = v.FindViewById<Button>(Resource.Id.btnASMS);
            Button btnShow = v.FindViewById<Button>(Resource.Id.btnAShow);

            btnShare.Click += BtnShare_Click;
            btnSMS.Click += BtnSMS_Click;
            btnShow.Click += BtnShow_Click; 

            return v;
        }

        private void BtnShow_Click(object sender, EventArgs e)
        {

            markhouses(gMap);
            
        }

        public async void markhouses(GoogleMap googleMap)
        {

            for (int i = 0; i < houses.Count; i++)
            {
                if (agencies[0].agencyname == houses[i].agencyname)
                {
                    var location = (await Geocoding.GetLocationsAsync(string.Format("$\"{0}\"", houses[i].location))).FirstOrDefault();

                    if (location == null) return;
                    LatLng loc = new LatLng(location.Latitude, location.Longitude);


                    MarkerOptions markerOptions = new MarkerOptions();
                    markerOptions.SetPosition(loc);
                    markerOptions.SetTitle(string.Format("{0}", houses[i].heading));

                    googleMap.AddMarker(markerOptions);
                }
              

            }


           
        }



        private async void BtnSMS_Click(object sender, EventArgs e)
        {
            //To use this, delete async
            //throw new NotImplementedException();

            try
            {
                string text = "Hi, I am" + uname + "saw your details on the Rent-a-go app. Could you please send me details of more houses for rent in the same price range?";
                string recipient = agencies[0].agencyphonenumber;
                var message = new SmsMessage(text, new[] { recipient });
                await Sms.ComposeAsync(message);
            }
            catch (Exception exp)
            {
                Toast.MakeText(Activity, "Exception Found", ToastLength.Long).Show();
            }
        }
        private async void BtnShare_Click(object sender, EventArgs e)
        {
            string ADetails = "";
            ADetails = agencies[0].agencyname +agencies[0].agencyemail +agencies[0].agencyphonenumber +agencies[0].agencylocation ;
            await ShareText(ADetails);
        }

        public async Task ShareText(string text)
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Text = text,
                Title = "Agent detail Share"
            }
            );
        }


        public void OnMapReady(GoogleMap googleMap)
        {
           
            googleMap.MapType = GoogleMap.MapTypeNormal;
            googleMap.UiSettings.ZoomControlsEnabled = true;
            googleMap.UiSettings.CompassEnabled = true;
            



            getAgencyLocation(googleMap);
            gMap = googleMap;


            
        }

        public async void getAgencyLocation(GoogleMap googleMap)
        {
            Console.WriteLine("Test - LastLoc");
            try
            {

                var address = agencies[0].agencylocation;
                var locations = await Geocoding.GetLocationsAsync(address);
                var location = locations?.FirstOrDefault();

                if (location != null)
                {
                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}");
                    CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
                    builder.Target(new LatLng(location.Latitude, location.Longitude));
                    builder.Zoom(20);
                    builder.Bearing(155);
                    builder.Tilt(80);

                    CameraPosition cameraPosition = builder.Build();
                    CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);

                    gMap.MoveCamera(cameraUpdate);

                    MarkerOptions markerOptions = new MarkerOptions();
                    markerOptions.SetPosition(new LatLng(location.Latitude, location.Longitude));
                    markerOptions.SetTitle(agencies[0].agencyname);
                    markerOptions.SetIcon(BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueGreen));


                    googleMap.AddMarker(markerOptions);

                   

                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
                Toast.MakeText(Activity, "Feature Not Supported", ToastLength.Short);
            }
           
            catch (Exception ex)
            {
                // Unable to get location
                Toast.MakeText(Activity, "Unable to get location", ToastLength.Short);
            }

        }

       

    }
}