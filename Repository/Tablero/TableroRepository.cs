using System.Data.SQLite;
using tl2_tp10_2023_adanSmith01.Models;
namespace tl2_tp10_2023_adanSmith01.Repository;

public class TableroRepository: ITableroRepository
{
    private readonly string _connectionString;

    public TableroRepository(string connectionString) => _connectionString = connectionString;

    public void CrearTablero(Tablero tablero)
    {
        var connection = new SQLiteConnection(_connectionString);
        try
        {
            connection.Open();
            string queryString = @" INSERT INTO Tablero (id_usuario_propietario, nombre, descripcion) 
                                    VALUES (@idPropietario, @nombreTablero, @descripcionTablero);
            ";
            var command = new SQLiteCommand(queryString, connection);

            command.Parameters.Add(new SQLiteParameter("@idPropietario", tablero.IdUsuarioPropietario));
            command.Parameters.Add(new SQLiteParameter("@nombreTablero", tablero.Nombre));
            command.Parameters.Add(new SQLiteParameter("@descripcionTablero", tablero.Descripcion));

            command.ExecuteNonQuery();

        }
        catch(SQLiteException)
        {
            throw new Exception("Hubo un problema en la base de datos al crear un nuevo tablero.");
        }
        catch(Exception ex)
        {
            throw new Exception($"Hubo un problema al crear un nuevo tablero. Motivo: {ex.Message}");
        }
        finally
        {
            connection.Close();
            connection.Dispose();
        }
    }

    public void ModificarTablero(Tablero tablero)
    {
        var connection = new SQLiteConnection(_connectionString);
        try
        {
            connection.Open();
            string queryString = @" UPDATE Tablero 
                                    SET nombre = @nombreTablero, descripcion = @descripcionTablero 
                                    WHERE id = @idTablero AND activo = @activo;
            ";
            var command = new SQLiteCommand(queryString, connection);
            
            command.Parameters.Add(new SQLiteParameter("@idPropietario", tablero.IdUsuarioPropietario));
            command.Parameters.Add(new SQLiteParameter("@nombreTablero", tablero.Nombre));
            command.Parameters.Add(new SQLiteParameter("@descripcionTablero", tablero.Descripcion));
            command.Parameters.Add(new SQLiteParameter("@idTablero", tablero.Id));
            command.Parameters.Add(new SQLiteParameter("@activo", 1));

            command.ExecuteNonQuery();
            

        }
        catch(SQLiteException)
        {
            throw new Exception("Hubo un problema en la base de datos al modificar el tablero especificado.");
        }
        catch(Exception ex)
        {
            throw new Exception($"Hubo un problema al modificar el tablero especificado. Motivo: {ex.Message}");
        }
        finally
        {
            connection.Close();
            connection.Dispose();
        }
    }

