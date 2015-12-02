using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using SavedIt.Portable.Data;
using UIKit;
using SavedIt.Portable.Models;
using SQLite.Net;
using SQLite.Net.Platform.XamarinIOS;

namespace SavedIt
{
    public partial class ViewController : UIViewController
    {
        private List<SavedItem> _savedItems = new List<SavedItem>();
        private SavedItemRepository _repository;

        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
            var connection = new SQLiteConnection(new SQLitePlatformIOS(), "savedItems.sqlite");
            _repository = new SavedItemRepository(connection);
            _savedItems = _repository.GetAll();
            UpdateTotalLabel();
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        partial void SaveButton_TouchUpInside(UIButton sender)
        {
            var item = new SavedItem();
            item.Date = DateTime.Now;
            item.Description = ItemTextBox.Text;
            decimal price = 0;
            decimal.TryParse(PreisTextBox.Text, out price);
            item.Price = price;

            _savedItems.Add(item);
            _repository.Create(item);

            UpdateTotalLabel();
        }

        private void UpdateTotalLabel()
        {
            SavedLabel.Text = _savedItems.Sum(i => i.Price).ToString("C");
        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);

            var detailController = segue.DestinationViewController as DetailViewController;
            if (detailController != null)
            {
                detailController.Saveditems = _savedItems;
            }
        }
    }
}