# uga-mpl-server

A simple ASP.NET Core Web API backend for a UGA marketplace project.

## Quick Start

1. Create a `.env` file in the project root (separator must be `__=__`).
2. Add required environment variables:

```env
POSTGRESQL_CONN_STRING__=__Host=...;Port=5432;Database=...;Username=...;Password=...
JWT_ISSUER__=__uga-mpl-server
JWT_KEY__=__your-secret-key
AUTHENTICATION_GOOGLE_CLIENT_ID_ANDROID__=__...
AUTHENTICATION_GOOGLE_CLIENT_ID_IOS__=__...
AUTHENTICATION_GOOGLE_CLIENT_ID_WEB__=__...
```

3. Run:

```bash
dotnet ef database update
dotnet run
```

Default URL: `http://localhost:5274`

## ProductController

`ProductController` provides authenticated product management (JWT required):

- `GET /api/product`: Get product list.
- `GET /api/product/{id}`: Get product details.
- `POST /api/product`: Create a product (seller is current user).
- `PATCH /api/product/{id}`: Update a product (owner only).
- `DELETE /api/product/{id}`: Delete a product (owner only).

Responses use `ProductDTO`, including seller info (`SellerName`) and basic product fields.
