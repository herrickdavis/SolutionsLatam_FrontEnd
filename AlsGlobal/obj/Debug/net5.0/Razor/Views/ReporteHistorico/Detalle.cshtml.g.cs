#pragma checksum "H:\Trabajos\ASLGlobal\AlsGlobalGit\AlsGlobal\Views\ReporteHistorico\Detalle.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "07e2f45d1f3a909215836cba698226f7c10915b2"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AlsGlobal.Pages.ReporteHistorico.Views_ReporteHistorico_Detalle), @"mvc.1.0.view", @"/Views/ReporteHistorico/Detalle.cshtml")]
namespace AlsGlobal.Pages.ReporteHistorico
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "H:\Trabajos\ASLGlobal\AlsGlobalGit\AlsGlobal\Views\_ViewImports.cshtml"
using AlsGlobal;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "H:\Trabajos\ASLGlobal\AlsGlobalGit\AlsGlobal\Views\_ViewImports.cshtml"
using AlsGlobal.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "H:\Trabajos\ASLGlobal\AlsGlobalGit\AlsGlobal\Views\_ViewImports.cshtml"
using AlsGlobal.Interfaces;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "H:\Trabajos\ASLGlobal\AlsGlobalGit\AlsGlobal\Views\_ViewImports.cshtml"
using AlsGlobal.Extensions;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "H:\Trabajos\ASLGlobal\AlsGlobalGit\AlsGlobal\Views\_ViewImports.cshtml"
using Microsoft.AspNetCore.Mvc.Localization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "H:\Trabajos\ASLGlobal\AlsGlobalGit\AlsGlobal\Views\_ViewImports.cshtml"
using Microsoft.Extensions.Localization;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"07e2f45d1f3a909215836cba698226f7c10915b2", @"/Views/ReporteHistorico/Detalle.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"a3e129b3d79431b7b287e16db00937267f97714e", @"/Views/_ViewImports.cshtml")]
    public class Views_ReporteHistorico_Detalle : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<ReporteHistoricoRequestViewModel>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", "/js/reporteHistorico/detalleReporteHistorico.js", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.ScriptTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_ScriptTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 3 "H:\Trabajos\ASLGlobal\AlsGlobalGit\AlsGlobal\Views\ReporteHistorico\Detalle.cshtml"
   
  var TipoMuestras = (IEnumerable<SelectListItem>)ViewBag.TipoMuestras;

#line default
#line hidden
#nullable disable
            WriteLiteral("<div class=\"row\">\r\n    <div class=\"col-md-5 mb-5\">\r\n        <div class=\"card card-custom\">\r\n            <div class=\"card-header\">\r\n                <div class=\"card-title\">\r\n                    <h3 class=\"card-label\">");
