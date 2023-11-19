namespace tl2_tp10_2023_adanSmith01.ViewModels;

using System.ComponentModel.DataAnnotations;
using tl2_tp10_2023_adanSmith01.Models;

public class CrearUsuarioViewModel
{
    private string nombre;
    private string contrasenia;
    private Rol rolUsuario;

    [Required(ErrorMessage = "Este campo es requerido")]
    [Display(Name = "Nombre de usuario")]
    public string Nombre { get => nombre; set => nombre = value; }

    [Required(ErrorMessage = "Este campo es requerido")]
    [Display(Name = "Contraseña")]
    public string Contrasenia { get => contrasenia; set => contrasenia = value; }

    [Display(Name = "Rol")]
    public Rol RolUsuario { get => rolUsuario; set => rolUsuario = value; }
}