using datntdev.Microservice.App.Identity.Identity;
using datntdev.Microservice.App.Identity.Models;
using datntdev.Microservice.Shared.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;

namespace datntdev.Microservice.App.Identity.Components.Pages.Auth;

public partial class SignUp
{
    private EditContext _editContext = default!;
    private string _alertTitle = string.Empty;
    private string _alertText = string.Empty;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    [Inject]
    private IdentityManager IdentityManager { get; set; } = default!;

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

    private async Task HandleValidSubmitAsync()
    {
        var registerResult = await IdentityManager.SignUpWithPassword(
            Model.Email!, Model.Password!);

        if (registerResult.Status == IdentityResultStatus.Success)
        {
            NavigationManager.NavigateTo(Constants.Endpoints.AuthSignIn, forceLoad: true);
        }
        else
        {
            _alertTitle = "Registration Failed";
            _alertText = "Invalid register attempt. Please try again.";
        }
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