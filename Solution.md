Solution overview

This document describes the architecture, components and how to run the WhatsOn API solution in this repository.

1) High-level overview

- Purpose: provide a backend API to search for movies and return movie metadata and trailers from external providers.
- Style: Minimal APIs (ASP.NET Core) with small service layer abstractions (MovieService, ShowService).
- External dependencies: The Movie Database (TMDb) for metadata and video references; YouTube/Vimeo are assumed for trailers referenced by TMDb videos.

2) Project structure

- WhatsOn.Api  minimal API project exposing endpoints and composing services.
- WhatsOn.Service  business logic (MovieService, ShowService) that calls external APIs.
- WhatsOn.Domain  shared domain models (Request/Response, PagedResult).
- Orchestration.*  helper projects for hosting defaults (e.g., Redis, service defaults) used for deployments.

3) Public endpoints

- GET /movies/getmovies
  - Query parameters: query (string), pageNumber (int), includeAdult (bool)
  - Returns: paginated movie search results (PagedResult<Movie>) and message when appropriate.

- GET /movies/moviedetails/{id}
  - Returns: detailed movie information, including videos when available (MovieDetailResponse).

(There are equivalent endpoints for shows under /shows.)

4) Key implementation details

- MovieService
  - Uses a configured HttpClient (base URL set to TMDb API) and attaches a DelegatingHandler (TheMovieDbAuthHandler) to inject the TMDb Bearer token.
  - Search behavior: if a non-empty query is provided the service calls the search endpoint (search/movie). Otherwise it uses discover/movie for discovery results.
  - Movie details: calls movie/{id}?append_to_response=videos to include trailers and other videos in the same response.
  - JSON: System.Text.Json with case-insensitive settings and a SnakeCaseLower policy to map TMDb responses.

- HTTP clients and resilience
  - HttpClients are configured via ConfigureHttpClients and include AddStandardResilienceHandler (retry/circuit-breaker policies are applied there).

- Caching and performance
  - Output caching policies are defined (MovieSearch 15 minutes, MovieDetails 24 hours, similar for shows).
  - Redis is configured in Program.cs (AddRedisClient and AddRedisOutputCache) to back the cache for distributed scenarios and scale-out.
  - Response compression is enabled and HTTPS redirection is enforced.
  - Rate limiting is enabled to protect against abuse and help stability at scale.

- Logging and error handling
  - Services catch HttpRequestException and generic Exception and log errors while returning a sanitized message to callers.

5) Configuration & secrets

- ExternalServicesSettings in appsettings.json contains TheMovieDbDocumentationApiBaseUrl (defaulted to https://api.themoviedb.org/3/).
- TheMovieDbAccessToken must be provided separately (user secrets, environment variable, or other secret store). Example scripts exist (set-tmdb-access-token.cmd uses dotnet user-secrets).

6) Running locally

1. From repository root run (or open WhatsOn.Api and run it from Visual Studio):
   dotnet run --project WhatsOn.Api

2. Ensure the TMDb access token is configured. Example using user-secrets (windows cmd included in repo):
   set-tmdb-access-token.cmd

3. Optional: configure Redis (or remove Redis configuration if not required). For local development the app runs without a Redis instance but some caching features will be limited.

7) Security and OWASP considerations

- Credentials: API secrets are not checked in and must be provided via secrets or environment variables.
- Transport: HTTPS redirection is enforced in Program.cs.
- Input validation: endpoints accept typed parameters (int, bool, string) and MovieService builds query strings carefully using Uri.EscapeDataString for user-provided text to mitigate header/URL injection.
- Auth to external API: the TMDb access token is passed via an Authorization header in TheMovieDbAuthHandler.
- Rate limiting and output caching reduce the risk from abusive clients and help availability.
- Additional mitigations to consider (not exhaustive): validate and limit page sizes, enable request body size limits, sanitize fields returned to clients, add authentication/authorization on the API if needed, secure headers (CSP, HSTS), and perform dependency scanning for vulnerabilities.

8) Scalability & deployment notes

- Stateless services with distributed Redis-backed cache make the solution friendly to horizontal scaling and global rollouts behind a load balancer/CDN.
- Output cache policies and long-lived movie detail caches reduce load on TMDb and speed responses for frequently requested resources.
- Use container images (WhatsOn.Api/Dockerfile exists) and orchestration (Kubernetes or serverless) for global deployments.