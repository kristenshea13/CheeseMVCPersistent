using CheeseMVC.Data;
using CheeseMVC.Models;
using CheeseMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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

        [HttpPost]
        public IActionResult Add(AddMenuViewModel addMenuViewModel)
        {
            if (ModelState.IsValid)
            {
                Menu newMenu = new Menu
                {
                    Name = addMenuViewModel.Name
                };

                context.Menus.Add(newMenu);

                context.SaveChanges();

                return Redirect("/Menu/ViewMenu/" + newMenu.ID);
            }

            return View(addMenuViewModel);
        }

        public IActionResult ViewMenu(int id)
        {
            List<CheeseMenu> items = context.CheeseMenus.Include(item => item.Cheese).
                Where(cm => cm.MenuID == id).ToList();

            Menu menu = context.Menus.Single(m => m.ID == id);

            ViewMenuViewModel viewModel = new ViewMenuViewModel
            {
                Menu = menu,
                Items = items
            };
            return View(viewModel);
        }

        public IActionResult AddItem(int id)
        {
            Menu menu = context.Menus.Single(m => m.ID == id);
            List<Cheese> cheeses = context.Cheeses.ToList();
            return View(new AddMenuItemViewModel(menu, cheeses));
        }

        [HttpPost]
        public IActionResult AddItem(AddMenuItemViewModel addMenuItemViewModel)
        {
            if (ModelState.IsValid)
            {
                var cheeseID = addMenuItemViewModel.CheeseID;
                var menuID = addMenuItemViewModel.MenuID;

                IList<CheeseMenu> existingitems =
                    context.CheeseMenus.
                    Where(cm => cm.CheeseID == cheeseID).
                    Where(cm => cm.MenuID == menuID).ToList();

                if (existingitems.Count == 0)
                {
                    CheeseMenu menuItem = new CheeseMenu
                    {
                        CheeseID = cheeseID,
                        MenuID = menuID
                    };

                    context.Add(menuItem);
                    context.SaveChanges();



                }
                return RedirectToAction("ViewMenu", new { id = menuID });

            }

            return View(addMenuItemViewModel);

        }

        [HttpPost]
        public IActionResult Remove(int[] menuIds)
        {
            foreach (int menuId in menuIds)
            {
                Menu theCheese = context.Menus.Single(c => c.ID == menuId);
                context.Menus.Remove(theCheese);
            }

            context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult RemoveCheese(int[] cheeseIds, int menuID)
        {
            foreach (int cheeseId in cheeseIds)
            {
                CheeseMenu theCheese = context.CheeseMenus.Single(c => c.CheeseID == cheeseId && c.MenuID == menuID);
                context.CheeseMenus.Remove(theCheese);
            }

            context.SaveChanges();

            return RedirectToAction("ViewMenu", new { id = menuID });
        }

    }
}