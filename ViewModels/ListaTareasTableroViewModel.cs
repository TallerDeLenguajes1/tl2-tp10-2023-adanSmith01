namespace tl2_tp10_2023_adanSmith01.ViewModels;
using tl2_tp10_2023_adanSmith01.Models;

public class ListaTareasTableroViewModel
{
    private string nombreTablero;
    private List<TareaViewModel> tareas;
    private List<UsuarioViewModel> usuarios;

    public string NombreTablero { get => nombreTablero; set => nombreTablero = value; }
    public List<TareaViewModel> Tareas { get => tareas; set => tareas = value; }
    public List<UsuarioViewModel> Usuarios { get => usuarios; set => usuarios = value; }

    public ListaTareasTableroViewModel(List<Tarea> tareas, List<Usuario> usuarios, string nombreTablero){
        this.usuarios = new List<UsuarioViewModel>();
        this.tareas = new List<TareaViewModel>();
        foreach(var tarea in tareas) this.tareas.Add(new TareaViewModel(tarea));
        foreach(var usuario in usuarios) this.usuarios.Add(new UsuarioViewModel(usuario));
        this.nombreTablero = nombreTablero;
    }
}