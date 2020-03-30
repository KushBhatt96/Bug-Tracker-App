﻿/*
 * REST endpoint for any requests regarding teams.
 */

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Tickets;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : BaseController
    {
        //Any API controller needs a root, API attribute, and must derive from the Base Controller class as seen above
        private readonly IMediator _mediator;
        public TicketsController(IMediator mediator)    //Injecting MediatR
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<TicketDTO>>> List ()       //We will be returning a list of tickets
        {
            return await _mediator.Send(new List.Query());          //Sending a message to the List Query Handler
        }

        [HttpGet("{id}")]    //Get a particular ticket --> pass in id as a root parameter
        public async Task<ActionResult<TicketDTO>> Details(Guid id)
        {
            return await _mediator.Send(new Details.Query{Id = id});      //take id from root paramter and see if it matches any of the guids of the tickets in the DB
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> Create(Create.Command command)
        {
            return await _mediator.Send(command);
        }

        [HttpPut("{id}")]       //Edit a particular ticket --> pass in id as root parameter
        [Authorize(Policy = "IsSubmitter")]
        public async Task<ActionResult<Unit>> Edit(Guid id, Edit.Command command)
        {
            command.Id = id;
            return await _mediator.Send(command);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "IsSubmitter")]
        public async Task<ActionResult<Unit>> Delete(Guid id)
        {
            return await _mediator.Send(new Delete.Command{Id = id});
        }

        [HttpPut("{id}/assign")]
        public async Task<ActionResult<Unit>> Assign(Guid id, Assign.Command command)
        {
            command.Id = id;
            return await _mediator.Send(command);
        }

        [HttpPut("{id}/remove")]
        public async Task<ActionResult<Unit>> Remove(Guid id, Remove.Command command)
        {
            command.Id = id;
            return await _mediator.Send(command);
        }

    }
}