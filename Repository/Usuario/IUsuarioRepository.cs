using tl2_tp10_2023_adanSmith01.Models;
namespace tl2_tp10_2023_adanSmith01.Repository;

public interface IUsuarioRepository
{
    void CrearUsuario(Usuario usuario);
    bool ExisteUsuario(string nombreUsuario);
    void ActualizarUsuario(Usuario usuario);
    List<Usuario> GetAllUsuarios();
    Usuario GetUsuario(int idUsuario);
    Usuario GetUsuario(string nombre, string contrasenia);
    void EliminarUsuario(int idUsuario);
    
}