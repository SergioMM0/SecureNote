using System.ComponentModel.DataAnnotations;
using API.Core.Domain.Context;
using API.Core.Domain.DTO.Note;
using API.Core.Domain.Entities;
using API.Core.Domain.Exception;
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
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(NoteDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll() {
        var userId = _context.UserId;
        if (userId is null) {
            return Unauthorized();
        }
        
        try {
            var result = await _noteService.GetAllByUserId((Guid) userId);
            return Ok(_mapper.Map<List<NoteDto>>(result));
        }
        catch (Exception e) {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
    
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(NoteDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById([FromRoute] Guid id) {
        try {
            var result = await _noteService.Get(id);
            if (result is null) {
                return NotFound();
            }
            return Ok(_mapper.Map<NoteDto>(result));
        }
        catch (Exception e) {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
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
    
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(NoteDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update(Guid id,[Required][FromBody] UpdateNoteDto dto) {
        try {
            dto.Id = id;
            var result = await _noteService.Update(_mapper.Map<Note>(dto));
            return Ok(_mapper.Map<NoteDto>(result));
        }
        catch (NotFoundException e) {
            return NotFound(e.Message);
        }
        catch (Exception e) {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
    
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete([FromRoute] Guid id) {
        try {
            await _noteService.Delete(id);
            return NoContent();
        }
        catch (NotFoundException e) {
            return NotFound(e.Message);
        }
        catch (Exception e) {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
}
