using System;
using System.ComponentModel.DataAnnotations;

namespace CarShop.Shared.Models;

public class ReviewDto
{
    public int Id { get; set; }
    public int CarId { get; set; }
    [Required(ErrorMessage = "User email is required")]
    public string? UserEmail { get; set; }
    [Required(ErrorMessage = "Comment is required")]
    public string? Comment { get; set; }
    public int Rating { get; set; }
    public DateTime ReviewDate { get; set; }
}

