using tl2_tp10_2023_adanSmith01.Models;
namespace tl2_tp10_2023_adanSmith01.Repository;

public interface ITareaRepository
{
    void CrearTarea(int idTablero, Tarea nuevaTarea);
    void ModificarTarea(Tarea tareaModificar);
    void AsignarUsuarioATarea(int idUsuario, int idTarea);
    Tarea GetTarea(int idTarea);
    List<Tarea> GetTareasDeUsuario(int idUsuario);
    List<Tarea> GetTareasDeTablero(int idTablero);
    //List<Tarea> GetTareasPorEstado(EstadoTarea estado);
    void EliminarTarea(int idTarea);
}