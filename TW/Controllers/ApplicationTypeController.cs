﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TW.Data;
using TW.Models;

namespace TW.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class ApplicationTypeController : Controller
	{
		private readonly ApplicationDbContext _db;

		public ApplicationTypeController(ApplicationDbContext db)
		{
			_db = db;
		}


		public IActionResult Index()
		{
			IEnumerable<ApplicationType> objList = _db.ApplicationType;

			return View(objList);
		}


		// GET -Create
		public IActionResult Create()
		{
			return View();
		}


		// POST -Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Create(ApplicationType obj)
		{
			if (ModelState.IsValid)
			{
				_db.ApplicationType.Add(obj);
				_db.SaveChanges();
				return RedirectToAction("Index");
			}
			return View(obj);
		}


		// GET - Edit
		public IActionResult Edit(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}
			var obj = _db.ApplicationType.FirstOrDefault(x => x.Id == id);
			if (obj == null)
			{
				return NotFound();
			}

			return View(obj);
		}

		// POST - Edit
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Edit(ApplicationType obj)
		{
			if (ModelState.IsValid)
			{
				_db.ApplicationType.Update(obj);
				_db.SaveChanges();
				return RedirectToAction("Index");
			}
			return View(obj);
		}


		// GET - Delete
		public IActionResult Delete(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}
			var obj = _db.ApplicationType.FirstOrDefault(x => x.Id == id);
			if (obj == null)
			{
				return NotFound();
			}

			return View(obj);
		}

		// POST - Delete
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult DeletePost(int? id)
		{
			var obj = _db.ApplicationType.FirstOrDefault(x => x.Id == id);

			if (obj == null)
			{
				return NotFound();
			}

			_db.ApplicationType.Remove(obj);
			_db.SaveChanges();
			return RedirectToAction("Index");

		}



	}
}
