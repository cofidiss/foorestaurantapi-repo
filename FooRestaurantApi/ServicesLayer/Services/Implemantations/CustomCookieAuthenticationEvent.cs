using FooRestaurantApi.DomainLayer.Entity;
using FooRestaurantApi.RepositoryLayer.RepositoryPattern.Interfaces;
using FooRestaurantApi.ServicesLayer.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System;
using System.Linq;
using System.Threading.Tasks;

public class CustomCookieAuthenticationEvent : CookieAuthenticationEvents, ICustomCookieAuthenticationEvent
{
    public static string ClaimDateFormat = "yyyy-MM-dd HH:mm:ss.fff";
    private readonly IUserRepository _userRepository;

    public CustomCookieAuthenticationEvent(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public override async Task ValidatePrincipal(CookieValidatePrincipalContext context)
    {
        var userPrincipal = context.Principal;


        var lastUpdateDateString = (from c in userPrincipal.Claims
                                    where c.Type == nameof(UserEntity.LastUpdateDate)
                                    select c.Value).First();



        var userId = Convert.ToInt64((from c in userPrincipal.Claims
                                      where c.Type == nameof(UserEntity.Id)
                                      select c.Value).First());


        var userList = await _userRepository.Filter(x => x.Id == userId);
        if (userList.Count() != 0)
        {
            var userEntiy = userList.First();

            var lastUpdateInDb = userEntiy.LastUpdateDate?.ToString(ClaimDateFormat) ?? String.Empty;
            if (lastUpdateDateString != lastUpdateInDb)
            {

                context.RejectPrincipal();

                await context.HttpContext.SignOutAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme);

            }








        }
        else
        {
            context.RejectPrincipal();

            await context.HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);



        }

    }
}