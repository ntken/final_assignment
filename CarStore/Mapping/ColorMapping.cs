using System;
using CarShop.Shared.Models;
using CarStore.Entities;

namespace CarStore.Mapping;

public static class ColorMapping
{
    public static ColorDto ToDto(this Color color)
    {
        var colorDto = new ColorDto(color.Id, color.Name ?? "Unknown Color");
        return colorDto;
    }
}
