using tl2_tp10_2023_adanSmith01.Models;
namespace tl2_tp10_2023_adanSmith01.Repository;

public interface IUsuarioRepository
{
    void CrearUsuario(Usuario nuevoUsuario);
    void ModificarUsuario(Usuario usuarioModificar);
    List<Usuario> GetAllUsuarios();
    Usuario GetUsuario(int idUsuario);
    void EliminarUsuario(int idUsuario);
}