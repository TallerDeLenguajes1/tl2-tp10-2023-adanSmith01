using tl2_tp10_2023_adanSmith01.Models;
namespace tl2_tp10_2023_adanSmith01.Repository;

public interface ITareaRepository
{
    void CrearTarea(int idTablero, Tarea tarea);
    void ModificarTarea(Tarea tarea);
    Tarea GetTarea(int idTarea);
    List<Tarea> GetTareasDeTablero(int idTablero);
    List<Tarea> GetTareasAsignadasAlUsuario(int idTablero, int idUsuario);
    List<Tarea> GetTareasNoAsignadasDelTablero(int idTablero);
    void DesasignarTareas(int idUsuario);
    void EliminarTarea(int idTarea);
}