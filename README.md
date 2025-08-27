# Clean Code Refactoring Challenge (BadShop)

Welcome! This small assignment is intended for you to showcase your level of design and coding skills. Your goal is to refactor the project while keeping behavior intact and to explain your reasoning.

## What you get

- Solution: `Challenge.Refactor/Challenge.Refactor.sln`
- Projects:
  - `BadShop.Library` — backend project with core business logic.
  - `BadShop.Api` — minimal API
  - `BadShop.Tests` — a few xUnit tests that capture the current (even if questionable) behavior.

## Your tasks

1. Refactor and redesign to improve readability, testability, maintainability. Take the code to what you consider perfection. You can change and redesign anything you want, including interfaces, API... Feel free to add new classes, methods, etc. If you feel it allows you to demonstrate your skill better, you are welcome but not required to add new features, either technical such as persistence (targeting SqlServer), logging, configuration... or functional.
2. Design for extensibility: Think about how requirements might change over time or across different users and design accordingly.Making design trade-offs often comes with questions about the domain. Make reasonable assumptions and document them.
3. Use git commits with [conventional](https://www.conventionalcommits.org/en/v1.0.0/) commit messages to track changes
4. Add Swagger documentation and endpoint
5. Summarily document what you changed, why, and trade-offs (if applicable), in `REFLECTION.md`. (Keep it short and simple.)

## Constraints

- Do not change the public behavior covered by the existing tests unless you also update the tests along with strong justification. Prefer adding tests instead.
- Target `net9.0` (project files already set). Use xUnit for tests.
- You can use any IDE you want, we use JetBrains Rider.
- Keep the use of external libraries to a minimum.

## How to run

- Build: `dotnet build Challenge.Refactor/Challenge.Refactor.sln`
- Run tests: `dotnet test Challenge.Refactor/Challenge.Refactor.sln`
- Run API: `dotnet run --project Challenge.Refactor/src/BadShop.Api`

## What to submit

- A Git repository containing your refactoring:
  - All modified and new source files.
  - A `REFLECTION.md` explaining:
    - Key smells and violations you identified.
    - How you refactored and why (principles/patterns used).
    - What you would do next with more time.
  - If needed, clear instructions to build/test and any assumptions.

## Time guidance

- Expected time: 1.5–3 hours. Depth over breadth is valued; focus on the core.

## Miscellaneous
- you are allowed to use AI.
- Come prepared to the interview to present (5 minutes max) the important design decisions and trade-offs you made. Expect questions.

Good luck and have fun!
