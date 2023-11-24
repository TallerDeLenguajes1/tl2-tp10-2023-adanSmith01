namespace tl2_tp10_2023_adanSmith01.ViewModels;
using tl2_tp10_2023_adanSmith01.Models;

public class TablerosUsuarioViewModel
{
    private string nombrePropietario;
    private List<TableroViewModel> tablerosUsuario;
    public TablerosUsuarioViewModel(List<Tablero> tableros, string nombrePropietario){
        this.tablerosUsuario = new List<TableroViewModel>();
        this.nombrePropietario = nombrePropietario;
        foreach(var tablero in tableros) tablerosUsuario.Add(new TableroViewModel(tablero));
    }

    public string NombrePropietario { get => nombrePropietario; set => nombrePropietario = value; }
    public List<TableroViewModel> TablerosUsuario { get => tablerosUsuario; set => tablerosUsuario = value; }
}