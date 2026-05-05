# uga-mpl-server

A simple ASP.NET Core Web API backend for a UGA marketplace project. The app is a mobile-only university marketplace where students can list, browse, and sell items to other UGA students.

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- PostgreSQL instance (local or hosted, e.g. [Neon](https://neon.tech))
- EF Core CLI tools:
    ```bash
    dotnet tool install --global dotnet-ef
    ```
- A Google Cloud project with OAuth 2.0 client IDs for Android, iOS, and Web

## Quick Start

1. Create a `.env` file in the project root (separator must be `__=__`).
2. Add required environment variables:

```env
POSTGRESQL_CONN_STRING__=__Host=...;Port=5432;Database=...;Username=...;Password=...;SSL Mode=VerifyFull;Channel Binding=Require;
JWT_ISSUER__=__http://localhost:5274/
JWT_KEY__=__<64-char hex string>
AUTHENTICATION_GOOGLE_CLIENT_ID_WEB__=__...
AUTHENTICATION_GOOGLE_CLIENT_SECRET_WEB__=__...
AUTHENTICATION_GOOGLE_CLIENT_ID_ANDROID__=__...
AUTHENTICATION_GOOGLE_CLIENT_ID_IOS__=__...
```

**Generating a JWT key** :: run this and paste the output as `JWT_KEY`:

```bash
openssl rand -hex 64
```

3. Run:

```bash
dotnet ef database update
dotnet run
```

Default URL: `http://localhost:5274`

> Migrations also run automatically on startup via `DbInitializer`, so `dotnet ef database update` is only needed on first run or after adding new migrations manually.

## Environment Variables

| Variable                                  | Description                                                 |
| ----------------------------------------- | ----------------------------------------------------------- |
| `POSTGRESQL_CONN_STRING`                  | Full Npgsql connection string to your PostgreSQL instance   |
| `JWT_ISSUER`                              | Issuer claim for app-issued JWTs :: use the server base URL |
| `JWT_KEY`                                 | HS256 signing key, minimum 64 hex characters                |
| `AUTHENTICATION_GOOGLE_CLIENT_ID_WEB`     | Google OAuth web client ID                                  |
| `AUTHENTICATION_GOOGLE_CLIENT_SECRET_WEB` | Google OAuth web client secret                              |
| `AUTHENTICATION_GOOGLE_CLIENT_ID_ANDROID` | Google OAuth Android client ID                              |
| `AUTHENTICATION_GOOGLE_CLIENT_ID_IOS`     | Google OAuth iOS client ID                                  |

## API Reference

### Auth :: `/api/auth`

| Method | Endpoint                   | Auth       | Description                                   |
| ------ | -------------------------- | ---------- | --------------------------------------------- |
| POST   | `/api/auth/google-signin`  | Google JWT | Sign in with Google, returns app JWT + user   |
| POST   | `/api/auth/google-signup`  | Google JWT | Verify Google token, check email availability |
| POST   | `/api/auth/create-account` | None       | Create account, returns app JWT + user        |

### User :: `/api/user`

| Method | Endpoint                          | Auth    | Description                       |
| ------ | --------------------------------- | ------- | --------------------------------- |
| GET    | `/api/user/by-email?email=`       | App JWT | Get user by email                 |
| GET    | `/api/user/{id}`                  | App JWT | Get user by ID                    |
| POST   | `/api/user`                       | None    | Create user                       |
| PATCH  | `/api/user/push-token`            | App JWT | Save Expo push notification token |
| POST   | `/api/user/wishlist/{productId}`  | App JWT | Add product to wishlist           |
| DELETE | `/api/user/wishlist/{productId}`  | App JWT | Remove product from wishlist      |
| POST   | `/api/user/subscribe/{productId}` | App JWT | Subscribe to product              |
| DELETE | `/api/user/subscribe/{productId}` | App JWT | Unsubscribe from product          |

### Product :: `/api/product`

| Method | Endpoint                           | Auth    | Description                                       |
| ------ | ---------------------------------- | ------- | ------------------------------------------------- |
| GET    | `/api/product`                     | App JWT | Get all products                                  |
| GET    | `/api/product/{id}`                | App JWT | Get product by ID                                 |
| GET    | `/api/product/category/{category}` | App JWT | Get products filtered by category                 |
| POST   | `/api/product`                     | App JWT | Create product                                    |
| PATCH  | `/api/product/{id}`                | App JWT | Update product details (seller only)              |
| PATCH  | `/api/product/{id}/availability`   | App JWT | Toggle availability (seller only)                 |
| PATCH  | `/api/product/{id}/location`       | App JWT | Set meetup coordinates (seller only)              |
| POST   | `/api/product/{id}/sell`           | App JWT | Mark as sold and record transaction (seller only) |
| GET    | `/api/product/{id}/subscribers`    | App JWT | Get subscriber list (seller only)                 |
| DELETE | `/api/product/{id}`                | App JWT | Delete product (seller only)                      |

### Transaction :: `/api/transaction`

| Method | Endpoint                     | Auth    | Description                            |
| ------ | ---------------------------- | ------- | -------------------------------------- |
| GET    | `/api/transaction/sales`     | App JWT | Get all sales for the current user     |
| GET    | `/api/transaction/purchases` | App JWT | Get all purchases for the current user |

## Notes

- Only `@uga.edu` email addresses are accepted at account creation.
- Google JWT (`Bearer` scheme) is used only for `/api/auth/google-signin` and `/api/auth/google-signup`. All other protected endpoints use the app JWT scheme (`AppJwt`).
- Expo push tokens are stored per user and exposed on `UserSummaryDTO` so sellers can notify buyers directly via Expo's Push API.

## ProductController

`ProductController` provides authenticated product management (JWT required):

- `GET /api/product`: Get product list.
- `GET /api/product/{id}`: Get product details.
- `POST /api/product`: Create a product (seller is current user).
- `PATCH /api/product/{id}`: Update a product (owner only).
- `DELETE /api/product/{id}`: Delete a product (owner only).

Responses use `ProductDTO`, including seller info (`SellerName`) and basic product fields.

## Important flag

```
%%%%%%%%%%%%%%%% VALIDATE UGA EMAIL DOMAIN %%%%%%%%%%%%%%%%
```
