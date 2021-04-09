using Microsoft.EntityFrameworkCore;
using MyWebModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebModels.Seeding
{
    public static class CitySeed
    {
        public static void Seed(this ModelBuilder builder)
        {
            builder.Entity<City>().HasData(
                new City() { Id = 1, CountryId = 1, ArName = "سوهاج", EnName = "Sohag" },
                new City() { Id = 2, CountryId = 1, ArName = "القاهرة", EnName = "Cairo" },
                new City() { Id = 3, CountryId = 1, ArName = "الاسكندرية", EnName = "Alexandria" },
                new City() { Id = 4, CountryId = 1, ArName = "اسوان", EnName = "Aswan" },
                new City() { Id = 5, CountryId = 1, ArName = "الاقصر", EnName = "Loxur" }
                );
        }
    }
}
