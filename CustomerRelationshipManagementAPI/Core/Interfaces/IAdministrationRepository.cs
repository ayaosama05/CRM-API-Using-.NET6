namespace CustomerRelationshipManagementAPI.Core.Interfaces
{
    public interface IAdministrationRepository
    {
        Task<RoleModel> AddNewRoleAsync(string name);
        Task<UserRoleModel> AddUserToRoleAsync(string userName, string roleName);
    }
}
