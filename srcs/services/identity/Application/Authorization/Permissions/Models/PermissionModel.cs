using datntdev.Microservice.Shared.Common;

namespace datntdev.Microservice.Srv.Identity.Application.Authorization.Permissions.Models;

public class PermissionModel(
    string permissionName,
    Constants.Permissions permission,
    Constants.Permissions parentPermission = Constants.Permissions.None,
    Constants.TenancySides tenancySide = Constants.TenancySides.Host | Constants.TenancySides.Tenant)
{
    public string PermissionName { get; set; } = permissionName;
    public Constants.Permissions Permission { get; set; } = permission;
    public Constants.TenancySides TenancySides { get; set; } = tenancySide;
    public Constants.Permissions? ParentPermission { get; set; } = parentPermission;
}

