namespace tl2_tp10_2023_adanSmith01.ViewModels;
using tl2_tp10_2023_adanSmith01.Models;

public class TableroViewModel
{
    private int id;
    private string nombre;
    private string descripcion;
    private int idUsuarioPropietario;
    private string nombrePropietario;

    public TableroViewModel(Tablero tablero, UsuarioViewModel usuarioVM){
        this.Id = tablero.Id;
        this.Nombre = tablero.Nombre;
        this.Descripcion = tablero.Descripcion;
        this.IdUsuarioPropietario = tablero.IdUsuarioPropietario;
        this.NombrePropietario = usuarioVM.Nombre;
    }

    public int Id { get => id; set => id = value; }
    public string Nombre { get => nombre; set => nombre = value; }
    public string Descripcion { get => descripcion; set => descripcion = value; }
    public int IdUsuarioPropietario { get => idUsuarioPropietario; set => idUsuarioPropietario = value; }
    public string NombrePropietario { get => nombrePropietario; set => nombrePropietario = value; }
}

