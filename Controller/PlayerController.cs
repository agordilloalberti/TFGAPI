using Microsoft.AspNetCore.Mvc;
using TFGAPI.Models;
using TFGAPI.Services;

namespace TFGAPI.Controller;

[Route("api/[controller]")]
[ApiController]
public class PlayerController : ControllerBase
{
    private readonly PlayerService _playerService;

    public PlayerController(PlayerService playerService)
    {
        _playerService = playerService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Player>>> Get() => await _playerService.GetAllAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<Player>> Get(string id)
    {
        var player = await _playerService.GetByIdAsync(id);
        return player is not null ? Ok(player) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Player player)
    {
        await _playerService.CreateAsync(player);
        return CreatedAtAction(nameof(Get), new { id = player.Id }, player);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, Player player)
    {
        var existingPlayer = await _playerService.GetByIdAsync(id);
        if (existingPlayer is null) return NotFound();

        player.Id = id;
        await _playerService.UpdateAsync(id, player);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var player = await _playerService.GetByIdAsync(id);
        if (player is null) return NotFound();

        await _playerService.DeleteAsync(id);
        return NoContent();
    }
}