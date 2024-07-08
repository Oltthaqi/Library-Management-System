namespace LibraryManagementSystem.Model
{
    public class Book
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Author { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public double? Price { get; set; }
        public int? Pages { get; set; }
        public byte[]? img { get; set; }
        public string? img64 { get; set; }


    }
}
