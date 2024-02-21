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
    public IActionResult ListarTareasTablero(int idTablero)
    {
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToRoute(new{controller="Logueo", action="Index"});

        var rolUsuarioAutenticado = HttpContext.Session.GetString("rol");

        try
        {
            var tablero = _tablerosRepo.GetTablero(idTablero);
            var usuarioPropietario = _usuariosRepo.GetUsuario(tablero.IdUsuarioPropietario);
            var usuariosAsignados = _usuariosRepo.GetAllUsuarios();
            var tareasTableroPorEstado = new Dictionary<EstadoTarea, List<Tarea>>();
            foreach(var estado in (EstadoTarea[])Enum.GetValues(typeof(EstadoTarea)))
            {
                var tareasTablero = _tareasRepo.GetTareasDelTablero(tablero.Id, estado);
                tareasTableroPorEstado.Add(estado, tareasTablero);
            }

            if(usuarioPropietario.Id != Convert.ToInt32(HttpContext.Session.GetString("id")))
            {
                if(rolUsuarioAutenticado == Rol.Administrador.ToString())
                {
                    return View(new ListaTareasTableroViewModel(tablero, tareasTableroPorEstado, usuariosAsignados, false));
                }
                else
                {
                    return View("Views/Shared/Error.cshtml", new ErrorViewModel{message = "ERROR 401. No tiene autorización para ver las tareas del tablero de otro usuario."});
                }
            }
                
            return View(new ListaTareasTableroViewModel(tablero, tareasTableroPorEstado, usuariosAsignados, true));
        }
        catch(Exception ex)
        {
            _logger.LogError(ex.ToString());
            return View("Views/Shared/Error.cshtml", new ErrorViewModel{message = ex.Message});
        }
    }

    [HttpGet]
    public IActionResult ListarTareasAsignadasYNoAsignadasDelTablero(int idTablero)
    {
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToRoute(new{controller="Logueo", action="Index"});

        var rolUsuarioAutenticado = HttpContext.Session.GetString("rol");

        try
        {
            var tablero = _tablerosRepo.GetTablero(idTablero);
            var usuarioCTA = _usuariosRepo.GetUsuario(Convert.ToInt32(HttpContext.Session.GetString("id")));
            var tareasTableroCTA = new Dictionary<EstadoTarea, List<Tarea>>();
            foreach(var estado in (EstadoTarea[])Enum.GetValues(typeof(EstadoTarea)))
            {
                var tareasTablero = _tareasRepo.GetTareasAsignadasAlUsuario(tablero.Id, usuarioCTA.Id, estado);
                tareasTableroCTA.Add(estado, tareasTablero);
            }

            if(tareasTableroCTA.Values.Any(lista => lista.Any()))
            {
                foreach(var estado in (EstadoTarea[])Enum.GetValues(typeof(EstadoTarea)))
                {
                    var tareasNoAsignadas = _tareasRepo.GetTareasNoAsignadasDelTablero(idTablero, estado);
                    tareasTableroCTA[estado].AddRange(tareasNoAsignadas);
                }
                return View(new ListaTareasAsignadasYNoAsignadasViewModel(tablero, tareasTableroCTA, usuarioCTA));
            }
            else
            {
                if(rolUsuarioAutenticado != Rol.Administrador.ToString()) return RedirectToRoute(new{controller = "Tablero", action = "MisTableros"});
                return RedirectToAction("ListarTareasTablero", new {idTablero});
            }
        }
        catch(Exception ex)
        {
            _logger.LogError(ex.ToString());
            return View("Views/Shared/Error.cshtml", new ErrorViewModel{message = ex.Message});
        }
    }
    
    [HttpGet]
    public IActionResult CrearTarea(int idTablero, EstadoTarea estado)
    {
        
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToRoute(new{controller="Logueo", action="Index"});

        try
        {
            var tablero = _tablerosRepo.GetTablero(idTablero);
            var usuarios = _usuariosRepo.GetAllUsuarios();
            if(tablero.IdUsuarioPropietario != Convert.ToInt32(HttpContext.Session.GetString("id"))) return RedirectToAction("Views/Shared/Error.cshtml", new ErrorViewModel{message = "ERROR 400. No puede crear una tarea en un tablero del que no es propietario."});
            return View(new TareaTableroUsuariosViewModel(usuarios, idTablero, estado));

        }catch(Exception ex)
        {
            _logger.LogError(ex.ToString());
            return RedirectToAction("Views/Shared/Error.cshtml", new ErrorViewModel{message = ex.Message});
        }
    }

    [HttpGet]
    public IActionResult ActualizarTarea(int idTarea)
    {
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToRoute(new{controller="Logueo", action="Index"});
        
        var rolUsuarioAutenticado = HttpContext.Session.GetString("rol");

        try
        {            
            var tarea = _tareasRepo.GetTarea(idTarea);
            var tablero = _tablerosRepo.GetTablero(tarea.IdTablero);
            if(rolUsuarioAutenticado != Rol.Administrador.ToString() && tablero.IdUsuarioPropietario != Convert.ToInt32(HttpContext.Session.GetString("id"))) return View("Views/Shared/Error.cshtml", new ErrorViewModel{message = "ERROR 400. No puede actualizar la tarea del tablero de otro usuario."});
            var usuarios = _usuariosRepo.GetAllUsuarios();
            return View(new TareaTableroUsuariosViewModel(tarea, usuarios));

        }catch(Exception ex)
        {
            _logger.LogError(ex.ToString());
            return RedirectToAction("Views/Shared/Error.cshtml", new ErrorViewModel{message = ex.Message});
        }
    }

    [HttpPost]
    public IActionResult CrearTarea(TareaTableroUsuariosViewModel tareaTU)
    {
        
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToRoute(new{controller="Logueo", action="Index"});
        
        if(!ModelState.IsValid) return RedirectToAction("CrearTarea", new{idTablero = tareaTU.IdTablero});

        try
        {
            var tablero = _tablerosRepo.GetTablero(tareaTU.IdTablero);
            if(tablero.IdUsuarioPropietario != Convert.ToInt32(HttpContext.Session.GetString("id"))) return RedirectToAction("Views/Shared/Error.cshtml", new ErrorViewModel{message = "ERROR 400. No puede crear una tarea en un tablero del que no es propietario."});

            var tareaNueva = new Tarea(tareaTU.Tarea);
            _tareasRepo.CrearTarea(tareaTU.IdTablero, tareaNueva);
            return RedirectToAction("ListarTareasTablero", new{idTablero = tareaTU.IdTablero});

        }
        catch(Exception ex)
        {
            _logger.LogError(ex.ToString());
            return RedirectToAction("Views/Shared/Error.cshtml", new ErrorViewModel{message = ex.Message});
        }
    }

    [HttpPost]
    public IActionResult ActualizarTarea(TareaTableroUsuariosViewModel tareaTU)
    {
        
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToRoute(new{controller="Logueo", action="Index"});

        if(!ModelState.IsValid) return RedirectToAction("ActualizarTarea", new{idTablero = tareaTU.IdTablero});

        var rolUsuarioAutenticado = HttpContext.Session.GetString("rol");

        try
        {   
            var tablero = _tablerosRepo.GetTablero(tareaTU.IdTablero);
            if(rolUsuarioAutenticado != Rol.Administrador.ToString() && tablero.IdUsuarioPropietario != Convert.ToInt32(HttpContext.Session.GetString("id"))) return View("Views/Shared/Error.cshtml", new ErrorViewModel{message = "ERROR 401. No tiene autorización para actualizar la tarea del tablero de otro usuario."});
         
            var tareaActualizada = new Tarea(tareaTU.Tarea);
            _tareasRepo.ModificarTarea(tareaActualizada);
            return RedirectToAction("ListarTareasTablero", new{idTablero = tareaActualizada.IdTablero});

        }catch(Exception ex)
        {
            _logger.LogError(ex.ToString());
            return RedirectToAction("Views/Shared/Error.cshtml", new ErrorViewModel{message = ex.Message});
        }
    }

    [HttpPost]
    public IActionResult ActualizarEstado(int idTarea, EstadoTarea estadoNuevo)
    {
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToRoute(new{controller="Logueo", action="Index"});

        try
        {
            var tarea = _tareasRepo.GetTarea(idTarea);
            if(tarea.IdUsuarioAsignado != null && tarea.IdUsuarioAsignado == Convert.ToInt32(HttpContext.Session.GetString("id")))
            {
                tarea.Estado = estadoNuevo;
                _tareasRepo.ModificarTarea(tarea);
            }
            return RedirectToAction("ListarTareasAsignadasYNoAsignadasDelTablero", new{idTablero = tarea.IdTablero});
        }
        catch(Exception ex)
        {
            _logger.LogError(ex.ToString());
            return RedirectToAction("Views/Shared/Error.cshtml", new ErrorViewModel{message = ex.Message});
        }
    }

    public IActionResult EliminarTarea(int idTarea){
        
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToRoute(new{controller="Logueo", action="Index"});

        var rolUsuarioAutenticado = HttpContext.Session.GetString("rol");

        try
        {            
            var id = _tareasRepo.GetTarea(idTarea).IdTablero;
            var tablero = _tablerosRepo.GetTablero(id);
            if(rolUsuarioAutenticado != Rol.Administrador.ToString() && tablero.IdUsuarioPropietario != Convert.ToInt32(HttpContext.Session.GetString("id"))) return View("Views/Shared/Error.cshtml", new ErrorViewModel{message = "ERROR 401. No tiene autorización para eliminar la tarea del tablero de otro usuario."});

            _tareasRepo.EliminarTarea(idTarea);
            return RedirectToAction("ListarTareasTablero", new{idTablero = id});

        }catch(Exception ex)
        {
            _logger.LogError(ex.ToString());
            return RedirectToAction("Views/Shared/Error.cshtml", new ErrorViewModel{message = ex.Message});
        }
    }

}