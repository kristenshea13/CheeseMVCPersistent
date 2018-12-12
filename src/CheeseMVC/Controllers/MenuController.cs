using Microsoft.AspNetCore.Mvc;
using CheeseMVC.Models;
using System.Collections.Generic;
using CheeseMVC.ViewModels;
using CheeseMVC.Data;
using System.Linq;

namespace CheeseMVC.Controllers
{
    public class MenuController : Controller
    {
        private readonly CheeseDbContext context;

        public MenuController(CheeseDbContext dbContext)
        {
            context = dbContext;
        }


        public IActionResult Index()
        {
            List<Menu> menus = context.Menus.ToList();
            return View(menus);

        }

        public IActionResult Add()
        {
            AddMenuViewModel addMenuViewModel = new AddMenuViewModel();
            return View(addMenuViewModel);

        }

        //[HttpPost]
        //public IActionResult Add(AddMenuViewModel addMenuViewModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        Menu newMenu = new Menu
        //        {
        //            Name = addMenuViewModel.Name
        //        };



        //    }

        //}


    }
}
