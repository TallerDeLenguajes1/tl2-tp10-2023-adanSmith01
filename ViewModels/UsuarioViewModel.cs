using System.ComponentModel.DataAnnotations;
using tl2_tp10_2023_adanSmith01.Models;
namespace tl2_tp10_2023_adanSmith01.ViewModels;

public class UsuarioViewModel
{
    private int id;
    private string nombre;
    private string contrasenia;
    private Rol rol;

    public int Id { get => id; set => id = value; }

    [Required(ErrorMessage = "Este campo es requerido")]
    [Display(Name = "Nombre de usuario")]
    [StringLength(30)]
    public string Nombre { get => nombre; set => nombre = value; }


    [Display(Name = "Contraseña")]
    [MinLength(8, ErrorMessage = "Como minimo 8 caracteres")]
    public string Contrasenia { get => contrasenia; set => contrasenia = value; }

    [Required(ErrorMessage = "Este campo es requerido")]
    [Display(Name = "Rol del usuario")]
    public Rol Rol { get => rol; set => rol = value; }

    public UsuarioViewModel(Usuario usuario){
        this.Id = usuario.Id;
        this.Nombre = usuario.NombreUsuario;
        this.Rol = usuario.RolUsuario;
    }

    public UsuarioViewModel(){}
}