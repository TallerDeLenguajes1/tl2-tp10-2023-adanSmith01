using tl2_tp10_2023_adanSmith01.Models;
namespace tl2_tp10_2023_adanSmith01.ViewModels;

public class UsuarioViewModel
{
    private int id;
    private string nombre;
    private Rol rol;

    public UsuarioViewModel(Usuario usuario){
        this.Id = usuario.Id;
        this.Nombre = usuario.NombreUsuario;
        this.Rol = usuario.RolUsuario;
    }

    public int Id { get => id; set => id = value; }
    public string Nombre { get => nombre; set => nombre = value; }
    public Rol Rol { get => rol; set => rol = value; }
}