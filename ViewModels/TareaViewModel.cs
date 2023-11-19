namespace tl2_tp10_2023_adanSmith01.ViewModels;
using tl2_tp10_2023_adanSmith01.Models;

public class TareaViewModel
{
    private int id;
    private string nombre;
    private string descripcion;
    private string color;
    private EstadoTarea estado;
    private int idTablero;
    private string nombreTablero;
    private int? idUsuarioAsignado; 
    private string nombreUsuarioAsignado;

    public TareaViewModel(Tarea tarea, TableroViewModel tableroVM, UsuarioViewModel usuarioVM){
        this.Id = tarea.Id;
        this.nombre = tarea.Nombre;
        this.Descripcion = tarea.Descripcion;
        this.Color = tarea.Color;
        this.Estado = tarea.Estado;
        this.IdTablero = tableroVM.Id;
        this.NombreTablero = tableroVM.Nombre;
        this.IdUsuarioAsignado = usuarioVM.Id;
        this.NombreUsuarioAsignado = usuarioVM.Nombre;
    }

    public int Id { get => id; set => id = value; }
    public string Nombre { get => nombre; set => nombre = value; }
    public string Descripcion { get => descripcion; set => descripcion = value; }
    public string Color { get => color; set => color = value; }
    public EstadoTarea Estado { get => estado; set => estado = value; }
    public int IdTablero { get => idTablero; set => idTablero = value; }
    public string NombreTablero { get => nombreTablero; set => nombreTablero = value; }
    public int? IdUsuarioAsignado { get => idUsuarioAsignado; set => idUsuarioAsignado = value; }
    public string NombreUsuarioAsignado { get => nombreUsuarioAsignado; set => nombreUsuarioAsignado = value; }
}