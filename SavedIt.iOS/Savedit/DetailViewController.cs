using Foundation;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using UIKit;
using SavedIt.Portable.Models;

namespace SavedIt
{
	partial class DetailViewController : UITableViewController
	{
        public List<SavedItem> Saveditems { get; set; } 
		public DetailViewController (IntPtr handle) : base (handle)
		{
            //TableView.RegisterClassForCellReuse(typeof(UITableViewCell), new NSString("DetailCell"));
            //Bereits im Designer geschehen
            TableView.Source = new DetailDataSource(this);
            Saveditems= new List<SavedItem>();
		}
	}

    internal class DetailDataSource : UITableViewSource
    {
        private DetailViewController detailViewController;
        public DetailDataSource(DetailViewController detailViewController)
        {
            this.detailViewController = detailViewController;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell(new NSString("DetailCell"));
            var item = detailViewController.Saveditems[indexPath.Row];
            cell.TextLabel.Text = item.Description;
            cell.DetailTextLabel.Text = string.Format("{0:C}, ({1:d})", item.Price, item.Date);
            return cell;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return detailViewController.Saveditems.Count;
        }
    }
}
