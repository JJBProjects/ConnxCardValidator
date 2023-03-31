using ConnxCardValidator.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace ConnxCardValidator.ViewModels
{
    public class CardValidationViewModel
    {
        [Required]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Numerical values only")]
        public string CardNumber { get; set; }

        [Required]
        public int CardTypeId { get; set; }

        public SelectList? CardTypes { get; set; }

        public bool IsValid { get; set; }
        public string? ValidationSummary { get; set; }
    }
}
