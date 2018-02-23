using System;
using Xamarin.Forms;

namespace Photos
{
    public class UserPicture
    {
       
        public string Id { get; set; }
        public string AccountId { get; set; }
      
        public ImageSource Picture { get; set; }


        public UserPicture(ImageSource picture, string id)
        {
            this.Picture = picture;
            this.Id = id;
           
        }

        public UserPicture()
        {
            
        }

    }
}


