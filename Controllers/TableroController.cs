using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp10_2023_adanSmith01.Models;
using tl2_tp10_2023_adanSmith01.Repository;
using tl2_tp10_2023_adanSmith01.ViewModels;
namespace tl2_tp10_2023_adanSmith01.Controllers;

public class TableroController: Controller
{
    private readonly ILogger<TableroController> _logger;
    private readonly ITableroRepository _tablerosRepo;
    private readonly IUsuarioRepository _usuariosRepo;
    private readonly ITareaRepository _tareasRepo;

    public TableroController(ILogger<TableroController> logger, ITableroRepository tablerosRepo, IUsuarioRepository usuariosRepo, ITareaRepository tareasRepo)
    {
        _logger = logger;
        _tablerosRepo = tablerosRepo;
        _usuariosRepo = usuariosRepo;
        _tareasRepo = tareasRepo;
    }

    [HttpGet]
    public IActionResult MisTableros()
    {
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToRoute(new{controller="Logueo", action="Index"});

        try
        {
            var usuario = _usuariosRepo.GetUsuario(Convert.ToInt32(HttpContext.Session.GetString("id")));
            var tablerosPropios = _tablerosRepo.GetTablerosDeUsuario(usuario.Id);
            var tablerosConTareasAsignadas = _tablerosRepo.GetTablerosConTareasAsignadas(usuario.Id);
            var usuariosPropietarios = _usuariosRepo.GetAllUsuarios();
            return View(new MisTablerosViewModel(tablerosPropios, tablerosConTareasAsignadas, usuariosPropietarios));
        }
        catch(Exception ex)
        {
            _logger.LogError(ex.ToString());
            return View("Views/Shared/Error.cshtml", new ErrorViewModel{message = ex.Message});
        }
    }

    public IActionResult ListarTablerosUsuario(int? idUsuarioPropietario)
    {
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToRoute(new{controller="Logueo", action="Index"});

        try
        {
            if(HttpContext.Session.GetString("rol") == Rol.Administrador.ToString())
            {
                if(idUsuarioPropietario == null)
                {
                    var usuariosPropietarios = _usuariosRepo.GetAllUsuarios();
                    var tablerosUsuarios = _tablerosRepo.GetAllTableros().Where(tablero => tablero.IdUsuarioPropietario != Convert.ToInt32(HttpContext.Session.GetString("id"))).ToList();
                    return View(new ListaTablerosUsuarioViewModel(tablerosUsuarios, usuariosPropietarios));
                }
                else
                {
                    var usuarioPropietario = _usuariosRepo.GetUsuario((int)idUsuarioPropietario);
                    if(usuarioPropietario == null) return View("Views/Shared/Error.cshtml", new ErrorViewModel{message="ERROR 404. No existe el usuario especificado."});
                    var tablerosUsuario = _tablerosRepo.GetTablerosDeUsuario(usuarioPropietario.Id);
                    return View(new ListaTablerosUsuarioViewModel(tablerosUsuario, new List<Usuario>{usuarioPropietario}));
                }
            }
            
            return View("Views/Shared/Error.cshtml", new ErrorViewModel{message = "ERROR 400. No tiene autorización para ingresar a la página."});
        }
        catch(Exception ex)
        {
            _logger.LogError(ex.ToString());
            return View("Views/Shared/Error.cshtml", new ErrorViewModel{message = ex.Message});
        }
    }

    [HttpGet]
    public IActionResult CrearTablero(){

        if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToRoute(new{controller="Logueo", action="Index"});

        try
        {
            var usuario = _usuariosRepo.GetUsuario(Convert.ToInt32(HttpContext.Session.GetString("id")));
            return View(new TableroUsuarioViewModel{Usuario = new UsuarioViewModel(usuario)});
        }
        catch(Exception ex){
            _logger.LogError(ex.ToString());
            return View("Views/Shared/Error.cshtml", new ErrorViewModel{message=ex.Message});
        }
    }

