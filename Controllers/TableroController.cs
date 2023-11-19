using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp10_2023_adanSmith01.Models;
using tl2_tp10_2023_adanSmith01.Repository;
namespace tl2_tp10_2023_adanSmith01.Controllers;

public class TableroController: Controller
{
    private readonly ILogger<TableroController> _logger;
    private ITableroRepository tableroRepo;

    public TableroController(ILogger<TableroController> logger)
    {
        _logger = logger;
        tableroRepo = new TableroRepository();
    }

    [HttpGet]
    public IActionResult CrearTablero(int idUsuario){
        if(!String.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))){
            ViewBag.IdUsuario = idUsuario;
            return View(new Tablero());
        }else{
            return RedirectToRoute(new {controller = "Logueo", action="Index"});
        }
    }

    [HttpGet]
    public IActionResult ActualizarTablero(int idTablero){
        if(!String.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return View(tableroRepo.GetTablero(idTablero));
        else return RedirectToRoute(new {controller = "Logueo", action="Index"});
    }

    [HttpGet]
    public IActionResult MisTableros(){
        if(!String.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))){
            var tableros = tableroRepo.GetTablerosDeUsuario(Convert.ToInt32(HttpContext.Session.GetString("id")));
            return View(tableros);
        }else{
            return RedirectToRoute(new {controller = "Logueo", action="Index"});
        }
    }

    [HttpGet]
    public IActionResult ListarTablerosUsuario(int idUsuario){
        if(!String.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))){
            var tableros = tableroRepo.GetTablerosDeUsuario(idUsuario);
            return View(tableros);
        }else{
            return RedirectToRoute(new {controller = "Logueo", action="Index"});
        }
        
    }

    [HttpPost]
    public IActionResult CrearTablero(Tablero nuevo){
        tableroRepo.CrearTablero(nuevo);
        return RedirectToAction("ListarTablerosUsuario", new{idUsuario = nuevo.IdUsuarioPropietario});
    }

    [HttpPost]
    public IActionResult ActualizarTablero(Tablero tableroAModificar){
        tableroRepo.ModificarTablero(tableroAModificar);
        return RedirectToAction("ListarTablerosUsuario", new{idUsuario = tableroAModificar.IdUsuarioPropietario});
    }

    public IActionResult EliminarTablero(int idTablero){
        var id = tableroRepo.GetTablero(idTablero).IdUsuarioPropietario;
        tableroRepo.EliminarTablero(idTablero);
        return RedirectToAction("ListarTablerosUsuario", new{idUsuario = id});
    }

    
}