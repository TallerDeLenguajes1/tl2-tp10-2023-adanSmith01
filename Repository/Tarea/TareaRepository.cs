using System.Data.SQLite;
using tl2_tp10_2023_adanSmith01.Models;
namespace tl2_tp10_2023_adanSmith01.Repository;

public class TareaRepository: ITareaRepository
{
    private readonly string _connectionString;

    public TareaRepository(string connectionString) => _connectionString = connectionString;

    public void CrearTarea(int idTablero, Tarea tarea)
    {
        var connection = new SQLiteConnection(_connectionString);
        try
        {

            connection.Open();
                
            string queryString = @"INSERT INTO Tarea (id_tablero, nombre, estado, descripcion, color, id_usuario_asignado) 
                                   VALUES (@idTablero, @nombreTarea, @estadoTarea, @descripcionTarea, @colorTarea, @idUsuarioAsignado);
            ";
            var command = new SQLiteCommand(queryString, connection);

            command.Parameters.Add(new SQLiteParameter("@idTablero", idTablero));
            command.Parameters.Add(new SQLiteParameter("@nombreTarea", tarea.Nombre));
            command.Parameters.Add(new SQLiteParameter("@estadoTarea", Convert.ToInt32(tarea.Estado)));
            command.Parameters.Add(new SQLiteParameter("@descripcionTarea", tarea.Descripcion));
            command.Parameters.Add(new SQLiteParameter("@colorTarea", tarea.Color));
            command.Parameters.Add(new SQLiteParameter("@idUsuarioAsignado", tarea.IdUsuarioAsignado));

            command.ExecuteNonQuery();

        }
        catch(SQLiteException)
        {
            throw new Exception("Hubo un problema en la base de datos al crear una nueva tarea.");
        }
        catch(Exception ex)
        {
            throw new Exception($"Hubo un problema al crear una nueva tarea. Motivo: {ex.Message}");
        }
        finally
        {
            connection.Close();
            connection.Dispose();
        }
    }

    public void ModificarTarea(Tarea tarea)
    {
        var connection = new SQLiteConnection(_connectionString);
        try
        {

            connection.Open();
            string queryString = @"UPDATE Tarea 
                                   SET id_tablero = @idTablero, nombre = @nombreNuevo, estado = @estadoNuevo, 
                                   descripcion = @descripcionNueva, color = @colorNuevo, id_usuario_asignado = @idUsuarioAsignadoNuevo
                                   WHERE id = @idTarea AND activo = @activo
            ;";
            var command = new SQLiteCommand(queryString, connection);

            command.Parameters.Add(new SQLiteParameter("@idTablero", tarea.IdTablero));
            command.Parameters.Add(new SQLiteParameter("@nombreNuevo", tarea.Nombre));
            command.Parameters.Add(new SQLiteParameter("@estadoNuevo", Convert.ToInt32(tarea.Estado)));
            command.Parameters.Add(new SQLiteParameter("@descripcionNueva", tarea.Descripcion));
            command.Parameters.Add(new SQLiteParameter("@colorNuevo", tarea.Color));
            command.Parameters.Add(new SQLiteParameter("@idUsuarioAsignadoNuevo", tarea.IdUsuarioAsignado));
            command.Parameters.Add(new SQLiteParameter("@idTarea", tarea.Id));
            command.Parameters.Add(new SQLiteParameter("@activo", 1));

            command.ExecuteNonQuery();

        }
        catch(SQLiteException)
        {
            throw new Exception("Hubo un problema en la base de datos al actualizar la tarea especificada.");
        }
        catch(Exception ex)
        {
            throw new Exception($"Hubo un problema al actualizar la tarea especificada. Motivo: {ex.Message}");
        }
        finally
        {
            connection.Close();
            connection.Dispose();
        }
    }

    public Tarea GetTarea(int idTarea)
    {
        Tarea tareaEncontrada = null;
        var connection = new SQLiteConnection(_connectionString);
        try
        {

            connection.Open();
            string queryString = @"SELECT * FROM Tarea 
                                   WHERE id = @idTarea AND activo = @activo
            ;";
            var command = new SQLiteCommand(queryString, connection);
            command.Parameters.Add(new SQLiteParameter("@idTarea", idTarea));
            command.Parameters.Add(new SQLiteParameter("@activo", 1));


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
            
            if(tareaEncontrada == null) throw new Exception("Tarea inexistente");
            return tareaEncontrada;
        }
        catch(SQLiteException)
        {
            throw new Exception("Hubo un problema en la base de datos al encontrar la tarea especificada.");
        }
        catch(Exception ex)
        {
            throw new Exception($"Hubo un problema al encontrar la tarea especificada. Motivo: {ex.Message}");
        }
        finally
        {
            connection.Close();
            connection.Dispose();
        }
        
    }

