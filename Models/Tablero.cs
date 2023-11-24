using tl2_tp10_2023_adanSmith01.ViewModels;

namespace tl2_tp10_2023_adanSmith01.Models;

public class Tablero
{
    private int id;
    private int idUsuarioPropietario;
    private string nombre;
    private string descripcion;

    public int Id { get => id; set => id = value; }
    public int IdUsuarioPropietario { get => idUsuarioPropietario; set => idUsuarioPropietario = value; }
    public string Nombre { get => nombre; set => nombre = value; }
    public string Descripcion { get => descripcion; set => descripcion = value; }

    public Tablero(TableroViewModel tablero){
        this.id = tablero.Id;
        this.nombre = tablero.Nombre;
        this.descripcion = tablero.Descripcion;
        this.idUsuarioPropietario = tablero.IdUsuarioPropietario;
    }

    public Tablero(){
        
    }
}