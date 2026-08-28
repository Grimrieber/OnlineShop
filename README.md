# OnlineShop

A multi-vendor marketplace — sellers open shops, list products, and message buyers; buyers browse,
favourite, review, and check out.

**Stack** — VB.NET · ASP.NET WebForms · Entity Framework · SQL Server

## Domain

Thirteen tables covering the full marketplace: `Shops`, `Products`, `ProductImages`, `Categories`,
`CartItems`, `Payments`, `Reviews`, `Favorites`, `UserProductViews`, and a two-tier messaging model
(`Conversations` / `Messages` for buyer-seller threads, `ShopMessages` for shop-level enquiries).

## Pages

Storefront (`Default`), product browse and detail, category browse, shop detail, and two separate
search surfaces — product search and shop search.

## Running it locally

**Requires** Visual Studio 2022, .NET Framework 4.x, and SQL Server LocalDB (ships with Visual Studio).

1. Create the database and schema:
   ```
   sqlcmd -S "(localdb)\MSSQLLocalDB" -Q "CREATE DATABASE OnlineShop"
   sqlcmd -S "(localdb)\MSSQLLocalDB" -d OnlineShop -I -i Database/schema.sql
   ```
2. Open the solution in Visual Studio and press F5.

The connection string in `Web.config` points at LocalDB using Integrated Security — there are no
credentials in this repository, and none are required.
