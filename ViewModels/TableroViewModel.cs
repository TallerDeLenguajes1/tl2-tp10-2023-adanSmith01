namespace tl2_tp10_2023_adanSmith01.ViewModels;
using System.ComponentModel.DataAnnotations;
using tl2_tp10_2023_adanSmith01.Models;

public class TableroViewModel
{
    private int id;
    private string nombre;
    private string descripcion;
    private int idUsuarioPropietario;
    public int Id { get => id; set => id = value; }

    [Required(ErrorMessage = "Este campo es requerido")]
    [Display(Name = "Nombre del tablero")]
    [StringLength(30)]
    public string Nombre { get => nombre; set => nombre = value; }

    [Display(Name = "Descripción del tablero")]
    [StringLength(200)]
    public string Descripcion { get => descripcion; set => descripcion = value; }

    [Display(Name = "Usuario Propietario")]
    public int IdUsuarioPropietario { get => idUsuarioPropietario; set => idUsuarioPropietario = value; }

    public TableroViewModel(Tablero tablero){
        this.id = tablero.Id;
        this.nombre = tablero.Nombre;
        this.descripcion = tablero.Descripcion;
        this.idUsuarioPropietario = tablero.IdUsuarioPropietario;
    }

    public TableroViewModel(){
        
    }
}