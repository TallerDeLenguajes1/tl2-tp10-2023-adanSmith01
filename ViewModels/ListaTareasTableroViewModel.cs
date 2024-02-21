namespace tl2_tp10_2023_adanSmith01.ViewModels;
using tl2_tp10_2023_adanSmith01.Models;

public class ListaTareasTableroViewModel
{
    private TableroViewModel tablero;
    private Dictionary<EstadoTarea, List<TareaViewModel>> tareasTableroPorEstado;
    private List<UsuarioViewModel> usuariosAsignados;
    private bool habilitarCrearTarea;

    public TableroViewModel Tablero { get => tablero; set => tablero = value; }
    public Dictionary<EstadoTarea, List<TareaViewModel>> TareasTableroPorEstado { get => tareasTableroPorEstado; set => tareasTableroPorEstado = value; }
    public List<UsuarioViewModel> Usuarios { get => usuariosAsignados; set => usuariosAsignados = value; }
    public bool HabilitarCrearTarea { get => habilitarCrearTarea; }

    public ListaTareasTableroViewModel(Tablero tablero, Dictionary<EstadoTarea, List<Tarea>> tareasTableroPorEstado, List<Usuario> usuariosAsignados,bool habilitarCrearTarea){
        this.usuariosAsignados = new List<UsuarioViewModel>();
        this.tareasTableroPorEstado = new Dictionary<EstadoTarea, List<TareaViewModel>>();
        this.tablero = new TableroViewModel(tablero);
        foreach(var tareasTablero in tareasTableroPorEstado)
        {
            var tareas = new List<TareaViewModel>();
            foreach(var tarea in tareasTablero.Value)
            {
                if(string.IsNullOrEmpty(tarea.Descripcion)) tarea.Descripcion = "Sin descripción";
                tareas.Add(new TareaViewModel(tarea));
            }
            this.tareasTableroPorEstado.Add(tareasTablero.Key, tareas);
        }
        foreach(var usuario in usuariosAsignados) this.usuariosAsignados.Add(new UsuarioViewModel(usuario));
        this.habilitarCrearTarea = habilitarCrearTarea;
    }
}