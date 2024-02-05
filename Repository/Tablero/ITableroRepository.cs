using tl2_tp10_2023_adanSmith01.Models;
namespace tl2_tp10_2023_adanSmith01.Repository;

public interface ITableroRepository
{
    void CrearTablero(Tablero tablero);
    void ModificarTablero(Tablero tablero);
    List<Tablero> GetAllTableros();
    Tablero GetTablero(int idTablero);
    List<Tablero> GetTablerosDeUsuario(int idUsuario);
    List<Tablero> GetTablerosConTareasAsignadas(int idUsuario);
    void EliminarTablero(int idTablero);
}