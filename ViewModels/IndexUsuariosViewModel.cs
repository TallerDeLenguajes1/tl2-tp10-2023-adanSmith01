namespace tl2_tp10_2023_adanSmith01.ViewModels;
using tl2_tp10_2023_adanSmith01.Models;

public class IndexUsuariosViewModel
{
    private List<UsuarioViewModel> listaUsuariosVM;
    public IndexUsuariosViewModel(List<Usuario> usuarios){
        this.ListaUsuariosVM = new List<UsuarioViewModel>();
        foreach(var usuario in usuarios) ListaUsuariosVM.Add(new UsuarioViewModel(usuario));
    }

    public List<UsuarioViewModel> ListaUsuariosVM { get => listaUsuariosVM; set => listaUsuariosVM = value; }
}