namespace tl2_tp10_2023_adanSmith01.ViewModels;
using tl2_tp10_2023_adanSmith01.Models;

public class ListaTareasAsignadasYNoAsignadasViewModel
{
    private TableroViewModel tableroCTA;
    private Dictionary<EstadoTarea, List<TareaViewModel>> tareasAsignadasYNoAsignadas;
    private UsuarioViewModel usuarioAsignado;

    public Dictionary<EstadoTarea, List<TareaViewModel>> TareasAsignadasYNoAsignadas { get => tareasAsignadasYNoAsignadas; }
    public UsuarioViewModel UsuarioAsignado { get => usuarioAsignado; }
    public TableroViewModel TableroCTA { get => tableroCTA; }

    public ListaTareasAsignadasYNoAsignadasViewModel(Tablero tableroCTA, Dictionary<EstadoTarea, List<Tarea>> tareasAsignadasYNoAsignadas, Usuario usuarioAsignado){
        this.tareasAsignadasYNoAsignadas = new Dictionary<EstadoTarea, List<TareaViewModel>>();
        this.usuarioAsignado = new UsuarioViewModel(usuarioAsignado);
        this.tableroCTA = new TableroViewModel(tableroCTA);
        foreach(var tareasTablero in tareasAsignadasYNoAsignadas)
        {
            var tareas = new List<TareaViewModel>();
            foreach(var tarea in tareasTablero.Value)
            {
                if(string.IsNullOrEmpty(tarea.Descripcion)) tarea.Descripcion = "Sin descripción";
                tareas.Add(new TareaViewModel(tarea));
            }
            this.tareasAsignadasYNoAsignadas.Add(tareasTablero.Key, tareas);
        } 
    }
}