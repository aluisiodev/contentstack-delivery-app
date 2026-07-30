# Contentstack Content Delivery Web Application

An ASP.NET Core (Razor Pages) web application that consumes structured content from
[Contentstack](https://www.contentstack.com/), a headless CMS, through the official
.NET Content Delivery SDK.

Content is authored and published in Contentstack, then fetched over the Content Delivery API
and rendered server-side — no content is hardcoded in the application.

---

## Tech stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core 10 (Razor Pages) |
| Language | C# |
| CMS | Contentstack (headless, EU region) |
| SDK | `contentstack.csharp` 2.25.2 |
| Serialization | Newtonsoft.Json |
| Front-end | HTML5, CSS3, vanilla JavaScript |

---

## Architecture

```
Browser
   │
   ▼
Razor Pages (presentation)
   │
   ▼
ContentstackClient (injected via DI)
   │
   ▼
Contentstack Content Delivery API
```

The `ContentstackClient` is registered once in `Program.cs` as a singleton and injected into
page models through constructor injection. Page models never instantiate the client directly,
which keeps the presentation layer decoupled from the data access concern and makes the
codebase testable.

---

## Content model

Two content types and one global field are defined in Contentstack:

**`author`** — content type
- `title` — author name
- `bio` — rich text

**`blog_post`** — content type
- `title` — post title
- `url` — post slug
- `body` — rich text
- `author` — **reference** to an `author` entry
- `seo` — **global field**

**`seo`** — global field (reusable across content types)
- `meta_title` — single line text
- `meta_description` — multi line text

### Why a reference for the author and a global field for SEO

A **reference** shares the *data*: many posts point to a single author entry, so correcting
an author's bio once updates every post that references it. No duplication exists to keep in sync.

A **global field** shares the *schema*: each post owns its own SEO values, but the structure of
that block is defined in one place. Adding a field to the global field propagates the new field
to every content type that uses it.

---

## Running locally

### Prerequisites

- [.NET SDK 10](https://dotnet.microsoft.com/download) or later
- A [Contentstack](https://www.contentstack.com/explorer) account with a stack

### 1. Clone

```bash
git clone https://github.com/aluisiodev/contentstack-delivery-app.git
cd contentstack-delivery-app/Web
```

### 2. Configure credentials

Credentials are **never committed**. They are supplied through
[.NET User Secrets](https://learn.microsoft.com/aspnet/core/security/app-secrets) in development:

```bash
dotnet user-secrets init
dotnet user-secrets set "Contentstack:ApiKey" "<your stack api key>"
dotnet user-secrets set "Contentstack:DeliveryToken" "<your delivery token>"
dotnet user-secrets set "Contentstack:Environment" "development"
```

Both values are found in the Contentstack dashboard under **Settings → Tokens → Delivery Tokens**.

> **Region note:** this stack is hosted in the EU data centre, so the client is initialised
> against the `eu-cdn.contentstack.com` host. Stacks in other regions require a different host.

### 3. Run

```bash
dotnet run
```

The app will be available at the URL printed in the console (typically `https://localhost:7xxx`).

> Entries must be **published** to the target environment before the Content Delivery API
> returns them. Unpublished entries are invisible to the delivery API by design.

---

## Technical decisions

**Dependency injection over direct instantiation.** The Contentstack client is registered as a
singleton and injected into page models. This avoids creating a new HTTP client per request and
allows the dependency to be substituted in tests.

**Secrets outside the repository.** `appsettings.json` ships with empty placeholder keys purely
as documentation of what configuration is required. Actual values live in User Secrets during
development and would come from environment variables or a secret store in production.

**Graceful degradation on API failure.** Content fetches are wrapped in exception handling that
logs the technical detail server-side and renders a human-readable message in place of the content,
rather than returning a 500 and failing the entire request. Stack traces are never surfaced to users.

**Explicit reference expansion.** Reference fields are not expanded by default — `IncludeReference`
is called only for references the page actually renders, keeping response payloads proportional
to what is used.

**Strongly typed mapping.** API responses are deserialized into C# models rather than consumed as
dynamic JSON, so field name mismatches surface as empty values against a known schema instead of
runtime key errors scattered through the views.

---

## Project status

### Implemented

- [x] ASP.NET Core Razor Pages application
- [x] Contentstack Content Delivery SDK integration
- [x] Content modelling: content types, references, reusable global fields
- [x] Development and production environments with scoped delivery tokens
- [x] Strongly typed content mapping
- [x] Dependency injection for the content client
- [x] Error handling with graceful degradation
- [x] Secrets management via User Secrets

### Roadmap

- [ ] Dedicated `IContentService` abstraction over the SDK
- [ ] Individual post detail pages routed by slug
- [ ] Redis caching layer (cache-aside pattern)
- [ ] Webhook endpoint for cache invalidation on publish
- [ ] Semantic, responsive and accessible front-end
- [ ] Progressive JavaScript enhancements
- [ ] xUnit unit and integration tests
- [ ] Deployment

---

## License

MIT
