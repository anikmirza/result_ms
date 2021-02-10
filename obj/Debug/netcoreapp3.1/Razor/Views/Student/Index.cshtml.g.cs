#pragma checksum "/home/anik/My Files/NETCoreProjects/result_ms/Views/Student/Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "cb83679ef72c659cfa58d993480302e23044e3ce"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Student_Index), @"mvc.1.0.view", @"/Views/Student/Index.cshtml")]
namespace AspNetCore
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
#line 1 "/home/anik/My Files/NETCoreProjects/result_ms/Views/_ViewImports.cshtml"
using result_ms;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "/home/anik/My Files/NETCoreProjects/result_ms/Views/_ViewImports.cshtml"
using result_ms.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"cb83679ef72c659cfa58d993480302e23044e3ce", @"/Views/Student/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"91d0e3b62edfc0640e5bc0ac47c3b7b62f1323bb", @"/Views/_ViewImports.cshtml")]
    public class Views_Student_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
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
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 1 "/home/anik/My Files/NETCoreProjects/result_ms/Views/Student/Index.cshtml"
  
    ViewData["Title"] = "Student Setup";

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
<div class=""text-center"">
    <h1 class=""display-6"">Student Setup</h1>
    <hr/>
    <div class=""row"">
        <div class=""col-md-6"">
            <div class=""form-group"">
                <div class=""row"">
                    <div class=""col-md-3"">
                        <label for=""name"">Name</label>
                    </div>
                    <div class=""col-md-9"">
                        <input id=""name"" type=""text"" class=""form-control"">
                    </div>
                </div>
            </div>
            <div class=""form-group"">
                <div class=""row"">
                    <div class=""col-md-3"">
                        <label for=""class"">Class</label>
                    </div>
                    <div class=""col-md-9"">
                        <select id=""class"" class=""form-control"">
                            ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "cb83679ef72c659cfa58d993480302e23044e3ce4096", async() => {
                WriteLiteral("Loading...");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral(@"
                        </select>
                    </div>
                </div>
            </div>
            <div class=""form-group"">
                <div class=""row"">
                    <div class=""col-md-3"">
                        <label for=""roll"">Roll</label>
                    </div>
                    <div class=""col-md-9"">
                        <input id=""roll"" type=""text"" class=""form-control"">
                    </div>
                </div>
            </div>
            <div class=""form-group"">
                <div class=""row"">
                    <div class=""col-md-3"">
                        <label for=""email"">Email</label>
                    </div>
                    <div class=""col-md-9"">
                        <input id=""email"" type=""text"" class=""form-control"">
                    </div>
                </div>
            </div>
            <div class=""form-group"">
                <div class=""row"">
                    <div class=""col-md-3"">
         ");
            WriteLiteral(@"               <label for=""phone"">phone</label>
                    </div>
                    <div class=""col-md-9"">
                        <input id=""phone"" type=""text"" class=""form-control"">
                    </div>
                </div>
            </div>
            <div>
                <button id=""Save"" class=""btn btn-primary"" style=""width: 100px;"" onclick=""page.save()""><i class=""fa fa-floppy-o""></i> Save</button>
                <button class=""btn btn-success"" style=""width: 100px;"" onclick=""page.clear()""><i class=""fa fa-recycle""></i> Clear</button>
            </div>
        </div>
        <div class=""col-md-6"">
            <table id=""student-list"" class=""table table-striped table-hover"">
                <thead>
                    <tr>
                        <th>Name</th>
                        <th>Roll</th>
                        <th colspan=""3"">Class</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                ");
            WriteLiteral("        <td colspan=\"5\">Loading...</td>\r\n                    </tr>\r\n                </tbody>\r\n            </table>\r\n        </div>\r\n    </div>\r\n</div>\r\n\r\n");
            DefineSection("Styles", async() => {
                WriteLiteral("\r\n    <link rel=\"stylesheet\" href=\"/lib/Toastr/toastr.css\" />\r\n    <link rel=\"stylesheet\" href=\"/lib/Toastr/toastr-responsive.css\" />\r\n    <link rel=\"stylesheet\" href=\"/css/setup.css\" />\r\n");
            }
            );
            WriteLiteral("\r\n");
            DefineSection("Scripts", async() => {
                WriteLiteral("\r\n    <script src=\"/lib/Toastr/toastr.js\"></script>\r\n    <script src=\"/js/student.js\"></script>\r\n");
            }
            );
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591