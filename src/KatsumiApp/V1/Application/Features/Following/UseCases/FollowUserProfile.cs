using AutoMapper;
using FluentValidation;
using KatsumiApp.V1.Data.Raven.Contexts;
using MediatR;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using Serilog;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace KatsumiApp.V1.Application.Features.Following.UseCases
{
    public class FollowUserProfile
    {
        public class Handler : IRequestHandler<Command, Result>
        {
            private readonly IMapper _mapper;
            private const bool Activate = true;
            public Handler(IMapper mapper)
            {
                _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            }

            public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
            {
                using var databaseSession = FollowingContext.DocumentStore.OpenAsyncSession();

                var following = await databaseSession.Query<KatsumiApp.V1.Application.Domain.Following>()
                                                     .Statistics(out QueryStatistics databaseSessionStatistics)
                                                     .FirstOrDefaultAsync(f => f.FollowedUsername == command.FollowedUsername &&
                                                                               f.FollowerUsername == command.FollowerUsername,
                                                                               token: cancellationToken);

                if (following is null)
                {
                    following = new KatsumiApp.V1.Application.Domain.Following()
                    {
                        FollowedUsername = command.FollowedUsername,
                        FollowerUsername = command.FollowerUsername,
                        FollowingIsActive = Activate
                    };

                    await databaseSession.StoreAsync(following, cancellationToken);
                }

                following.FollowingIsActive = Activate;

                await databaseSession.SaveChangesAsync(cancellationToken);

                var result = _mapper.Map<Result>(following);

                Log.Information($"A following between {command.FollowerUsername} and {command.FollowedUsername} has been modified. Total time taken: {databaseSessionStatistics.DurationInMs} ms");

                return result;
            }
        }

        public class Command : IRequest<Result>
        {
            public string FollowerUsername { get; set; }
            public string FollowedUsername { get; set; }
        }

        public class Result
        {
            public string Id { get; set; }
            public bool FollowingIsActive { get; set; }
        }

        public class Mapper : Profile
        {
            public Mapper()
            {
                CreateMap<KatsumiApp.V1.Application.Domain.Following, Result>();
            }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                const int MaximumUsernameLength = 14;

                RuleFor(x => x.FollowerUsername)
                    .NotEmpty()
                    .NotNull()
                    .MaximumLength(MaximumUsernameLength);

                RuleFor(x => x.FollowerUsername)
                    .Matches(@"^[0-9a-zA-Z ]+$")
                    .WithMessage("Just numbers and letters are allowed.");

                RuleFor(x => x.FollowedUsername)
                    .NotEmpty()
                    .NotNull()
                    .MaximumLength(MaximumUsernameLength);

                RuleFor(x => x.FollowedUsername)
                    .Matches(@"^[0-9a-zA-Z ]+$")
                    .WithMessage("Just numbers and letters are allowed.");

                RuleFor(x => x)
                    .Must(x => x.FollowedUsername != x.FollowerUsername)
                    .WithMessage(x => $"{x.FollowerUsername}, you cannot follow yourself.");
            }
        }
    }
}
