using technicaltest.Protos;
using FluentValidation;
using technicaltest.Models;

namespace technicaltest.Validators
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
		//DI SESUAIKAN VALIDASI NYA
		//RuleFor(c => c.Name).NotEmpty().MaximumLength(100);
		RuleFor(c => c.Name).NotNull().NotEmpty();
		RuleFor(c => c.Description).MaximumLength(255);

        }
    }
}
