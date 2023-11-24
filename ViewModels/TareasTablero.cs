namespace tl2_tp10_2023_adanSmith01.ViewModels;
using tl2_tp10_2023_adanSmith01.Models;

public class TareasTableroViewModel
{
    private string nombreTablero;
    private List<TareaViewModel> tareasVM;
    private List<UsuarioViewModel> usuariosVM;

    public string NombreTablero { get => nombreTablero; set => nombreTablero = value; }
    public List<TareaViewModel> TareasVM { get => tareasVM; set => tareasVM = value; }
    public List<UsuarioViewModel> UsuariosVM { get => usuariosVM; set => usuariosVM = value; }

    public TareasTableroViewModel(List<Tarea> tareas, List<Usuario> usuarios, string nombreTablero){
        this.usuariosVM = new List<UsuarioViewModel>();
        this.tareasVM = new List<TareaViewModel>();
        foreach(var tarea in tareas) tareasVM.Add(new TareaViewModel(tarea));
        foreach(var usuario in usuarios) UsuariosVM.Add(new UsuarioViewModel(usuario));
        this.nombreTablero = nombreTablero;
    }
}