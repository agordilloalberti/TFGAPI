using Microsoft.AspNetCore.Mvc;
using TFGAPI.Models;
using TFGAPI.Services;

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
    public async Task<ActionResult<List<Player>>> Get() =>
        await _playerService.GetAllAsync();

    [HttpGet("{name}")]
    public async Task<ActionResult<Player>> Get(string name)
    {
        var player = await _playerService.GetByNameAsync(name);
        return player is not null ? Ok(player) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Player player)
    {
        var success = await _playerService.CreateAsync(player);
        if (!success) return Conflict("A player with that name already exists.");

        return CreatedAtAction(nameof(Get), new { name = player.Name }, player);
    }


    [HttpPut("{name}")]
    public async Task<IActionResult> Update(string name, Player player)
    {
        var existingPlayer = await _playerService.GetByNameAsync(name);
        if (existingPlayer is null) return NotFound();

        player.Name = name;
        await _playerService.UpdateAsync(name, player);
        return NoContent();
    }

    [HttpDelete("{name}")]
    public async Task<IActionResult> Delete(string name)
    {
        var player = await _playerService.GetByNameAsync(name);
        if (player is null) return NotFound();

        await _playerService.DeleteAsync(name);
        return NoContent();
    }
}