namespace lab7
{
    public class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AuthorID { get; set; }
        public string AuthorName { get; set; }
        public int ReleaseYear { get; set; }
        public int VolumeOfSheets { get; set; }
        public int Circulation { get; set; }
        public decimal Price { get; set; }  

  
        public Book()
        {
            Price = 0;
        }
    }
}