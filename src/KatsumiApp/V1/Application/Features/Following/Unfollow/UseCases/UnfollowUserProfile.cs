﻿using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KatsumiApp.V1.Data.Contexts;

namespace KatsumiApp.V1.Application.Features.Following.Unfollow.UseCases
{
    public class UnfollowUserProfile
    {
        public class Handler : IRequestHandler<Command, Result>
        {
            private readonly FollowingContext _followingContext;
            private readonly IMapper _mapper;
            private const bool Inactivate = false;

            public Handler(FollowingContext followingContext, IMapper mapper)
            {
                _followingContext = followingContext ?? throw new ArgumentNullException(nameof(followingContext));
                _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            }

            public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
            {
                var following = await _followingContext.Followings
                                         .Where(f => f.FollowedUsername == command.FollowedUsername &&
                                                     f.FollowerUsername == command.FollowerUsername)
                                         .FirstOrDefaultAsync(cancellationToken: cancellationToken);

                if (following is null)
                {
                    return null;
                }

                following.FollowingIsActive = Inactivate;

                await _followingContext.SaveChangesAsync(cancellationToken);

                var result = _mapper.Map<Result>(following);

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
            public bool FollowingIsActive { get; set; }
        }

        private class Mapper : Profile
        {
            public Mapper()
            {
                CreateMap<Models.Following, Result>();
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
            }
        }
    }
}
