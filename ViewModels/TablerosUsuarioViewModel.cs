namespace tl2_tp10_2023_adanSmith01.ViewModels;
using tl2_tp10_2023_adanSmith01.Models;

public class TablerosUsuarioViewModel
{
    private bool mostrarIds;
    private UsuarioViewModel propietario;
    private List<TableroViewModel> tablerosUsuario;
    public TablerosUsuarioViewModel(List<Tablero> tableros, UsuarioViewModel propietario, bool mostrarIds){
        this.tablerosUsuario = new List<TableroViewModel>();
        this.propietario = propietario;
        foreach(var tablero in tableros) tablerosUsuario.Add(new TableroViewModel(tablero));
        this.mostrarIds = mostrarIds;
    }

    public UsuarioViewModel Propietario { get => propietario; set => propietario = value; }
    public List<TableroViewModel> TablerosUsuario { get => tablerosUsuario; set => tablerosUsuario = value; }
    public bool MostrarIds { get => mostrarIds; set => mostrarIds = value; }
}