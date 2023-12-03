namespace tl2_tp10_2023_adanSmith01.ViewModels;
using System.ComponentModel.DataAnnotations;
using tl2_tp10_2023_adanSmith01.Models;

public class TableroUsuariosViewModel
{
    private TableroViewModel tablero;
    private List<UsuarioViewModel> usuarios;

    public TableroViewModel Tablero { get => tablero; set => tablero = value; }
    public List<UsuarioViewModel> Usuarios { get => usuarios; set => usuarios = value; }

    public TableroUsuariosViewModel(List<Usuario> usuarios){
        this.usuarios = new List<UsuarioViewModel>();
        foreach(var usuario in usuarios) this.usuarios.Add(new UsuarioViewModel(usuario));
    }

    public TableroUsuariosViewModel(Tablero tablero, List<Usuario> usuarios){
        this.tablero = new TableroViewModel(tablero);
        this.usuarios = new List<UsuarioViewModel>();
        foreach(var usuario in usuarios) this.usuarios.Add(new UsuarioViewModel(usuario));
    }

    public TableroUsuariosViewModel(){}
}