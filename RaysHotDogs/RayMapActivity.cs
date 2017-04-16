using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;

namespace RaysHotDogs
{
    [Activity(Label = "Visit Ray's Store!")]
    public class RayMapActivity : Activity
    {
        private Button externalMapButton;
        private FrameLayout mapFrameLayout;
        private MapFragment mapFragment;
        private GoogleMap googleMap;
        private LatLng rayLocation;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            rayLocation = new LatLng(50.846704, 4.352446);

            SetContentView(Resource.Layout.RayMapView);
            FindViews();
            HandleEvents();
            CreateMapFragment();
            UpdateMapView();

        }
        private void CreateMapFragment()
        {
            mapFragment = FragmentManager.FindFragmentByTag("map") as MapFragment;

            if (mapFragment == null)
            {
                var googleMapOption = new GoogleMapOptions()
                    .InvokeMapType(GoogleMap.MapTypeSatellite)
                    .InvokeZoomControlsEnabled(true)
                    .InvokeCompassEnabled(true);

                FragmentTransaction fragmentTransaction = FragmentManager.BeginTransaction();
                mapFragment = MapFragment.NewInstance(googleMapOption);
                fragmentTransaction.Add(Resource.Id.mapFrameLayout, mapFragment, "map");
                fragmentTransaction.Commit();
            }
        }

        private void UpdateMapView()
        {
            var mapReadyCallBack = new LocalMapReady();

            mapReadyCallBack.MapReady += (sender, args) =>
            {
                googleMap = (sender as LocalMapReady).Map;

                if(googleMap != null)
                {
                    MarkerOptions markerOption = new MarkerOptions();
                    markerOption.SetPosition(rayLocation);
                    markerOption.SetTitle("Ray's Hot Dogs");
                    googleMap.AddMarker(markerOption);

                    CameraUpdate cameraUpdate = CameraUpdateFactory.NewLatLngZoom(rayLocation, 15);
                    googleMap.MoveCamera(cameraUpdate);
                }
            };

            mapFragment.GetMapAsync(mapReadyCallBack);
        }


        private void FindViews()
        {
            externalMapButton = FindViewById<Button>(Resource.Id.externalMapButton);
        }

        private void HandleEvents()
        {
            externalMapButton.Click += ExternalMapButton_Click;
        }

        private void ExternalMapButton_Click(object sender, EventArgs e)
        {
            Android.Net.Uri rayLocationUri = Android.Net.Uri.Parse("geo:50.846704,4.352446");
            Intent mapIntent = new Intent(Intent.ActionView, rayLocationUri);
            StartActivity(mapIntent);

        }

        private class LocalMapReady : Java.Lang.Object, IOnMapReadyCallback
        {
            public GoogleMap Map { get; private set; }

            public event EventHandler MapReady;

            public void OnMapReady(GoogleMap googleMap)
            {
                Map = googleMap;
                var handler = MapReady;
                if (handler != null)
                    handler(this, EventArgs.Empty);
            }
        }
    }
}