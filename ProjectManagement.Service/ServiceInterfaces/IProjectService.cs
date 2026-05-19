using ProjectManagement.Domain.Entities;
using ProjectManagement.Service.Shared.Interface;
using static ProjectManagement.Domain.Enums.EnumExtensions;

namespace ProjectManagement.Service.ServiceInterfaces
{
    public interface IProjectService : IBaseService<Project>
    {
    }
}
