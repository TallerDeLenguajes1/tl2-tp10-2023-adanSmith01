using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp10_2023_adanSmith01.Models;
using tl2_tp10_2023_adanSmith01.Repository;
using tl2_tp10_2023_adanSmith01.ViewModels;
namespace tl2_tp10_2023_adanSmith01.Controllers;

public class TareaController : Controller
{
    private readonly ILogger<TareaController> _logger;
    private ITareaRepository tareasRepo;
    private ITableroRepository tableroRepo;
    private IUsuarioRepository usuarioRepo;

    public TareaController(ILogger<TareaController> logger)
    {
        _logger = logger;
        tareasRepo = new TareaRepository();
        tableroRepo = new TableroRepository();
        usuarioRepo = new UsuarioRepository();
    }

    [HttpGet]
    public IActionResult ListarTareas(int idTablero){
        if(!String.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))){
            var tablero = tableroRepo.GetTablero(idTablero);
            if(!String.IsNullOrEmpty(tablero.Nombre)){
                var tareas = tareasRepo.GetTareasDeTablero(idTablero);
                var usuarios = usuarioRepo.GetAllUsuarios();
                return View(new TareasTableroViewModel(tareas, usuarios, tablero.Nombre));
            }else{
                return RedirectToRoute(new {controller = "Logueo", action="Index"});//Debe retornar un 404
            }
        }else{
            return RedirectToRoute(new {controller = "Logueo", action="Index"});
        }
        
    }

    [HttpGet]
    public IActionResult CrearTarea(int idTablero){
        if(!String.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))){
            var tablero = tableroRepo.GetTablero(idTablero);
            if(!String.IsNullOrEmpty(tablero.Nombre)){
                var usuarios = usuarioRepo.GetAllUsuarios();
                List<UsuarioViewModel> usuariosVM = new List<UsuarioViewModel>();
                foreach(var usuario in usuarios) usuariosVM.Add(new UsuarioViewModel(usuario));
                return View(new CrearTareaViewModel(usuariosVM, idTablero));
            }else{
                return RedirectToRoute(new {controller = "Logueo", action="Index"});
            }
        }else{
            return RedirectToRoute(new {controller = "Logueo", action="Index"});
        }
        
    }

    [HttpGet]
    public IActionResult ActualizarTarea(int idTarea){
        if(!String.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))){
            var tarea = tareasRepo.GetTarea(idTarea);
            if(!String.IsNullOrEmpty(tarea.Nombre)){
                return View(new ActualizarTareaViewModel(tarea, usuarioRepo.GetAllUsuarios()));
            }else{
                return RedirectToRoute(new {controller = "Logueo", action="Index"});//Error 404
            }
        }
        else return RedirectToRoute(new {controller = "Logueo", action="Index"});
    }

    [HttpPost]
    public IActionResult CrearTarea(int idTablero, CrearTareaViewModel nueva){
        var tareaNueva = new Tarea(nueva.TareaVM);
        tareasRepo.CrearTarea(idTablero, tareaNueva);
        return RedirectToAction("ListarTareas", new{idTablero = idTablero});
    }

    [HttpPost]
    public IActionResult ActualizarTarea(ActualizarTareaViewModel tareaAModificar){
        var tareaActualizada = new Tarea(tareaAModificar.TareaVM);
        tareasRepo.ModificarTarea(tareaActualizada);
        return RedirectToAction("ListarTareas", new{idTablero = tareaActualizada.IdTablero});
    }

    public IActionResult EliminarTarea(int idTarea){
        var id = tareasRepo.GetTarea(idTarea).IdTablero;
        tareasRepo.EliminarTarea(idTarea);
        return RedirectToAction("ListarTareas", new{idTablero = id});
    }

}