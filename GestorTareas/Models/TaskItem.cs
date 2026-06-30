using System;
using System.ComponentModel.DataAnnotations;

namespace GestorTareas.Models
{
    /// <summary>
    /// Represents the possible states of a task.
    /// </summary>
    public enum TaskStatus
    {
        Pending,    // Pendiente
        InProgress, // En Progreso
        Completed   // Completada
    }

    /// <summary>
    /// Represents the priority level of a task.
    /// </summary>
    public enum TaskPriority
    {
        Low,    // Baja
        Medium, // Media
        High    // Alta
    }

    /// <summary>
    /// Main model class that represents a task in the GestorTareas application.
    /// Data annotations are used for both server-side and client-side validation.
    /// </summary>
    public class TaskItem
    {
        /// <summary>
        /// Unique identifier. Assigned automatically by the repository.
        /// </summary>
        public int Id { get; set; }

        [Required(ErrorMessage = "El titulo es obligatorio.")]
        [StringLength(100, MinimumLength = 3,
            ErrorMessage = "El titulo debe tener entre 3 y 100 caracteres.")]
        [Display(Name = "Titulo")]
        public string Title { get; set; }

        [Required(ErrorMessage = "La descripcion es obligatoria.")]
        [StringLength(500, MinimumLength = 10,
            ErrorMessage = "La descripcion debe tener entre 10 y 500 caracteres.")]
        [Display(Name = "Descripcion")]
        public string Description { get; set; }

        [Required(ErrorMessage = "La prioridad es obligatoria.")]
        [Display(Name = "Prioridad")]
        public TaskPriority Priority { get; set; }

        [Display(Name = "Estado")]
        public TaskStatus Status { get; set; }

        [Required(ErrorMessage = "La fecha de vencimiento es obligatoria.")]
        [Display(Name = "Fecha de vencimiento")]
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }

        [Display(Name = "Fecha de creacion")]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Computed property: true if the task is past its due date and not yet completed.
        /// Not stored; evaluated at runtime.
        /// </summary>
        public bool IsOverdue => DueDate.Date < DateTime.Today && Status != TaskStatus.Completed;
    }
}
