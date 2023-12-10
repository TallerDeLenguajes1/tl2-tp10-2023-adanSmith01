namespace tl2_tp10_2023_adanSmith01.ViewModels;
using tl2_tp10_2023_adanSmith01.Models;

public class ListaTablerosUsuarioViewModel
{
    private bool habilitarEnlace;
    private bool hacerBMTablero;
    private List<TableroViewModel> tablerosPropios;
    private List<TableroViewModel> tablerosConYSinTareasAsignadas;
    private List<UsuarioViewModel> usuariosPropietarios;
    private UsuarioViewModel usuarioPropietario;

    public List<TableroViewModel> TablerosPropios { get => tablerosPropios;}
    public List<UsuarioViewModel> UsuariosPropietarios { get => usuariosPropietarios;}
    public bool HabilitarEnlace { get => habilitarEnlace;}
    public bool HacerBMTablero { get => hacerBMTablero;}
    public List<TableroViewModel> TablerosConYSinTareasAsignadas { get => tablerosConYSinTareasAsignadas; set => tablerosConYSinTareasAsignadas = value; }
    public UsuarioViewModel UsuarioPropietario { get => usuarioPropietario; set => usuarioPropietario = value; }

    public ListaTablerosUsuarioViewModel(List<Tablero> tablerosPropios, List<Usuario> usuariosPropietarios)
    {
        this.tablerosPropios = new List<TableroViewModel>();
        this.usuariosPropietarios = new List<UsuarioViewModel>();
        this.tablerosConYSinTareasAsignadas = new List<TableroViewModel>();
        foreach(var tablero in tablerosPropios) this.tablerosPropios.Add(new TableroViewModel(tablero));
        foreach(var usuario in usuariosPropietarios) this.usuariosPropietarios.Add(new UsuarioViewModel(usuario));
        habilitarEnlace = false;
        hacerBMTablero = true;
    }

    public ListaTablerosUsuarioViewModel(List<Tablero> tablerosPropios, Usuario usuarioPropietario)
    {
        this.tablerosPropios = new List<TableroViewModel>();
        this.usuariosPropietarios = new List<UsuarioViewModel>();
        this.usuarioPropietario = new UsuarioViewModel(usuarioPropietario);
        this.tablerosConYSinTareasAsignadas = new List<TableroViewModel>();
        foreach(var tablero in tablerosPropios) this.tablerosPropios.Add(new TableroViewModel(tablero));
        habilitarEnlace = false;
        hacerBMTablero = true;
    }

    public ListaTablerosUsuarioViewModel(List<Tablero> tablerosPropios, Usuario usuarioPropietario, List<Tablero> tablerosConYSinTareasAsignadas, List<Usuario> usuariosPropietarios)
    {
        this.tablerosPropios = new List<TableroViewModel>();
        this.tablerosConYSinTareasAsignadas = new List<TableroViewModel>();
        this.usuariosPropietarios = new List<UsuarioViewModel>();
        this.usuarioPropietario = new UsuarioViewModel(usuarioPropietario);
        foreach(var tablero in tablerosPropios) this.tablerosPropios.Add(new TableroViewModel(tablero));
        foreach(var tablero in tablerosConYSinTareasAsignadas) this.tablerosConYSinTareasAsignadas.Add(new TableroViewModel(tablero));
        foreach(var usuario in usuariosPropietarios) this.usuariosPropietarios.Add(new UsuarioViewModel(usuario));
        habilitarEnlace = true;
        hacerBMTablero = true;
    }


}