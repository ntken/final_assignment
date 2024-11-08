namespace CarStore.Entities
{
    public class Review
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public Car? Car { get; set; }
        public int Rating { get; set; } // Rating out of 5
        public string? Comment { get; set; }
        public string? UserEmail { get; set; }
        public DateTime ReviewDate { get; set; } = DateTime.Now;
    }
}
