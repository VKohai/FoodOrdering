#region FoodOrdering

global using FoodOrdering.Pages;
global using FoodOrdering.Models;
global using FoodOrdering.Services;
global using FoodOrdering.Pages.Orders;
global using FoodOrdering.Pages.Products;
global using FoodOrdering.Pages.Authentication;

global using static FoodOrdering.Services.SupabaseService;

#endregion

global using System.Collections.ObjectModel;

#region Supabase

global using Supabase;
global using Supabase.Gotrue;
global using Supabase.Postgrest.Models;
global using Supabase.Gotrue.Exceptions;
global using Supabase.Postgrest.Attributes;
#endregion
