namespace ProductApp.Models
{
    /// <summary>
    /// Reprezentuje model posta pobieranego lub wysyłanego do zewnętrznego API.
    /// </summary>
    public class PostData
    {
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
    }
}
