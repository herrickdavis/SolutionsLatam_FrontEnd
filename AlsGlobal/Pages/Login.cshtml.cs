using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using AlsGlobal.Models;
using AlsGlobal.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AlsGlobal.Pages
{
  public class LoginModel : PageModel
  {
    private readonly IAutenticacionService _autenticacionService;
    private readonly IHttpContextAccessor _httpContext;
    public LoginModel(IAutenticacionService autenticacionService,
                    IHttpContextAccessor httpContext)
    {
      _autenticacionService = autenticacionService;
      _httpContext = httpContext;
    }
    [BindProperty]
    public string returnUrl { get; set; }
    public void OnGet(string returnUrl = null)
    {
      this.returnUrl = returnUrl;
    }
    [BindProperty]
    public LoginViewModel Usuario { get; set; }
    public async Task<IActionResult> OnPostAsync()
    {
      if (!ModelState.IsValid)
      {
        return Page();
      }
      var response = await _autenticacionService.Login(Usuario);

      if (ContieneError(response.ResponseResult)) return Page();

      await RealizarLogin(response);

      if (string.IsNullOrEmpty(returnUrl)) return RedirectToAction("Index", "Muestras");

      return LocalRedirect(returnUrl);
    }

    public async Task<IActionResult> OnGetSalir()
    {
      await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
      return RedirectToPage("/Login");
    }
    private async Task RealizarLogin(UsuarioRespuestaViewModel response)
    {
      var claims = new List<Claim>();
      claims.Add(new Claim("Token", response.token));
      claims.Add(new Claim(ClaimTypes.NameIdentifier, response.nombre));
      claims.Add(new Claim("Menu", JsonSerializer.Serialize(response.menu)));
      claims.Add(new Claim(ClaimTypes.Email, response.email));
      claims.Add(new Claim("data_campo", response.data_campo));
      claims.Add(new Claim(ClaimTypes.Role, response.rol.ToString()));
      claims.Add(new Claim("rol", response.rol.ToString()));
      var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

      var authProperties = new AuthenticationProperties
      {
        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60),
        IsPersistent = true
      };

      await HttpContext.SignInAsync(
          CookieAuthenticationDefaults.AuthenticationScheme,
          new ClaimsPrincipal(claimsIdentity),
          authProperties);
    }
    protected bool ContieneError(ResponseResult response)
    {
      if (response != null && response.Errors.Message.Any())
      {
        foreach (var message in response.Errors.Message)
        {
          ModelState.AddModelError(string.Empty, message);
        }

        return true;
      }

      return false;
    }
  }
}
