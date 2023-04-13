using AlsGlobal.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Json;
namespace AlsGlobal.Extensions
{
  public static class ClaimsPrincipalExtension
  {
    public static string GetName(this ClaimsPrincipal principal)
    {
      if (principal == null)
      {
        throw new ArgumentException(nameof(principal));
      }

      var claim = principal.FindFirst(ClaimTypes.NameIdentifier);
      return claim?.Value;
    }
    public static IEnumerable<MenuViewModel> GetMenu(this ClaimsPrincipal principal)
    {
      if (principal == null)
      {
        throw new ArgumentException(nameof(principal));
      }

      var claim = principal.FindFirst("Menu");
      if (claim != null)
      {
        return JsonSerializer.Deserialize<IEnumerable<MenuViewModel>>(claim.Value);
      }
      return null;
    }
    public static string GetToken(this ClaimsPrincipal principal)
    {
      if (principal == null)
      {
        throw new ArgumentException(nameof(principal));
      }

      var claim = principal.FindFirst("Token");
      return claim?.Value;
    }
    public static string GetEmail(this ClaimsPrincipal principal)
    {
      if (principal == null)
      {
        throw new ArgumentException(nameof(principal));
      }

      var claim = principal.FindFirst(ClaimTypes.Email);
      return claim?.Value;
    }
    public static string Get_data_campo(this ClaimsPrincipal principal)
    {
      if (principal == null)
      {
        throw new ArgumentException(nameof(principal));
      }

      var claim = principal.FindFirst("data_campo");
      return claim?.Value;
    }
    public static int GetRol(this ClaimsPrincipal principal)
    {
      if (principal == null)
      {
        throw new ArgumentException(nameof(principal));
      }

      var claim = principal.FindFirst("rol");
      return claim?.Value != null ? int.Parse(claim.Value) : 0;
    }
    
  }
}
