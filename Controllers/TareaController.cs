using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp10_2023_adanSmith01.Models;
using tl2_tp10_2023_adanSmith01.Repository;
namespace tl2_tp10_2023_adanSmith01.Controllers;

public class TareaController : Controller
{
    private readonly ILogger<TareaController> _logger;
    private ITareaRepository tareasRepo;

    public TareaController(ILogger<TareaController> logger)
    {
        _logger = logger;
        tareasRepo = new TareaRepository();
    }

    [HttpGet]
    public IActionResult ListarTareasUsuario(int idUsuario){
        var tareas = tareasRepo.GetTareasDeUsuario(idUsuario);
        return View(tareas);
    }

    [HttpGet]
    public IActionResult ListarTareasTablero(int idTablero){
        var tareas = tareasRepo.GetTareasDeTablero(idTablero);
        return View(tareas);
    }

    [HttpGet]
    public IActionResult CrearTarea(int idTablero){
        ViewBag.IdTablero = idTablero;
        return View();
    }

    [HttpGet]
    public IActionResult ActualizarTarea(int idTarea){
        return View(tareasRepo.GetTarea(idTarea));
    }

    [HttpPost]
    public IActionResult CrearTarea(int idTablero, Tarea nueva){
        tareasRepo.CrearTarea(idTablero, nueva);
        return RedirectToAction("ListarTareasTablero", new{idTablero = idTablero});
    }

    [HttpPost]
    public IActionResult ActualizarTarea(Tarea tareaAModificar){
        tareasRepo.ModificarTarea(tareaAModificar);
        return RedirectToAction("ListarTareasTablero", new{idTablero = tareaAModificar.IdTablero});
    }

    public IActionResult EliminarTarea(int idTarea){
        var id = tareasRepo.GetTarea(idTarea).IdTablero;
        tareasRepo.EliminarTarea(idTarea);
        return RedirectToAction("ListarTareasTablero", new{idTablero = id});
    }

}