using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp10_2023_adanSmith01.Models;
using tl2_tp10_2023_adanSmith01.Repository;
using tl2_tp10_2023_adanSmith01.ViewModels;
namespace tl2_tp10_2023_adanSmith01.Controllers;

public class TableroController: Controller
{
    private readonly ILogger<TableroController> _logger;
    private ITableroRepository tableroRepo;
    private IUsuarioRepository usuarioRepo;

    public TableroController(ILogger<TableroController> logger)
    {
        _logger = logger;
        tableroRepo = new TableroRepository();
        usuarioRepo = new UsuarioRepository();
    }

    [HttpGet]
    public IActionResult CrearTablero(int? idUsuario){
        if(!String.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))){
            if(idUsuario == null){
                return View(new CrearTableroViewModel(Convert.ToInt32(HttpContext.Session.GetString("id"))));
            }else{
                var usuario = usuarioRepo.GetUsuario((int)idUsuario);
                if(!String.IsNullOrEmpty(usuario.NombreUsuario)){
                    return View(new CrearTableroViewModel((int)idUsuario));
                }else{
                    return RedirectToRoute(new {controller = "Logueo", action="Index"});//Debe retornar un 404
                }
            }
        }else{
            return RedirectToRoute(new {controller = "Logueo", action="Index"});
        }
    }

    [HttpGet]
    public IActionResult ActualizarTablero(int idTablero){
        if(!String.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))){
            var tablero = tableroRepo.GetTablero(idTablero);
            if(!String.IsNullOrEmpty(tablero.Nombre)){
                return View(new TableroViewModel(tablero));
            }else{
                return RedirectToRoute(new {controller = "Logueo", action="Index"});//Debe retornar un 404
            }
        }else{
            return RedirectToRoute(new {controller = "Logueo", action="Index"});
        }
    }

    [HttpGet]
    public IActionResult ListarTableros(int? idUsuario){
        if(!String.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))){
            List<Tablero> tableros = new List<Tablero>();
            if(idUsuario == null){
                tableros = tableroRepo.GetTablerosDeUsuario(Convert.ToInt32(HttpContext.Session.GetString("id")));
                return View(new TablerosUsuarioViewModel(tableros, new UsuarioViewModel(), false));
            }else{
                var usuario = usuarioRepo.GetUsuario((int)idUsuario);
                if(!String.IsNullOrEmpty(usuario.NombreUsuario)){
                    var usuarioVM = new UsuarioViewModel(usuario);
                    tableros = tableroRepo.GetTablerosDeUsuario((int)idUsuario);
                    return View(new TablerosUsuarioViewModel(tableros, usuarioVM, true));
                }else{
                    return RedirectToRoute(new {controller = "Logueo", action="Index"}); // Puede redireccionar a un 404
                }
            }
        }else{
            return RedirectToRoute(new {controller = "Logueo", action="Index"});
        }
        
    }

    [HttpPost]
    public IActionResult CrearTablero(CrearTableroViewModel nuevo){
        if(ModelState.IsValid){
            var nuevoTablero = new Tablero(nuevo.TableroVM);
            tableroRepo.CrearTablero(nuevoTablero);
            if(nuevo.IdUsuarioPropietario == Convert.ToInt32(HttpContext.Session.GetString("id"))) return RedirectToAction("ListarTableros", null);
            return RedirectToAction("ListarTableros", new{idUsuario = nuevoTablero.IdUsuarioPropietario});
        }
        return RedirectToRoute(new{controller = "Logueo", action="Index"});
    }

    [HttpPost]
    public IActionResult ActualizarTablero(TableroViewModel tableroAModificar){
        if(ModelState.IsValid){
            var tableroActualizado = new Tablero(tableroAModificar);
            tableroRepo.ModificarTablero(tableroActualizado);
            return RedirectToAction("ListarTableros", new{idUsuario = tableroActualizado.IdUsuarioPropietario});
        }
        return RedirectToRoute(new{controller="Logueo", action="Index"});
    }

    public IActionResult EliminarTablero(int idTablero){
        var id = tableroRepo.GetTablero(idTablero).IdUsuarioPropietario;
        tableroRepo.EliminarTablero(idTablero);
        return RedirectToAction("ListarTableros", new{idUsuario = id});
    }

    
}