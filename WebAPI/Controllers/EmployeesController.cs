using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.DTO;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly NorthwindContext _context;

        public EmployeesController(NorthwindContext context)
        {
            _context = context;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<IEnumerable<EmployeeDTO>> GetEmployees()
        {
            return _context.Employees.Select(e => new EmployeeDTO
            {
                EmployeeId = e.EmployeeId,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Title = e.Title,
            });
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDTO>> GetEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            EmployeeDTO EmpDTO = new EmployeeDTO
            {
                EmployeeId = employee.EmployeeId,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Title = employee.Title,

            };
            if (employee == null)
            {
                return NotFound();
            }

            return EmpDTO;
        }

        // PUT: api/Employees/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ResultDTO> PutEmployee(int id, EmployeeDTO employeeDTO)
        {
            if (id != employeeDTO.EmployeeId)
            {
                return new ResultDTO { OK = false, Code = 400 };    //BadRequest
            }
            Employee? Emp = await _context.Employees.FindAsync(id);
            if (Emp == null)
            {
                return new ResultDTO { OK = false, Code = 404 };
            }
            else
            {
                Emp.LastName = employeeDTO.LastName;
                Emp.FirstName = employeeDTO.FirstName;
                Emp.Title = employeeDTO.Title;
                _context.Entry(Emp).State = EntityState.Modified;
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(id))
                    {
                        return new ResultDTO { OK = false, Code = 404 };    //NotFound
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return new ResultDTO { OK = true, Code = 204 };     //NoContent
        }

        // POST: api/Employees
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ResultDTO> PostEmployee(EmployeeDTO employeeDTO)
        {
            Employee Emp = new Employee
            {
                LastName = employeeDTO.LastName,
                FirstName = employeeDTO.FirstName,
                Title = employeeDTO.Title,
            };

            _context.Employees.Add(Emp);
            await _context.SaveChangesAsync();
            return new ResultDTO
            {
                OK = true,
                Code = 200, 
            };
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.EmployeeId == id);
        }
    }
}
