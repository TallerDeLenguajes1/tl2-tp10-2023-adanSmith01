using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp10_2023_adanSmith01.Models;
using tl2_tp10_2023_adanSmith01.Repository;
using tl2_tp10_2023_adanSmith01.ViewModels;
namespace tl2_tp10_2023_adanSmith01.Controllers;

public class UsuarioController : Controller
{
    private readonly ILogger<UsuarioController> _logger;
    private IUsuarioRepository usuarioRepo;

    public UsuarioController(ILogger<UsuarioController> logger)
    {
        _logger = logger;
        usuarioRepo = new UsuarioRepository();
    }

    [HttpGet]
    public IActionResult Index(){
        if(!String.IsNullOrEmpty(HttpContext.Session.GetString("usuario")) && (HttpContext.Session.GetString("rol")) == "Administrador"){
            var usuarios = usuarioRepo.GetAllUsuarios();
            return View(new IndexUsuariosViewModel(usuarios));
        }else{
            return RedirectToRoute(new {controller = "Logueo", action="Index"});
        }
    }

    [HttpGet]
    public IActionResult CrearUsuario(){
        if(!String.IsNullOrEmpty(HttpContext.Session.GetString("usuario")) && (HttpContext.Session.GetString("rol")) == Rol.Administrador.ToString()) return View(new CrearUsuarioViewModel());
        else return RedirectToRoute(new {controller = "Logueo", action="Index"});
    }

    [HttpGet]
    public IActionResult ActualizarUsuario(int idUsuario){
        if(!String.IsNullOrEmpty(HttpContext.Session.GetString("usuario")) && (HttpContext.Session.GetString("rol")) == Rol.Administrador.ToString()){
            var usuario = usuarioRepo.GetUsuario(idUsuario);
            if(!String.IsNullOrEmpty(usuario.NombreUsuario)){
                return View(new UsuarioViewModel(usuario));
            }// En caso contrario, se muestra un 404 Not Found
        }
        return RedirectToRoute(new {controller = "Logueo", action="Index"});
    }

    [HttpPost]
    public IActionResult CrearUsuario(CrearUsuarioViewModel usuario){
        var nuevo = new Usuario(usuario.UsuarioNuevo);
        usuarioRepo.CrearUsuario(nuevo);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult ActualizarUsuario(UsuarioViewModel usuario){
        var usuarioAModificar = new Usuario(usuario);
        usuarioRepo.ModificarUsuario(usuarioAModificar);
        return RedirectToAction("Index");
    }

    public IActionResult DarBaja(int idUsuario){
        usuarioRepo.EliminarUsuario(idUsuario);
        return RedirectToAction("Index");
    }
}
