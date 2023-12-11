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
    public IActionResult ListarTareas(int idTablero, bool? mostrarTareasAsignadasYNoAsignadas){
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToRoute(new{controller="Logueo", action="Index"});
        
        var rolUsuarioAutenticado = HttpContext.Session.GetString("rol");
        
        try
        {
            var tablero = _tablerosRepo.GetTablero(idTablero);
            var usuarioPropietario = _usuariosRepo.GetUsuario(tablero.IdUsuarioPropietario);
            var usuarios = _usuariosRepo.GetAllUsuarios();
            var tareasTablero = _tareasRepo.GetTareasDeTablero(tablero.Id);


            if(mostrarTareasAsignadasYNoAsignadas == null)
            {
                if(rolUsuarioAutenticado != Rol.Administrador.ToString() && usuarioPropietario.Id != Convert.ToInt32(HttpContext.Session.GetString("id"))) return RedirectToAction("Error");
                return View(new ListaTareasTableroViewModel(tareasTablero, usuarios, tablero, true, false));
            }else
            {
                if((bool)mostrarTareasAsignadasYNoAsignadas)
                {
                    var tareasAsignadas = tareasTablero.Where(tarea => tarea.IdUsuarioAsignado == Convert.ToInt32(HttpContext.Session.GetString("id"))).ToList();
                    if(usuarioPropietario.Id == Convert.ToInt32(HttpContext.Session.GetString("id")))
                    {
                        var tareasNoAsignadas = tareasTablero.Where(tarea => tarea.IdUsuarioAsignado == null).ToList();
                        tareasAsignadas.AddRange(tareasNoAsignadas);
                        return View(new ListaTareasTableroViewModel(tareasAsignadas, usuarios, tablero, true, false));
                    }else
                    {
                        if(!tareasAsignadas.Any())
                        {
                            if(rolUsuarioAutenticado != Rol.Administrador.ToString()) return RedirectToAction("Error");
                            return RedirectToAction("ListarTareas", new{idTablero = tablero.Id});
                        }else
                        {
                            var tareasNoAsignadas = tareasTablero.Where(tarea => tarea.IdUsuarioAsignado == null).ToList();
                            tareasAsignadas.AddRange(tareasNoAsignadas);
                            return View(new ListaTareasTableroViewModel(tareasAsignadas, usuarios, tablero, false, true));
                        }
                    }
                }else
                {
                    if(rolUsuarioAutenticado != Rol.Administrador.ToString()) if(usuarioPropietario.Id != Convert.ToInt32(HttpContext.Session.GetString("id"))) return RedirectToAction("Error");
                    return RedirectToAction("ListarTareas", new{idTablero = tablero.Id});
                }
            }
        }catch(Exception ex)
        {
            _logger.LogError(ex.ToString());
            return RedirectToAction("Error");
        }
    }

    [HttpGet]
    public IActionResult CrearTarea(int idTablero){
        
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToRoute(new{controller="Logueo", action="Index"});

        try
        {
            var tablero = _tablerosRepo.GetTablero(idTablero);
            var usuarios = _usuariosRepo.GetAllUsuarios();
            if(tablero.IdUsuarioPropietario != Convert.ToInt32(HttpContext.Session.GetString("id"))) return RedirectToAction("Error");
            return View(new TareaTableroUsuariosViewModel(usuarios, idTablero));

        }catch(Exception ex)
        {
            _logger.LogError(ex.ToString());
            return RedirectToAction("Error");
        }
    }

    [HttpGet]
    public IActionResult ActualizarTarea(int idTarea){
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToRoute(new{controller="Logueo", action="Index"});
        
        var rolUsuarioAutenticado = HttpContext.Session.GetString("rol");

        try
        {            
            var tarea = _tareasRepo.GetTarea(idTarea);
            var tablero = _tablerosRepo.GetTablero(tarea.IdTablero);
            if(rolUsuarioAutenticado != Rol.Administrador.ToString() && tablero.IdUsuarioPropietario != Convert.ToInt32(HttpContext.Session.GetString("id"))) return RedirectToAction("Error");
            var usuarios = _usuariosRepo.GetAllUsuarios();
            return View(new TareaTableroUsuariosViewModel(tarea, usuarios));

        }catch(Exception ex)
        {
            _logger.LogError(ex.ToString());
            return RedirectToAction("Error");
        }
    }

    [HttpPost]
    public IActionResult CrearTarea(TareaTableroUsuariosViewModel tareaTU){
        
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToRoute(new{controller="Logueo", action="Index"});
        
        if(!ModelState.IsValid) return RedirectToAction("ListarTareas", new{idTablero = tareaTU.IdTablero});

        try
        {
            var tablero = _tablerosRepo.GetTablero(tareaTU.IdTablero);
            if(tablero.IdUsuarioPropietario != Convert.ToInt32(HttpContext.Session.GetString("id"))) return RedirectToAction("Error");

            var tareaNueva = new Tarea(tareaTU.Tarea);
            _tareasRepo.CrearTarea(tareaTU.IdTablero, tareaNueva);
            return RedirectToAction("ListarTareas", new{idTablero = tareaTU.IdTablero});

        }catch(Exception ex)
        {
            _logger.LogError(ex.ToString());
            return RedirectToAction("Error");
        }
    }

    [HttpPost]
    public IActionResult ActualizarTarea(TareaTableroUsuariosViewModel tareaTU){
        
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToRoute(new{controller="Logueo", action="Index"});

        if(!ModelState.IsValid) return RedirectToAction("ListarTareas", new{idTablero = tareaTU.IdTablero});

        var rolUsuarioAutenticado = HttpContext.Session.GetString("rol");

        try
        {   
            var tablero = _tablerosRepo.GetTablero(tareaTU.IdTablero);
            if(rolUsuarioAutenticado != Rol.Administrador.ToString()) if(tablero.IdUsuarioPropietario != Convert.ToInt32(HttpContext.Session.GetString("id"))) return RedirectToAction("Error");
         
            var tareaActualizada = new Tarea(tareaTU.Tarea);
            _tareasRepo.ModificarTarea(tareaActualizada);
            return RedirectToAction("ListarTareas", new{idTablero = tareaActualizada.IdTablero});

        }catch(Exception ex)
        {
            _logger.LogError(ex.ToString());
            return RedirectToAction("Error");
        }
    }

    [HttpPost]
    public IActionResult ActualizarEstado(int idTarea, EstadoTarea estadoNuevo){
        var tarea = _tareasRepo.GetTarea(idTarea);
        tarea.Estado = estadoNuevo;
        _tareasRepo.ModificarTarea(tarea);
        return RedirectToAction("ListarTareas", new{idTablero = tarea.IdTablero, mostrarTareasAsignadasYNoAsignadas=true});
    }

    public IActionResult EliminarTarea(int idTarea){
        
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToRoute(new{controller="Logueo", action="Index"});

        var rolUsuarioAutenticado = HttpContext.Session.GetString("rol");

        try
        {            
            var id = _tareasRepo.GetTarea(idTarea).IdTablero;
            var tablero = _tablerosRepo.GetTablero(id);
            if(rolUsuarioAutenticado != Rol.Administrador.ToString()) if(tablero.IdUsuarioPropietario != Convert.ToInt32(HttpContext.Session.GetString("id"))) return RedirectToAction("Error");

            _tareasRepo.EliminarTarea(idTarea);
            return RedirectToAction("ListarTareas", new{idTablero = id});

        }catch(Exception ex)
        {
            _logger.LogError(ex.ToString());
            return RedirectToAction("Error");
        }
    }

    public IActionResult Error(){
        return View(new ErrorViewModel());
    }
}