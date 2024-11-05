using System;
using CarShop.Shared.Models;
using CarStore.Entities;

namespace CarStore.Mapping;

public static class CarMapping
{
    public static Car ToEntity(this CreateCarDto car)
    {
            return new Car()
            {
                CompanyId = car.CompanyId,
                ModelId = car.ModelId,
                ColorId = car.ColorId,
                Price = car.Price,
                ReleasedDate = car.ReleasedDate,
                Description = car.Description,
                Image = car.Image
            }; 
    }

    public static CarSummaryDto ToCarSummaryDto(this Car car)
    {
        return new(
                car.Id,
                car.Company!.Name,
                car.Model!.Name,
                car.Color!.Name,
                car.Price,
                car.ReleasedDate,
                car.Description,
                car.Image
            );        
    }

    public static CarDetailsDto ToCarDetailsDto(this Car car)
    {
        return new(
                car.Id,
                car.CompanyId,
                car.ModelId,
                car.ColorId,
                car.Price,
                car.ReleasedDate,
                car.Description,
                car.Image
            );        
    }

    public static Car ToEntity(this UpdateCarDto car, int id)
    {
            return new Car()
            {
                Id = id,
                CompanyId = car.CompanyId,
                ModelId = car.ModelId,
                ColorId = car.ColorId,
                Price = car.Price,
                ReleasedDate = car.ReleasedDate,
                Description = car.Description,
                Image = car.Image
            }; 
    }
}
