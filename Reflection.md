# Key smells and violations identified in the original solution

## ClamJam.Library

### Class-wide smells and violations
1. The static properties (items, tax, currency) :
    - makes the system not thread-safe,
    - values are shared accross all instances of the ShopManager making the app stateful
    - testability of the solution decreases, as it is harder to mock and test in isolation features
      This can be noticed while running all the initial unit tests and one fails because the state was influenced by
      previous operations conducted by the other unit tests
2. The Shop Manager breaks the SRP (single responsibility principle) as it is responsible of multiple operations :
   cart management, tax calculations, coupon management and export
3. Complex contexts that are represented with the help of string and doubles instead of classes
4. Fragile and non extensible way of serializing objects (the items list)
5. Hardcoded values are used across the application (for tax percentage, coupon types, currency),
   making it harder to extend the existing functionality, thus breaking the open-closed principle
6. There is no validation of the inputs, which might lead to unexpected situations when illegal/invalid
   values are processed
7. Non-suggestive parameter and variable names :
    - In "Add" method : params "n" and "p" (could be renamed to name and price)
    - In "Checkout" : variable "s" that represents the final price/ price to be paid

### "Add" method
1. Adding to the cart with a fragile serialization
2. Returns a hard-coded value as a response and does not treat error paths

### "Checkout" method
1. For calculating the final price of the cart, a variable of type "double" is used. For money related 
calculations, that require tax calculation, exchange rates etc. "decimal" type is recommented to be used
as it holds a better precision, even though it needs more space and is less performant, the need
for precision for money-related calculations takes precedence
2. When calculating the sum of all prices for the items in the cart there is no exception handling
for price parsing and there is no consideration for  different VAT rates being applied on the current cart
3. Exception swallowing : When trying to parse the coupon percentage the catch of the "try, catch"
does not log or rethrow the exception, leaving the user clueless of an operation which went wrong

### "Export" method
1. String concatenation can be made through string interpolation and string builder (especially for string creation through loops)
2. Coupling the users of the method only to a particular export format that is not extensible at all
3. Mixes the domain responsibilities (currency and cart management) and export logic (CSV specific)

# How the code refactored and why (principles/patterns used)

## Code structure
The original code was a fragile monolithic implementation and has been refactored into a modular, testable and maintainable structure, 
inspired by Clear Architecture. The new structure is split into :
- Domain project : Contains business rules, domain models, dtos and entities and it does not depend on any other project
- Applicatiom project : Orchestrates the application logic and depends on the Domain project
- Infrastructure project : Provides implementations for external concerns such as data storage, export 

This new structure makes it easier to work separately on each module and makes it easy to swap infrastructure concerns, however
it adds more overhead 

## Domain modelling
In the old solution, items used to be stored and transferred as raw serialized strings, in the new solution, the items are
strongly typed and also validated 
The new way of domaing modelling brings the following pros and cons:

PROS:
- cuts significantly deserialization related errors
- provides domain documentation through proper representation of business objects

CONS:
- more code/classes to maintain

## Extensibility and flexibility
The coupons/discounts, tax rate and export format were hardcoded and not easily extensible as whenever you have to modify or 
add a new strategy the core class required modifications, thus violating the open-closed principle 
In the new solution multiple patterns have been used to make the solution more flexible :
- The taxation is determined dynamically based on the product type and adding a new strategy requires only the implementation of the ITaxStrategy interface
  and mapping it to the right product type (Strategy pattern)
- The coupons/discounts are created dynamically based on the coupon type passed from the user through a factory that can be extended 
with a new coupon type (Factory pattern)
- The export logic is determined through a factory that is based on user input and is easily extensible only by adding
  a new implementation of the IExportService (Factory pattern)

## Exception and validation handling
With the old solution the erros were swallowed making it nearly imposible for a normal user to understand what went wrong
With the new solution, explicit checks and meaningful exceptions were added in order to guard against invalid data
and make failures easier to debug/understand

# What I would do next with more time
Business wise :
- I would expand the discount/coupon logic to be able to :
    - add discounts per item, the posibility of adding multiple coupons per cart
    - add a ranking system on discounts that determine the best applicable discount(s), as not all discounts can be applied
- Support for different payment providers
- Support for different currencies
- Support for different shipping providers and integrating the costs into the pricing service
- Expand the products with stock, expiration date, needs/does not need refrigeration

Technical wise
- For the persistence layer, I would choose to go for a relational data base implementation, as the in-memory repository is not scalable
- Introduce configuration management
- Introduce caching for concepts that have a higher read than write rate (e.g. Products that update on a daily basis)
- Introduce logging and monitoring
- Add circuit breakers and retry policies
- Add authentication and authorization
