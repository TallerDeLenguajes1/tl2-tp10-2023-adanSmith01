using System.Data.SQLite;
using tl2_tp10_2023_adanSmith01.Models;
namespace tl2_tp10_2023_adanSmith01.Repository;

public class TareaRepository: ITareaRepository
{
    private string connectionString = @"Data Source = DB/kanban.sqlite;Initial Catalog=Northwind;" + "Integrated Security=true";

    public void CrearTarea(int idTablero, Tarea nuevaTarea){
        using(var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            
            string queryString = @"
            INSERT INTO Tarea (id_tablero, nombre, estado, descripcion, color, id_usuario_asignado) 
            VALUES (@idTablero, @nombreTarea, @estadoTarea, @descripcionTarea, @colorTarea, @idUsuarioAsignado);
            ";
            var command = new SQLiteCommand(queryString, connection);

            command.Parameters.Add(new SQLiteParameter("@idTablero", idTablero));
            command.Parameters.Add(new SQLiteParameter("@nombreTarea", nuevaTarea.Nombre));
            command.Parameters.Add(new SQLiteParameter("@estadoTarea", Convert.ToInt32(nuevaTarea.Estado)));
            command.Parameters.Add(new SQLiteParameter("@descripcionTarea", nuevaTarea.Descripcion));
            command.Parameters.Add(new SQLiteParameter("@colorTarea", nuevaTarea.Color));
            command.Parameters.Add(new SQLiteParameter("@idUsuarioAsignado", nuevaTarea.IdUsuarioAsignado));

            command.ExecuteNonQuery();
            connection.Close();
        }
    }

    public void ModificarTarea(Tarea tareaAModificar){
        using(var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            string queryString = @"
            UPDATE Tarea 
            SET id_tablero = @idTablero, nombre = @nombreNuevo, estado = @estadoNuevo, 
            descripcion = @descripcionNueva, color = @colorNuevo, id_usuario_asignado = @idUsuarioAsignadoNuevo
            WHERE id = @idTarea;";
            var command = new SQLiteCommand(queryString, connection);

            command.Parameters.Add(new SQLiteParameter("@idTablero", tareaAModificar.IdTablero));
            command.Parameters.Add(new SQLiteParameter("@nombreNuevo", tareaAModificar.Nombre));
            command.Parameters.Add(new SQLiteParameter("@estadoNuevo", Convert.ToInt32(tareaAModificar.Estado)));
            command.Parameters.Add(new SQLiteParameter("@descripcionNueva", tareaAModificar.Descripcion));
            command.Parameters.Add(new SQLiteParameter("@colorNuevo", tareaAModificar.Color));
            command.Parameters.Add(new SQLiteParameter("@idUsuarioAsignadoNuevo", tareaAModificar.IdUsuarioAsignado));
            command.Parameters.Add(new SQLiteParameter("@idTarea", tareaAModificar.Id));

            command.ExecuteNonQuery();
            connection.Close();
        }
    }
    
    public void AsignarUsuarioATarea(int idUsuario, int idTarea){
        using(var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            string queryString = @"UPDATE Tarea SET id_usuario_asignado = @idUsuario WHERE id = @idTarea;";
            var command = new SQLiteCommand(queryString, connection);

            command.Parameters.Add(new SQLiteParameter("@idUsuario", idUsuario));
            command.Parameters.Add(new SQLiteParameter("@idTarea", idTarea));

            command.ExecuteNonQuery();
            connection.Close();
        }
    }

    public Tarea? GetTarea(int idTarea){
        Tarea tareaEncontrada = null;
        using(var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            string queryString = @"SELECT * FROM Tarea WHERE id = @idTarea;";
            
            var command = new SQLiteCommand(queryString, connection);
            command.Parameters.Add(new SQLiteParameter("@idTarea", idTarea));
            using(var reader = command.ExecuteReader())
            {
                if(reader.Read()){
                    tareaEncontrada = new Tarea
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        IdTablero = Convert.ToInt32(reader["id_tablero"]),
                        Nombre = reader["nombre"].ToString(),
                        Estado = (EstadoTarea)Convert.ToInt32(reader["estado"]),
                        Descripcion = reader["descripcion"].ToString(),
                        Color = reader["color"].ToString(),
                        IdUsuarioAsignado = (reader["id_usuario_asignado"] == DBNull.Value) ? (int?)null : Convert.ToInt32(reader["id_usuario_asignado"])
                    };
                }
            }

            connection.Close();
        }

