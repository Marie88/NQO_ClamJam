# Key smells and violations identified in the original solution

## ClamJam.Library

### Class-wide smells and violations
1. The static properties (items, tax, currency) :
    - make the system not thread-safe,
    - values are shared accross all instances of the ShopManager making the app stateful
    - testability of the solution decreases as it is harder to mock and test in isolation features. 
      This can be seen when running all the initial unit tests and one fails because the state was influenced by
      previous operations conducted by the other unit tests
2. The Shop Manager breaks the SRP (single responsibility principle) as it is tasked with multiple operations :
   cart management, tax calculations, coupon management and export
3. Concepts that are complex are represented with the help of string and doubles instead of classes
4. Fragile and non extensible way to serialize objects (the items list)
5. Hardcoded values are used accross the application (for tax percentage, coupon types, currency),
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
calculations, that enquire calculating tax, exchange rates etc. "decimal" type is recommented to be used
as it holds a better precision, even though it needs more space and is less performant, the need
for precision for money-related calculations takes precedence.
2. When extracting the sum of all the prices for the items in the cart no exception handling
is present for the parsing of the price and there is no consideration for 
different VAT rates being applied on the current cart
3. Exception swallowing : When trying to parse the coupon percentage the catch of the "try, catch"
does not log or rethrow the exception, leaving the user clueless of an operation going wrong

### "Export" method
1. String concatenation can be made through string interpolation and string builder (especially for string creation through loops)
2. Coupling the users of the method only to a particular export format that is not extensible at all
3. Mixes the domain responsibilities (knowing about currency and ITEMS in the cart) and export logic (CSV specific)
