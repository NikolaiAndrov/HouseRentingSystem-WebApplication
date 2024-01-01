# House Renting Worldwide

House Renting Worldwide is an ASP.NET MVC web application designed to facilitate house renting for users seeking rental accommodations. The platform caters to both Agents and Admins, each with specific roles and functionalities.

## Features

### Users
- Users can:
 - Rent and leave houses.


### Agents
- Agents can:
  - Create, edit, and delete their own house listings.
  - View their house inventory.

### Admins
- Admins have extended privileges to:
  - Create, edit, delete, and rent all houses.

### All Users
- All Users can:
  - Search by category or key word.
  - Select houses per page.
  - Sort houses by newest, oldest, price ascending, price descending, not rented first

### Web API for Statistics
- The application includes a Web API endpoint for statistics:
  - The statistics provides information about total houses and rented houses count.

## Installation

To run this application locally, follow these steps:

1. Clone the repository locally.
2. Provide the connection string to the database.
3. Set multiple startup projects: HouseRentingSystem.Web, HouseRentingSystem.WebApi
4. Run the application
