namespace H5Api.Models
{
    public class StickyData
    {
        public long Id { get; set; }

        public string Uid { get; set; }

        public string Content { get; set; }

        public DateTime create_time { get; set; }

        public DateTime? update_time { get; set; }
    }
}
