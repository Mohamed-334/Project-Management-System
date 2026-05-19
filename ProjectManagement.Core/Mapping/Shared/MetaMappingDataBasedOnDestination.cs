using AutoMapper;
using ProjectManagement.Domain.Shared.BaseEntity.Interfaces;
using ProjectManagement.Service.ServiceInterfaces;

namespace ProjectManagement.Core.Mapping.Shared
{
    public class MetaMappingDataBasedOnDestination<TSource, TDestination>
    : IMappingAction<TSource, TDestination>
    where TDestination : IBaseEntity
    {
        private readonly IAuthenticatedUserService _authenticatedUserService;

        public MetaMappingDataBasedOnDestination(IAuthenticatedUserService authenticatedUserService)
        {
            _authenticatedUserService = authenticatedUserService;
        }

        public void Process(TSource source, TDestination destination, ResolutionContext context)
        {
            var userName = _authenticatedUserService.GetAuthenticatedUserName();

            if (destination.CreationDate == null && destination.CreationDate == null)
            {
                destination.CreationDate = DateTime.Now;
                destination.CreatorName = userName;
            }
            else
            {
                destination.ModificationDate = DateTime.Now;
                destination.ModifierName = userName;
            }
        }
    }
}
