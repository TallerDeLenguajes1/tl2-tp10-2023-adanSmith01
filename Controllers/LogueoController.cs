using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp10_2023_adanSmith01.Models;
using tl2_tp10_2023_adanSmith01.Repository;
using tl2_tp10_2023_adanSmith01.ViewModels;
namespace tl2_tp10_2023_adanSmith01.Controllers;

public class LogueoController: Controller
{
    private readonly ILogger<LogueoController> _logger;
    private readonly IUsuarioRepository _usuariosRepo;

    public LogueoController(ILogger<LogueoController> logger, IUsuarioRepository usuariosRepo)
    {
        _logger = logger;
        _usuariosRepo = usuariosRepo;
    }

    [HttpGet]
    public IActionResult Index(){
        return View();
    }

    [HttpPost]
    public IActionResult ProcesoLogueo(LogueoViewModel logueoUsuario){
        try
        {
            if(!ModelState.IsValid) return RedirectToAction("Index");

            var usuarioLogueado = _usuariosRepo.GetUsuario(logueoUsuario.NombreUsuario, logueoUsuario.ContraseniaUsuario);

            LoguearUsuario(usuarioLogueado);

            _logger.LogInformation($"El usuario {usuarioLogueado.NombreUsuario} ingreso correctamente");

            return RedirectToRoute(new {controller = "Tablero", action = "ListarTableros"});

        }catch(Exception ex)
        {
            _logger.LogWarning($"Error: {ex} Intento de acceso invalido - Usuario: {logueoUsuario.NombreUsuario} - Clave ingresada: {logueoUsuario.ContraseniaUsuario}");
            
            return RedirectToAction("Index");
        }
    }

    private void LoguearUsuario(Usuario usuario){
        HttpContext.Session.SetString("id", usuario.Id.ToString());
        HttpContext.Session.SetString("usuario", usuario.NombreUsuario);
        HttpContext.Session.SetString("rol", usuario.RolUsuario.ToString());
    }
}
