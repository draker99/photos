using System;
namespace Photos.Models
{
    public class PictureComment
    {
        public string Id { get; set; }
        public string PictureId { get; set; }
        public string Comment { get; set; }

        public PictureComment()
        {
        }
    }
}
