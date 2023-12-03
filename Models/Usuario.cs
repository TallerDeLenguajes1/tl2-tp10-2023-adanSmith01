using tl2_tp10_2023_adanSmith01.ViewModels;

namespace tl2_tp10_2023_adanSmith01.Models;

public enum Rol
{
    Administrador,
    Operador
}

public class Usuario
{
    private int id;
    private string nombreUsuario;
    private string contrasenia;
    private Rol rolUsuario;
    
    public int Id { get => id; set => id = value; }
    public string NombreUsuario { get => nombreUsuario; set => nombreUsuario = value; }
    public string Contrasenia { get => contrasenia; set => contrasenia = value; }
    public Rol RolUsuario { get => rolUsuario; set => rolUsuario = value; }

    public Usuario(UsuarioViewModel usuario){
        this.id = usuario.Id;
        this.nombreUsuario = usuario.Nombre;
        this.contrasenia = usuario.Contrasenia;
        this.rolUsuario = usuario.Rol;
    }

    public Usuario(){}
}