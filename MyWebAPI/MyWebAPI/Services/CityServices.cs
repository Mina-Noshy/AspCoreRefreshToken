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
    public interface ICityServices
    {
        Task<ActionResult<IEnumerable<City>>> GetAll();
        City Find(int id);
        Task<ActionResult<bool>> Add(City city);
        Task<ActionResult<bool>> Update(City city);
        Task<ActionResult<bool>> Delete(int id);
        Task<bool> IsExists(int id);
    }

    public class CityServices : ICityServices
    {
        private readonly AppDbContext context;

        public CityServices(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<ActionResult<bool>> Add(City city)
        {
            context.Add(city);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<ActionResult<bool>> Delete(int id)
        {
            context.Remove(Find(id));
            await context.SaveChangesAsync();
            return true;
        }

        public City Find(int id)
        {
            return context.Cities.Include(s => s.GetCountry).FirstOrDefault(x => x.Id == id);
        }

        public async Task<ActionResult<IEnumerable<City>>> GetAll()
        {
            return await context.Cities.Include(s => s.GetCountry).ToListAsync();
        }

        public async Task<bool> IsExists(int id)
        {
            return await context.Cities.AnyAsync(x => x.Id == id);
        }

        public async Task<ActionResult<bool>> Update(City city)
        {
            context.Update(city);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
