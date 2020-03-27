using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using Domain;
using MediatR;
using Persistence;

namespace Application.Tickets
{
    public class Details
    {
        
        public class Query : IRequest<Ticket>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Ticket>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Ticket> Handle(Query request, CancellationToken cancellationToken)
            {
                var ticket = await _context.Tickets.FindAsync(request.Id);

                if(ticket == null){
                    throw new RestException(HttpStatusCode.NotFound, new {activity = "Not found"});
                }
                return ticket;
            }
        }
    }
}