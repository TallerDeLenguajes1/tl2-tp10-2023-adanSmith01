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
    public IActionResult Index(){
        var tableros = tableroRepo.GetAllTableros();
        return View(tableros);
    }

    [HttpGet]
    public IActionResult CrearTablero(){
        return View(new Tablero());
    }

    [HttpGet]
    public IActionResult ActualizarTablero(int idTablero){
        return View(tableroRepo.GetTablero(idTablero));
    }

    [HttpPost]
    public IActionResult CrearTablero(Tablero nuevo){
        tableroRepo.CrearTablero(nuevo);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult ActualizarTablero(Tablero tableroAModificar){
        tableroRepo.ModificarTablero(tableroAModificar);
        return RedirectToAction("Index");
    }

    public IActionResult EliminarTablero(int idTablero){
        tableroRepo.EliminarTablero(idTablero);
        return RedirectToAction("Index");
    }
}