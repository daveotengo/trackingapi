using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using trackingapi.Data;
using trackingapi.Models;

namespace trackingapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IssueController : ControllerBase
    {
        private readonly IssueDbContext context;

        public IssueController(IssueDbContext context) => this.context = context;

        [HttpGet]
        public async Task<IEnumerable<Issue>> Get() => await context.Issues.ToListAsync();

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Issue),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id) {
            var issue= await context.Issues.FindAsync(id);
            return issue == null ? NotFound() : Ok(issue);

        }

        [HttpPost]
        [ProducesResponseType(typeof(Issue), StatusCodes.Status201Created)]
        public async Task<IActionResult> Create(Issue issue) {
            await context.Issues.AddAsync(issue);
            await context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = issue.Id }, issue);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id,Issue issue)
        {
            if (id != issue.Id) return BadRequest();
            context.Entry(issue).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }


        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(int id)
        {
            var issueToDelete = await context.Issues.FindAsync(id);
            if (issueToDelete == null) return NotFound();
            context.Issues.Remove(issueToDelete);
            await context.SaveChangesAsync();

            return NoContent();
        }


    }


}
