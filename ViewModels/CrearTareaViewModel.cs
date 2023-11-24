namespace tl2_tp10_2023_adanSmith01.ViewModels;
using tl2_tp10_2023_adanSmith01.Models;

public class CrearTareaViewModel
{
    private TareaViewModel tareaVM;
    private List<UsuarioViewModel> usuarios;
    private int idTablero;

    public TareaViewModel TareaVM { get => tareaVM; set => tareaVM = value; }
    public List<UsuarioViewModel> Usuarios { get => usuarios; set => usuarios = value; }
    public int IdTablero { get => idTablero; set => idTablero = value; }
    
    public CrearTareaViewModel(List<UsuarioViewModel> usuarios, int idTablero){
        this.usuarios = usuarios;
        this.idTablero = idTablero;
    }

    public CrearTareaViewModel(){

    }
}