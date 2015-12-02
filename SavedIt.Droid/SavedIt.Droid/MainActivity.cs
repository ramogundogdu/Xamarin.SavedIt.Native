using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Views.InputMethods;
using Newtonsoft.Json;
using SavedIt.Portable.Data;
using SavedIt.Portable.Models;
using SQLite.Net;
using SQLite.Net.Platform.XamarinAndroid;
using Environment = System.Environment;

namespace SavedIt.Droid
{
    [Activity(Label = "SavedIt.Droid", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        List<SavedItem> _savedItems = new List<SavedItem>();
        private SavedItemRepository _repository;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            EditText descriptionText = FindViewById<EditText>(Resource.Id.descriptionText);
            EditText priceText = FindViewById<EditText>(Resource.Id.priceText);
            TextView totalText = FindViewById<TextView>(Resource.Id.totalSavedText);

            Button saveButton = FindViewById<Button>(Resource.Id.saveButton);

            var path = System.IO.Path.Combine(System.Environment.GetFolderPath(
                System.Environment.SpecialFolder.Personal), "savedItems.sqlite");
            var connection = new SQLiteConnection(new SQLitePlatformAndroid(), path);
            _repository = new SavedItemRepository(connection);
            _savedItems = _repository.GetAll();
            UpdateTotalLabel(totalText);

            saveButton.Click += (sender, e) =>
            {
                decimal price = 0;
                if (decimal.TryParse(priceText.Text, out price))
                {
                    var savedItem = new SavedItem();
                    savedItem.Date = DateTime.Now;
                    savedItem.Description = descriptionText.Text;
                    savedItem.Price = price;
                    _savedItems.Add(savedItem);
                    _repository.Create(savedItem);
                    UpdateTotalLabel(totalText);
                    descriptionText.Text = string.Empty;
                    priceText.Text = string.Empty;
                    HideKeyBoard();
                }
            };

            Button detailsButton = FindViewById<Button>(Resource.Id.detailsButton);
            detailsButton.Click += DetailsButton_Click;
        }

        private void UpdateTotalLabel(TextView totalText)
        {
            totalText.Text = _savedItems.Sum(s => s.Price).ToString("C");
        }

        private void DetailsButton_Click(object sender, EventArgs e)
        {
           var intent = new Intent(this, typeof(DetailActivity));
            var json = JsonConvert.SerializeObject(_savedItems);
            intent.PutExtra("SavedItems", json);
            StartActivity(intent);
        }

        private void HideKeyBoard()
        {
            InputMethodManager imm = (InputMethodManager) GetSystemService(InputMethodService);
            if (imm.IsAcceptingText)
            {
                imm.HideSoftInputFromInputMethod(CurrentFocus.WindowToken, 0);
            }
        }
    }
}

