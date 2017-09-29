namespace user_dash.Models
{
    public class CommentRenderModel : BaseEntity
    {
        public int ID { get; set; }
        
        public string Author { get; set; }

        public string Content { get; set; }
        
        public string Created_At { get; set; }

        public string TimePast { get; set; }               
        
    }
}