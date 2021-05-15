using System.Collections;
using System.Collections.Generic;
using DTWorldz.Items.Models;
using DTWorldz.Items.SO;
using UnityEngine;
namespace DTWorldz.Items
{
    public interface IRecipe
    {
        int Id { get; set; }
        List<RecipeRequirement> Requirements { get; set; }
        BaseItemSO Output { get; set; }
    }
}