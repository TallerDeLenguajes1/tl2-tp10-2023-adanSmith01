using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp10_2023_adanSmith01.Models;
using tl2_tp10_2023_adanSmith01.Repository;
using tl2_tp10_2023_adanSmith01.ViewModels;
namespace tl2_tp10_2023_adanSmith01.Controllers;

public class UsuarioController : Controller
{
    private readonly ILogger<UsuarioController> _logger;
    private readonly IUsuarioRepository _usuariosRepo;

    public UsuarioController(ILogger<UsuarioController> logger, IUsuarioRepository usuariosRepo)
    {
        _logger = logger;
        _usuariosRepo = usuariosRepo;
    }

    [HttpGet]
    public IActionResult ListarUsuarios(){
        try
        {
            if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToRoute(new{controller="Logueo", action="Index"});

            if(HttpContext.Session.GetString("rol") != Rol.Administrador.ToString()) return View("Views/Shared/Error.cshtml", new ErrorViewModel{message = "ERROR 400. No tiene autorizacion para ingresar a la pagina."});

            var usuarios = _usuariosRepo.GetAllUsuarios();
            return View(new MostrarUsuariosViewModel(usuarios));

        }catch(Exception ex)
        {
            _logger.LogError(ex.ToString());
            return View("Views/Shared/Error.cshtml", new ErrorViewModel{message = ex.Message});
        }
    }

    [HttpGet]
    public IActionResult CrearUsuario(){
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToRoute(new{controller="Logueo", action="Index"});
        if(HttpContext.Session.GetString("rol") != Rol.Administrador.ToString()) return View("Views/Shared/Error.cshtml", new ErrorViewModel{message="ERROR 400. No tiene autorizacion para ingresar a la pagina."});

        return View(new UsuarioViewModel());
    }

    [HttpGet]
    public IActionResult ActualizarUsuario(int idUsuario){

        if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToRoute(new{controller="Logueo", action="Index"});
        
        if(HttpContext.Session.GetString("rol") != Rol.Administrador.ToString()) return View("Views/Shared/Error.cshtml", new ErrorViewModel{message="ERROR 400. No tiene autorizacion para ingresar a la pagina."});
        
        try
        {
            var usuario = _usuariosRepo.GetUsuario(idUsuario);
            return View(new UsuarioViewModel(usuario));

        }catch(Exception ex)
        {
            _logger.LogError(ex.ToString());
            return View("Views/Shared/Error.cshtml", new ErrorViewModel{message = ex.Message});
        }
    }

    [HttpPost]
    public IActionResult CrearUsuario(UsuarioViewModel usuario){

        if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToRoute(new{controller="Logueo", action="Index"});
        
        if(HttpContext.Session.GetString("rol") != Rol.Administrador.ToString()) return View("Views/Shared/Error.cshtml", new ErrorViewModel{message="ERROR 400. No tiene autorizacion para ingresar a la pagina."});
        
        if(!ModelState.IsValid) return RedirectToAction("CrearUsuario");

        if(_usuariosRepo.ExisteUsuario(usuario.Nombre))
        {
            TempData["UsuarioExistente"] = "Ya existe un usuario con ese nombre. Por favor, elija otro nombre de usuario.";
            return RedirectToAction("CrearUsuario");
        }

        var nuevo = new Usuario(usuario);

        try
        {
            _usuariosRepo.CrearUsuario(nuevo);
            return RedirectToAction("ListarUsuarios");

        }catch(Exception ex)
        {
            _logger.LogError(ex.ToString());
            return View("Views/Shared/Error.cshtml", new ErrorViewModel{message = ex.Message});
        }
    }

    [HttpPost]
    public IActionResult ActualizarUsuario(UsuarioViewModel usuario){
        if(String.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToRoute(new{controller="Logueo", action="Index"});
        
        if(HttpContext.Session.GetString("rol") != Rol.Administrador.ToString()) return RedirectToRoute(new{controller="Logueo", action="Index"});
        
        if(!ModelState.IsValid) return RedirectToAction("ActualizarUsuario", new{idUsuario = usuario.Id});
        
        try
        {
            var usuarioAModificar = new Usuario(usuario);
            if(_usuariosRepo.ExisteUsuario(usuario.Nombre) && _usuariosRepo.GetUsuario(usuario.Id).NombreUsuario != usuario.Nombre)
            {
                TempData["UsuarioExistente"] = "Ya existe un usuario con ese nombre. Por favor, elija otro nombre de usuario.";
                return RedirectToAction("ActualizarUsuario", new{idUsuario = usuario.Id});
            }
            _usuariosRepo.ModificarUsuario(usuarioAModificar);
            return RedirectToAction("ListarUsuarios");

        }catch(Exception ex)
        {
            _logger.LogError(ex.ToString());
            return View("Views/Shared/Error.cshtml", new ErrorViewModel{message = ex.Message});
        }
    }

    public IActionResult EliminarUsuario(int idUsuario){
        
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToRoute(new{controller="Logueo", action="Index"});
        
        if(HttpContext.Session.GetString("rol") != Rol.Administrador.ToString()) return RedirectToRoute(new{controller="Logueo", action="Index"});

        try
        {
            _usuariosRepo.EliminarUsuario(idUsuario);
            return RedirectToAction("ListarUsuarios");

        }catch(Exception ex)
        {
            _logger.LogError(ex.ToString());
            return View("Views/Shared/Error.cshtml", new ErrorViewModel{message = ex.Message});
        }
    }
}
