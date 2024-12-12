using API.Core.Domain.Context;
using API.Core.Domain.DTO.Note;
using API.Core.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
[Authorize]
public class NoteController : ControllerBase {
    private readonly INoteService _noteService;
    private readonly IMapper _mapper;
    private readonly CurrentContext _context;

    public NoteController(INoteService noteService, IMapper mapper, CurrentContext context) {
        _noteService = noteService;
        _mapper = mapper;
        _context = context;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(NoteDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create() {
        try {
            var result = await _noteService.Create();
            return Ok(_mapper.Map<NoteDto>(result));
        }
        catch (Exception e) {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
}
