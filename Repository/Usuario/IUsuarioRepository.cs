using tl2_tp10_2023_adanSmith01.Models;
namespace tl2_tp10_2023_adanSmith01.Repository;

public interface IUsuarioRepository
{
    void CrearUsuario(Usuario nuevoUsuario);
    bool ExisteUsuario(string nombreUsuario);
    void ModificarUsuario(Usuario usuarioModificar);
    List<Usuario> GetAllUsuarios();
    Usuario GetUsuario(int idUsuario);
    Usuario GetUsuario(string nombre, string contrasenia);
    void EliminarUsuario(int idUsuario);
    
}