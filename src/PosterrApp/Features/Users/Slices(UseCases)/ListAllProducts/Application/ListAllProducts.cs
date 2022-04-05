﻿using AutoMapper;
using MediatR;
using PosterrApp.Data;
using PosterrApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PosterrApp.Features.Users
{
    public class ListAllProducts
    {
        public class Command : IRequest<IEnumerable<Result>>
        {
        }

        public class Result
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int QuantityInStock { get; set; }
        }

        public class MapperProfile : Profile
        {
            public MapperProfile()
            {
                CreateMap<Product, Result>();
            }
        }

        public class Handler : IRequestHandler<Command, IEnumerable<Result>>
        {
            private readonly ProductContext _db;
            private readonly IMapper _mapper;

            public Handler(ProductContext db, IMapper mapper)
            {
                _db = db ?? throw new ArgumentNullException(nameof(db));
                _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            }

            public Task<IEnumerable<Result>> Handle(Command request, CancellationToken cancellationToken)
            {
                var results = _mapper.ProjectTo<Result>(_db.Products);
                return Task.FromResult(results.AsEnumerable());
            }
        }
    }
}
