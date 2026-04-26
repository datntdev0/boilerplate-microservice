using datntdev.Microservice.Shared.Application.Services;
using datntdev.Microservice.Shared.Common;
using datntdev.Microservice.Shared.Common.Extensions;
using datntdev.Microservice.Srv.Identity.Application.Authorization.Permissions.Models;
using System.Collections.Immutable;

using static datntdev.Microservice.Shared.Common.Constants;

namespace datntdev.Microservice.Srv.Identity.Application.Authorization.Permissions;

public class PermissionAppProvider : BaseAppProvider
{
    private readonly ImmutableDictionary<Constants.Permissions, PermissionModel> _permissions;

    public PermissionAppProvider()
    {
        _permissions = LoadPermissions();
    }

    public PermissionModel[] GetAllPermissions(TenancySides? tenancySide = null)
    {
        return _permissions.Values
            .WhereIf(tenancySide != null, x => (x.TenancySides & tenancySide) == tenancySide)
            .ToArray();
    }

    private static ImmutableDictionary<Constants.Permissions, PermissionModel> LoadPermissions()
    {
        var permissions = new Dictionary<Constants.Permissions, PermissionModel>
            {
                {
                    Constants.Permissions.Tenancy,
                    new PermissionModel(
                        permissionName: "Tenant management",
                        permission: Constants.Permissions.Tenancy,
                        parentPermission: Constants.Permissions.None,
                        tenancySide: TenancySides.Host)
                },
                {
                    Constants.Permissions.Tenancy_Read,
                    new PermissionModel(
                        permissionName: "Tenant read",
                        permission: Constants.Permissions.Tenancy_Read,
                        parentPermission: Constants.Permissions.Tenancy,
                        tenancySide: TenancySides.Host)
                },
                {
                    Constants.Permissions.Tenancy_Write,
                    new PermissionModel(
                        permissionName: "Tenant write",
                        permission: Constants.Permissions.Tenancy_Write,
                        parentPermission: Constants.Permissions.Tenancy,
                        tenancySide: TenancySides.Host)
                },
                {
                    Constants.Permissions.Users,
                    new PermissionModel(
                        permissionName: "User management",
                        permission: Constants.Permissions.Users,
                        parentPermission: Constants.Permissions.None,
                        tenancySide: TenancySides.Host | TenancySides.Tenant)
                },
                {
                    Constants.Permissions.Users_Read,
                    new PermissionModel(
                        permissionName: "User read",
                        permission: Constants.Permissions.Users_Read,
                        parentPermission: Constants.Permissions.Users,
                        tenancySide: TenancySides.Host | TenancySides.Tenant)
                },
                {
                    Constants.Permissions.Users_Write,
                    new PermissionModel(
                        permissionName: "User write",
                        permission: Constants.Permissions.Users_Write,
                        parentPermission: Constants.Permissions.Users,
                        tenancySide: TenancySides.Host | TenancySides.Tenant)
                },
                {
                    Constants.Permissions.Roles,
                    new PermissionModel(
                        permissionName: "Role management",
                        permission: Constants.Permissions.Roles,
                        parentPermission: Constants.Permissions.None,
                        tenancySide: TenancySides.Host | TenancySides.Tenant)
                },
                {
                    Constants.Permissions.Roles_Read,
                    new PermissionModel(
                        permissionName: "Role read",
                        permission: Constants.Permissions.Roles_Read,
                        parentPermission: Constants.Permissions.Roles,
                        tenancySide: TenancySides.Host | TenancySides.Tenant)
                },
                {
                    Constants.Permissions.Roles_Write,
                    new PermissionModel(
                        permissionName: "Role write",
                        permission: Constants.Permissions.Roles_Write,
                        parentPermission: Constants.Permissions.Roles,
                        tenancySide: TenancySides.Host | TenancySides.Tenant)
                },
            };
        return permissions.ToImmutableDictionary();
    }
}
