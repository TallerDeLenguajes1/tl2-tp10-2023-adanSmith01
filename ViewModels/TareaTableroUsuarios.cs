namespace tl2_tp10_2023_adanSmith01.ViewModels;
using tl2_tp10_2023_adanSmith01.Models;

public class TareaTableroUsuariosViewModel
{
    private int idTablero;
    private TareaViewModel tarea;
    private List<UsuarioViewModel> usuarios;

    public int IdTablero { get => idTablero; set => idTablero = value; }
    public TareaViewModel Tarea { get => tarea; set => tarea = value; }
    public List<UsuarioViewModel> Usuarios { get => usuarios; set => usuarios = value; }
    
    public TareaTableroUsuariosViewModel(List<Usuario> usuarios, int idTablero, EstadoTarea estado){
        this.usuarios = new List<UsuarioViewModel>();
        foreach(var usuario in usuarios) this.usuarios.Add(new UsuarioViewModel(usuario));
        this.tarea = new TareaViewModel(){Estado = estado};
        this.idTablero = idTablero;
    }

    public TareaTableroUsuariosViewModel(Tarea tarea, List<Usuario> usuarios){
        this.tarea = new TareaViewModel(tarea);
        this.usuarios = new List<UsuarioViewModel>();
        foreach(var usuario in usuarios) this.usuarios.Add(new UsuarioViewModel(usuario));
    }

    public TareaTableroUsuariosViewModel(){}
}