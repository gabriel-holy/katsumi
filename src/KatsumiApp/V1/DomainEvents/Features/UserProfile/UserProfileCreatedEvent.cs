using MediatR;
using System.Threading;
using System.Threading.Tasks;


namespace KatsumiApp.V1.DomainEvents.Features.UserProfile
{
    public class UserProfileCreatedEvent
    {
        public class Handler : IRequestHandler<Command, Result>
        {
            public Handler(/*serviceBus Injected*/)
            {
            }

            public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
            {
                // with command, send event to serviceBus

                return new Result();
            }
        }

        public class Command : IRequest<Result>
        {
        }

        public class Result
        {
            public bool EventHasBeenSent { get; set; }
        }
    }
}