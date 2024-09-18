﻿using Microsoft.AspNetCore.Mvc;
using Coursework.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Coursework.Models.DTOs;

namespace Coursework.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IssuePrioritiesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public IssuePrioritiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/IssueStatuses/GetAll
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<IssuePriorityDTO>>> GetAll()
        {
            var statuses = await _context.IssueStatuses.ToListAsync();
            var statusDTOs = statuses.Select(s => new IssueStatusDTO
            {
                Id = s.Id,
                Name = s.Name
            }).ToList();

            return Ok(statusDTOs);
        }
    }
}