    public List<Tarea> GetTareasDelTablero(int idTablero)
    {
        var tareasTablero = new List<Tarea>();
        var connection = new SQLiteConnection(_connectionString);
        try
        {

            connection.Open();
            string queryString = @"SELECT * FROM Tarea 
                                  WHERE id_tablero = @idTablero AND activo = @activo
            ;";
            var command = new SQLiteCommand(queryString, connection);
            command.Parameters.Add(new SQLiteParameter("@idTablero", idTablero));
            command.Parameters.Add(new SQLiteParameter("@activo", 1));

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
                        IdUsuarioAsignado = (reader["id_usuario_asignado"] == DBNull.Value) ? null : Convert.ToInt32(reader["id_usuario_asignado"])
                    };
                    tareasTablero.Add(tarea);
                }
            }

            return tareasTablero;

        }
        catch(SQLiteException)
        {
            throw new Exception("Hubo un problema en la base de datos al encontrar las tareas del tablero especificado.");
        }
        catch(Exception ex)
        {
            throw new Exception($"Hubo un problema al encontrar las tareas del tablero especificado. Motivo: {ex.Message}");
        }
        finally
        {
            connection.Close();
            connection.Dispose();
        }
    }

    public List<Tarea> GetTareasDelTablero(int idTablero, EstadoTarea estado)
    {
        var tareasTablero = new List<Tarea>();
        var connection = new SQLiteConnection(_connectionString);
        try
        {

            connection.Open();
            string queryString = @"SELECT * FROM Tarea 
                                  WHERE id_tablero = @idTablero AND estado = @estado AND activo = @activo
            ;";
            var command = new SQLiteCommand(queryString, connection);
            command.Parameters.Add(new SQLiteParameter("@idTablero", idTablero));
            command.Parameters.Add(new SQLiteParameter("@estado", Convert.ToInt32(estado)));
            command.Parameters.Add(new SQLiteParameter("@activo", 1));

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
                        IdUsuarioAsignado = (reader["id_usuario_asignado"] == DBNull.Value) ? null : Convert.ToInt32(reader["id_usuario_asignado"])
                    };
                    tareasTablero.Add(tarea);
                }
            }

            return tareasTablero;

        }
        catch(SQLiteException)
        {
            throw new Exception("Hubo un problema en la base de datos al encontrar las tareas del tablero y estado especificados.");
        }
        catch(Exception ex)
        {
            throw new Exception($"Hubo un problema al encontrar las tareas del tablero y estado especificados. Motivo: {ex.Message}");
        }
        finally
        {
            connection.Close();
            connection.Dispose();
        }
    }

    public List<Tarea> GetTareasAsignadasAlUsuario(int idTablero, int idUsuario)
    {
        var tareasUsuario = new List<Tarea>();
        var connection = new SQLiteConnection(_connectionString);
        try
        {
            connection.Open();
            string queryString = @"SELECT * FROM Tarea 
                                   WHERE id_usuario_asignado = @idUsuarioAsignado AND id_tablero = @idTablero AND activo = @activo";
            var command = new SQLiteCommand(queryString, connection);
            command.Parameters.Add(new SQLiteParameter("@idUsuarioAsignado", idUsuario));
            command.Parameters.Add(new SQLiteParameter("@idTablero", idTablero));
            command.Parameters.Add(new SQLiteParameter("@activo", 1));

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
                        IdUsuarioAsignado = Convert.ToInt32(reader["id_usuario_asignado"])
                    };
                    tareasUsuario.Add(tarea);
                }
            }

            return tareasUsuario;

        }
        catch(SQLiteException)
        {
            throw new Exception("Hubo un problema en la base de datos al encontrar las tareas asignadas al usuario especificado.");
        }
        catch(Exception ex)
        {
            throw new Exception($"Hubo un problema al encontrar las tareas asignadas al usuario especificado. Motivo: {ex.Message}");
        }
        finally
        {
            connection.Close();
            connection.Dispose();
        }
    }

    public List<Tarea> GetTareasAsignadasAlUsuario(int idTablero, int idUsuario, EstadoTarea estado)
    {
        var tareasUsuario = new List<Tarea>();
        var connection = new SQLiteConnection(_connectionString);
        try
        {
            connection.Open();
            string queryString = @"SELECT * FROM Tarea 
                                   WHERE id_usuario_asignado = @idUsuarioAsignado AND id_tablero = @idTablero AND estado = @estado AND activo = @activo";
            var command = new SQLiteCommand(queryString, connection);
            command.Parameters.Add(new SQLiteParameter("@idUsuarioAsignado", idUsuario));
            command.Parameters.Add(new SQLiteParameter("@idTablero", idTablero));
            command.Parameters.Add(new SQLiteParameter("@estado", Convert.ToInt32(estado)));
            command.Parameters.Add(new SQLiteParameter("@activo", 1));

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
                        IdUsuarioAsignado = Convert.ToInt32(reader["id_usuario_asignado"])
                    };
                    tareasUsuario.Add(tarea);
                }
            }

            return tareasUsuario;

        }
        catch(SQLiteException)
        {
            throw new Exception("Hubo un problema en la base de datos al encontrar las tareas asignadas al usuario y de estado especificados.");
        }
        catch(Exception ex)
        {
            throw new Exception($"Hubo un problema al encontrar las tareas asignadas al usuario y de estado especificados. Motivo: {ex.Message}");
        }
        finally
        {
            connection.Close();
            connection.Dispose();
        }
    }

    public List<Tarea> GetTareasNoAsignadasDelTablero(int idTablero)
    {
        var tareasNoAsignadasTablero = new List<Tarea>();
        var connection = new SQLiteConnection(_connectionString);
        try
        {

            connection.Open();
            string queryString = @"SELECT * FROM Tarea 
                                   WHERE id_tablero = @idTablero AND id_usuario_asignado IS NULL AND activo = @activo
            ;";
            var command = new SQLiteCommand(queryString, connection);
            command.Parameters.Add(new SQLiteParameter("@idTablero", idTablero));
            command.Parameters.Add(new SQLiteParameter("@activo", 1));

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
                    };
                    tareasNoAsignadasTablero.Add(tarea);
                }
            }

            return tareasNoAsignadasTablero;

        }
        catch(SQLiteException)
        {
            throw new Exception("Hubo un problema en la base de datos al encontrar las tareas no asignadas del tablero especificado.");
        }
        catch(Exception ex)
        {
            throw new Exception($"Hubo un problema al encontrar las tareas no asignadas del tablero especificado. Motivo: {ex.Message}");
        }
        finally
        {
            connection.Close();
            connection.Dispose();
        }
    }

    public List<Tarea> GetTareasNoAsignadasDelTablero(int idTablero, EstadoTarea estado)
    {
        var tareasNoAsignadasTablero = new List<Tarea>();
        var connection = new SQLiteConnection(_connectionString);
        try
        {

            connection.Open();
            string queryString = @"SELECT * FROM Tarea 
                                   WHERE id_tablero = @idTablero AND id_usuario_asignado IS NULL AND estado = @estado AND activo = @activo
            ;";
            var command = new SQLiteCommand(queryString, connection);
            command.Parameters.Add(new SQLiteParameter("@idTablero", idTablero));
            command.Parameters.Add(new SQLiteParameter("@estado", Convert.ToInt32(estado)));
            command.Parameters.Add(new SQLiteParameter("@activo", 1));

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
                    };
                    tareasNoAsignadasTablero.Add(tarea);
                }
            }

            return tareasNoAsignadasTablero;

        }
        catch(SQLiteException)
        {
            throw new Exception("Hubo un problema en la base de datos al encontrar las tareas no asignadas del tablero y de estado especificados.");
        }
        catch(Exception ex)
        {
            throw new Exception($"Hubo un problema al encontrar las tareas no asignadas del tablero y de estado especificados. Motivo: {ex.Message}");
        }
        finally
        {
            connection.Close();
            connection.Dispose();
        }
    }

    public void DesasignarTareas(int idUsuario)
    {
        var connection = new SQLiteConnection(_connectionString);
        try
        {
            connection.Open();
            string queryString = @"UPDATE Tarea 
                                   SET id_usuario_asignado = @valor
                                   WHERE id_usuario_asignado = @idUsuario AND activo = @activo
            ;";
            var command = new SQLiteCommand(queryString, connection);
            command.Parameters.Add(new SQLiteParameter("@valor", null));
            command.Parameters.Add(new SQLiteParameter("@idUsuario", idUsuario));
            command.Parameters.Add(new SQLiteParameter("@activo", 1));

            command.ExecuteNonQuery();

        }
        catch(SQLiteException)
        {
            throw new Exception("Hubo un problema en la base de datos para desasignar las tareas vinculadas al usuario especificado.");
        }
        catch(Exception ex)
        {
            throw new Exception($"Hubo un problema para desasignar las tareas vinculadas al usuario especificado. Motivo: {ex.Message}");
        }
        finally
        {
            connection.Close();
            connection.Dispose();
        }
    }

    public void EliminarTarea(int idTarea)
    {
        var connection = new SQLiteConnection(_connectionString);

        try
        {

            connection.Open();
            string queryString = @"UPDATE Tarea
                                  SET activo = @inactivo 
                                  WHERE id = @idTarea AND activo = @activo
            ;";
            var command = new SQLiteCommand(queryString, connection);
            command.Parameters.Add(new SQLiteParameter("@inactivo", -1));
            command.Parameters.Add(new SQLiteParameter("@idTarea", idTarea));
            command.Parameters.Add(new SQLiteParameter("@activo", 1));
            
            command.ExecuteNonQuery();

        }
        catch(SQLiteException)
        {
            throw new Exception("Hubo un problema en la base de datos al eliminar la tarea especificada.");
        }
        catch(Exception ex)
        {
            throw new Exception($"Hubo un problema al eliminar la tarea especificada. Motivo: {ex.Message}");
        }
        finally
        {
            connection.Close();
            connection.Dispose();
        }
    }
}