    [HttpGet]
    public IActionResult ActualizarTablero(int idTablero){
        
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToRoute(new{controller="Logueo", action="Index"});
        
        var rolUsuarioAutenticado = HttpContext.Session.GetString("rol");

        try
        {
            var tablero = _tablerosRepo.GetTablero(idTablero);
            if(rolUsuarioAutenticado != Rol.Administrador.ToString() && tablero.IdUsuarioPropietario != Convert.ToInt32(HttpContext.Session.GetString("id"))) return View("Views/Shared/Error.cshtml", new ErrorViewModel{message = "ERROR 400. No tiene autorizacion para actualizar el tablero de otro usuario."});
            var usuarioPropietario = _usuariosRepo.GetUsuario(tablero.IdUsuarioPropietario);
            var tableroUsuario = new TableroUsuarioViewModel{Tablero = new TableroViewModel(tablero), Usuario = new UsuarioViewModel(usuarioPropietario)};
            return View(tableroUsuario);

        }catch(Exception ex)
        {
            _logger.LogError(ex.ToString());
            return RedirectToAction("Views/Shared/Error.cshtml", new ErrorViewModel{message = ex.Message});
        }
    }

    [HttpPost]
    public IActionResult CrearTablero(TableroUsuarioViewModel tableroUsuario){
        
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToRoute(new{controller="Logueo", action="Index"});
        
        if(!ModelState.IsValid) return RedirectToAction("CrearTablero");

        try
        {
            if(tableroUsuario.Tablero.IdUsuarioPropietario != Convert.ToInt32(HttpContext.Session.GetString("id"))) return RedirectToAction("Views/Shared/Error.cshtml", new ErrorViewModel{message = "ERROR 400. No puede crear un tablero para otro usuario"});
            var nuevoTablero = new Tablero(tableroUsuario.Tablero);
            _tablerosRepo.CrearTablero(nuevoTablero);
            return RedirectToAction("MisTableros");

        }catch(Exception ex)
        {
            _logger.LogError(ex.ToString());
            return View("Views/Shared/Error.cshtml", new ErrorViewModel{message = ex.Message});
        }
    }

    [HttpPost]
    public IActionResult ActualizarTablero(TableroUsuarioViewModel tableroUVM){
        
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToRoute(new{controller="Logueo", action="Index"});
                
        var rolUsuarioAutenticado = HttpContext.Session.GetString("rol");

        if(!ModelState.IsValid) return RedirectToAction("ActualizarTablero", new{idTablero = tableroUVM.Tablero.Id});

        try
        {
            if(rolUsuarioAutenticado != Rol.Administrador.ToString() && tableroUVM.Tablero.IdUsuarioPropietario != Convert.ToInt32(HttpContext.Session.GetString("id"))) return View("Views/Shared/Error.cshtml", new ErrorViewModel{message = "ERROR 400. No tiene autorizacion para actualizar el tablero de otro usuario."});
            var tableroActualizado = new Tablero(tableroUVM.Tablero);
            _tablerosRepo.ModificarTablero(tableroActualizado);
            
            if(tableroActualizado.IdUsuarioPropietario != Convert.ToInt32(HttpContext.Session.GetString("id"))) return RedirectToAction("ListarTablerosUsuario");

            return RedirectToAction("MisTableros"); 

        }catch(Exception ex)
        {
            _logger.LogError(ex.ToString());
            return View("Views/Shared/Error.cshtml", new ErrorViewModel{message = ex.Message});
        }
    }

    public IActionResult EliminarTablero(int idTablero){
        
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToRoute(new{controller="Logueo", action="Index"});

        var rolUsuarioAutenticado = HttpContext.Session.GetString("rol");

        try
        {
            var IdUsuarioPropietario = _tablerosRepo.GetTablero(idTablero).IdUsuarioPropietario;
            if(rolUsuarioAutenticado != Rol.Administrador.ToString() && IdUsuarioPropietario != Convert.ToInt32(HttpContext.Session.GetString("id"))) return View("Views/Shared/Error.cshtml", new ErrorViewModel{message = "ERROR 400. No tiene autorizacion para eliminar el tablero de otro usuario."});
            
            foreach(var tarea in _tareasRepo.GetTareasDeTablero(idTablero)) _tareasRepo.EliminarTarea(tarea.Id);
            _tablerosRepo.EliminarTablero(idTablero);
            
            if(IdUsuarioPropietario != Convert.ToInt32(HttpContext.Session.GetString("id"))) return RedirectToAction("ListarTablerosUsuario");

            return RedirectToAction("MisTableros");

        }catch(Exception ex)
        {
            _logger.LogError(ex.ToString());
            return View("Views/Shared/Error.cshtml", new ErrorViewModel{message = ex.Message});
        }
    }
}