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
    public IActionResult Index(){
        try
        {
            if(String.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToRoute(new{controller="Logueo", action="Index"});

            if(HttpContext.Session.GetString("rol") != Rol.Administrador.ToString()) return RedirectToRoute(new{controller="Logueo", action="Index"});

            var usuarios = _usuariosRepo.GetAllUsuarios();

            return View(new IndexUsuariosViewModel(usuarios));

        }catch(Exception ex)
        {
            _logger.LogError(ex.ToString());
            return RedirectToAction("Error");
        }
    }

    [HttpGet]
    public IActionResult CrearUsuario(){
        if(String.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToRoute(new{controller="Logueo", action="Index"});
        if(HttpContext.Session.GetString("rol") != Rol.Administrador.ToString()) return RedirectToAction("Error");

        return View(new UsuarioViewModel());
    }

    [HttpGet]
    public IActionResult ActualizarUsuario(int idUsuario){
        try
        {

            if(String.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToRoute(new{controller="Logueo", action="Index"});
            if(HttpContext.Session.GetString("rol") != Rol.Administrador.ToString()) return RedirectToRoute(new{controller="Logueo", action="Index"});


            var usuario = _usuariosRepo.GetUsuario(idUsuario);
            return View(new UsuarioViewModel(usuario));

        }catch(Exception ex)
        {
            _logger.LogError(ex.ToString());
            return RedirectToAction("Index");
        }
    }

    [HttpPost]
    public IActionResult CrearUsuario(UsuarioViewModel usuario){
        try
        {

            if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToRoute(new{controller="Logueo", action="Index"});
            if(HttpContext.Session.GetString("rol") != Rol.Administrador.ToString()) return RedirectToRoute(new{controller="Logueo", action="Index"});
            if(!ModelState.IsValid) return RedirectToAction("CrearUsuario");


            var nuevo = new Usuario(usuario);
            _usuariosRepo.CrearUsuario(nuevo);
            return RedirectToAction("Index");

        }catch(Exception ex)
        {
            _logger.LogError(ex.ToString());
            return RedirectToAction("Error");
        }
    }

    [HttpPost]
    public IActionResult ActualizarUsuario(UsuarioViewModel usuario){
        try
        {

            if(String.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToRoute(new{controller="Logueo", action="Index"});
            if(HttpContext.Session.GetString("rol") != Rol.Administrador.ToString()) return RedirectToRoute(new{controller="Logueo", action="Index"});
            if(!ModelState.IsValid) return RedirectToAction("ActualizarUsuario", new{idUsuario = usuario.Id});


            var usuarioAModificar = new Usuario(usuario);
            _usuariosRepo.ModificarUsuario(usuarioAModificar);
            return RedirectToAction("Index");

        }catch(Exception ex)
        {
            _logger.LogError(ex.ToString());
            return RedirectToAction("Error");
        }
    }

    public IActionResult EliminarUsuario(int idUsuario){
        try
        {

            if(String.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToRoute(new{controller="Logueo", action="Index"});
            if(HttpContext.Session.GetString("rol") != Rol.Administrador.ToString()) return RedirectToRoute(new{controller="Logueo", action="Index"});

            _usuariosRepo.EliminarUsuario(idUsuario);
            return RedirectToAction("Index");

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
