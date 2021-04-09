using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWebModels.Database;
using MyWebModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebAPI.Services
{
    public interface ICountryServices
    {
        Task<ActionResult<IEnumerable<Country>>> GetAll();
        Country Find(int id);
        Task<ActionResult<bool>> Add(Country country);
        Task<ActionResult<bool>> Update(Country country);
        Task<ActionResult<bool>> Delete(int id);
        Task<bool> IsExists(int id);
    }

    public class CountryServices : ICountryServices
    {
        private readonly AppDbContext context;

        public CountryServices(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<ActionResult<bool>> Add(Country country)
        {
            context.Add(country);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<ActionResult<bool>> Delete(int id)
        {
            context.Remove(Find(id));
            await context.SaveChangesAsync();
            return true;
        }

        public Country Find(int id)
        {
            return context.Countries.FirstOrDefault(x => x.Id == id);
        }

        public async Task<ActionResult<IEnumerable<Country>>> GetAll()
        {
            return await context.Countries.ToListAsync();
        }

        public async Task<bool> IsExists(int id)
        {
            return await context.Countries.AnyAsync(x => x.Id == id);
        }

        public async Task<ActionResult<bool>> Update(Country country)
        {
            context.Update(country);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
