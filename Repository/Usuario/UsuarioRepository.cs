using System.Data.SQLite;
using Encrypt.Net.Text;
using tl2_tp10_2023_adanSmith01.Models;
namespace tl2_tp10_2023_adanSmith01.Repository;

public class UsuarioRepository: IUsuarioRepository
{
    private readonly string _connectionString;

    public UsuarioRepository(string connectionString) => _connectionString = connectionString;

    public void CrearUsuario(Usuario usuario)
    {
        var connection = new SQLiteConnection(_connectionString);
        try
        {

            connection.Open();
            string queryString = @"INSERT INTO Usuario (nombre_de_usuario, contrasenia, rol) 
                                   VALUES (@nombreUsuario, @contraseniaUsuario, @rolUsuario);
            ";
            var command = new SQLiteCommand(queryString, connection);

            command.Parameters.Add(new SQLiteParameter("@nombreUsuario", usuario.NombreUsuario));
            command.Parameters.Add(new SQLiteParameter("@contraseniaUsuario", Cifrado.sha256(usuario.Contrasenia).Hash));
            command.Parameters.Add(new SQLiteParameter("@rolUsuario", Convert.ToInt32(usuario.RolUsuario)));
            command.ExecuteNonQuery();

        }
        catch(SQLiteException)
        {
            throw new Exception($"Hubo un problema en la base de datos para validar al usuario. ERROR 500");
        }
        catch(Exception ex)
        {
            throw new Exception($"Hubo un problema al crear un nuevo usuario. Motivo: {ex.Message}");
        }
        finally
        {
            connection.Close();
            connection.Dispose();
        }
    }

    public bool ExisteUsuario(string nombreUsuario)
    {
        var connection = new SQLiteConnection(_connectionString);
        try
        {
            connection.Open();
            string queryString = @"SELECT COUNT(*) FROM Usuario
                                   WHERE nombre_de_usuario = @nombreUsuario AND activo = 1;";
            var command = new SQLiteCommand(queryString, connection);
            command.Parameters.Add(new SQLiteParameter("@nombreUsuario", nombreUsuario));
            
            return Convert.ToInt32(command.ExecuteScalar()) > 0;
        }
        catch(SQLiteException)
        {
            throw new Exception($"Hubo un problema en la base de datos para validar al usuario.");
        }
        catch(Exception ex)
        {
            throw new Exception($"No es posible realizar la operacion. Motivo: {ex.Message}");
        }
        finally
        {
            connection.Close();
            connection.Dispose();
        }
    }

    public void ActualizarUsuario(Usuario usuario)
    {
        var connection = new SQLiteConnection(_connectionString);
        try
        {

            connection.Open();
            string queryString = (!string.IsNullOrEmpty(usuario.Contrasenia))? @"UPDATE Usuario 
                                                                                SET nombre_de_usuario = @nombreNuevo, contrasenia = @contraseniaUsuario, rol = @rolUsuario  
                                                                                WHERE id = @idUsuario AND activo = 1;" : @"UPDATE Usuario SET nombre_de_usuario = @nombreNuevo, rol = @rolUsuario  
                                                                                WHERE id = @idUsuario AND activo = 1;";
            var command = new SQLiteCommand(queryString, connection);

            command.Parameters.Add(new SQLiteParameter("@idUsuario", usuario.Id));
            command.Parameters.Add(new SQLiteParameter("@nombreNuevo", usuario.NombreUsuario));
            if(!string.IsNullOrEmpty(usuario.Contrasenia))command.Parameters.Add(new SQLiteParameter("@contraseniaUsuario", Cifrado.sha256(usuario.Contrasenia).Hash));
            command.Parameters.Add(new SQLiteParameter("@rolUsuario", Convert.ToInt32(usuario.RolUsuario)));
            
            command.ExecuteNonQuery();

        }catch(SQLiteException)
        {
            throw new Exception($"Hubo un problema en la base de datos al modificar al usuario especificado.");
        }
        catch(Exception ex)
        {
            throw new Exception($"Hubo un problema al modificar al usuario especificado. Motivo: {ex.Message}");
        }
        finally
        {
            connection.Close();
            connection.Dispose();
        }
    }

