Scenario

A client wants a website where users can easily search for movie trailers across multiple online channels.

Target users

- Movie and series enthusiasts (potentially thousands to millions of viewers)

Requirements

- Use an existing movie database for metadata (e.g., The Movie Database — https://www.themoviedb.org/documentation/api). You may choose a different API if preferred.
- Use an existing video hosting service for trailers (e.g., YouTube, Vimeo).
- Provide an easy-to-use search experience for movies and series.
- Support paginated search results.
- Provide a detail page (API response) per film or series that includes metadata and trailer(s).

Additional context

- The application should be future-proof and follow industry standards.
- Global scale is a goal, so design for scalability.
- Excellent UX and high performance are required.

Assignment

1. Build an API that allows searching for movies. The API should return the necessary movie data in responses.
   - Consider whether to use the external Search API or other endpoints when handling end-user queries.
2. Optimize search queries and response times. Provide sensible defaults and caching where appropriate.
3. You may make reasonable assumptions; document them.
4. Apply software development best practices (clean code, configuration, error handling, logging, tests where feasible).
5. Aim to complete the assignment within ~4 hours — a working, well-explained solution is preferred over perfect polish.
6. A frontend is not required — focus on the API and its behaviour.

Bonus points

- Demonstrate a globally scalable deployment approach.
- Support multiple locales/localization.
- Provide a clear explanation of how the application is protected against OWASP Top 10 vulnerabilities.

