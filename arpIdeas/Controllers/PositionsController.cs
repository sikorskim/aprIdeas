using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using arpIdeas.Models;
using Microsoft.AspNetCore.Mvc;

namespace arpIdeas.Controllers
{
    public class PositionsController : Controller
    {
        public IActionResult Index(string info)
        {
            if (!string.IsNullOrEmpty(info))
            {
                ViewBag.Info = info;
            }
            Position position = new Position();            
            return View(position.getPositions());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] Position position)
        {
            position.Id = position.getId();
            if (ModelState.IsValid)
            {
                position.add();
                return RedirectToAction(nameof(Index));
            }
            return View(position);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Position position = new Position();
            position = position.getPosition((int)id);
            if (position== null)
            {
                return NotFound();
            }

            return View(position);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Position position = new Position();
            position = position.getPosition((int)id);
            if (position == null)
            {
                return NotFound();
            }

            return View(position);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Position position)
        {
            if (id != position.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                position.edit();
                return RedirectToAction(nameof(Index));
            }
            return View(position);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Position position = new Position();
            position = position.getPosition((int)id);
            if (position.checkConstraints())
            {                
                return RedirectToAction(nameof(Index), new { info = "Nie można usunąć stanowiska "+position.Name+", gdyż istnieją pracownicy powiązani ze stanowiskiem!"});
            }


            if (position == null)
            {
                return NotFound();
            }

            return View(position);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            Position employee = new Position();
            employee = employee.getPosition((int)id);
            employee.delete();
            return RedirectToAction(nameof(Index));
        }
    }
}