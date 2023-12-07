using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp10_2023_adanSmith01.Models;
using tl2_tp10_2023_adanSmith01.Repository;
using tl2_tp10_2023_adanSmith01.ViewModels;
namespace tl2_tp10_2023_adanSmith01.Controllers;

public class TableroController: Controller
{
    private readonly ILogger<TableroController> _logger;
    private readonly ITableroRepository _tablerosRepo;
    private readonly IUsuarioRepository _usuariosRepo;

    public TableroController(ILogger<TableroController> logger, ITableroRepository tablerosRepo, IUsuarioRepository usuariosRepo)
    {
        _logger = logger;
        _tablerosRepo = tablerosRepo;
        _usuariosRepo = usuariosRepo;
    }

    [HttpGet]
    public IActionResult ListarTableros(int? idUsuario){
        if(String.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToRoute(new{controller="Logueo", action="Index"});

        if(HttpContext.Session.GetString("rol") == Rol.Administrador.ToString()){
            if(idUsuario == null){
                var tableros = _tablerosRepo.GetAllTableros();
                var usuarios = _usuariosRepo.GetAllUsuarios();
                return View(new ListaTablerosUsuarioViewModel(tableros, usuarios, true));
            }else{
                var tableros = _tablerosRepo.GetTablerosDeUsuario((int)idUsuario);
                return View(new ListaTablerosUsuarioViewModel(tableros, true));
            }
        }else{
            var tableros = _tablerosRepo.GetTablerosDeUsuario(Convert.ToInt32(HttpContext.Session.GetString("id")));
            return View(new ListaTablerosUsuarioViewModel(tableros, false));
        }
    }

    [HttpGet]
    public IActionResult CrearTablero(){
        if(String.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToRoute(new{controller="Logueo", action="Index"});
        if(HttpContext.Session.GetString("rol") != Rol.Administrador.ToString()) return RedirectToRoute(new{controller="Logueo", action="Index"});


        var usuarios = _usuariosRepo.GetAllUsuarios();
        return View(new TableroUsuariosViewModel(usuarios));
    }

    [HttpGet]
    public IActionResult ActualizarTablero(int idTablero){
        if(String.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToRoute(new{controller="Logueo", action="Index"});
        if(HttpContext.Session.GetString("rol") != Rol.Administrador.ToString()) return RedirectToRoute(new{controller="Logueo", action="Index"});


        var tablero = _tablerosRepo.GetTablero(idTablero);
        var usuarios = _usuariosRepo.GetAllUsuarios();
        return View(new TableroUsuariosViewModel(tablero, usuarios));
    }

    [HttpPost]
    public IActionResult CrearTablero(TableroUsuariosViewModel nuevo){
        if(!ModelState.IsValid) return RedirectToRoute(new{controller="Logueo", action="Index"});
        if(String.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToRoute(new{controller="Logueo", action="Index"});
        if(HttpContext.Session.GetString("rol") != Rol.Administrador.ToString()) return RedirectToRoute(new{controller="Logueo", action="Index"});


        var nuevoTablero = new Tablero(nuevo.Tablero);
        _tablerosRepo.CrearTablero(nuevoTablero);
        return RedirectToAction("AllBoards");
    }

    [HttpPost]
    public IActionResult ActualizarTablero(TableroUsuariosViewModel tableroUVM){
        if(!ModelState.IsValid) return RedirectToRoute(new{controller="Logueo", action="Index"});
        if(String.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToRoute(new{controller="Logueo", action="Index"});
        if(HttpContext.Session.GetString("rol") != Rol.Administrador.ToString()) return RedirectToRoute(new{controller="Logueo", action="Index"});


        var tableroActualizado = new Tablero(tableroUVM.Tablero);
        _tablerosRepo.ModificarTablero(tableroActualizado);
        return RedirectToAction("AllBoards");
    }

    public IActionResult EliminarTablero(int idTablero){
        if(String.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToRoute(new{controller="Logueo", action="Index"});
        if(HttpContext.Session.GetString("rol") != Rol.Administrador.ToString()) return RedirectToRoute(new{controller="Logueo", action="Index"});

        var id = _tablerosRepo.GetTablero(idTablero).IdUsuarioPropietario;
        _tablerosRepo.EliminarTablero(idTablero);
        return RedirectToAction("AllBoards");
    }
}