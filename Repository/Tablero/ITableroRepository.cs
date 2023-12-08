using tl2_tp10_2023_adanSmith01.Models;
namespace tl2_tp10_2023_adanSmith01.Repository;

public interface ITableroRepository
{
    void CrearTablero(Tablero nuevoTablero);
    void ModificarTablero(Tablero tableroModificar);
    List<Tablero> GetAllTableros();
    Tablero GetTablero(int idTablero);
    List<Tablero> GetTablerosDeUsuario(int idUsuario);
    void EliminarTablero(int idTablero);
}