using System;
namespace Photos
{
    public class PictureComment
    {
        public string Id { get; set; }
        public UserPicture PictureId { get; set; }
        public string Comment { get; set; }

        public PictureComment()
        {
        }

        public PictureComment(string comment, UserPicture pictureid)
        {
            this.Comment = comment;
            this.PictureId = PictureId;
        }

        public PictureComment(string comment)
        {
            this.Comment = comment;
        }
    }
}
