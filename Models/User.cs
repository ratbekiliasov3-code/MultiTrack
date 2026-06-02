namespace MultiTrack.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";

        public List<TodoItem> TodoItems { get; set; } = new();
    }
}