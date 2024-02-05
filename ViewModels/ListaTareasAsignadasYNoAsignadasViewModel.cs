namespace tl2_tp10_2023_adanSmith01.ViewModels;
using tl2_tp10_2023_adanSmith01.Models;

public class ListaTareasAsignadasYNoAsignadasViewModel
{
    private TableroViewModel tableroCTA;
    private List<TareaViewModel> tareasAsignadasYNoAsignadas;
    private UsuarioViewModel usuarioAsignado;

    public List<TareaViewModel> TareasAsignadasYNoAsignadas { get => tareasAsignadasYNoAsignadas; }
    public UsuarioViewModel UsuarioAsignado { get => usuarioAsignado; }
    public TableroViewModel TableroCTA { get => tableroCTA; }

    public ListaTareasAsignadasYNoAsignadasViewModel(Tablero tableroCTA, List<Tarea> tareasAsignadasYNoAsignadas, Usuario usuarioAsignado){
        this.tareasAsignadasYNoAsignadas = new List<TareaViewModel>();
        this.usuarioAsignado = new UsuarioViewModel(usuarioAsignado);
        this.tableroCTA = new TableroViewModel(tableroCTA);
        foreach(var tarea in tareasAsignadasYNoAsignadas) this.tareasAsignadasYNoAsignadas.Add(new TareaViewModel(tarea)); 
    }
}