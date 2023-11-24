namespace tl2_tp10_2023_adanSmith01.ViewModels;
using tl2_tp10_2023_adanSmith01.Models;

public class ActualizarTareaViewModel
{
    private TareaViewModel tareaVM;
    private List<UsuarioViewModel> usuarios;

    public TareaViewModel TareaVM { get => tareaVM; set => tareaVM = value; }
    public List<UsuarioViewModel> Usuarios { get => usuarios; set => usuarios = value; }

    public ActualizarTareaViewModel(Tarea tarea, List<Usuario> usuarios){
        this.tareaVM = new TareaViewModel(tarea);
        this.usuarios = new List<UsuarioViewModel>();
        foreach(var usuario in usuarios) this.usuarios.Add(new UsuarioViewModel(usuario)); 
    }

    public ActualizarTareaViewModel(){

    }
}