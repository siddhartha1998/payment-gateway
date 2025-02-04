using MediatR;
using PaymentGateway.Server.Application.Common;
using PaymentGateway.Server.Application.Common.Interface;

namespace PaymentGateway.Server.Application.Users.Commands
{
    public class CreateUserCommand : IRequest<Result>
    {
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string ConfirmPassword { get; set; }
    }

    public class CreateUserCommandHandler(IIdentityService identityService) : IRequestHandler<CreateUserCommand, Result>
    {
        private readonly IIdentityService _identityService = identityService;

        public async Task<Result> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            return await _identityService.CreateUserAsync(request);
        }
    }
}
