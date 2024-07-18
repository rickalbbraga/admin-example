using Microsoft.AspNetCore.Authorization;

namespace API.Policies;

public class AgeAuthorization : AuthorizationHandler<MinimalAge>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimalAge requirement)
    {
        var claim = context.User.FindFirst(c => c.Type.Equals("birthdata"));
        
        if (claim is null) return Task.CompletedTask;
        
        var birthData = Convert.ToDateTime(claim.Value);
        
        if ((DateTime.Now.Year - birthData.Year) >= requirement.Age) context.Succeed(requirement);

        return Task.CompletedTask;
    }
}