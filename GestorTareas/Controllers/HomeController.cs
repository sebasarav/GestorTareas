using GestorTareas.Models;
using System.Web.Mvc;

namespace GestorTareas.Controllers
{
    /// <summary>
    /// Handles requests for the application home page.
    /// Manages user identification via Session state.
    /// 
    /// State management used:
    ///   - Session["UserName"]: stores the user's display name across requests.
    ///   - TempData:            passes one-time success/error messages to the next view.
    ///   - ViewBag:             sends task count to the view without creating a formal model.
    /// </summary>
    public class HomeController : Controller
    {
        // GET: Home/Index
        public ActionResult Index()
        {
            // Retrieve the user name from Session (null if not yet set)
            ViewBag.UserName  = Session["UserName"] as string;
            ViewBag.TaskCount = TaskRepository.GetCount(Session);
            return View();
        }

        // POST: Home/SetUserName
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SetUserName(string userName)
        {
            if (!string.IsNullOrWhiteSpace(userName))
            {
                // Persist the name across the session so all views can display it
                Session["UserName"] = userName.Trim();
                TempData["SuccessMessage"] = "¡Bienvenido, " + userName.Trim() + "! Ya puedes gestionar tus tareas.";
            }
            else
            {
                TempData["ErrorMessage"] = "Por favor, ingresa un nombre de usuario válido.";
            }

            return RedirectToAction("Index");
        }
    }
}
