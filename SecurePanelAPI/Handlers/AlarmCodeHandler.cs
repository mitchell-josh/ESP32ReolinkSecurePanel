using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using SecurePanelAPI.Services;
using SecurePanelDb;
using SecurePanelDb.Models;
using SecurePanelModels.Utils;

namespace SecurePanelAPI.Handlers;

public class AlarmCodeHandler(
    IHttpContextAccessor httpContextAccessor, 
    SecurePanelDbContext db,
    IPasswordHasher<AlarmUser> hasher) : AuthorizationHandler<AlarmCodeRequirement>
{
    /// <summary>
    /// Custom authorization logic that validates the numeric PIN sent in request headers 
    /// against the hashed values stored in the database.
    /// </summary>
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AlarmCodeRequirement requirement)
    {
        var request = httpContextAccessor?.HttpContext?.Request;

        if ((request != null)
            && request.Headers.TryGetValue("X-Alarm-User", out var alarmUsername)
            && request.Headers.TryGetValue("X-Alarm-Code", out var providedPin))
        {
            var alarmUser = db.AlarmUsers.SingleOrDefault(u => u.Username == alarmUsername.ToString());

            if (alarmUser != null)
            {
                var result = hasher.VerifyHashedPassword(
                    alarmUser, 
                    alarmUser.AlarmCodeHash!, 
                    providedPin.ToString()!);
                
                if (result == PasswordVerificationResult.Success)
                {
                    var claims = new[]
                    {
                        new Claim(ClaimTypes.Name, alarmUser.Username!)
                    };
                    
                    var identity = new ClaimsIdentity(claims, "AlarmCode");
                    var principal = new ClaimsPrincipal(identity);
                    
                    httpContextAccessor!.HttpContext!.User = principal;
                    
                    context.Succeed(requirement);
                }
                else if (result == PasswordVerificationResult.SuccessRehashNeeded)
                {
                    throw new ArgumentException("Alarm code does not match.");
                }
            }
        }

        return Task.CompletedTask;
    }
}