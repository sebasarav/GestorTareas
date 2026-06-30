using GestorTareas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

// Alias to avoid ambiguity with System.Threading.Tasks.TaskStatus
using ItemStatus   = GestorTareas.Models.TaskStatus;
using ItemPriority = GestorTareas.Models.TaskPriority;

namespace GestorTareas.Controllers
{
    /// <summary>
    /// Handles all task-related operations: list, create, detail view, and delete.
    /// Also exposes two JSON endpoints consumed by AJAX calls in the frontend.
    ///
    /// State management used:
    ///   - Session:   task list is stored and retrieved via TaskRepository (Session-backed).
    ///   - TempData:  one-time confirmation and error messages after redirect.
    ///   - ViewBag:   sends statistics (counts) and username to views.
    /// </summary>
    public class TaskController : Controller
    {
        // ─────────────────────────────────────────────────────────────────────
        //  Standard MVC Actions
        // ─────────────────────────────────────────────────────────────────────

        // GET: Task/Index
        public ActionResult Index()
        {
            var tasks = TaskRepository.GetAll(Session);

            // Pass summary statistics to the view via ViewBag
            ViewBag.TotalCount      = tasks.Count;
            ViewBag.PendingCount    = tasks.Count(t => t.Status == ItemStatus.Pending);
            ViewBag.InProgressCount = tasks.Count(t => t.Status == ItemStatus.InProgress);
            ViewBag.CompletedCount  = tasks.Count(t => t.Status == ItemStatus.Completed);
            ViewBag.OverdueCount    = tasks.Count(t => t.IsOverdue);
            ViewBag.UserName        = Session["UserName"] as string;

            return View(tasks);
        }

        // GET: Task/Create
        public ActionResult Create()
        {
            ViewBag.UserName = Session["UserName"] as string;
            PopulateDropDowns(ItemPriority.Medium, ItemStatus.Pending);

            // Default values for a new task
            return View(new TaskItem
            {
                DueDate  = DateTime.Today.AddDays(7),
                Status   = ItemStatus.Pending,
                Priority = ItemPriority.Medium
            });
        }

        // POST: Task/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TaskItem task)
        {
            // Additional server-side rule: due date cannot be in the past
            if (task.DueDate.Date < DateTime.Today)
            {
                ModelState.AddModelError("DueDate",
                    "La fecha de vencimiento no puede ser anterior a hoy.");
            }

            if (ModelState.IsValid)
            {
                TaskRepository.Add(Session, task);

                // TempData survives exactly one redirect; used to show a confirmation banner
                TempData["SuccessMessage"] = "La tarea \"" + task.Title + "\" fue registrada exitosamente.";
                return RedirectToAction("Index");
            }

            // Re-populate drop-downs in case of validation failure
            ViewBag.UserName = Session["UserName"] as string;
            PopulateDropDowns(task.Priority, task.Status);
            return View(task);
        }

        // GET: Task/Details/5
        public ActionResult Details(int id)
        {
            var task = TaskRepository.GetById(Session, id);

            if (task == null)
            {
                TempData["ErrorMessage"] = "La tarea con ID " + id + " no fue encontrada.";
                return RedirectToAction("Index");
            }

            ViewBag.UserName = Session["UserName"] as string;
            return View(task);
        }

        // POST: Task/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            // Grab the title before deleting so we can show it in the message
            var task      = TaskRepository.GetById(Session, id);
            string title  = task?.Title ?? "Tarea";
            bool deleted  = TaskRepository.Delete(Session, id);

            if (deleted)
                TempData["SuccessMessage"] = "La tarea \"" + title + "\" fue eliminada correctamente.";
            else
                TempData["ErrorMessage"]   = "No se pudo eliminar la tarea. Puede que ya no exista.";

            return RedirectToAction("Index");
        }

        // ─────────────────────────────────────────────────────────────────────
        //  AJAX JSON Endpoints
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>
        /// AJAX endpoint: returns task list filtered by status as JSON.
        /// Called by the filter buttons in Task/Index without reloading the page.
        /// GET: Task/SearchByStatus?status=Pending
        /// </summary>
        [HttpGet]
        public JsonResult SearchByStatus(string status)
        {
            List<TaskItem> tasks;

            if (string.IsNullOrEmpty(status) || status == "All")
            {
                tasks = TaskRepository.GetAll(Session);
            }
            else if (Enum.TryParse(status, out ItemStatus parsedStatus))
            {
                tasks = TaskRepository.GetByStatus(Session, parsedStatus);
            }
            else
            {
                tasks = TaskRepository.GetAll(Session);
            }

            // Project to an anonymous object — avoids circular references and exposes only what the view needs
            var result = tasks.Select(t => new
            {
                id            = t.Id,
                title         = t.Title,
                shortDesc     = t.Description.Length > 80
                                    ? t.Description.Substring(0, 80) + "..."
                                    : t.Description,
                status        = t.Status.ToString(),
                statusLabel   = GetStatusLabel(t.Status),
                priority      = t.Priority.ToString(),
                priorityLabel = GetPriorityLabel(t.Priority),
                dueDate       = t.DueDate.ToString("dd/MM/yyyy"),
                isOverdue     = t.IsOverdue
            }).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// AJAX endpoint: returns aggregate counts as JSON.
        /// Used to refresh the statistics cards without a full page reload.
        /// GET: Task/GetSummary
        /// </summary>
        [HttpGet]
        public JsonResult GetSummary()
        {
            var tasks = TaskRepository.GetAll(Session);

            var summary = new
            {
                total      = tasks.Count,
                pending    = tasks.Count(t => t.Status == ItemStatus.Pending),
                inProgress = tasks.Count(t => t.Status == ItemStatus.InProgress),
                completed  = tasks.Count(t => t.Status == ItemStatus.Completed),
                overdue    = tasks.Count(t => t.IsOverdue)
            };

            return Json(summary, JsonRequestBehavior.AllowGet);
        }

        // ─────────────────────────────────────────────────────────────────────
        //  Private Helpers
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Populates ViewBag with SelectLists for Priority and Status drop-downs.
        /// </summary>
        private void PopulateDropDowns(ItemPriority selectedPriority, ItemStatus selectedStatus)
        {
            ViewBag.PriorityList = new SelectList(new[]
            {
                new { Value = "Low",    Text = "Baja"       },
                new { Value = "Medium", Text = "Media"      },
                new { Value = "High",   Text = "Alta"       }
            }, "Value", "Text", selectedPriority.ToString());

            ViewBag.StatusList = new SelectList(new[]
            {
                new { Value = "Pending",    Text = "Pendiente"   },
                new { Value = "InProgress", Text = "En Progreso" },
                new { Value = "Completed",  Text = "Completada"  }
            }, "Value", "Text", selectedStatus.ToString());
        }

        /// <summary>Returns the Spanish label for a given TaskStatus value.</summary>
        private string GetStatusLabel(ItemStatus status)
        {
            switch (status)
            {
                case ItemStatus.Pending:    return "Pendiente";
                case ItemStatus.InProgress: return "En Progreso";
                case ItemStatus.Completed:  return "Completada";
                default: return status.ToString();
            }
        }

        /// <summary>Returns the Spanish label for a given TaskPriority value.</summary>
        private string GetPriorityLabel(ItemPriority priority)
        {
            switch (priority)
            {
                case ItemPriority.Low:    return "Baja";
                case ItemPriority.Medium: return "Media";
                case ItemPriority.High:   return "Alta";
                default: return priority.ToString();
            }
        }
    }
}
