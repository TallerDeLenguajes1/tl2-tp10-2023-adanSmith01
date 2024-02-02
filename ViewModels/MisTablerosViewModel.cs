namespace tl2_tp10_2023_adanSmith01.ViewModels;
using tl2_tp10_2023_adanSmith01.Models;

public class MisTablerosViewModel
{
    private List<TableroViewModel> tablerosPropios;
    private List<TableroViewModel> tablerosConTareasAsignadas;
    private List<UsuarioViewModel> usuariosPropietarios;

    public MisTablerosViewModel(List<Tablero> tablerosPropios, List<Tablero> tablerosConTareasAsignadas, List<Usuario> usuariosPropietarios)
    {
        this.tablerosPropios = new List<TableroViewModel>();
        this.tablerosConTareasAsignadas = new List<TableroViewModel>();
        this.usuariosPropietarios = new List<UsuarioViewModel>();
        foreach(var tableroPropio in tablerosPropios) this.tablerosPropios.Add(new TableroViewModel(tableroPropio));
        foreach(var tableroCTA in tablerosConTareasAsignadas) this.tablerosConTareasAsignadas.Add(new TableroViewModel(tableroCTA));
        foreach(var usuarioPropietario in usuariosPropietarios) this.usuariosPropietarios.Add(new UsuarioViewModel(usuarioPropietario));
    }

    public List<TableroViewModel> TablerosPropios { get => tablerosPropios; }
    public List<TableroViewModel> TablerosConTareasAsignadas { get => tablerosConTareasAsignadas; }
    public List<UsuarioViewModel> UsuariosPropietarios { get => usuariosPropietarios; }
}