#nullable restore
#line 11 "H:\Trabajos\ASLGlobal\AlsGlobalGit\AlsGlobal\Views\ReporteHistorico\Detalle.cshtml"
                                      Write(SharedLocalizer["ReporteHistorico.TituloReporteHistorico"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h3>\r\n                </div>\r\n            </div>\r\n            <div class=\"card-body\">\r\n                <div class=\"row\">\r\n                    <div class=\"col-md-12\">\r\n                        <div class=\"form-group\">\r\n                            <label> ");
#nullable restore
#line 18 "H:\Trabajos\ASLGlobal\AlsGlobalGit\AlsGlobal\Views\ReporteHistorico\Detalle.cshtml"
                               Write(SharedLocalizer["TipoMuestras"]);

#line default
#line hidden
#nullable disable
            WriteLiteral(" </label>\r\n                            ");
#nullable restore
#line 19 "H:\Trabajos\ASLGlobal\AlsGlobalGit\AlsGlobal\Views\ReporteHistorico\Detalle.cshtml"
                       Write(Html.DropDownList("id_tipo_muestra", TipoMuestras, new
                           {
                                @class = "form-control selectpicker",
                               @data_size = "7",
                               @data_live_search = "true",
                               @title = SharedLocalizer["ValorDefectoSelect"]
                            }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\r\n                        </div>\r\n                    </div>\r\n                    <div class=\"col-md-12\">\r\n                        <div class=\"form-group\">\r\n                            <label> ");
#nullable restore
#line 31 "H:\Trabajos\ASLGlobal\AlsGlobalGit\AlsGlobal\Views\ReporteHistorico\Detalle.cshtml"
                               Write(SharedLocalizer["Proyecto"]);

#line default
#line hidden
#nullable disable
            WriteLiteral(" </label>\r\n                            <select id=\"id_proyecto\" class=\"form-control selectpicker\" data-size=\"5\" data-live-search=\"true\" data-hide-disabled=\"true\" data-show-selectAll=\"true\" data-actions-box=\"true\" multiple=\"multiple\"");
            BeginWriteAttribute("title", " title=", 1613, "", 1658, 1);
#nullable restore
#line 32 "H:\Trabajos\ASLGlobal\AlsGlobalGit\AlsGlobal\Views\ReporteHistorico\Detalle.cshtml"
WriteAttributeValue("", 1620, SharedLocalizer["ValorDefectoSelect"], 1620, 38, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">\r\n                            </select>\r\n                        </div>\r\n                    </div>\r\n                    <div class=\"col-md-12\">\r\n                        <div class=\"form-group\">\r\n                            <label> ");
#nullable restore
#line 38 "H:\Trabajos\ASLGlobal\AlsGlobalGit\AlsGlobal\Views\ReporteHistorico\Detalle.cshtml"
                               Write(SharedLocalizer["Estaciones"]);

#line default
#line hidden
#nullable disable
            WriteLiteral(" </label>\r\n                            <select id=\"id_estacion\" class=\"form-control selectpicker\" data-size=\"5\" data-hide-disabled=\"true\" data-actions-box=\"true\" multiple=\"multiple\" data-live-search=\"true\"");
            BeginWriteAttribute("title", " title=", 2126, "", 2171, 1);
#nullable restore
#line 39 "H:\Trabajos\ASLGlobal\AlsGlobalGit\AlsGlobal\Views\ReporteHistorico\Detalle.cshtml"
WriteAttributeValue("", 2133, SharedLocalizer["ValorDefectoSelect"], 2133, 38, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">\r\n                            </select>\r\n                        </div>\r\n                    </div>\r\n                    <div class=\"col-md-12\" id=\"divNormas\">\r\n                        <div class=\"form-group\">\r\n                            <label> ");
#nullable restore
#line 45 "H:\Trabajos\ASLGlobal\AlsGlobalGit\AlsGlobal\Views\ReporteHistorico\Detalle.cshtml"
                               Write(SharedLocalizer["Normas"]);

#line default
#line hidden
#nullable disable
            WriteLiteral(" </label>\r\n                            <select id=\"id_limite\" class=\"form-control selectpicker\" data-size=\"5\" data-live-search=\"true\"");
            BeginWriteAttribute("title", " title=", 2578, "", 2623, 1);
#nullable restore
#line 46 "H:\Trabajos\ASLGlobal\AlsGlobalGit\AlsGlobal\Views\ReporteHistorico\Detalle.cshtml"
WriteAttributeValue("", 2585, SharedLocalizer["ValorDefectoSelect"], 2585, 38, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">\r\n                            </select>\r\n                        </div>\r\n                    </div>\r\n                    <div class=\"col-md-12\">\r\n                        <div class=\"form-group\">\r\n                            <label> ");
#nullable restore
#line 52 "H:\Trabajos\ASLGlobal\AlsGlobalGit\AlsGlobal\Views\ReporteHistorico\Detalle.cshtml"
                               Write(SharedLocalizer["Parametros"]);

#line default
#line hidden
#nullable disable
            WriteLiteral(" </label>\r\n                            <select id=\"id_parametro\" class=\"form-control selectpicker\" data-size=\"5\"  data-hide-disabled=\"true\" data-actions-box=\"true\" multiple=\"multiple\" data-live-search=\"true\"");
            BeginWriteAttribute("title", " title=", 3093, "", 3138, 1);
#nullable restore
#line 53 "H:\Trabajos\ASLGlobal\AlsGlobalGit\AlsGlobal\Views\ReporteHistorico\Detalle.cshtml"
WriteAttributeValue("", 3100, SharedLocalizer["ValorDefectoSelect"], 3100, 38, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(@">
                            </select>
                        </div>
                    </div>
                    <div class=""col-md-12"">
                        <div class=""btn-group"" role=""group"" aria-label=""Basic example"">
                            <button class=""btn btn-info btn-lg btn-block"">
                                <span class=""fas fa-search""></span> ");
#nullable restore
#line 60 "H:\Trabajos\ASLGlobal\AlsGlobalGit\AlsGlobal\Views\ReporteHistorico\Detalle.cshtml"
                                                               Write(SharedLocalizer["BotonBuscar"]);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
                            </button>
                            <button type=""button"" class=""btn btn-success""><i class=""flaticon-download""></i></button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class=""col-md-7"">
        <div class=""card card-custom"">
            <div class=""card-header"">
                <div class=""card-title"">
                    <h3 class=""card-label"">");
#nullable restore
#line 73 "H:\Trabajos\ASLGlobal\AlsGlobalGit\AlsGlobal\Views\ReporteHistorico\Detalle.cshtml"
                                      Write(SharedLocalizer["ReporteHistorico.Grafico"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h3>\r\n                </div>\r\n            </div>\r\n            <div class=\"card-body\" id=\"divGrafico\">\r\n\r\n            </div>\r\n        </div>\r\n    </div>\r\n</div>\r\n");
            DefineSection("Scripts", async() => {
                WriteLiteral("\r\n    <script>\r\n        var urlGetTipoMuestras = \"");
#nullable restore
#line 84 "H:\Trabajos\ASLGlobal\AlsGlobalGit\AlsGlobal\Views\ReporteHistorico\Detalle.cshtml"
                             Write(Url.Action("GetTipoMuestras", "ReporteHistorico"));

#line default
#line hidden
#nullable disable
                WriteLiteral("\"\r\n        var urlObtenerEstaciones = \"");
#nullable restore
#line 85 "H:\Trabajos\ASLGlobal\AlsGlobalGit\AlsGlobal\Views\ReporteHistorico\Detalle.cshtml"
                               Write(Url.Action("ObtenerEstaciones", "ReporteHistorico"));

#line default
#line hidden
#nullable disable
                WriteLiteral("\";\r\n        var urlObtenerEstacionesProyectosYLimites = \"");
#nullable restore
#line 86 "H:\Trabajos\ASLGlobal\AlsGlobalGit\AlsGlobal\Views\ReporteHistorico\Detalle.cshtml"
                                                Write(Url.Action("ObtenerEstacionesProyectosYLimites", "ReporteHistorico"));

#line default
#line hidden
#nullable disable
                WriteLiteral("\";\r\n        var urlObtenerParametros = \"");
#nullable restore
#line 87 "H:\Trabajos\ASLGlobal\AlsGlobalGit\AlsGlobal\Views\ReporteHistorico\Detalle.cshtml"
                               Write(Url.Action("ObtenerParametros", "ReporteHistorico"));

#line default
#line hidden
#nullable disable
                WriteLiteral("\";\r\n        var urlObtenerGrafico = \"");
#nullable restore
#line 88 "H:\Trabajos\ASLGlobal\AlsGlobalGit\AlsGlobal\Views\ReporteHistorico\Detalle.cshtml"
                            Write(Url.Action("ObtenerGraficos", "ReporteHistorico"));

#line default
#line hidden
#nullable disable
                WriteLiteral("\";\r\n        var urlGetDataHistoricaExcel = \"");
#nullable restore
#line 89 "H:\Trabajos\ASLGlobal\AlsGlobalGit\AlsGlobal\Views\ReporteHistorico\Detalle.cshtml"
                                   Write(Url.Action("GetDataHistoricaExcel", "ReporteHistorico"));

#line default
#line hidden
#nullable disable
                WriteLiteral("\";\r\n    </script>\r\n    ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "07e2f45d1f3a909215836cba698226f7c10915b214496", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_ScriptTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.ScriptTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_ScriptTagHelper);
                __Microsoft_AspNetCore_Mvc_TagHelpers_ScriptTagHelper.Src = (string)__tagHelperAttribute_0.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
#nullable restore
#line 91 "H:\Trabajos\ASLGlobal\AlsGlobalGit\AlsGlobal\Views\ReporteHistorico\Detalle.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_ScriptTagHelper.AppendVersion = true;

#line default
#line hidden
#nullable disable
                __tagHelperExecutionContext.AddTagHelperAttribute("asp-append-version", __Microsoft_AspNetCore_Mvc_TagHelpers_ScriptTagHelper.AppendVersion, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n");
            }
            );
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public IStringLocalizer<SharedResources> SharedLocalizer { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<ReporteHistoricoRequestViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
