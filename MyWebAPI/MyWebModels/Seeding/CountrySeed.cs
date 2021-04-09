using Microsoft.EntityFrameworkCore;
using MyWebModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebModels.Seeding
{
    public static class CountrySeed
    {
        public static void Seed(this ModelBuilder builder)
        {
            builder.Entity<Country>().HasData(
                new Country() { Id = 1, ArName = "مصر", EnName = "Egypt", Code = "EG", Language = "ar" }
                );
        }
    }
}
