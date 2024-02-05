using CustomerService.Commands;
using CustomerService.Database.Models;
using CustomerService.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CustomerService.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CustomerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("ListCustomer")]
        public async Task<IActionResult> ListCustomer(string searchWord = "")
        {
            try
            {
                var response = await _mediator.Send(new ListCustomerQuery(searchWord));
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetCustomer")]
        public async Task<IActionResult> GetCustomer(string id)
        {
            try
            {
                var response = await _mediator.Send(new GetCustomerQuery(id));
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AddCustomer")]
        public async Task<IActionResult> AddCustomer(AddCustomerCommand command)
        {
            try
            {
                var response = await _mediator.Send(command);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("DeleteCustomer")]
        public async Task<IActionResult> DeleteCustomer(string id)
        {
            try
            {
                var response = await _mediator.Send(new DeleteCustomerCommand(id));
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
