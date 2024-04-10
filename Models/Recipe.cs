using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace RecipeBook.Models
{
    public enum RecipeType
    {
        Cooking = 0, Baking = 1, Drink = 2
    }

    public class Recipe
    {
        public Recipe()
            : this(string.Empty, 0, string.Empty, string.Empty, new List<Ingredient>(), new List<Direction>())
        {
        }

        public Recipe(string title, int type, string description, string imageUrl, List<Ingredient> ingredients, List<Direction> directions)
            : this(null, title, type, description, imageUrl, ingredients, directions)
        {
        }

        public Recipe(int? id, string title, int type, string description, string imageUrl, List<Ingredient> ingredients, List<Direction> directions)
        {
            Id = id;
            Title = title;
            Type = type;
            Description = description;
            ImageUrl = imageUrl;
            Ingredients = ingredients;
            Directions = directions;
        }

        public int? Id { get; set; }

        [Required(ErrorMessage = "Title is required!")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Type is required!")]
        public int Type { get; set; }
        [Required(ErrorMessage = "A discription is required!")]
        public string Description { get; set; }
        [Required(ErrorMessage = "A url is required!")]
        [DisplayName("Image Url")]
        public string ImageUrl { get; set; }
        [Required(ErrorMessage = "Ingredients are required!")]
        public List<Ingredient> Ingredients { get; set; }
        [Required(ErrorMessage = "Directions are required!")]
        public List<Direction> Directions { get; set; }
    }
}