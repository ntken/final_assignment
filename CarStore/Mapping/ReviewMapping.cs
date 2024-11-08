
using CarShop.Shared.Models;
using CarStore.Entities;

public static class ReviewMapping
{
    public static ReviewDto ToDto(this Review review) =>
        new ReviewDto
        {
            Id = review.Id,
            CarId = review.CarId,
            UserEmail = review.UserEmail,
            Comment = review.Comment,
            Rating = review.Rating,
            ReviewDate = review.ReviewDate
        };

    public static Review ToEntity(this ReviewDto reviewDto) =>
        new Review
        {
            CarId = reviewDto.CarId,
            Rating = reviewDto.Rating,
            Comment = reviewDto.Comment,
            UserEmail = reviewDto.UserEmail,
            ReviewDate = reviewDto.ReviewDate
        };
}
