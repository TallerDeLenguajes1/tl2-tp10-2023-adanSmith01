using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp10_2023_adanSmith01.Models;
using tl2_tp10_2023_adanSmith01.Repository;
using tl2_tp10_2023_adanSmith01.ViewModels;
namespace tl2_tp10_2023_adanSmith01.Controllers;

public class LogueoController: Controller
{
    private readonly ILogger<LogueoController> _logger;
    private IUsuarioRepository usuarioRepo;

    public LogueoController(ILogger<LogueoController> logger)
    {
        _logger = logger;
        usuarioRepo = new UsuarioRepository();
    }

    [HttpGet]
    public IActionResult Index(){
        return View();
    }

    [HttpPost]
    public IActionResult ProcesoLogueo(LogueoViewModel logueoUsuario){
        var usuarioLogueado = usuarioRepo.GetUsuario(logueoUsuario.NombreUsuario, logueoUsuario.ContraseniaUsuario);
        if(!String.IsNullOrEmpty(usuarioLogueado.NombreUsuario)){
            LoguearUsuario(usuarioLogueado);
            if(HttpContext.Session.GetString("rol") == Rol.Administrador.ToString()) return RedirectToRoute(new {controller = "Usuario", action = "Index"});
            else return RedirectToRoute(new {controller = "Tablero", action="MisTableros"});
        }else{
            return RedirectToAction("Index");
        }
    }

    private void LoguearUsuario(Usuario usuario){
        HttpContext.Session.SetString("id", usuario.Id.ToString());
        HttpContext.Session.SetString("usuario", usuario.NombreUsuario);
        HttpContext.Session.SetString("rol", usuario.RolUsuario.ToString());
    }
}
