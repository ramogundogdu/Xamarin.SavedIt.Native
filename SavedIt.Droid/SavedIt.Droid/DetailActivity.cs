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
using Newtonsoft.Json;
using SavedIt.Portable.Models;

namespace SavedIt.Droid
{
    [Activity(Label = "@string/Details")]
    public class DetailActivity : ListActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            var json = Intent.GetStringExtra("SavedItems");
            List<SavedItem> savedItems = JsonConvert.DeserializeObject<List<SavedItem>>(json) ?? new List<SavedItem>();
            // Create your application here

            ListAdapter = new SavedItemAdapter(this, savedItems);
        }
    }

    internal class SavedItemAdapter : BaseAdapter<SavedItem>
    {
        private DetailActivity context;
        private List<SavedItem> savedItems;

        public SavedItemAdapter(DetailActivity context, List<SavedItem> savedItems)
        {
            this.context = context;
            this.savedItems = savedItems;
        }

        public override SavedItem this[int position]
        {
            get { return savedItems[position]; }
        }

        public override int Count
        {
            get { return savedItems.Count; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = savedItems[position];
            View view = convertView ?? context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItem2, null);

            view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = item.Description;
            view.FindViewById<TextView>(Android.Resource.Id.Text2).Text = string.Format("{0:C}({1:d})", item.Price, item.Date);
            return view;
        }
    }
}