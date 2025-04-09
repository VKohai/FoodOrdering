namespace FoodOrdering;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        #region Authentication Pages
        Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
        Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
        Routing.RegisterRoute(nameof(StaffOnlyPage), typeof(StaffOnlyPage)); 
        #endregion

        Routing.RegisterRoute(nameof(CartPage), typeof(CartPage));
        Routing.RegisterRoute(nameof(CatalogPage), typeof(CatalogPage));

        #region Product Pages
        Routing.RegisterRoute(nameof(AddProductPage), typeof(AddProductPage));
        Routing.RegisterRoute(nameof(EditProductPage), typeof(EditProductPage));
        Routing.RegisterRoute(nameof(ProductDetailsPage), typeof(ProductDetailsPage));
        #endregion

        #region Order pages
        Routing.RegisterRoute(nameof(OrdersPage), typeof(OrdersPage));
        Routing.RegisterRoute(nameof(OrderDetailsPage), typeof(OrderDetailsPage)); 
        #endregion
    }
}