using System;
using CarShop.Shared.Models;
using CarStore.Entities;

namespace CarStore.Mapping;

public static class ModelMapping
{
    public static ModelDto ToDto(this Model model)
    {
        var modelDto = new ModelDto(model.Id, model.Name ?? "Unknown Model");
        return modelDto;
    }

}
