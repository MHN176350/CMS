using Microsoft.AspNetCore.Mvc;
using ProjectPRN.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using System.Reflection.Metadata.Ecma335;

namespace ProjectPRN.Controllers
{
    public static class SessionExtensions
    {
        public static void SetObject(this ISession ses, string key, object value)
        {
            ses.SetString(key, JsonConvert.SerializeObject(value));
        }
        public static T GetObject<T>(this ISession ses, string key)
        {
            var value = ses.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        prn211Context ctx = new prn211Context();
        public IActionResult Index()
        {
            Student s = HttpContext.Session.GetObject<Student>("us");
            if (s == null)
            {
                return View();
            }
            else
            {

                ViewBag.st = s.DisplayName.ToString();
                Console.Write(s.DisplayName);
                return View(ctx.Courses.ToList());
            }
        }
        public IActionResult Detail(String id)
        {
            Student s = HttpContext.Session.GetObject<Student>("us");
            ViewBag.name = s.DisplayName;
            if (ctx.Enrollments.FirstOrDefault(e => e.CId == Int32.Parse(id) && e.SId == s.Id && e.Status == 1) != null)
            {
                ViewBag.er = 1;
            }
            else
            {
                ViewBag.er = 0;
            }
            ViewBag.Lecture = ctx.Lectures.ToList();
            Course c = ctx.Courses.FirstOrDefault(c => c.Id == Int32.Parse(id));
            return View(c);
        }
        public IActionResult Subjects()
        {
            Student s = HttpContext.Session.GetObject<Student>("us");
            if (s != null)
            {
                ViewBag.name = s.DisplayName;
            }
           
            ViewBag.Lecture = ctx.Lectures.ToList();
            List<Course> c = ctx.Courses.ToList();
            return View(c);
        }
        public IActionResult Login(String msg)
        {
            if (msg == null)
            {
                return View();
            }
            else
            {
                ViewBag.msg = msg;
                return View();
            }
        }
        private MemoryStream DownloadFile(String filename, String uploadPath)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), uploadPath, filename);
            var memory = new MemoryStream();
            if (System.IO.File.Exists(path))
            {
                var net = new System.Net.WebClient();
                var Data = net.DownloadData(path);
                var Content = new System.IO.MemoryStream(Data);
                memory = Content;
            }
            memory.Position = 0;
            return memory;
        }
        public IActionResult Download(String fn)
        {
            var memory = this.DownloadFile(fn, "wwwroot\\Document");
            return File(memory.ToArray(), "application/vnd.ms-word", fn);
        }
        public IActionResult DownloadSubmit(String fn, String c)
        {
            Course course = ctx.Courses.FirstOrDefault(cou => cou.Id == Int32.Parse(c));
            Student s = HttpContext.Session.GetObject<Student>("us");
            var memory = this.DownloadFile(fn, "wwwroot\\Assignment\\"+ course.Code + "\\" + s.Email);
            return File(memory.ToArray(), "application/vnd.ms-word", fn);
        }
        public IActionResult Enroll(String id)
        {
            Student s = HttpContext.Session.GetObject<Student>("us");
            if (s != null)
            {
                if (ctx.Enrollments.FirstOrDefault(e => e.SId == s.Id && e.CId == Int32.Parse(id)) == null)
                {
                    Enrollment e = new Enrollment();
                    e.CId = Int32.Parse(id);
                    e.SId = s.Id;
                    
                    ctx.Enrollments.Add(e);
                    ctx.SaveChanges();
                    Progress p = new Progress();
                    p.EId = ctx.Enrollments.FirstOrDefault(en => en.CId == Int32.Parse(id) && en.SId == s.Id).Id;
                    ctx.Progresses.Add(p);
                    ctx.SaveChanges();
                }
                else
                {
                    Enrollment e = ctx.Enrollments.FirstOrDefault(e => e.SId == s.Id && e.CId == Int32.Parse(id));
                    e.Status = 1;
                    ctx.SaveChanges();
                }
            }
            return RedirectToAction("Subjects");
        }
        public IActionResult MyCourse(int id)
        {

            Student s = HttpContext.Session.GetObject<Student>("us");
            List<Enrollment> ls = ctx.Enrollments.Where(e => e.SId == s.Id && e.Status == 1).ToList();
            List<Course> list = new List<Course>();
            for (int i = 0; i < ls.Count; i++)
            {
                if (ctx.Courses.FirstOrDefault(c => c.Id == ls[i].CId) != null)
                {
                    list.Add(ctx.Courses.FirstOrDefault(c => c.Id == ls[i].CId));
                }

            }
            
            ViewBag.name = s.DisplayName;
            ViewBag.sid = s.Id;
            ViewBag.cur = id;
            ViewBag.Course = list;
            if (id != 0)
            {
                Enrollment e = ctx.Enrollments.FirstOrDefault(e => e.SId == s.Id && e.CId == id);
                ViewBag.std = e.EDate.Value;
                ViewBag.prog = ctx.Progresses.Where(p => p.EId == e.Id).ToList();
                ViewBag.end = e.EDate.Value.AddDays(2).ToString("yyyy-MM-ddTHH:mm:ss");
                if (DateTime.Now >= e.EDate.Value.AddDays(2))
                {
                    e.Ovd = 1;
                    ctx.SaveChanges();
                }
                ViewBag.eid = e.Id;
                CourseContent ct = ctx.CourseContents.FirstOrDefault(ct => ct.CId == id);
                return View(ct);

            }

            return View();
        }
        public IActionResult Unroll(String id)
        {
            Student s = HttpContext.Session.GetObject<Student>("us");
            if (s != null)
            {

                Enrollment e = ctx.Enrollments.FirstOrDefault(e => e.SId == s.Id && e.CId == Int32.Parse(id));
                e.Status = 0;
                ctx.SaveChanges();
            }
            return RedirectToAction("Subjects");
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpPost]
        public IActionResult Login(Student s)
        {
            Student stu = ctx.Students.FirstOrDefault(stu => stu.Email.Equals(s.Email) && stu.Password.Equals(s.Password));
            if (stu == null)
            {
                ViewBag.msg = "Invalid Username or Password";
                return View();
            }
            else
            {
                HttpContext.Session.SetObject("us", stu);
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public IActionResult Subjects(String kw)
        {
            Student s = HttpContext.Session.GetObject<Student>("us");
            ViewBag.name = s.DisplayName;
            ViewBag.Lecture = ctx.Lectures.ToList();
            List<Course> c = ctx.Courses.Where(p => p.Code.Contains(kw) || p.Name.Contains(kw)).ToList();
            if (c.Count == 0)
            {
                ViewBag.ept = 1;
                return View();
            }
            else
            {
                ViewBag.ept = 0;
                return View(c);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file, String type, String c)
        {
            Student s = HttpContext.Session.GetObject<Student>("us");
            if (file == null || file.Length == 0)
            {
                return Content("file not selected");
            }

            
            int id = ctx.Enrollments.FirstOrDefault(en => en.CId == Int32.Parse(c) && en.SId == s.Id).Id;
            Progress p = ctx.Progresses.FirstOrDefault(p => p.EId == id);
            Course course = ctx.Courses.FirstOrDefault(cou => cou.Id == Int32.Parse(c));
            var fpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Assignment/" + course.Code + "/" + s.Email);
            if(!Directory.Exists(fpath))
            {
                Directory.CreateDirectory(fpath);
            }
            var path = Path.Combine(fpath, file.FileName);
            if (type.Equals("1"))
            {
                p.As1 = file.FileName;
            }
            else if (type.Equals("2"))
            {
                p.As2 = file.FileName;
            }
            else if (type.Equals("3"))
            {
                p.As3 = file.FileName;
            }
            ctx.SaveChanges();
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return RedirectToAction("MyCourse");
        }
        public IActionResult Dashboard()
        {
            Student s = HttpContext.Session.GetObject<Student>("us");
            List<Enrollment> ls = ctx.Enrollments.Where(e => e.SId == s.Id && e.Status == 1).ToList();
            List<Course> list = new List<Course>();
            for (int i = 0; i < ls.Count; i++)
            {
                if (ctx.Courses.FirstOrDefault(c => c.Id == ls[i].CId) != null)
                {
                    list.Add(ctx.Courses.FirstOrDefault(c => c.Id == ls[i].CId));
                }

            }
            ViewBag.name = s.DisplayName;
            ViewBag.enr = ls;
            ViewBag.prog = ctx.Progresses.ToList();
            ViewBag.Lecture = ctx.Lectures.ToList();
            ViewBag.ept = ls.Count();
            return View(ls);
        }
        [HttpPost]
        public IActionResult Overdue(String eidID)
        {
            Enrollment e = ctx.Enrollments.FirstOrDefault(e => e.Id == Int32.Parse(eidID));
            if (e != null)
            {
                e.Ovd = 1;
            }
            ctx.SaveChanges();
            return RedirectToAction("MyCourse");
        }
        
        public async Task<IActionResult> Delete(String fn, String type, String c)
        {
            Student s = HttpContext.Session.GetObject<Student>("us");
            Course course = ctx.Courses.FirstOrDefault(cou => cou.Id == Int32.Parse(c));

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Assignment/" + course.Code + "/" + s.Email,
                        fn);
            int id = ctx.Enrollments.FirstOrDefault(en => en.CId == Int32.Parse(c) && en.SId == s.Id).Id;
            Progress p = ctx.Progresses.FirstOrDefault(p => p.EId == id);
            if (type.Equals("1"))
            {
                p.As1 = null;
                p.Ed1 -= 1;
            }
            else if (type.Equals("2")) { p.As2 = null; p.Ed2 -= 1; }
            else if (type.Equals("3")) { p.As3 = null; p.Ed3 -= 1; }
            ctx.SaveChanges();
            System.IO.File.Delete(path);

            return RedirectToAction("MyCourse");
        }
        public IActionResult Logout(String eidID)
        {
            HttpContext.Session.Clear();
            
            return RedirectToAction("Index");
        }

    }
}