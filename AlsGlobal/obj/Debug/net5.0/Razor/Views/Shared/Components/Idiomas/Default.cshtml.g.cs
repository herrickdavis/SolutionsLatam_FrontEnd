#pragma checksum "C:\Users\Urbano\Source\Repos\SolutionsLatam_FrontEnd\AlsGlobal\Views\Shared\Components\Idiomas\Default.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "83418da3d2f3a843498a7e80908bdb9712a92942"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AlsGlobal.Pages.Shared.Components.Idiomas.Views_Shared_Components_Idiomas_Default), @"mvc.1.0.view", @"/Views/Shared/Components/Idiomas/Default.cshtml")]
namespace AlsGlobal.Pages.Shared.Components.Idiomas
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
#line 1 "C:\Users\Urbano\Source\Repos\SolutionsLatam_FrontEnd\AlsGlobal\Views\_ViewImports.cshtml"
using AlsGlobal;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\Urbano\Source\Repos\SolutionsLatam_FrontEnd\AlsGlobal\Views\_ViewImports.cshtml"
using AlsGlobal.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\Urbano\Source\Repos\SolutionsLatam_FrontEnd\AlsGlobal\Views\_ViewImports.cshtml"
using AlsGlobal.Interfaces;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "C:\Users\Urbano\Source\Repos\SolutionsLatam_FrontEnd\AlsGlobal\Views\_ViewImports.cshtml"
using AlsGlobal.Extensions;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "C:\Users\Urbano\Source\Repos\SolutionsLatam_FrontEnd\AlsGlobal\Views\_ViewImports.cshtml"
using Microsoft.AspNetCore.Mvc.Localization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "C:\Users\Urbano\Source\Repos\SolutionsLatam_FrontEnd\AlsGlobal\Views\_ViewImports.cshtml"
using Microsoft.Extensions.Localization;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"83418da3d2f3a843498a7e80908bdb9712a92942", @"/Views/Shared/Components/Idiomas/Default.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"a3e129b3d79431b7b287e16db00937267f97714e", @"/Views/_ViewImports.cshtml")]
    #nullable restore
    public class Views_Shared_Components_Idiomas_Default : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    #nullable disable
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral(" <!--begin::Languages-->\r\n");
            WriteLiteral("<div class=\"dropdown mr-1\">\r\n    <!--begin::Toggle-->\r\n    <div class=\"topbar-item\" data-toggle=\"dropdown\" data-offset=\"10px,0px\">\r\n        <div class=\"btn btn-icon btn-clean btn-dropdown btn-lg\">\r\n            ");
#nullable restore
#line 7 "C:\Users\Urbano\Source\Repos\SolutionsLatam_FrontEnd\AlsGlobal\Views\Shared\Components\Idiomas\Default.cshtml"
       Write(Html.GetActualLanguage());

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
        </div>
    </div>
    <!--end::Toggle-->
    <!--begin::Dropdown-->
    <div class=""dropdown-menu p-0 m-0 dropdown-menu-anim-up dropdown-menu-sm"">
        <!--begin::Nav-->
        <ul class=""navi navi-hover py-4"">
            <!--begin::Item-->
            <li class=""navi-item"">
                <a href=""?culture=en-US"" class=""navi-link"">
                    <span class=""symbol symbol-20 mr-3"">
                        <img src=""/assets/media/svg/flags/226-united-states.svg""");
            BeginWriteAttribute("alt", " alt=\"", 820, "\"", 826, 0);
            EndWriteAttribute();
            WriteLiteral(" />\r\n                    </span>\r\n                    <span class=\"navi-text\">");
#nullable restore
#line 21 "C:\Users\Urbano\Source\Repos\SolutionsLatam_FrontEnd\AlsGlobal\Views\Shared\Components\Idiomas\Default.cshtml"
                                       Write(SharedLocalizer["TraduccionIngles"]);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"</span>
                </a>
            </li>
            <!--end::Item-->
            <!--begin::Item-->
            <li class=""navi-item active"">
                <a href=""?culture=es-PE"" class=""navi-link"">
                    <span class=""symbol symbol-20 mr-3"">
                        <img src=""/assets/media/svg/flags/188-peru.svg""");
            BeginWriteAttribute("alt", " alt=\"", 1286, "\"", 1292, 0);
            EndWriteAttribute();
            WriteLiteral(" />\r\n                    </span>\r\n                    <span class=\"navi-text\">");
#nullable restore
#line 31 "C:\Users\Urbano\Source\Repos\SolutionsLatam_FrontEnd\AlsGlobal\Views\Shared\Components\Idiomas\Default.cshtml"
                                       Write(SharedLocalizer["TraduccionEspanol"]);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"</span>
                </a>
            </li>
            <!--end::Item-->
            <!--begin::Item-->
            <li class=""navi-item"">
                <a href=""?culture=pt-BR"" class=""navi-link"">
                    <span class=""symbol symbol-20 mr-3"">
                        <img src=""/assets/media/svg/flags/255-brazil.svg""");
            BeginWriteAttribute("alt", " alt=\"", 1748, "\"", 1754, 0);
            EndWriteAttribute();
            WriteLiteral(" />\r\n                    </span>\r\n                    <span class=\"navi-text\">");
#nullable restore
#line 41 "C:\Users\Urbano\Source\Repos\SolutionsLatam_FrontEnd\AlsGlobal\Views\Shared\Components\Idiomas\Default.cshtml"
                                       Write(SharedLocalizer["TraduccionPortugues"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</span>\r\n                </a>\r\n            </li>\r\n            <!--end::Item-->\r\n        </ul>\r\n        <!--end::Nav-->\r\n    </div>\r\n    <!--end::Dropdown-->\r\n</div>\r\n<!--end::Languages-->");
        }
        #pragma warning restore 1998
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public IStringLocalizer<SharedResources> SharedLocalizer { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; } = default!;
        #nullable disable
    }
}
#pragma warning restore 1591
