namespace tl2_tp10_2023_adanSmith01.ViewModels;

using System.ComponentModel.DataAnnotations;
using tl2_tp10_2023_adanSmith01.Models;

public class CrearUsuarioViewModel
{
    private UsuarioViewModel usuarioNuevo;
    public UsuarioViewModel UsuarioNuevo {get => usuarioNuevo; set => usuarioNuevo = value; }
}