using MediatR;
using PaymentGateway.Server.Application.Common.Interface;
using PaymentGateway.Server.Infrastructure.Identity;

namespace PaymentGateway.Server.Application.Users.Queries
{
    public class GetCurrentUserQuery : IRequest<CurrentUserDetail>
    {
    }

    public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, CurrentUserDetail>
    {
        private readonly IIdentityService _identityService;
        private readonly ICurrentUserService _currentUserService;

        public GetCurrentUserQueryHandler(IIdentityService identityService,
                                          ICurrentUserService currentUserService)
        {
            _identityService = identityService;
            _currentUserService = currentUserService;
        }
        public async Task<CurrentUserDetail> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
        {
            ApplicationUser user = await _identityService.GetUserByIdAsync(int.Parse(_currentUserService.UserId));
            var userDetail = new CurrentUserDetail
            {
                Id = user.Id,
                Email = user.Email,
                Fullname = user.FullName,
                Username = user.UserName,
                PhoneNumber = user.PhoneNumber,
                IsActive = user.IsActive
            };

            return userDetail;
        }
    }
}
