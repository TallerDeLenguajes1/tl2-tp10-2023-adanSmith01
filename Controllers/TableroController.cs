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
    public IActionResult ListarTableros(string value){
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToRoute(new{controller="Logueo", action="Index"});

        try
        {
            int idUsuario;
            var usuariosPropietarios = _usuariosRepo.GetAllUsuarios();
            if(HttpContext.Session.GetString("rol") == Rol.Administrador.ToString()){
                if(!string.IsNullOrEmpty(value))
                {
                    if(value.ToLower() == "all")
                    {
                        var tableros = _tablerosRepo.GetAllTableros();
                        return View(new ListaTablerosUsuarioViewModel(tableros,usuariosPropietarios));
                    }else if(int.TryParse(value, out idUsuario))
                    {
                        var usuarioPropietario = _usuariosRepo.GetUsuario(idUsuario);
                        var tableros = _tablerosRepo.GetTablerosDeUsuario(usuarioPropietario.Id);
                        return View(new ListaTablerosUsuarioViewModel(tableros, usuarioPropietario));
                    }else
                    {
                        return RedirectToAction("Error");
                    }
                }else
                {
                    var usuario = _usuariosRepo.GetUsuario(Convert.ToInt32(HttpContext.Session.GetString("id")));
                    var tablerosPropios = _tablerosRepo.GetTablerosDeUsuario(usuario.Id);
                    var tablerosConYSinTareasAsignadas = _tablerosRepo.GetBoardsWithAssignedTasksByUser(usuario.Id);
                    return View(new ListaTablerosUsuarioViewModel(tablerosPropios, usuario, tablerosConYSinTareasAsignadas, usuariosPropietarios));
                }
                
            }else
            {
                if(!string.IsNullOrEmpty(value)) return RedirectToAction("Error");

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

        try
        {
            if(idUsuario != null && idUsuario != Convert.ToInt32(HttpContext.Session.GetString("id"))) return RedirectToRoute("Error");
            var usuario = _usuariosRepo.GetUsuario(Convert.ToInt32(HttpContext.Session.GetString("id")));
            var tableroUsuario = new TableroUsuarioViewModel{Usuario = new UsuarioViewModel(usuario)};
            return View(tableroUsuario);

        }catch(Exception ex){
            _logger.LogError(ex.ToString());
            return RedirectToAction("Error");
        }
    }

    [HttpGet]
    public IActionResult ActualizarTablero(int idTablero){
        
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToRoute(new{controller="Logueo", action="Index"});
        
        var rolUsuarioAutenticado = HttpContext.Session.GetString("rol");

        try
        {
            var tablero = _tablerosRepo.GetTablero(idTablero);

            if(rolUsuarioAutenticado != Rol.Administrador.ToString() && tablero.IdUsuarioPropietario != Convert.ToInt32(HttpContext.Session.GetString("id"))) return RedirectToAction("ListarTableros");
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
    public IActionResult CrearTablero(TableroUsuarioViewModel tableroUsuario){
        
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToRoute(new{controller="Logueo", action="Index"});
        
        var rolUsuarioAutenticado = HttpContext.Session.GetString("rol");

        if(!ModelState.IsValid)
        {
            if(rolUsuarioAutenticado != Rol.Administrador.ToString()) return RedirectToAction("ListarTableros");
            return RedirectToAction("ListarTableros", new{value="all"});
        } 

        try
        {
            if(tableroUsuario.Tablero.IdUsuarioPropietario != Convert.ToInt32(HttpContext.Session.GetString("id"))) return RedirectToAction("Error");
            var nuevoTablero = new Tablero(tableroUsuario.Tablero);
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
                
        var rolUsuarioAutenticado = HttpContext.Session.GetString("rol");

        if(!ModelState.IsValid)
        {
            if(rolUsuarioAutenticado != Rol.Administrador.ToString()) return RedirectToAction("ListarTableros");
            return RedirectToAction("ListarTableros", new{value="all"});
        }

        try
        {
            if(rolUsuarioAutenticado != Rol.Administrador.ToString() && tableroUVM.Tablero.IdUsuarioPropietario != Convert.ToInt32(HttpContext.Session.GetString("id"))) return RedirectToAction("Error");
            
            var tableroActualizado = new Tablero(tableroUVM.Tablero);
            
            _tablerosRepo.ModificarTablero(tableroActualizado);

            if(tableroActualizado.IdUsuarioPropietario != Convert.ToInt32(HttpContext.Session.GetString("id"))) return RedirectToAction("ListarTableros", new{value=tableroUVM.Tablero.IdUsuarioPropietario});
            
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
            
            if(rolUsuarioAutenticado != Rol.Administrador.ToString() && IdUsuarioPropietario != Convert.ToInt32(HttpContext.Session.GetString("id"))) return RedirectToAction("Error");
            
            _tablerosRepo.EliminarTablero(idTablero);

            if(IdUsuarioPropietario != Convert.ToInt32(HttpContext.Session.GetString("id"))) return RedirectToAction("ListarTableros", new{value=IdUsuarioPropietario});

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