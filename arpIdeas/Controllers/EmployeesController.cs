using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using arpIdeas.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace arpIdeas.Controllers
{
    public class EmployeesController : Controller
    {
        public IActionResult Index(string searchQuery, int? positionId)
        {
            Employee emp = new Employee();
            List<Employee> employees = emp.getEmployees();
            Position position = new Position();
            ViewData["Position"] = new SelectList(position.getPositions(), "Id", "Name");

            if (!String.IsNullOrEmpty(searchQuery))
            {                
                return View(employees.Where(p=>p.LastName.ToLower().Contains(searchQuery.ToLower()) || p.NIP.Contains(searchQuery)));
            }

            if (positionId != null)
            {
                return View(employees.Where(p=>p.PositionId==positionId));
            }

            return View(emp.getEmployees());
        }

        public IActionResult Create()
        {
            Position position = new Position();
            ViewData["Position"] = new SelectList(position.getPositions(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,DateOfBirth,NIP,Salary,PositionId")] Employee employee)
        {
            Position position = new Position();
            ViewData["Position"] = new SelectList(position.getPositions(), "Id", "Name");

            employee.Id = employee.getId();

            if (ModelState.IsValid)
            {
                employee.add();
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Employee employee = new Employee();
            employee = employee.getEmployees().First(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            Position position = new Position();
            ViewData["Position"] = new SelectList(position.getPositions(), "Id", "Name");
            if (id == null)
            {
                return NotFound();
            }

            Employee employee = new Employee();
            employee = employee.getEmployee((int)id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,DateOfBirth,NIP,Salary,PositionId")] Employee employee)
        {
            Position position = new Position();
            ViewData["Position"] = new SelectList(position.getPositions(), "Id", "Name");
            if (id != employee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                employee.edit();
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Employee employee = new Employee();
            employee = employee.getEmployee((int)id);

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Employee employee = new Employee();
            employee = employee.getEmployee((int)id);
            employee.delete();
            return RedirectToAction(nameof(Index));
        }

    }
}