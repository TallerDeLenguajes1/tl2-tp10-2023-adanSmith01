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
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToRoute(new{controller="Logueo", action="Index"});

        try
        {
            var usuariosPropietarios = _usuariosRepo.GetAllUsuarios();
            if(HttpContext.Session.GetString("rol") == Rol.Administrador.ToString()){
                if(idUsuario == null){
                    var tableros = _tablerosRepo.GetAllTableros();
                    return View(new ListaTablerosUsuarioViewModel(tableros, usuariosPropietarios));
                }else{
                    var tableros = _tablerosRepo.GetTablerosDeUsuario((int)idUsuario);
                    var usuario = _usuariosRepo.GetUsuario((int)idUsuario);
                    return View(new ListaTablerosUsuarioViewModel(tableros, usuario));
                }
            }else{
                var usuario = _usuariosRepo.GetUsuario(Convert.ToInt32(HttpContext.Session.GetString("id")));
                var tablerosPropios = _tablerosRepo.GetTablerosDeUsuario(usuario.Id);
                var tablerosConYSinTareasAsignadas = _tablerosRepo.GetBoardsWithAssignedTasksByUser(usuario.Id);
                return View(new ListaTablerosUsuarioViewModel(tablerosPropios, usuario, tablerosConYSinTareasAsignadas, usuariosPropietarios));
            }

        }catch(Exception ex){
            _logger.LogError(ex.ToString());
            return RedirectToAction("Error");
        }
    }

    [HttpGet]
    public IActionResult CrearTablero(int? idUsuario){

        if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToRoute(new{controller="Logueo", action="Index"});
        var rolUsuarioAutenticado = HttpContext.Session.GetString("rol");
        Usuario usuario = new Usuario();

        try
        {
            if(rolUsuarioAutenticado == Rol.Administrador.ToString()){
                usuario = _usuariosRepo.GetUsuario((int)idUsuario);
            }else{
                if(idUsuario != null) return RedirectToAction("Error");
                usuario = _usuariosRepo.GetUsuario(Convert.ToInt32(HttpContext.Session.GetString("id")));
            }
            var tableroUsuario = new TableroUsuarioViewModel{Usuario = new UsuarioViewModel(usuario)};
            return View(tableroUsuario);

        }catch(Exception ex){
            _logger.LogError(ex.ToString());
            return RedirectToAction("Error");
        }
    }

    [HttpGet]
    public IActionResult ActualizarTablero(int idTablero){
        
        if(String.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToRoute(new{controller="Logueo", action="Index"});
        
        var rolUsuarioAutenticado = HttpContext.Session.GetString("rol");

        try
        {
            var tablero = _tablerosRepo.GetTablero(idTablero);

            if(rolUsuarioAutenticado != Rol.Administrador.ToString()) if(tablero.IdUsuarioPropietario != Convert.ToInt32(HttpContext.Session.GetString("id"))) return RedirectToAction("ListarTableros");
            var usuarioPropietario = _usuariosRepo.GetUsuario(tablero.IdUsuarioPropietario);
            var tableroUsuario = new TableroUsuarioViewModel{Tablero = new TableroViewModel(tablero), Usuario = new UsuarioViewModel(usuarioPropietario)};
            return View(tableroUsuario);

        }catch(Exception ex)
        {
            _logger.LogError(ex.ToString());
            return RedirectToAction("Error");
        }
    }

    [HttpPost]
    public IActionResult CrearTablero(TableroUsuarioViewModel nuevo){
        
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToRoute(new{controller="Logueo", action="Index"});
        
        if(!ModelState.IsValid) return RedirectToAction("ListarTableros");

        var rolUsuarioAutenticado = HttpContext.Session.GetString("rol");

        try
        {
            var nuevoTablero = new Tablero(nuevo.Tablero);
            if(rolUsuarioAutenticado != Rol.Administrador.ToString()) if(nuevoTablero.IdUsuarioPropietario != Convert.ToInt32(HttpContext.Session.GetString("id"))) return RedirectToAction("Error");
            _tablerosRepo.CrearTablero(nuevoTablero);
            return RedirectToAction("ListarTableros");

        }catch(Exception ex)
        {
            _logger.LogError(ex.ToString());
            return RedirectToAction("Error");
        }
    }

    [HttpPost]
    public IActionResult ActualizarTablero(TableroUsuarioViewModel tableroUVM){
        
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToRoute(new{controller="Logueo", action="Index"});
                
        if(!ModelState.IsValid) return RedirectToRoute("ListarTableros");

        var rolUsuarioAutenticado = HttpContext.Session.GetString("rol");

        try
        {
            var tableroActualizado = new Tablero(tableroUVM.Tablero);
            if(rolUsuarioAutenticado != Rol.Administrador.ToString()) if(tableroActualizado.IdUsuarioPropietario != Convert.ToInt32(HttpContext.Session.GetString("id"))) return RedirectToAction("Error");
            _tablerosRepo.ModificarTablero(tableroActualizado);
            return RedirectToAction("ListarTableros");

        }catch(Exception ex)
        {
            _logger.LogError(ex.ToString());
            return RedirectToAction("Error");
        }
    }

    public IActionResult EliminarTablero(int idTablero){
        
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToRoute(new{controller="Logueo", action="Index"});

        var rolUsuarioAutenticado = HttpContext.Session.GetString("rol");

        try
        {
            var IdUsuarioPropietario = _tablerosRepo.GetTablero(idTablero).IdUsuarioPropietario;
            if(rolUsuarioAutenticado != Rol.Administrador.ToString()) if(IdUsuarioPropietario != Convert.ToInt32(HttpContext.Session.GetString("id"))) return RedirectToAction("Error");
            _tablerosRepo.EliminarTablero(idTablero);
            return RedirectToAction("ListarTableros");

        }catch(Exception ex)
        {
            _logger.LogError(ex.ToString());
            return RedirectToAction("Error");
        }
    }

    public IActionResult Error(){
        return View(new ErrorViewModel());
    }
}