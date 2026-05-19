using AutoMapper;
using ProjectManagement.Domain.Shared.BaseEntity.Interfaces;
using ProjectManagement.Service.ServiceInterfaces;

namespace ProjectManagement.Core.Mapping.Shared
{
    public class MetaMappingDataBasedOnSource<TSource, TDestination>
    : IMappingAction<TSource, TDestination>
    where TSource : IBaseEntity
    {
        private readonly IAuthenticatedUserService _authenticatedUserService;

        public MetaMappingDataBasedOnSource(IAuthenticatedUserService authenticatedUserService)
        {
            _authenticatedUserService = authenticatedUserService;
        }

        public void Process(TSource source, TDestination destination, ResolutionContext context)
        {
            var userName = _authenticatedUserService.GetAuthenticatedUserName();

            if (source.CreationDate == null && source.CreationDate == null)
            {
                source.CreationDate = DateTime.Now;
                source.CreatorName = userName;
            }
            else
            {
                source.ModificationDate = DateTime.Now;
                source.ModifierName = userName;
            }
        }
    }
}
