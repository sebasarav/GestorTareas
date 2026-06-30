using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestorTareas.Models
{
    /// <summary>
    /// Provides data access operations for TaskItem objects.
    /// 
    /// Storage strategy: In-memory list persisted in the user's HTTP Session.
    /// This means data is kept for the duration of the browser session and does
    /// not require a database. Each user has their own isolated task list.
    /// 
    /// This approach is used to demonstrate Session-based state management,
    /// as specified in the course requirements (Chapter 7).
    /// </summary>
    public static class TaskRepository
    {
        // Key used to store the task list in Session
        private const string SessionKey = "TaskList";

        /// <summary>
        /// Retrieves the task list from the current Session.
        /// If none exists, initializes and stores an empty list first.
        /// </summary>
        private static List<TaskItem> GetSessionList(HttpSessionStateBase session)
        {
            if (session[SessionKey] == null)
            {
                session[SessionKey] = new List<TaskItem>();
            }
            return (List<TaskItem>)session[SessionKey];
        }

        /// <summary>
        /// Returns all tasks for the current session user.
        /// </summary>
        public static List<TaskItem> GetAll(HttpSessionStateBase session)
        {
            return GetSessionList(session);
        }

        /// <summary>
        /// Returns tasks filtered by status.
        /// If status is null, all tasks are returned.
        /// </summary>
        public static List<TaskItem> GetByStatus(HttpSessionStateBase session, TaskStatus? status)
        {
            var tasks = GetSessionList(session);
            return status.HasValue
                ? tasks.Where(t => t.Status == status.Value).ToList()
                : tasks;
        }

        /// <summary>
        /// Finds and returns a single task by its unique ID.
        /// Returns null if no task with the given ID exists.
        /// </summary>
        public static TaskItem GetById(HttpSessionStateBase session, int id)
        {
            return GetSessionList(session).FirstOrDefault(t => t.Id == id);
        }

        /// <summary>
        /// Adds a new task to the session list.
        /// Automatically assigns a unique ID and the current creation date.
        /// </summary>
        public static void Add(HttpSessionStateBase session, TaskItem task)
        {
            var tasks = GetSessionList(session);

            // Auto-increment ID based on current max
            task.Id = tasks.Count > 0 ? tasks.Max(t => t.Id) + 1 : 1;
            task.CreatedDate = DateTime.Now;

            tasks.Add(task);

            // Explicitly save the updated list back to session
            session[SessionKey] = tasks;
        }

        /// <summary>
        /// Removes a task from the session list by ID.
        /// Returns true if found and deleted; false if the task was not found.
        /// </summary>
        public static bool Delete(HttpSessionStateBase session, int id)
        {
            var tasks = GetSessionList(session);
            var task = tasks.FirstOrDefault(t => t.Id == id);

            if (task == null) return false;

            tasks.Remove(task);
            session[SessionKey] = tasks;
            return true;
        }

        /// <summary>
        /// Returns the total number of tasks in the current session.
        /// </summary>
        public static int GetCount(HttpSessionStateBase session)
        {
            return GetSessionList(session).Count;
        }
    }
}
