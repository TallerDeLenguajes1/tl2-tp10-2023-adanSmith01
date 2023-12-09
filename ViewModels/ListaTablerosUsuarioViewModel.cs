namespace tl2_tp10_2023_adanSmith01.ViewModels;
using tl2_tp10_2023_adanSmith01.Models;

public class ListaTablerosUsuarioViewModel
{
    private bool mostrarIdUsuario;
    private bool hacerABMTablero;
    private List<TableroViewModel> tablerosPropios;
    private List<TableroViewModel> tablerosConYSinTareasAsignadas;
    private List<UsuarioViewModel> usuariosPropietarios;
    private UsuarioViewModel usuarioPropietario;

    public List<TableroViewModel> TablerosPropios { get => tablerosPropios;}
    public List<UsuarioViewModel> UsuariosPropietarios { get => usuariosPropietarios;}
    public bool MostrarIdUsuario { get => mostrarIdUsuario;}
    public bool HacerABMTablero { get => hacerABMTablero;}
    public List<TableroViewModel> TablerosConYSinTareasAsignadas { get => tablerosConYSinTareasAsignadas; set => tablerosConYSinTareasAsignadas = value; }
    public UsuarioViewModel UsuarioPropietario { get => usuarioPropietario; set => usuarioPropietario = value; }

    public ListaTablerosUsuarioViewModel(List<Tablero> tablerosPropios, List<Usuario> usuariosPropietarios)
    {
        this.tablerosPropios = new List<TableroViewModel>();
        this.usuariosPropietarios = new List<UsuarioViewModel>();
        foreach(var tablero in tablerosPropios) this.tablerosPropios.Add(new TableroViewModel(tablero));
        foreach(var usuario in usuariosPropietarios) this.usuariosPropietarios.Add(new UsuarioViewModel(usuario));
        mostrarIdUsuario = true;
        hacerABMTablero = true;
    }

    public ListaTablerosUsuarioViewModel(List<Tablero> tablerosPropios, Usuario usuarioPropietario)
    {
        this.tablerosPropios = new List<TableroViewModel>();
        this.usuarioPropietario = new UsuarioViewModel(usuarioPropietario);
        foreach(var tablero in tablerosPropios) this.tablerosPropios.Add(new TableroViewModel(tablero));
        mostrarIdUsuario = true;
        hacerABMTablero = true;
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
        mostrarIdUsuario = false;
        hacerABMTablero = true;
    }


}