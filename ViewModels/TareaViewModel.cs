namespace tl2_tp10_2023_adanSmith01.ViewModels;
using System.ComponentModel.DataAnnotations;
using tl2_tp10_2023_adanSmith01.Models;

public class TareaViewModel
{
    private int id;
    private int idTablero;
    private string nombre;
    private string descripcion;
    private string color;
    private EstadoTarea estado;
    private int? idUsuarioAsignado;

    public int Id { get => id; set => id = value; }
    public int IdTablero { get => idTablero; set => idTablero = value; }

    [Required(ErrorMessage = "Este campo es requerido")]
    [Display(Name = "Nombre de la tarea")]
    public string Nombre { get => nombre; set => nombre = value; }

    [Display(Name = "Descripción de la tarea")]
    public string Descripcion { get => descripcion; set => descripcion = value; }

    [Display(Name = "Color de la tarea")]
    public string Color { get => color; set => color = value; }

    [Display(Name = "Estado de la tarea")]
    public EstadoTarea Estado { get => estado; set => estado = value; }

    [Display(Name = "Usuario a asignar")]
    public int? IdUsuarioAsignado { get => idUsuarioAsignado; set => idUsuarioAsignado = value; }

    public TareaViewModel(Tarea tarea){
        this.id = tarea.Id;
        this.idTablero = tarea.IdTablero;
        this.nombre = tarea.Nombre;
        this.descripcion = tarea.Descripcion;
        this.color = tarea.Color;
        this.estado = tarea.Estado;
        this.idUsuarioAsignado = tarea.IdUsuarioAsignado;
    }

    public TareaViewModel(){

    }
}