using System;
namespace Photos
{
    public class PictureComment
    {
        public string Id { get; set; }
        public string PictureId { get; set; }
        public string CurrentDate { get; set; }
        public string Comment { get; set; }

        public PictureComment()
        {
        }

        public PictureComment(string comment)
        {
            this.Comment = comment;
        }

        public PictureComment(string comment, string pic_id, string comm_id, string currentdate)
        {
            this.Comment = comment;
            this.PictureId = pic_id;
            this.CurrentDate = currentdate;
            this.Id = comm_id;
        }
    }
}
