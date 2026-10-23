# Changelog

Major project changes are listed below in reverse chronological order. Timestamps use the `Europe/Berlin` time zone (CEST, UTC+02:00 where applicable) and are based on Git commit timestamps.

## 2026-10-24

- **01:14** — Updated the README to document the service unit tests, GitHub Actions workflow, test commands, and AI-assisted documentation.

## 2026-10-24

- **01:03** — Added a GitHub Actions workflow that restores, builds, and runs the .NET test suite on pushes to `main`.
- **01:00** — Added service unit test suites for transactions, categories, places, and persons, using xUnit and Moq.
- **00:08** — Added the initial unit test suite and introduced/updated service and repository interfaces to support testable service dependencies.

## 2026-10-21

- **23:12** — Updated API documentation to describe the health endpoint and current configuration.
- **23:08** — Added the `GET /api/health` health endpoint.
- **22:22** — Documented the current security limitations and development roadmap.
- **22:06** — Added the initial API README.

## 2026-10-19

- **21:16** — Fixed the category MongoDB collection name and configured database connections for places and categories.
- **21:06** — Implemented the person repository, service, and controller.
- **20:57** — Implemented the transaction repository, service, and controller.
- **15:48** — Initialized the project structure.

## Notes

- This changelog summarizes major milestones; it is not intended to list every commit or minor change.
- The documentation, including this changelog, was created with the assistance of AI.