    public List<Tablero> GetAllTableros()
    {
        var tableros = new List<Tablero>();
        var connection = new SQLiteConnection(_connectionString);
        try
        {
            connection.Open();
            string queryString = @"SELECT * FROM Tablero
                                   WHERE activo = @activo
            ;";
            var command = new SQLiteCommand(queryString, connection);
            command.Parameters.Add(new SQLiteParameter("@activo", 1));

            using(var reader = command.ExecuteReader())
            {
                while(reader.Read()){
                    Tablero tablero = new Tablero
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        IdUsuarioPropietario = Convert.ToInt32(reader["id_usuario_propietario"]),
                        Nombre = reader["nombre"].ToString(),
                        Descripcion = reader["descripcion"].ToString()
                    };
                    tableros.Add(tablero); 
                }
            }

            return tableros;
        }
        catch(SQLiteException)
        {
            throw new Exception("Hubo un problema en la base de datos al obtener los datos de los tableros registrados.");
        }
        catch(Exception ex)
        {
            throw new Exception($"Hubo un problema al obtener los datos de los tableros registrados. Motivo: {ex.Message}");
        }
        finally
        {
            connection.Close();
            connection.Dispose();
        }

    }

    public Tablero GetTablero(int idTablero)
    {
        Tablero tableroEncontrado = null;
        var connection = new SQLiteConnection(_connectionString);

        try
        {
            connection.Open();
            string queryString = @"SELECT * FROM Tablero 
                                  WHERE id = @idTablero AND activo = @activo;";
            
            var command = new SQLiteCommand(queryString, connection);
            command.Parameters.Add(new SQLiteParameter("@idTablero", idTablero));
            command.Parameters.Add(new SQLiteParameter("@activo", 1));

            using(var reader = command.ExecuteReader())
            {
                if(reader.Read()){
                    tableroEncontrado = new Tablero
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        IdUsuarioPropietario = Convert.ToInt32(reader["id_usuario_propietario"]),
                        Nombre = reader["nombre"].ToString(),
                        Descripcion = reader["descripcion"].ToString()
                    };
                }
            }

            if(tableroEncontrado == null) throw new Exception("Tablero inexistente");
            return tableroEncontrado;

        }
        catch(SQLiteException)
        {
            throw new Exception("Hubo un problema en la base de datos al encontrar el tablero especificado.");
        }
        catch(Exception ex)
        {
            throw new Exception($"Hubo un problema al encontrar el tablero especificado. Motivo: {ex.Message}");
        }
        finally
        {
            connection.Close();
            connection.Dispose();
        }

    }

    public List<Tablero> GetTablerosDeUsuario(int idUsuario)
    {
        var tablerosUsuario = new List<Tablero>();
        var connection = new SQLiteConnection(_connectionString);
        try
        {
            connection.Open();
            
            string queryString = @"SELECT * FROM Tablero WHERE id_usuario_propietario = @idPropietario;";
            var command = new SQLiteCommand(queryString, connection);
            command.Parameters.Add(new SQLiteParameter("@idPropietario", idUsuario));

            using(var reader = command.ExecuteReader())
            {
                while(reader.Read()){
                    Tablero tablero = new Tablero
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        IdUsuarioPropietario = Convert.ToInt32(reader["id_usuario_propietario"]),
                        Nombre = reader["nombre"].ToString(),
                        Descripcion = reader["descripcion"].ToString()
                    };
                    tablerosUsuario.Add(tablero); 
                }
            }

            return tablerosUsuario;

        }
        catch(SQLiteException)
        {
            throw new Exception("Hubo un problema en la base de datos al encontrar los tableros del usuario especificado.");
        }
        catch(Exception ex)
        {
            throw new Exception($"Hubo un problema al encontrar los tableros del usuario especificado. Motivo: {ex.Message}");
        }
        finally
        {
            connection.Close();
            connection.Dispose();
        }
    }

    public List<Tablero> GetTablerosConTareasAsignadas(int idUsuario)
    {
        var tableros = new List<Tablero>();
        var connection = new SQLiteConnection(_connectionString);
        try
        {
            connection.Open();
            string queryString = @"SELECT DISTINCT(tablero.id), tablero.nombre, tablero.descripcion, id_usuario_propietario
                                    FROM usuario INNER JOIN tablero ON(usuario.id = id_usuario_propietario)
                                    INNER JOIN tarea ON(tablero.id = tarea.id_tablero)
                                    WHERE id_usuario_asignado = @idUsuarioAsignado AND id_usuario_propietario != @idUsuarioAsignado AND tablero.activo = @tableroActivo AND tarea.activo = @tareaActiva;";
            var command = new SQLiteCommand(queryString, connection);
            command.Parameters.Add(new SQLiteParameter("@idUsuarioAsignado", idUsuario));
            command.Parameters.Add(new SQLiteParameter("@tableroActivo", 1));
            command.Parameters.Add(new SQLiteParameter("@tareaActiva", 1));

            using(var reader = command.ExecuteReader())
            {
                while(reader.Read())
                {
                    Tablero tablero = new Tablero
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        IdUsuarioPropietario = Convert.ToInt32(reader["id_usuario_propietario"]),
                        Nombre = reader["nombre"].ToString(),
                        Descripcion = reader["descripcion"].ToString()
                    };
                    tableros.Add(tablero); 
                }
            }
            
            return tableros;
        }
        catch(SQLiteException)
        {
            throw new Exception("Hubo un problema en la base de datos al encontrar los tableros con y sin tareas asignadas del usuario especificado.");
        }
        catch(Exception ex)
        {
            throw new Exception($"Hubo un problema al encontrar los tableros con y sin tareas asignadas del usuario especificado. Motivo: {ex.Message}");
        }
        finally
        {
            connection.Close();
            connection.Dispose();
        }
    }

    public void EliminarTablero(int idTablero)
    {
        var connection = new SQLiteConnection(_connectionString);
        try
        {

            connection.Open();
            string queryString = @"UPDATE Tablero SET activo = @inactivo
                                   WHERE id = @idTablero AND activo = @activo
            ;";
            var command = new SQLiteCommand(queryString, connection);
            command.Parameters.Add(new SQLiteParameter("@inactivo", 0));
            command.Parameters.Add(new SQLiteParameter("@idTablero", idTablero));
            command.Parameters.Add(new SQLiteParameter("@activo", 1));

            command.ExecuteNonQuery();

        }
        catch(SQLiteException)
        {
            throw new Exception("Hubo un problema en la base de datos al eliminar el tablero especificado.");
        }
        catch(Exception ex)
        {
            throw new Exception($"Hubo un problema al eliminar el tablero especificado. Motivo: {ex.Message}");
        }
        finally
        {
            connection.Close();
            connection.Dispose();
        }
    }
}