using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace AlsGlobal.Extensions
{
  public static class RazorHelper
  {
    public static IHtmlContent GetActualLanguage(this IHtmlHelper html)
    {
      var currentCulture = CultureInfo.CurrentCulture.Name;
      string urlImage = "/assets/media/svg/flags/188-peru.svg";
      switch (currentCulture)
      {
        case "en-US": urlImage = "/assets/media/svg/flags/226-united-states.svg"; break;
        case "pt-BR": urlImage = "/assets/media/svg/flags/255-brazil.svg"; break;
        default:
          urlImage = "/assets/media/svg/flags/188-peru.svg";break;
      }
      return new HtmlString($"<img class=\"h-20px w-20px rounded-sm\" src=\"{urlImage}\" alt=\"\" />");
    }
    public static IHtmlContent GetIcon(this IHtmlHelper html, string extension)
    {
      string className = "far fa-file";
      switch(extension){
        case "jpg":
        case "png":
        case "gif":
        case "jpeg":
          className = "far fa-file-image text-info";break;
        case "pdf":
          className = "far fa-file-pdf text-danger";break;
        case "xlsx":
        case "xlx":
          className = "far fa-file-excel text-success";break;
        case "ppt":
          className ="far fa-file-powerpoint text-danger";break;
        case "docx":
          className = "far fa-file-word text-info";break;
      }
      return new HtmlString($"<i class=\"{className}\" />");
    }
    public static string AddFormatDate(this RazorPage page, string texto)
    {
      DateTime date = DateTime.UtcNow;
      if (texto !=null && texto.Contains("/") && texto.Contains(":") && texto.Length > 8)
      {
        if (DateTime.TryParse(texto, out date))
        {
          return date.ToString("yyyy/MM/dd HH:mm");
        }
      }
      return texto;
    }
    public static IHtmlContent ShowStatus(this IHtmlHelper page, string activo, string textoActivo, string textoInactivo)
    { 
      activo = activo ?? "S";
      var @class = new Dictionary<string, string>(){
        {"N", "label-danger"},
        {"S", "label-primary"}
      };
      var @text = activo == "S" ? textoActivo : textoInactivo;
      return new HtmlString($"<span class=\"label {@class[activo]} label-pill label-inline mr-2\">{@text}</span>");
    }
  }
}
