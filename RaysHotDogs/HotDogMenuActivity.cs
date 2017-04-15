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
using RaysHotDogs.Core.Model;
using RaysHotDogs.Core.Service;
using RaysHotDogs.Adapters;

namespace RaysHotDogs
{
    [Activity(Label = "HotDogMenuActivity")]
    public class HotDogMenuActivity : Activity
    {
        private ListView hotDogListView;
        private List<HotDog> allHotDogs;
        private HotDogDataService hotDogDataService; 
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here

            SetContentView(Resource.Layout.HotDogListView);
            hotDogListView = FindViewById<ListView>(Resource.Id.hotDogListView);
            hotDogDataService = new HotDogDataService();
            allHotDogs = hotDogDataService.GetAllHotDogs();

            hotDogListView.Adapter = new HotDogListAdapter(this, allHotDogs);
            hotDogListView.FastScrollEnabled = true;

            hotDogListView.ItemClick += HotDogListView_ItemClick;
        }

        private void HotDogListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var hotDog = allHotDogs[e.Position];

            var intent = new Intent();
            intent.SetClass(this, typeof(HotDogDetailActivity));
            intent.PutExtra("selectHotdogId", hotDog.HotDogId);

            StartActivityForResult(intent, 100);
        }
    }
}