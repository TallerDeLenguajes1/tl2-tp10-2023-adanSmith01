namespace tl2_tp10_2023_adanSmith01.ViewModels;
using tl2_tp10_2023_adanSmith01.Models;

public class MostrarUsuariosViewModel
{
    private List<UsuarioViewModel> listaUsuariosVM;

    public List<UsuarioViewModel> ListaUsuariosVM { get => listaUsuariosVM; }

    public MostrarUsuariosViewModel(List<Usuario> usuarios){
        this.listaUsuariosVM = new List<UsuarioViewModel>();
        foreach(var usuario in usuarios) this.listaUsuariosVM.Add(new UsuarioViewModel(usuario));
    }
}