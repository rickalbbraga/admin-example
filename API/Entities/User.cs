using Microsoft.AspNetCore.Identity;

namespace API.Entities;

public sealed class User : IdentityUser
{
    public DateTime BirthDate { get; private set; }

    private User()
    {
        
    }

    private User(string name, string email, DateTime birthDate)
    {
        UserName = name;
        Email = email;
        BirthDate = birthDate;
    }

    public static User Create(string name, string email, DateTime birthDate)
        => new User(name, email, birthDate);
}