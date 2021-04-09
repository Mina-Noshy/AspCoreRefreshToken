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
    public interface IMessageServices
    {
        Task<ActionResult<IEnumerable<Message>>> GetAll();
        Message Find(decimal id);
        Task<ActionResult<bool>> Add(Message message);
        Task<ActionResult<bool>> Update(Message message);
        Task<ActionResult<bool>> Delete(decimal id);
        Task<bool> IsExists(decimal id);
    }

    public class MessageServices : IMessageServices
    {
        private readonly AppDbContext context;

        public MessageServices(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<ActionResult<bool>> Add(Message message)
        {
            context.Add(message);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<ActionResult<bool>> Delete(decimal id)
        {
            context.Remove(Find(id));
            await context.SaveChangesAsync();
            return true;
        }

        public Message Find(decimal id)
        {
            return context.Messages.Include(x => x.GetSender).Include(x => x.GetChat).ThenInclude(x => x.GetUser_1).Include(x => x.GetChat).ThenInclude(x => x.GetUser_2).FirstOrDefault(x => x.Id == id);
        }

        public async Task<ActionResult<IEnumerable<Message>>> GetAll()
        {
            return await context.Messages.Include(x => x.GetSender).Include(x => x.GetChat).ThenInclude(x => x.GetUser_1).Include(x => x.GetChat).ThenInclude(x => x.GetUser_2).ToListAsync();
        }

        public async Task<bool> IsExists(decimal id)
        {
            return await context.Messages.AnyAsync(x => x.Id == id);
        }

        public async Task<ActionResult<bool>> Update(Message message)
        {
            context.Update(message);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
