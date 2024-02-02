namespace tl2_tp10_2023_adanSmith01.ViewModels;
using tl2_tp10_2023_adanSmith01.Models;

public class ListaTablerosUsuarioViewModel
{
    private List<TableroViewModel> tablerosUsuario;
    private List<UsuarioViewModel> usuariosPropietarios;

    public ListaTablerosUsuarioViewModel(List<Tablero> tablerosUsuario, List<Usuario> usuariosPropietarios)
    {
        this.tablerosUsuario = new List<TableroViewModel>();
        this.usuariosPropietarios = new List<UsuarioViewModel>();
        foreach(var tablero in tablerosUsuario)
        {
            if(string.IsNullOrEmpty(tablero.Descripcion)) tablero.Descripcion = "Sin Descripcion";
            this.tablerosUsuario.Add(new TableroViewModel(tablero));
        }
        foreach(var usuario in usuariosPropietarios) this.usuariosPropietarios.Add(new UsuarioViewModel(usuario));
    }

    public List<TableroViewModel> TablerosUsuario { get => tablerosUsuario; }
    public List<UsuarioViewModel> UsuariosPropietarios { get => usuariosPropietarios; }
}