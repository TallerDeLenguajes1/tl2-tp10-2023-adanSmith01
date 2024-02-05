namespace tl2_tp10_2023_adanSmith01.ViewModels;
using tl2_tp10_2023_adanSmith01.Models;

public class ListaTareasTableroViewModel
{
    private TableroViewModel tablero;
    private List<TareaViewModel> tareasTablero;
    private List<UsuarioViewModel> usuariosAsignados;

    public TableroViewModel Tablero { get => tablero; set => tablero = value; }
    public List<TareaViewModel> TareasTablero { get => tareasTablero; set => tareasTablero = value; }
    public List<UsuarioViewModel> Usuarios { get => usuariosAsignados; set => usuariosAsignados = value; }

    public ListaTareasTableroViewModel(Tablero tablero, List<Tarea> tareasTablero, List<Usuario> usuariosAsignados){
        this.usuariosAsignados = new List<UsuarioViewModel>();
        this.tareasTablero = new List<TareaViewModel>();
        this.tablero = new TableroViewModel(tablero);
        foreach(var tarea in tareasTablero) 
        {
            if(string.IsNullOrEmpty(tarea.Descripcion)) tarea.Descripcion = "Sin descripcion";
            this.tareasTablero.Add(new TareaViewModel(tarea));
        }
        foreach(var usuario in usuariosAsignados) this.usuariosAsignados.Add(new UsuarioViewModel(usuario));
    }
}