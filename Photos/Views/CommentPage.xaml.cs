using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Collections.ObjectModel;

using Xamarin.Forms;

namespace Photos
{
    public partial class CommentPage : ContentPage
    {
        public ObservableCollection<PictureComment> comment { get; set; }

        public CommentPage()
        {
            InitializeComponent();

            comment = new ObservableCollection<PictureComment>();
            commentLstView.ItemsSource = comment;
        }

        void SaveComment (object sender, EventArgs e)
        {
            
            //comment.Add(new PictureComment())
            var text = ((Editor)sender).Text;
            comment.Add(new PictureComment(text));
            Debug.WriteLine("Comment saved" + text);
        }
    }
}
