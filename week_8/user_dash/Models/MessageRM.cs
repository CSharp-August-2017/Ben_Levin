using System.Collections.Generic;

namespace user_dash.Models
{
    public class MessageRenderModel : BaseEntity
    {
        public int ID { get; set; }
        
        public string Author { get; set; }

        public string Content { get; set; }

        public List<CommentRenderModel> Comments { get; set; }
        
        public string Created_At { get; set; }

        public string TimePast { get; set; }           
        
    }
}