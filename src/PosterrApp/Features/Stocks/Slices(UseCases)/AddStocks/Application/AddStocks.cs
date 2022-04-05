﻿using AutoMapper;
using FluentValidation;
using MediatR;
using PosterrApp.Data;
using PosterrApp.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PosterrApp.Features.Stocks
{
    public class AddStocks
    {
        public class Command : IRequest<Result>
        {
            public int ProductId { get; set; }
            public int Amount { get; set; }
        }

        public class Result
        {
            public int QuantityInStock { get; set; }
        }

        public class MapperProfile : Profile
        {
            public MapperProfile()
            {
                CreateMap<Product, Result>();
            }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Amount).GreaterThan(0);
            }
        }

        public class Handler : IRequestHandler<Command, Result>
        {
            private readonly ProductContext _db;
            private readonly IMapper _mapper;

            public Handler(ProductContext db, IMapper mapper)
            {
                _db = db ?? throw new ArgumentNullException(nameof(db));
                _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            }

            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                var product = await _db.Products.FindAsync(new object[] { request.ProductId }, cancellationToken: cancellationToken);
                product.QuantityInStock += request.Amount;
                await _db.SaveChangesAsync(cancellationToken);

                var result = _mapper.Map<Result>(product);
                return result;
            }
        }
    }
}
