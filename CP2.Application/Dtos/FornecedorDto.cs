using CP2.Domain.Interfaces.Dtos;
using FluentValidation;
using System.Security.Cryptography.X509Certificates;

namespace CP2.Application.Dtos
{
    public class FornecedorDto : IFornecedorDto
    {
        public string Nome { get; set; } = string.Empty;
        public string CNPJ { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Endereco { get; set; } = string.Empty;
        public DateTime CriadoEm { get; set; }

        public void Validate()
        {
            var validateResult = new FornecedorDtoValidation().Validate(this);

            if (!validateResult.IsValid)
                throw new Exception(string.Join(" e ", validateResult.Errors.Select(x => x.ErrorMessage)));
        }
    }

    internal class FornecedorDtoValidation : AbstractValidator<FornecedorDto>
    {
        public FornecedorDtoValidation()
        {
            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("O nome do fornecedor é obrigatório.")
                .MaximumLength(100).WithMessage("O nome pode ter no máximo 100 caracteres.")
                .Matches(@"^[a-zA-Z\s]+$").WithMessage("O nome deve conter apenas letras e espaços.");

            RuleFor(x => x.CNPJ)
                .NotEmpty().WithMessage("O CNPJ é obrigatório.")
                .Length(14).WithMessage("O CNPJ deve conter exatamente 14 caracteres.")
                .Matches(@"^\d{14}$").WithMessage("O CNPJ deve conter apenas números.");

            RuleFor(x => x.Telefone)
                .NotEmpty().WithMessage("O telefone é obrigatório.")
                .Length(10, 11).WithMessage("O telefone deve ter entre 10 e 11 caracteres.")
                .Matches(@"^\d{10,11}$").WithMessage("O telefone deve conter apenas números.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("O e-mail é obrigatório.")
                .MaximumLength(100).WithMessage("O e-mail pode ter no máximo 100 caracteres.")
                .EmailAddress().WithMessage("O e-mail deve ser válido.");

            RuleFor(x => x.Endereco)
                .NotEmpty().WithMessage("O endereço é obrigatório.")
                .MaximumLength(100).WithMessage("O endereço pode ter no máximo 100 caracteres.")
                .Matches(@"^[a-zA-Z0-9\s,]+$").WithMessage("O endereço deve conter apenas letras, números, vírgulas e espaços.");

            RuleFor(x => x.CriadoEm)
                .NotEmpty().WithMessage("A data de criação é obrigatória.")
                .LessThanOrEqualTo(DateTime.Now).WithMessage("A data de criação não pode ser futura.");

            RuleFor(x => x)
                .Must(x => ValidarCNPJ(x.CNPJ)).WithMessage("O CNPJ informado é inválido.");
        }

        private bool ValidarCNPJ(string cnpj)
        {
            return cnpj.Length == 14 && cnpj.All(char.IsDigit);
        }
    }
}
