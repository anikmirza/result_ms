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
    public class ResultController : Controller
    {
        private readonly ILogger<ResultController> _logger;
        private DBSContext DB;

        public ResultController(ILogger<ResultController> logger)
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

        public JsonResult GetSubjectList()
        {
            DB = new DBSContext();
            var _List = DB.Subjects.Select(x => new {
                id = x.SubjectId,
                name = x.Name
            }).OrderBy(x => x.name).ToList();
            return Json(_List);
        }

        public JsonResult GetStudentList(int classId)
        {
            DB = new DBSContext();
            var _List = DB.Students.Where(x => x.ClassId == classId).Select(x => new {
                id = x.StudentId,
                name = x.Name
            }).OrderBy(x => x.name).ToList();
            return Json(_List);
        }

        public JsonResult GetResultList()
        {
            DB = new DBSContext();
            var _List = (from R in DB.Results
            join S in DB.Students on R.StudentId equals S.StudentId
            join C in DB.Classes on S.ClassId equals C.ClassId
            join SJ in DB.Subjects on R.SubjectId equals SJ.SubjectId
            select new {
                id = R.ResultId,
                className = C.Name + " (" + C.Section + ")",
                name = S.Name,
                subjectName = SJ.Name,
                mark = R.Mark
            }).OrderBy(x => x.className).ThenBy(x => x.name).ThenBy(x => x.subjectName).ToList();
            return Json(_List);
        }

        public JsonResult GetResultDetails(int Id)
        {
            DB = new DBSContext();
            var ResultObj = (from R in DB.Results
            join S in DB.Students on R.StudentId equals S.StudentId
            where R.ResultId == Id
            select new {
                classId = S.ClassId,
                studentId = R.StudentId,
                subjectId = R.SubjectId,
                mark = R.Mark
            }).First();
            return Json(ResultObj);
        }

        [HttpPost]
        public async Task<string> Save()
        {
            try
            {
                string PostObjectStr = await GetPostData();
                PO_Result_Save PostObject = JsonConvert.DeserializeObject<PO_Result_Save>(PostObjectStr);
                DB = new DBSContext();
                string Message = "";

                if (!IsValid(PostObject, out Message))
                {
                    return JsonConvert.SerializeObject(new { IsSuccess = false, Message = Message });
                }
                Result _Result = 0 == PostObject.Id ?  new Result() : DB.Results.Find(PostObject.Id);
                _Result.StudentId = PostObject.StudentId;
                _Result.SubjectId = PostObject.SubjectId;
                _Result.Mark = Math.Round(PostObject.Mark, 2);

                if (0 == PostObject.Id)
                {
                    _Result.DateOfEntry = DateTime.Now;
                    DB.Results.Add(_Result);
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
                Result _Result = DB.Results.Find(Id);
                DB.Results.Remove(_Result);
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

        private bool IsValid(PO_Result_Save PostObject, out string Message)
        {
            Message = "";

            if (0 == PostObject.StudentId)
            {
                Message = "Student is Required!";
                return false;
            }
            if (0 == PostObject.SubjectId)
            {
                Message = "Subject is Required!";
                return false;
            }
            if (0 == PostObject.Mark)
            {
                Message = "Mark is Required!";
                return false;
            }
            if (PostObject.Mark < 0)
            {
                Message = "Mark can not be negetive!";
                return false;
            }
            if (PostObject.Mark > 100)
            {
                Message = "Mark can not be greater than 100!";
                return false;
            }
            if (0 == PostObject.Id && DB.Results.Where(x => x.StudentId == PostObject.StudentId
                && x.SubjectId == PostObject.SubjectId).Count() > 0)
            {
                Message = "Duplicate Exists!";
                return false;
            }
            if (PostObject.Id > 0 && DB.Results.Where(x => x.StudentId == PostObject.StudentId
                && x.SubjectId == PostObject.SubjectId && x.ResultId != PostObject.Id).Count() > 0)
            {
                Message = "Duplicate Exists!";
                return false;
            }
            return true;
        }
    }
}
