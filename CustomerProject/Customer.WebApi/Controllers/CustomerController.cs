using CustomerProject.WebApi.Models;
using CustomerProject.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace CustomerProject.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomerController(ICustomerService customerService) : ControllerBase
{
    private readonly ICustomerService _customerService = customerService;

    [HttpGet("GetAllAsync")]
    public async Task<IActionResult> GetAllCustomersAsync()
    {
        var value = await _customerService.GetCustomersAsync();
        return Ok(value);
    }

    [HttpGet("GetAsync/{Id}")]
    public async Task<IActionResult> GetCustomerAsync(int Id)
    {
        var result = await _customerService.GetCustomerAsync(Id);
        if (result is null)
        {
            return NotFound("The item not found");
        }
        return Ok(result);
    }

    [HttpPost("AddAsync")]
    public async Task<IActionResult> GetAllCustomersAsync(Customers customer)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Model is Not Valid.");
        }

        if (await _customerService.AddCustomerAsync(customer))
        {
            return Ok("Done");
        }
        return BadRequest("Something went wrong please try again.");
    }

    [HttpPut("UpdateAsync")]
    public async Task<IActionResult> EditCustomerAsync(Customers customer)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Model is Not Valid.");
        }

        if (await _customerService.EditCustomerAsync(customer))
        {
            return Ok("Done");
        }
        return BadRequest("Something went wrong please try again.");
    }

    [HttpDelete("DeleteAsync")]
    public async Task<IActionResult> DeleteAsync(int Id)
    {
        if (await _customerService.DeleteCustomerAsync(Id))
        {
            return Ok("Done");
        }
        return BadRequest("Something went wrong please try again.");
    }
}