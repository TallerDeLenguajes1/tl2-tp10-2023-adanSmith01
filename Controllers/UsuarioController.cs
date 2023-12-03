using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp10_2023_adanSmith01.Models;
using tl2_tp10_2023_adanSmith01.Repository;
using tl2_tp10_2023_adanSmith01.ViewModels;
namespace tl2_tp10_2023_adanSmith01.Controllers;

public class UsuarioController : Controller
{
    private readonly ILogger<UsuarioController> _logger;
    private IUsuarioRepository usuariosRepo;

    public UsuarioController(ILogger<UsuarioController> logger)
    {
        _logger = logger;
        usuariosRepo = new UsuarioRepository();
    }

    [HttpGet]
    public IActionResult Index(){
        if(String.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToRoute(new{controller="Logueo", action="Index"});
        if(HttpContext.Session.GetString("rol") != Rol.Administrador.ToString()) return RedirectToRoute(new{controller="Logueo", action="Index"});

        var usuarios = usuariosRepo.GetAllUsuarios();
        return View(new IndexUsuariosViewModel(usuarios));
    }

    [HttpGet]
    public IActionResult CrearUsuario(){
        if(String.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToRoute(new{controller="Logueo", action="Index"});
        if(HttpContext.Session.GetString("rol") != Rol.Administrador.ToString()) return RedirectToRoute(new{controller="Logueo", action="Index"});


        return View(new CrearUsuarioViewModel());
    }

    [HttpGet]
    public IActionResult ActualizarUsuario(int idUsuario){
        if(String.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToRoute(new{controller="Logueo", action="Index"});
        if(HttpContext.Session.GetString("rol") != Rol.Administrador.ToString()) return RedirectToRoute(new{controller="Logueo", action="Index"});


        var usuario = usuariosRepo.GetUsuario(idUsuario);
        return View(new UsuarioViewModel(usuario));
    }

    [HttpPost]
    public IActionResult CrearUsuario(CrearUsuarioViewModel usuario){
        if(String.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToRoute(new{controller="Logueo", action="Index"});
        if(HttpContext.Session.GetString("rol") != Rol.Administrador.ToString()) return RedirectToRoute(new{controller="Logueo", action="Index"});
        if(!ModelState.IsValid) return RedirectToAction("CrearUsuario");


        var nuevo = new Usuario(usuario.UsuarioNuevo);
        usuariosRepo.CrearUsuario(nuevo);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult ActualizarUsuario(UsuarioViewModel usuario){
        if(String.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToRoute(new{controller="Logueo", action="Index"});
        if(HttpContext.Session.GetString("rol") != Rol.Administrador.ToString()) return RedirectToRoute(new{controller="Logueo", action="Index"});
        if(!ModelState.IsValid) return RedirectToAction("ActualizarUsuario", new{idUsuario = usuario.Id});


        var usuarioAModificar = new Usuario(usuario);
        usuariosRepo.ModificarUsuario(usuarioAModificar);
        return RedirectToAction("Index");
    }

    public IActionResult DarBaja(int idUsuario){
        if(String.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToRoute(new{controller="Logueo", action="Index"});
        if(HttpContext.Session.GetString("rol") != Rol.Administrador.ToString()) return RedirectToRoute(new{controller="Logueo", action="Index"});

        usuariosRepo.EliminarUsuario(idUsuario);
        return RedirectToAction("Index");
    }
}
