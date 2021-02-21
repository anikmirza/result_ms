using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using result_ms.Models;
using result_ms.PostObject;
using Newtonsoft.Json;
using result_ms.Helper;

namespace result_ms.Controllers
{
    public class ClassController : Controller
    {
        private readonly ILogger<ClassController> _logger;
        private DBSContext DB;

        public ClassController(ILogger<ClassController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public JsonResult GetClassList()
        {
            DB = new DBSContext();
            var _List = DB.Classes.Select(x => new {
                id = x.ClassId,
                name = x.Name,
                section = x.Section
            }).OrderBy(x => x.name).ThenBy(x => x.section).ToList();
            return Json(_List);
        }

        [HttpPost]
        public async Task<string> Save()
        {
            try
            {
                string PostObjectStr = await GetPostData();
                PO_Class_Save PostObject = JsonConvert.DeserializeObject<PO_Class_Save>(PostObjectStr);
                DB = new DBSContext();
                string Message = "";

                if (!IsValid(PostObject, out Message))
                {
                    return JsonConvert.SerializeObject(new { IsSuccess = false, Message = Message });
                }
                Class _Class = 0 == PostObject.Id ?  new Class() : DB.Classes.Find(PostObject.Id);
                _Class.Name = PostObject.Name;
                _Class.Section = PostObject.Section;

                if (0 == PostObject.Id)
                {
                    _Class.DateOfEntry = DateTime.Now;
                    DB.Classes.Add(_Class);
                }
                DB.SaveChanges();
                return JsonConvert.SerializeObject(new { IsSuccess = true });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new {
                    IsSuccess = false,
                    Message = Common.GetError(ex)
                });
            }
        }

        [HttpPost]
        public async Task<string> Delete()
        {
            try
            {
                string PostObjectStr = await GetPostData();
                int Id = JsonConvert.DeserializeObject<int>(PostObjectStr);
                DB = new DBSContext();
                Class _Class = DB.Classes.Find(Id);
                DB.Classes.Remove(_Class);
                DB.SaveChanges();
                return JsonConvert.SerializeObject(new { IsSuccess = true });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new {
                    IsSuccess = false,
                    Message = Common.GetError(ex)
                });
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async Task<string> GetPostData()
        {
            string Str = "";

            using (System.IO.StreamReader reader = new System.IO.StreamReader(Request.Body))
            {
                Str = await reader.ReadToEndAsync();
            }
            return Str;
        }

        private bool IsValid(PO_Class_Save PostObject, out string Message)
        {
            Message = "";

            if (string.IsNullOrEmpty(PostObject.Name))
            {
                Message = "Name is Required!";
                return false;
            }
            if (string.IsNullOrEmpty(PostObject.Section))
            {
                Message = "Section is Required!";
                return false;
            }
            if (0 == PostObject.Id && DB.Classes.Where(x => x.Name == PostObject.Name).Count() > 0)
            {
                Message = "Duplicate Name Exists!";
                return false;
            }
            if (PostObject.Id > 0 && DB.Classes.Where(x => x.Name == PostObject.Name && x.ClassId != PostObject.Id).Count() > 0)
            {
                Message = "Duplicate Name Exists!";
                return false;
            }
            return true;
        }
    }
}
