using System;
using CarShop.Shared.Models;
using CarStore.Entities;

namespace CarStore.Mapping;

public static class CompanyMapping
{
    public static CompanyDto ToDto(this Company company)
    {
        return new CompanyDto(company.Id, company.Name);
    }
}
