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

        //string An;
        //string Ap;
        //string Am;
        //string Al;
        //string l;
        // string n;


        public AgencydetailFragment(List<Agency> A, string username, string userphone)
        {
            agencies = A;
            uname = username;
            uphone = userphone;

        }

        /*public AgencydetailFragment(string agencyname, string agencyphonenumber, string agencyemail, string agencylocation)// , string name, string location)
        {
            An = agencyname; Ap = agencyphonenumber; Am = agencyemail; Al = agencylocation;// l = location; n = name;

        }*/



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

            btnShare.Click += BtnShare_Click;
            btnSMS.Click += BtnSMS_Click;

            return v;
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
            gMap = googleMap;
            googleMap.MapType = GoogleMap.MapTypeNormal;
            googleMap.UiSettings.ZoomControlsEnabled = true;
            googleMap.UiSettings.CompassEnabled = true;

            getLastLocation(googleMap);
            //getLocation(googleMap);



            //LatLng loc = new LatLng(lasLoc);
            //CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
            //builder.Target(loc);
            //builder.Zoom(20);
            //builder.Tilt(65);

            //CameraPosition cPos = builder.Build();
            //CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cPos);
            //googleMap.MoveCamera(cameraUpdate);

            //MarkerOptions markerOptions = new MarkerOptions();
            //markerOptions.SetPosition(loc);
            //markerOptions.SetTitle("NZSE City Campus");

            //googleMap.AddMarker(markerOptions);
        }

        public async void getLastLocation(GoogleMap googleMap)
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

                    googleMap.AddMarker(markerOptions);

                    //var location = await Geolocation.GetLastKnownLocationAsync();
                    //if (location != null)
                    //{
                    //    Console.WriteLine($"Last Loc - Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                    //    MarkerOptions curLoc = new MarkerOptions();
                    //    curLoc.SetPosition(new LatLng(location.Latitude, location.Longitude));
                    //    var address = await Geocoding.GetPlacemarksAsync(location.Latitude, location.Longitude);
                    //    var placemark = address?.FirstOrDefault();
                    //    var geocodeAddress = "";
                    //    if (placemark != null)
                    //    {
                    //        geocodeAddress =
                    //        $"AdminArea: {placemark.AdminArea}\n" +
                    //        $"CountryCode: {placemark.CountryCode}\n" +
                    //        $"CountryName: {placemark.CountryName}\n" +
                    //        $"FeatureName: {placemark.FeatureName}\n" +
                    //        $"Locality: {placemark.Locality}\n" +
                    //        $"PostalCode: {placemark.PostalCode}\n" +
                    //        $"SubAdminArea: {placemark.SubAdminArea}\n" +
                    //        $"SubLocality: {placemark.SubLocality}\n" +
                    //        $"SubThoroughfare: {placemark.SubThoroughfare}\n" +
                    //        $"Thoroughfare: {placemark.Thoroughfare}\n";

                    //    }
                    //    curLoc.SetTitle("You were here" + geocodeAddress);
                    //    curLoc.SetIcon(BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueAzure));
                    //    googleMap.AddMarker(curLoc);
                    //}

                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
                Toast.MakeText(Activity, "Feature Not Supported", ToastLength.Short);
            }
            //catch (FeatureNotEnabledException fneEx)
            //{
            //    // Handle not enabled on device exception
            //    Toast.MakeText(Activity, "Feature Not Enabled", ToastLength.Short);
            //}
            //catch (PermissionException pEx)
            //{
            //    // Handle permission exception
            //    Toast.MakeText(Activity, "Needs more permission", ToastLength.Short);
            //}
            catch (Exception ex)
            {
                // Unable to get location
                Toast.MakeText(Activity, "Unable to get location", ToastLength.Short);
            }

        }

        /*public async void getLocation(GoogleMap googleMap)
        {
            Console.WriteLine("Test - Location");
            try
            {
                var address = l ;
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



                    MarkerOptions markerOptions = new MarkerOptions();
                    markerOptions.SetPosition(new LatLng(location.Latitude, location.Longitude));
                    markerOptions.SetTitle(agencies[0].agencyname);

                    googleMap.AddMarker(markerOptions);

                    //var request = new GeolocationRequest(GeolocationAccuracy.Medium);
                    //var location = await Geolocation.GetLocationAsync(request);

                    //if (location != null)
                    //{
                    //    Console.WriteLine($"current Loc - Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                    //    MarkerOptions curLoc = new MarkerOptions();
                    //    curLoc.SetPosition(new LatLng(location.Latitude, location.Longitude));
                    //    var address = await Geocoding.GetPlacemarksAsync(location.Latitude, location.Longitude);
                    //    var placemark = address?.FirstOrDefault();
                    //    var geocodeAddress = "";
                    //    if (placemark != null)
                    //    {
                    //        geocodeAddress =
                    //        $"AdminArea: {placemark.AdminArea}\n" +
                    //        $"CountryCode: {placemark.CountryCode}\n" +
                    //        $"CountryName: {placemark.CountryName}\n" +
                    //        $"FeatureName: {placemark.FeatureName}\n" +
                    //        $"Locality: {placemark.Locality}\n" +
                    //        $"PostalCode: {placemark.PostalCode}\n" +
                    //        $"SubAdminArea: {placemark.SubAdminArea}\n" +
                    //        $"SubLocality: {placemark.SubLocality}\n" +
                    //        $"SubThoroughfare: {placemark.SubThoroughfare}\n" +
                    //        $"Thoroughfare: {placemark.Thoroughfare}\n";

                    //    }
                    //    curLoc.SetTitle("You are here" + geocodeAddress);
                    //    curLoc.SetIcon(BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueAzure));

                    //    googleMap.AddMarker(curLoc);

                }
            }


            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
                Toast.MakeText(Activity, "Feature Not Supported", ToastLength.Short);
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
                Toast.MakeText(Activity, "Feature Not Enabled", ToastLength.Short);
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
                Toast.MakeText(Activity, "Needs more permission", ToastLength.Short);
            }
            catch (Exception ex)
            {
                getLastLocation(googleMap);
            }
        }*/


    }
}