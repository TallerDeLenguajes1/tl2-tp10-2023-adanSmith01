using tl2_tp10_2023_adanSmith01.Models;
namespace tl2_tp10_2023_adanSmith01.Repository;

public interface ITareaRepository
{
    void CrearTarea(int idTablero, Tarea tarea);
    void ModificarTarea(Tarea tarea);
    Tarea GetTarea(int idTarea);
    List<Tarea> GetTareasDelTablero(int idTablero);
    List<Tarea> GetTareasDelTablero(int idTablero, EstadoTarea estado);
    List<Tarea> GetTareasAsignadasAlUsuario(int idTablero, int idUsuario);
    List<Tarea> GetTareasAsignadasAlUsuario(int idTablero, int idUsuario, EstadoTarea estado);
    List<Tarea> GetTareasNoAsignadasDelTablero(int idTablero);
    List<Tarea> GetTareasNoAsignadasDelTablero(int idTablero, EstadoTarea estado);
    void DesasignarTareas(int idUsuario);
    void EliminarTarea(int idTarea);
}