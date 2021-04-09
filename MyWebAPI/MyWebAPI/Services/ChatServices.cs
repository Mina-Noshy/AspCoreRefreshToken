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
    public interface IChatServices
    {
        Task<ActionResult<IEnumerable<Chat>>> GetAll();
        Chat Find(int id);
        Task<ActionResult<bool>> Add(Chat chat);
        Task<ActionResult<bool>> Update(Chat chat);
        Task<ActionResult<bool>> Delete(int id);
        Task<bool> IsExists(int id);
    }

    public class ChatServices : IChatServices
    {
        private readonly AppDbContext context;

        public ChatServices(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<ActionResult<bool>> Add(Chat chat)
        {
            context.Add(chat);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<ActionResult<bool>> Delete(int id)
        {
            context.Remove(Find(id));
            await context.SaveChangesAsync();
            return true;
        }

        public Chat Find(int id)
        {
            return context.Chats.Include(x => x.GetUser_1).Include(x => x.GetUser_2).FirstOrDefault(x => x.Id == id);
        }

        public async Task<ActionResult<IEnumerable<Chat>>> GetAll()
        {
            return await context.Chats.Include(x => x.GetUser_1).Include(x => x.GetUser_2).ToListAsync();
        }

        public async Task<bool> IsExists(int id)
        {
            return await context.Chats.AnyAsync(x => x.Id == id);
        }

        public async Task<ActionResult<bool>> Update(Chat chat)
        {
            context.Update(chat);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
