using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;

namespace datntdev.Microservice.App.Identity.Components.Pages.Auth;

public partial class SignUp
{
    private EditContext _editContext = default!;
    
    [SupplyParameterFromForm]
    private InputModel Model { get; set; } = new();

    protected override void OnInitialized()
    {
        base.OnInitialized();
        _editContext = new EditContext(Model);
    }

    private string GetInvalidClass(string fieldName)
    {
        var fieldIdentifier = new FieldIdentifier(Model, fieldName);
        return _editContext.IsValid(fieldIdentifier) ? string.Empty : "is-invalid";
    }

    private Task HandleValidSubmitAsync()
    {
        return Task.CompletedTask;
    }

    public class InputModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string? FirstName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string? LastName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }
    }
}