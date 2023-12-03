namespace tl2_tp10_2023_adanSmith01.ViewModels;
using tl2_tp10_2023_adanSmith01.Models;

public class ListaTablerosUsuarioViewModel
{
    private bool permisoParaABM;
    private List<TableroViewModel> tableros;
    private List<UsuarioViewModel> usuarios;

    public List<TableroViewModel> Tableros { get => tableros; set => tableros = value; }
    public List<UsuarioViewModel> Usuarios { get => usuarios; set => usuarios = value; }
    public bool PermisoParaABM {get => permisoParaABM;}
    
    public ListaTablerosUsuarioViewModel(List<Tablero> tableros, List<Usuario> usuarios, bool permisoParaABM){
        this.tableros = new List<TableroViewModel>();
        this.usuarios = new List<UsuarioViewModel>();
        foreach(var tablero in tableros) this.tableros.Add(new TableroViewModel(tablero));
        foreach(var usuario in usuarios) this.usuarios.Add(new UsuarioViewModel(usuario));
        this.permisoParaABM = permisoParaABM;
    }

    public ListaTablerosUsuarioViewModel(List<Tablero> tableros, bool permisoParaABM){
        this.usuarios = new List<UsuarioViewModel>();
        this.tableros = new List<TableroViewModel>();
        foreach(var tablero in tableros) this.tableros.Add(new TableroViewModel(tablero));
        this.permisoParaABM = permisoParaABM;
    }
}