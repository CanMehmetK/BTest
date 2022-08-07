[![.NET Core](https://github.com/CanMehmetK/BTest/workflows/.NET%20Core/badge.svg)](https://github.com/CanMehmetK/B-Test/actions)

## Bet Test 

Please complete the following test using the tech stack that will best demonstrate your technical
ability. During the presentation, you'll be asked about the design decisions, including the tech stack
you've chosen and your reasoning behind it.
## Front-end:
Create a web front end.
At BET, we prefer the use of ASP.NET MVC or a SPA framework (Angular 2+ or react.js), typescript or
JavaScript.
The candidate is not limited to BET's preferences and can use any framework desired as long it's a
web front end.
Ensure your front end is presentable and well formatted to be reviewed by an interview panel.
## Middleware:
Create a REST API that will facilitate the communication between then Web front end and your
database.
At BET, we prefer the use of .net framework web API or .net core web API using C#. Candidates is not
limited to our preferences as long it's a REST API.
Data persistence:
Any relational database (preferred is MS SQL)

## Test:
For this test, we would like you to create a basic e-commerce site. Below are the functional
requirements that are needed:
1. User registration (Register with an email address and password and confirm password),
with email validation
2. User login (with an email address and password)
3. Display a list of products (show product image, product name, price and quantity)
4. A 'add to cart button next to each product
5. View shopping cart (show the product, quantity and price with a Total value of all the
products selected)
6. Checkout button (send an email to the logged-in users' email address with all the
products they have purchased with an order number)
7. Store user information in a database as well as the products purchased for each user
against an order number.

**Important points to remember when doing this test:**
1. Applying SOLID principles
2. Dependency injection
3. Performance in terms of paging etc.
4. Proper database design
5. Securing of middleware (API)
6. Deployment notes for the application, including database, as well as any configuration.

This test should take you between 5 to 7 days. If you have any questions, please feel free to contact
BET recruitment.

Good luck.

Check **appsettings.json** **DefaultConnection**

Dev: Start BTest.SPA project

or **run publish command**

dotnet publish -p:PublishProfile=FolderProfile

start bin/Release/net6.0/publish/BTest.SPA.exe

or bin/Release/net6.0/publish/BTest.SPA.dll ;) 