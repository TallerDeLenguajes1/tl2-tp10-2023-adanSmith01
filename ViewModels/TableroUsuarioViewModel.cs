namespace tl2_tp10_2023_adanSmith01.ViewModels;
using System.ComponentModel.DataAnnotations;
using tl2_tp10_2023_adanSmith01.Models;

public class TableroUsuarioViewModel
{
    private TableroViewModel tablero;

    private UsuarioViewModel usuario;

    public TableroViewModel Tablero { get => tablero; set => tablero = value; }
    public UsuarioViewModel Usuario { get => usuario; set => usuario = value; }
}