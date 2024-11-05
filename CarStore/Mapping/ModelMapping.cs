using System;
using CarShop.Shared.Models;
using CarStore.Entities;

namespace CarStore.Mapping;

public static class ModelMapping
{
    public static ModelDto ToDto(this Model model)
    {
        return new ModelDto(model.Id, model.Name);
    }
}
