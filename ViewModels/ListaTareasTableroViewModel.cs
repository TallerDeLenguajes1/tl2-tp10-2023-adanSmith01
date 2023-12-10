namespace tl2_tp10_2023_adanSmith01.ViewModels;
using tl2_tp10_2023_adanSmith01.Models;

public class ListaTareasTableroViewModel
{
    private TableroViewModel tablero;
    private List<TareaViewModel> tareas;
    private List<UsuarioViewModel> usuarios;
    private bool permisoBM;
    private bool permisoModificarEstado;

    public TableroViewModel Tablero { get => tablero; set => tablero = value; }
    public List<TareaViewModel> Tareas { get => tareas; set => tareas = value; }
    public List<UsuarioViewModel> Usuarios { get => usuarios; set => usuarios = value; }
    public bool PermisoBM { get => permisoBM; }
    public bool PermisoModificarEstado { get => permisoModificarEstado; }

    public ListaTareasTableroViewModel(List<Tarea> tareas, List<Usuario> usuarios, Tablero tablero, bool permisoBM, bool permisoModificarEstado){
        this.usuarios = new List<UsuarioViewModel>();
        this.tareas = new List<TareaViewModel>();
        foreach(var tarea in tareas) this.tareas.Add(new TareaViewModel(tarea));
        foreach(var usuario in usuarios) this.usuarios.Add(new UsuarioViewModel(usuario));
        this.tablero = new TableroViewModel(tablero);
        this.permisoBM = permisoBM;
        this.permisoModificarEstado = permisoModificarEstado;
    }
}