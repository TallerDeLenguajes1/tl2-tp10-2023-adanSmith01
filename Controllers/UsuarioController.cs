using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp10_2023_adanSmith01.Models;
using tl2_tp10_2023_adanSmith01.Repository;
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
        var usuarios = usuarioRepo.GetAllUsuarios();
        return View(usuarios);
    }

    [HttpGet]
    public IActionResult CrearUsuario(){
        return View(new Usuario());
    }

    [HttpGet]
    public IActionResult ActualizarUsuario(int idUsuario){
        return View(usuarioRepo.GetUsuario(idUsuario));
    }

    [HttpPost]
    public IActionResult CrearUsuario(Usuario nuevo){
        usuarioRepo.CrearUsuario(nuevo);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult ActualizarUsuario(Usuario usuarioAModificar){
        usuarioRepo.ModificarUsuario(usuarioAModificar);
        return RedirectToAction("Index");
    }

    public IActionResult DarBaja(int idUsuario){
        usuarioRepo.EliminarUsuario(idUsuario);
        return RedirectToAction("Index");
    }
}
