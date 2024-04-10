using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RecipeBook.Areas.Identity.Data;
using RecipeBook.Models;

namespace RecipeBook.Controllers
{
    [Authorize]
    public class RecipeController : Controller
    {
        private readonly UserManager<RecipeBookUser> _userManager;
        private RecipeBookContext _dbContext;

        public RecipeController(RecipeBookContext db)
        {
            _dbContext = db;
        }

        // 
        // GET: /Recipe/ 
        [AllowAnonymous]
        public IActionResult Index(string? search = null)
        {
            if(search == null)
            {
                IEnumerable<Recipe> recipeList = _dbContext.Recipes;
                return View(recipeList);
            }
            else
            {
                IEnumerable<Recipe> recipeList = _dbContext.Recipes.AsEnumerable().Where(x => x.Title.Contains(search, StringComparison.OrdinalIgnoreCase));
                return View(recipeList);
            }
        }

        // 
        // GET: /Recipe/RecipeContainer
        [AllowAnonymous]
        public IActionResult Search(string term)
        {
            IEnumerable<Recipe> recipeList = _dbContext.Recipes.AsEnumerable().Where(x => x.Title.Contains(term, StringComparison.OrdinalIgnoreCase));
            return RedirectToAction("Index", new { recipeList });
        }


        public IActionResult Create()
        {
            return View(new Recipe());
        }

        //POST
        [HttpPost]
        public IActionResult Create(Recipe model)
        {
            model.Ingredients.RemoveAll(x => x.Name == null);
            model.Directions.RemoveAll(x => x.DirectionText == null);

            if (ModelState.IsValid)
            {
                _dbContext.Recipes.Add(model);
                _dbContext.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        //GET
        [AllowAnonymous]
        [Route("Get/{id:int}")]
        public IActionResult Get(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var recipeFromDb = _dbContext.Recipes.FirstOrDefault(i => i.Id == id);

            if (recipeFromDb == null)
            {
                return NotFound();
            }
            else
            {
                var ingredientList = _dbContext.Ingredients.Where(i => i.RecipeId == recipeFromDb.Id).ToList();
                var directionsList = _dbContext.Directions.Where(i => i.RecipeId == recipeFromDb.Id).ToList();
            }

            return View(recipeFromDb);
        }

        //GET
        [AllowAnonymous]
        [Route("Get/{title}")]
        public IActionResult Get(string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                return NotFound();
            }

            var recipeFromDb = _dbContext.Recipes.OrderByDescending(i => i.Id).FirstOrDefault(i => string.Equals(i.Title, title));

            if (recipeFromDb == null)
            {
                return NotFound();
            }
            else
            {
                var ingredientList = _dbContext.Ingredients.Where(i => i.RecipeId == recipeFromDb.Id).ToList();
                var directionsList = _dbContext.Directions.Where(i => i.RecipeId == recipeFromDb.Id).ToList();
            }

            return View(recipeFromDb);
        }

        //GET
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var recipeFromDb = _dbContext.Recipes.Find(id);

            if (recipeFromDb == null)
            {
                return NotFound();
            }
            else
            {
                var ingredientList = _dbContext.Ingredients.Where(i => i.RecipeId == recipeFromDb.Id).ToList();
                var directionsList = _dbContext.Directions.Where(i => i.RecipeId == recipeFromDb.Id).ToList();
            }

            return View(recipeFromDb);
        }

        //POST
        [HttpPost]
        public IActionResult Edit(Recipe model)
        {
            model.Ingredients.RemoveAll(x => x.Name == null);
            model.Directions.RemoveAll(x => x.DirectionText == null);

            if (ModelState.IsValid)
            {
                try
                {
                    _dbContext.RemoveRange(_dbContext.Ingredients.Where(x => x.RecipeId == model.Id));
                    _dbContext.RemoveRange(_dbContext.Directions.Where(x => x.RecipeId == model.Id));

                    var recipe = new Recipe()
                    {
                        Id = model.Id,
                        Title = model.Title,
                        Type = model.Type,
                        Description = model.Description,
                        ImageUrl = model.ImageUrl,
                        Ingredients = model.Ingredients,
                        Directions = model.Directions
                    };

                    _dbContext.Recipes.Update(recipe);
                    _dbContext.SaveChanges();

                    return RedirectToAction("Get", "Recipe", new { id = model.Id });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return View(model);
                }
            }

            return View(model);

        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var recipeFromDb = _dbContext.Recipes.Find(id);

            if (recipeFromDb == null)
            {
                return NotFound();
            }
            else
            {
                var ingredientList = _dbContext.Ingredients.Where(i => i.RecipeId == recipeFromDb.Id).ToList();
                var directionsList = _dbContext.Directions.Where(i => i.RecipeId == recipeFromDb.Id).ToList();
            }

            _dbContext.Recipes.Remove(recipeFromDb);
            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        public IActionResult GetRecipes()
        {
            var recipeList = _dbContext.Recipes.ToList();

            foreach (var recipe in recipeList)
            {
                var ingredientList = _dbContext.Ingredients.Where(i => i.RecipeId == recipe.Id).ToList();
                var directionsList = _dbContext.Directions.Where(i => i.RecipeId == recipe.Id).ToList();
            }

            return View(recipeList);
        }
    }
}