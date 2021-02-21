using System;
using System.Collections.Generic;
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
    public class SubjectController : Controller
    {
        private readonly ILogger<SubjectController> _logger;
        private DBSContext DB;

        public SubjectController(ILogger<SubjectController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public JsonResult GetSubjectList()
        {
            DB = new DBSContext();
            var _List = 
            DB.Subjects.Select(x => new {
                id = x.SubjectId,
                name = x.Name
            }).OrderBy(x => x.name).ToList();
            return Json(_List);
        }

        [HttpPost]
        public async Task<string> Save()
        {
            try
            {
                string PostObjectStr = await GetPostData();
                PO_Subject_Save PostObject = JsonConvert.DeserializeObject<PO_Subject_Save>(PostObjectStr);
                DB = new DBSContext();
                string Message = "";

                if (!IsValid(PostObject, out Message))
                {
                    return JsonConvert.SerializeObject(new { IsSuccess = false, Message = Message });
                }
                Subject _Subject = 0 == PostObject.Id ?  new Subject() : DB.Subjects.Find(PostObject.Id);
                _Subject.Name = PostObject.Name;

                if (0 == PostObject.Id)
                {
                    _Subject.DateOfEntry = DateTime.Now;
                    DB.Subjects.Add(_Subject);
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
                Subject _Subject = DB.Subjects.Find(Id);
                DB.Subjects.Remove(_Subject);
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

        private bool IsValid(PO_Subject_Save PostObject, out string Message)
        {
            Message = "";

            if (string.IsNullOrEmpty(PostObject.Name))
            {
                Message = "Name is Required!";
                return false;
            }
            if (0 == PostObject.Id && DB.Subjects.Where(x => x.Name == PostObject.Name).Count() > 0)
            {
                Message = "Duplicate Name Exists!";
                return false;
            }
            if (PostObject.Id > 0 && DB.Subjects.Where(x => x.Name == PostObject.Name && x.SubjectId != PostObject.Id).Count() > 0)
            {
                Message = "Duplicate Name Exists!";
                return false;
            }
            return true;
        }
    }
}
