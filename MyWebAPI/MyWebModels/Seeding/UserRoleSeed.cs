using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebModels.Seeding
{
    public static class UserRoleSeed
    {
        public static void Seed(this ModelBuilder builder)
        {
            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string> { RoleId = "57784dee-54ff-4115-9835-da06239d6117", UserId = "6510262c-bbcb-4629-b1e7-20de05ef7ae6" },
                new IdentityUserRole<string> { RoleId = "57784dee-54ff-4115-9835-da06239d6117", UserId = "92ffc4e1-5b23-40c3-b68a-fb1e9ac67668" }
                );
        }
    }
}
