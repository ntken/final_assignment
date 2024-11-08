using System;
using System.ComponentModel.DataAnnotations;

namespace CarStoreUI.Models;

public class ReviewDto
{
    public int Id { get; set; }
    public int CarId { get; set; }
    public string? UserEmail { get; set; }
    public string? Comment { get; set; }
    public int Rating { get; set; }
    public DateTime ReviewDate { get; set; }
}

