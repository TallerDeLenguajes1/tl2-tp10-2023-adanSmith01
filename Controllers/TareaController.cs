using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp10_2023_adanSmith01.Models;
using tl2_tp10_2023_adanSmith01.Repository;
using tl2_tp10_2023_adanSmith01.ViewModels;
namespace tl2_tp10_2023_adanSmith01.Controllers;

public class TareaController : Controller
{
    private readonly ILogger<TareaController> _logger;
    private readonly ITareaRepository _tareasRepo;
    private readonly ITableroRepository _tablerosRepo;
    private readonly IUsuarioRepository _usuariosRepo;

    public TareaController(ILogger<TareaController> logger, ITableroRepository tablerosRepo, IUsuarioRepository usuariosRepo, ITareaRepository tareasRepo)
    {
        _logger = logger;
        _tareasRepo = tareasRepo;
        _tablerosRepo = tablerosRepo;
        _usuariosRepo = usuariosRepo;
    }

    [HttpGet]
    public IActionResult ListarTareas(int idTablero){
        if(String.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToRoute(new{controller="Logueo", action="Index"});

        var tablero = _tablerosRepo.GetTablero(idTablero);
        var tareas = _tareasRepo.GetTareasDeTablero(idTablero);
        var usuarios = _usuariosRepo.GetAllUsuarios();
        return View(new ListaTareasTableroViewModel(tareas, usuarios, tablero.Nombre));
    }

    [HttpGet]
    public IActionResult CrearTarea(int idTablero){
        if(String.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToRoute(new{controller="Logueo", action="Index"});

        var usuarios = _usuariosRepo.GetAllUsuarios();
        return View(new TareaTableroUsuariosViewModel(usuarios, idTablero));
    }

    [HttpGet]
    public IActionResult ActualizarTarea(int idTarea){
        if(String.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToRoute(new{controller="Logueo", action="Index"});

        var tarea = _tareasRepo.GetTarea(idTarea);
        var usuarios = _usuariosRepo.GetAllUsuarios();
        return View(new TareaTableroUsuariosViewModel(tarea, usuarios));
    }

    [HttpPost]
    public IActionResult CrearTarea(TareaTableroUsuariosViewModel tareaTU){
        if(String.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToRoute(new{controller="Logueo", action="Index"});
        if(!ModelState.IsValid) return RedirectToRoute(new {controller = "Logueo", action="Index"});

        var tareaNueva = new Tarea(tareaTU.Tarea);
        _tareasRepo.CrearTarea(tareaTU.IdTablero, tareaNueva);
        return RedirectToAction("ListarTareas", new{idTablero = tareaTU.IdTablero});
    }

    [HttpPost]
    public IActionResult ActualizarTarea(TareaTableroUsuariosViewModel tareaTU){
        if(String.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToRoute(new{controller="Logueo", action="Index"});
        
        var tareaActualizada = new Tarea(tareaTU.Tarea);
        _tareasRepo.ModificarTarea(tareaActualizada);
        return RedirectToAction("ListarTareas", new{idTablero = tareaActualizada.IdTablero});
    }

    public IActionResult EliminarTarea(int idTarea){
        if(String.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToRoute(new{controller="Logueo", action="Index"});
        
        var id = _tareasRepo.GetTarea(idTarea).IdTablero;
        _tareasRepo.EliminarTarea(idTarea);
        return RedirectToAction("ListarTareas", new{idTablero = id});
    }
}