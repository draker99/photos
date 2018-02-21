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
        protected int pic_id;
        public CommentPage()
        {
            InitializeComponent();

            comment = new ObservableCollection<PictureComment>();

        }

        public CommentPage(int current_pic_id, UserPicture pic)
        {
            InitializeComponent();
            this.pic_id = current_pic_id;

            i_comment.Source = pic.Picture;
            comment = new ObservableCollection<PictureComment>();

        }

        void SaveComment(object sender, EventArgs e)
        {

            //comment.Add(new PictureComment())
            var text = editor.Text;

            comment.Add(new PictureComment(text, this.pic_id.ToString()));
            Debug.WriteLine("Comment saved");
        }
    }
}
