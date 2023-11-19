namespace tl2_tp10_2023_adanSmith01.ViewModels;

using System.ComponentModel.DataAnnotations;
using tl2_tp10_2023_adanSmith01.Models;

public class ActualizarUsuarioViewModel
{
    private int idUsuario;
    private string nombreUsuario;
    private string contraseniaUsuario;
    private Rol rolUsuario;

    public int IdUsuario {get => idUsuario; set => idUsuario = value;}

    [Required(ErrorMessage = "Este campo es requerido")]
    [Display(Name = "Nombre de usuario")]
    public string Nombre { get => nombreUsuario; set => nombreUsuario = value; }

    [Display(Name = "Contraseña")]
    public string Contrasenia { get => contraseniaUsuario; set => contraseniaUsuario = value; }

    [Display(Name = "Rol")]
    public Rol RolUsuario { get => rolUsuario; set => rolUsuario = value; }

    public ActualizarUsuarioViewModel(Usuario usuario){
        this.idUsuario = usuario.Id;
        this.nombreUsuario = usuario.NombreUsuario;
        this.contraseniaUsuario = usuario.Contrasenia;
        this.rolUsuario = usuario.RolUsuario;
    }

    public ActualizarUsuarioViewModel(){

    }
}