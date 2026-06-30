# GestorTareas — Sistema de Control de Tareas

## Descripcion

GestorTareas es una aplicacion web que permite registrar, consultar y gestionar tareas personales. El usuario puede crear tareas con titulo, descripcion, prioridad y fecha de vencimiento, consultarlas en una lista con filtros dinamicos y ver el detalle de cada una. La aplicacion identifica visualmente las tareas vencidas y muestra estadisticas de estado en tiempo real.

---

## Tecnologias utilizadas

- ASP.NET MVC 5 / .NET Framework 4.8
- C# (logica del servidor)
- Razor Views (vistas)
- Bootstrap 3 (estilos y componentes UI)
- jQuery 3 (AJAX y manipulacion del DOM)
- jQuery Validation (validacion client-side)
- JavaScript puro (validaciones, contadores, confirmaciones)
- HTML5 / CSS3

---

## Instrucciones para ejecutar el proyecto

### Requisitos previos
- Visual Studio 2019 o 2022
- .NET Framework 4.8

### Pasos

1. Clonar o descargar el repositorio:
   ```
   git clone https://github.com/sebasarav/GestorTareas.git
   ```

2. Abrir el archivo de solucion `GestorTareas.sln` con Visual Studio.

3. Restaurar los paquetes NuGet:
   - Click derecho sobre la solucion en el Solution Explorer
   - Seleccionar **Restore NuGet Packages**

4. Compilar la solucion:
   - `Build` → `Build Solution` o `Ctrl + Shift + B`

5. Ejecutar la aplicacion:
   - Presionar **F5** o el boton **IIS Express**
   - El navegador abrira automaticamente la pagina de inicio

6. Usar la aplicacion:
   - Ingresar un nombre de usuario en la pagina de inicio
   - Navegar a **Nueva Tarea** para registrar tareas
   - Usar los filtros en **Mis Tareas** para filtrar por estado

---

## Estructura del proyecto

```
GestorTareas/
├── Controllers/
│   ├── HomeController.cs      
│   └── TaskController.cs      
├── Models/
│   ├── TaskItem.cs            
│   └── TaskRepository.cs      
├── Views/
│   ├── Home/
│   │   └── Index.cshtml       
│   ├── Task/
│   │   ├── Index.cshtml       
│   │   ├── Create.cshtml      
│   │   └── Details.cshtml     
│   └── Shared/
│       └── _Layout.cshtml     
├── Scripts/
│   └── task-scripts.js        
├── Content/
│   └── site.css               
└── GestorTareas.sln
```

---

## Estudiante

**Nombre:** Sebastián Arias Avilés   
**Curso:** 03101 – Programacion Avanzada en Web  
**Universidad:** Universidad Estatal a Distancia (UNED)  
**Fecha:** Junio 2026

---

## Video demostrativo

[![Ver video: Gestor de Tarea](https://img.youtube.com/vi/3sCrlSt_Bow/0.jpg)](https://youtu.be/3sCrlSt_Bow)
