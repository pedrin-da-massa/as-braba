
using System.ComponentModel.DataAnnotations;

namespace MinhaApi.Models;

public class Fornecedor
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome pode ter até 100 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "Informe um e-mail válido.")]
    [StringLength(150)]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "O CNPJ é obrigatório.")]
    [StringLength(18, MinimumLength = 14)]
    public string Cnpj { get; set; } = string.Empty;

    [Required(ErrorMessage = "O telefone é obrigatório.")]
    [Phone(ErrorMessage = "Informe um telefone válido.")]
    [StringLength(20)]
    public string Telefone { get; set; } = string.Empty;

    public bool Ativo { get; set; } = true;
}