        return tareaEncontrada;
    }

    public List<Tarea> GetTareasDeUsuario(int idUsuario){
        List<Tarea> tareasUsuario = new List<Tarea>();
        using(var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            string queryString = @"SELECT * FROM Tarea WHERE id_usuario_asignado = @idUsuarioAsignado";
            var command = new SQLiteCommand(queryString, connection);
            command.Parameters.Add(new SQLiteParameter("@idUsuarioAsignado", idUsuario));

            using(var reader = command.ExecuteReader())
            {
                while(reader.Read()){
                    Tarea tarea = new Tarea
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        IdTablero = Convert.ToInt32(reader["id_tablero"]),
                        Nombre = reader["nombre"].ToString(),
                        Estado = (EstadoTarea)Convert.ToInt32(reader["estado"]),
                        Descripcion = reader["descripcion"].ToString(),
                        Color = reader["color"].ToString(),
                        IdUsuarioAsignado = (reader["id_usuario_asignado"] == DBNull.Value) ? (int?)null : Convert.ToInt32(reader["id_usuario_asignado"])
                    };
                    tareasUsuario.Add(tarea);
                }
            }
            connection.Close();
        }

        return tareasUsuario;
    }

    public List<Tarea> GetTareasDeTablero(int idTablero){
        List<Tarea> tareasTablero = new List<Tarea>();
        using(var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            string queryString = @"SELECT * FROM Tarea WHERE id_tablero = @idTablero;";
            var command = new SQLiteCommand(queryString, connection);
            command.Parameters.Add(new SQLiteParameter("@idTablero", idTablero));

            using(var reader = command.ExecuteReader())
            {
                while(reader.Read()){
                    Tarea tarea = new Tarea
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        IdTablero = Convert.ToInt32(reader["id_tablero"]),
                        Nombre = reader["nombre"].ToString(),
                        Estado = (EstadoTarea)Convert.ToInt32(reader["estado"]),
                        Descripcion = reader["descripcion"].ToString(),
                        Color = reader["color"].ToString(),
                        IdUsuarioAsignado = (reader["id_usuario_asignado"] == DBNull.Value) ? (int?)null : Convert.ToInt32(reader["id_usuario_asignado"])
                    };
                    tareasTablero.Add(tarea);
                }
            }
            connection.Close();
        }

        return tareasTablero;
    }

    public List<Tarea> GetTareasPorEstado(EstadoTarea estado){
        List<Tarea> tareasEstado = new List<Tarea>();

        using(var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            string queryString = @"SELECT * FROM Tarea WHERE estado = @estado";
            var command = new SQLiteCommand(queryString, connection);
            command.Parameters.Add(new SQLiteParameter("@estado", Convert.ToInt32(estado)));

            using(var reader = command.ExecuteReader())
            {
                while(reader.Read()){
                    Tarea tarea = new Tarea
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        IdTablero = Convert.ToInt32(reader["id_tablero"]),
                        Nombre = reader["nombre"].ToString(),
                        Estado = (EstadoTarea)Convert.ToInt32(reader["estado"]),
                        Descripcion = reader["descripcion"].ToString(),
                        Color = reader["color"].ToString(),
                        IdUsuarioAsignado = (reader["id_usuario_asignado"] == DBNull.Value) ? (int?)null : Convert.ToInt32(reader["id_usuario_asignado"])
                    };
                    tareasEstado.Add(tarea);
                }
            }
            connection.Close();
        }
        return tareasEstado;
    }

    public void EliminarTarea(int idTarea){
        using(var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            string queryString = @"DELETE FROM Tarea WHERE id = @idTarea;";
            var command = new SQLiteCommand(queryString, connection);
            command.Parameters.Add(new SQLiteParameter("@idTarea", idTarea));

            command.ExecuteNonQuery();
            connection.Close();
        }
    }
}