using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace tl2_tp10_2023_adanSmith01.ViewModels;

public class LogueoViewModel
{
    private string nombreUsuario;
    private string contraseniaUsuario;

    [Required(ErrorMessage = "Este campo es requerido")]
    [Display(Name = "Nombre de usuario")]
    public string NombreUsuario { get => nombreUsuario; set => nombreUsuario = value; }
    
    [Required(ErrorMessage = "Este campo es requerido")]
    [Display(Name = "Contraseña")]
    public string ContraseniaUsuario { get => contraseniaUsuario; set => contraseniaUsuario = value; }
}