using Microsoft.AspNetCore.Authorization;

namespace API.Policies;

public class MinimalAge(int age) : IAuthorizationRequirement 
{
    public int Age { get; private set; } = age;
}