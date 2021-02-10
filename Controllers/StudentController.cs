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
    public class StudentController : Controller
    {
        private readonly ILogger<StudentController> _logger;
        private DBSContext DB;

        public StudentController(ILogger<StudentController> logger)
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
                name = x.Name + " (" + x.Section + ")"
            }).OrderBy(x => x.name).ToList();
            return Json(_List);
        }

        public JsonResult GetStudentList()
        {
            DB = new DBSContext();
            var _List = (from S in DB.Students
            join C in DB.Classes on S.ClassId equals C.ClassId
            select new {
                id = S.StudentId,
                name = S.Name,
                roll = S.Roll,
                className = C.Name + " (" + C.Section + ")"
            }).OrderBy(x => x.className).OrderBy(x => x.name).ToList();
            return Json(_List);
        }

        public JsonResult GetStudentDetails(int Id)
        {
            DB = new DBSContext();
            var StudentObj = DB.Students.Where(x => x.StudentId == Id).Select(x => new {
                name = x.Name,
                classId = x.ClassId,
                roll = x.Roll,
                phone = x.Phone,
                email = x.Email
            }).First();
            return Json(StudentObj);
        }

        [HttpPost]
        public async Task<string> Save()
        {
            try
            {
                string PostObjectStr = await GetPostData();
                PO_Student_Save PostObject = JsonConvert.DeserializeObject<PO_Student_Save>(PostObjectStr);
                DB = new DBSContext();
                string Message = "";

                if (!IsValid(PostObject, out Message))
                {
                    return JsonConvert.SerializeObject(new { IsSuccess = false, Message = Message });
                }
                Student _Student = 0 == PostObject.Id ?  new Student() : DB.Students.Find(PostObject.Id);
                _Student.ClassId = PostObject.ClassId;
                _Student.Name = PostObject.Name;
                _Student.Roll = PostObject.Roll;
                _Student.Phone = PostObject.Phone;
                _Student.Email = PostObject.Email;

                if (0 == PostObject.Id)
                {
                    _Student.DateOfEntry = DateTime.Now;
                    DB.Students.Add(_Student);
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
                Student _Student = DB.Students.Find(Id);
                DB.Students.Remove(_Student);
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

        private bool IsValid(PO_Student_Save PostObject, out string Message)
        {
            Message = "";

            if (string.IsNullOrEmpty(PostObject.Name))
            {
                Message = "Name is Required!";
                return false;
            }
            if (0 == PostObject.ClassId)
            {
                Message = "Class is Required!";
                return false;
            }
            if (0 == PostObject.Roll)
            {
                Message = "Roll is Required!";
                return false;
            }
            if (0 == PostObject.Id && DB.Students.Where(x => x.Name == PostObject.Name).Count() > 0)
            {
                Message = "Duplicate Name Exists!";
                return false;
            }
            if (PostObject.Id > 0 && DB.Students.Where(x => x.Name == PostObject.Name && x.StudentId != PostObject.Id).Count() > 0)
            {
                Message = "Duplicate Name Exists!";
                return false;
            }
            if (0 == PostObject.Id && DB.Students.Where(x => x.ClassId == PostObject.ClassId && x.Roll == PostObject.Roll).Count() > 0)
            {
                Message = "Duplicate Roll Exists!";
                return false;
            }
            if (PostObject.Id > 0 && DB.Students.Where(x => x.ClassId == PostObject.ClassId && x.Roll == PostObject.Roll && x.StudentId != PostObject.Id).Count() > 0)
            {
                Message = "Duplicate Roll Exists!";
                return false;
            }
            return true;
        }
    }
}
