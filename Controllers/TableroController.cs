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
        ViewBag.IdUsuario = idUsuario;
        return View(new Tablero());
    }

    [HttpGet]
    public IActionResult ActualizarTablero(int idTablero){
        return View(tableroRepo.GetTablero(idTablero));
    }

    [HttpGet]
    public IActionResult ListarTablerosUsuario(int idUsuario){
        var tableros = tableroRepo.GetTablerosDeUsuario(idUsuario);
        return View(tableros);
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