    public List<Usuario> GetAllUsuarios()
    {
        var usuarios = new List<Usuario>();
        var connection = new SQLiteConnection(_connectionString);
        try
        {
            connection.Open();
            string queryString = @"SELECT id, nombre_de_usuario, rol FROM Usuario
                                  WHERE activo = @activo
            ;";
            var command = new SQLiteCommand(queryString, connection);
            command.Parameters.Add(new SQLiteParameter("@activo", 1));

            using(var reader = command.ExecuteReader())
            {
                while(reader.Read())
                {
                    var usuario = new Usuario
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        NombreUsuario = reader["nombre_de_usuario"].ToString(),
                        RolUsuario = (Rol) Convert.ToInt32(reader["rol"])
                    };
                    usuarios.Add(usuario);
                }
            }
            
            return usuarios;

        }catch(SQLiteException)
        {
            throw new Exception("Hubo un problema en la base de datos para obtener los datos de los usuarios registrados.");
        }
        catch(Exception ex)
        {
            throw new Exception($"Hubo un problema para obtener los datos de los usuarios registrados. Motivo: {ex.Message}");
        }
        finally
        {
            connection.Close();
            connection.Dispose();
        }
    }

    public Usuario GetUsuario(int idUsuario)
    {
        Usuario usuarioEncontrado = null;
        var connection = new SQLiteConnection(_connectionString);

        try
        {
            connection.Open();
            string queryString = @"SELECT id, nombre_de_usuario, rol FROM Usuario 
                                  WHERE id = @idUsuario AND activo = 1";
            var command = new SQLiteCommand(queryString, connection);
            command.Parameters.Add(new SQLiteParameter("@idUsuario", idUsuario));

            using(var reader = command.ExecuteReader())
            {
                if(reader.Read()){
                    usuarioEncontrado = new Usuario
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        NombreUsuario = reader["nombre_de_usuario"].ToString(),
                        RolUsuario = (Rol)Convert.ToInt32(reader["rol"])
                    };
                }
            }

            if(usuarioEncontrado == null) throw new Exception("Usuario inexistente.");
            return usuarioEncontrado;
        }
        catch(SQLiteException)
        {
            throw new Exception("Hubo un problema en la base de datos para encontrar al usuario especificado.");
        }
        catch(Exception ex)
        {
            throw new Exception($"Hubo un problema para encontrar al usuario especificado. Motivo: {ex.Message}");
        }
        finally
        {
            connection.Close();
            connection.Dispose();
        }
    }

    public Usuario GetUsuario(string nombre, string contrasenia)
    {
        Usuario usuarioEncontrado = null;
        var connection = new SQLiteConnection(_connectionString);

        try
        {
            connection.Open();
            string queryString = @"SELECT id, nombre_de_usuario, rol FROM Usuario 
                                  WHERE nombre_de_usuario = @nombreUsuario AND contrasenia = @contraseniaUsuario AND activo = 1";
            var command = new SQLiteCommand(queryString, connection);
            command.Parameters.Add(new SQLiteParameter("@nombreUsuario", nombre));
            command.Parameters.Add(new SQLiteParameter("@contraseniaUsuario", Cifrado.sha256(contrasenia).Hash));

            using(var reader = command.ExecuteReader())
            {
                if(reader.Read()){
                    usuarioEncontrado = new Usuario
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        NombreUsuario = reader["nombre_de_usuario"].ToString(),
                        RolUsuario = (Rol)Convert.ToInt32(reader["rol"])
                    };
                }
            }

            return usuarioEncontrado;
        }
        catch(SQLiteException)
        {
            throw new Exception("Hubo un problema en la base de datos para encontrar al usuario especificado.");
        }
        catch(Exception ex)
        {
            throw new Exception($"Hubo un problema para encontrar al usuario especificado. Motivo: {ex.Message}");
        }
        finally
        {
            connection.Close();
            connection.Dispose();
        }
    }

    public void EliminarUsuario(int idUsuario)
    {
        var connection = new SQLiteConnection(_connectionString);

        try
        {
            connection.Open();
            string queryString = @"UPDATE Usuario SET activo = @inactivo
                                  WHERE id = @idUsuario AND activo = @activo;";
            var command = new SQLiteCommand(queryString, connection);
            command.Parameters.Add(new SQLiteParameter("@inactivo", -1));
            command.Parameters.Add(new SQLiteParameter("@idUsuario", idUsuario));
            command.Parameters.Add(new SQLiteParameter("@activo", 1));
            
            command.ExecuteNonQuery();

        }catch(SQLiteException)
        {
            throw new Exception("Hubo un problema en la base de datos al eliminar al usuario especificado.");
        }
        catch(Exception ex)
        {
            throw new Exception($"Hubo un problema al eliminar al usuario especificado. Motivo: {ex.Message}");
        }
        finally
        {
            connection.Close();
            connection.Dispose();
        }
    }
}