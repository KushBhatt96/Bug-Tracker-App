using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Activities;
using Application.Interfaces;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
/*
 * Query object to return pages of teams
 */
namespace Application.Teams
{
    public class List
    {
        public class TeamsEnvelope
        {
            public List<TeamDto> Teams { get; set; }
            public int TeamCount { get; set; }
        }
        public class Query : IRequest<TeamsEnvelope>
        {
            public Query(int? limit, int? offset, bool isMember, bool isManager)
            {
                Limit = limit;
                Offset = offset;
                IsMember = isMember;
                IsManager = isManager;
            }
            public int? Limit { get; set; }
            public int? Offset { get; set; }
            public bool IsMember { get; set; }
            public bool IsManager { get; set; }
        }

        public class Handler : IRequestHandler<Query, TeamsEnvelope>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            private readonly IUserAccessor _userAccessor;
            public Handler(DataContext context, IMapper mapper, IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _mapper = mapper;
                _context = context;
            }

            public async Task<TeamsEnvelope> Handle(Query request, CancellationToken cancellationToken)
            {
                var queryable = _context.Teams
                    // .Where(x => x.Name == request.Name)
                    // .OrderBy(x => x.Name)
                    .AsQueryable();

                if (request.IsMember && !request.IsManager)
                {
                    queryable = queryable.Where(x => x.TeamMembers.Any(a => a.AppUser.UserName == _userAccessor.GetCurrentUsername()));
                }

                if (request.IsManager && !request.IsMember)
                {
                    queryable = queryable.Where(x => x.TeamMembers.Any(a => a.AppUser.UserName == _userAccessor.GetCurrentUsername() && a.IsManager));
                }

                var teams = await queryable
                    .Skip(request.Offset ?? 0)
                    .Take(request.Limit ?? 3).ToListAsync();

                return new TeamsEnvelope
                {
                    Teams = _mapper.Map<List<Team>, List<TeamDto>>(teams),
                    TeamCount = queryable.Count()
                };
            }
        }
    }
}