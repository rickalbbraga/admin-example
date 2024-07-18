using API.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class UserDbContext(DbContextOptions<UserDbContext> opts) : IdentityDbContext<User>(opts);