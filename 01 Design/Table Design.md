# Table Design

As part of this project, I will need to include a database. I believe the below tables to be necessary.

## UserRoles

### Overview

This table will contain information around the different types of user role.

At this point, this will include the `Admin` role, which will be able to log in and access the system, as well as the `Customer` role which will be used as part of the booking.

The `UserRoles` table has a one-to-many relationship with the `Users` table. That is, one role can belong to multiple users.

### Structure

- UserRolesId: Integer, Primary Key, Not Null, Unique, Auto Increment
- RoleName: Text, Unique, Not Null

### Example

```csv
UserRoleId,RoleName
1,Admin
2,Customer
```

## Users

### Overview

This table will contain the minimal amount of information on a specific user.

The `UsernameOrEmail` will be used either for the email address of a `Customer` or for the username of an `Admin`.

The `Users` table has a many-to-one relationship with the `UserRoles` table. That is, many users can have the same one role.

The `Users` table has a one-to-one relationship with the `UserCredentials` table. As one user will have one hashed password.

The `Users` table will have a one-to-one relationship with the `UserContactDetails` table. As one user will have one set of contact details.

The `Users` table will have a many-to-one relationship with the `Bookings` table. This is because, a user can have multiple bookings.


### Structure

- UsersId: Integer, Primary Key, Not Null, Unique, Auto Increment
- UsernameOrEmail: Text, Unique, Not Null
- UserRoles.UserRolesId: Integer, Foreign Key, Not Null

### Example

```csv
UsersId,UsernameOrEmail,UserRoles.UserRolesId
1,DefaultAdmin,1
2,john.smith@mail.me,2
3,jane.doe@mail.com,2
4,AnotherAdmin,1
```

## UserCredentials

### Overview

This table will contain the hashed password for a user.

Only `Admin` users at this stage will have credentials.

The `UserCredentials` table will have a one-to-one relationship with the `Users` table. As one user will have one hashed password.

### Structure

- UserCredentialsId: Integer, Primary Key, Not Null, Unique, Auto Increment
- Users.UsersId: Integer, Foreign Key, Not Null, Unique
- HashedPassword, Text, Not Null

### Example

```csv
UserCredentialsId,Users.UsersId,HashedPassword
1,1,5f4dcc3b5aa765d61d8327deb882cf99
2,4,126fcc5a5dd775d64e9327deb882cf12
```

## UserContactDetails

### Overview

This table will contain the contact details for a user.

Only `Customer` users at this stage will have contact details.

The `UserContactDetails` table will have a one-to-one relationship with the `Users` table. As one user will have one set of contact details.

### Structure

- UserContactDetailsId: Integer, Primary Key, Not Null, Unique, Auto Increment
- FullName: Text
- Users.UsersId: Integer, Foreign Key, Not Null, Unique
- Telephone: Text

### Example

```csv
UserContactDetailsId,Users.UsersId,Telephone
1,2,0800001066
```

## Tours

### Overview

This table will contain data around the tours that are available.

The `Tours` table will have a many-to-one relationship with the `RoomTypesTours` table. This is because, one tour type will have many entries in the room types tours table, due to the different room types.

The `Tours` table will have a many-to-one relationship with the `Bookings` table. This is because, a tour can have multiple bookings.

### Structure

- ToursId: Integer, Primary Key, Not Null, Unique, Auto Increment
- TourStartDate: Date, Unique, Not Null

### Example

```csv
ToursId,TourStartDate
1,2022-06-06
2,2022-06-13
```

## RoomTypes

### Overview

This table will store information about the different types of room.

The `RoomTypes` table will have a one-to-one relationship with the `RoomCostInfo` table. This is because one room type will have one set of pricing.

The `RoomTypes` table will have a many-to-one relationship with the `RoomTypesTours` table. This is because, one room type will have many entries in the room types tours table, due to the different room types.

The `RoomTypes` table will have a many-to-one relationship with the `Bookings` table. This is because, a room type can belong to multiple bookings.

### Structure

- RoomTypesId: Integer, Primary Key, Not Null, Unique, Auto Increment
- RoomName: Text, Unique, Not Null

### Example

```csv
RoomTypesId,RoomName
1,Penthouse
2,Luxury
```

## RoomCostInfo

### Overview

This table will contain costing information about a room.

The `CurrentPricePence` field store the price of the room in pence. Pence has been used to prevent floating point errors.

The `DiscountRate` field will be the discount for single occupancy. It will be stored as an integer, and converted in the C# code. For example 20 will be stored for a discount of 20%.

The `RoomCostInfo` table will have a one-to-one relationship with the `RoomTypes` table. This is because one room type will have one set of pricing.

### Structure

- RoomCostInfoId: Integer, Primary Key, Not Null, Unique, Auto Increment
- RoomTypes.RoomTypesId: Integer, Foreign Key, Not Null, Unique
- DefaultQuantity: Integer, Not Null
- CurrentPricePence: Integer, Not Null
- DiscountRate: Integer, Not Null

### Example

```csv
RoomCostInfoId,RoomTypes.RoomTypesId,DefaultQuantity,CurrentPricePence,DiscountRate
1,1,1,78500,20
2,2,2,56500,18
```

## RoomTypesTours

### Overview

This table will link a room and a tour, in order to track the current quantity.

The `RoomTypesTours` table will have a many-to-one relationship with the `Tours` table. This is because, one tour type will have many entries in the room types tours table, due to the different room types.

The `RoomTypesTours` table will have a many-to-one relationship with the `RoomTypes` table. This is because, one room type will have many entries in the room types tours table, due to the different room types.

The `CurrentQuantity` field will need controlling in the code, to stop it going above the default quantity of `RoomCostInfo`.

### Structure

- RoomTypesToursId: Integer, Primary Key, Not Null, Unique, Auto Increment
- Tours.ToursId: Integer, Foreign Key, Not Null
- RoomTypes.RoomTypesId: Integer, Foreign Key, Not Null
- CurrentQuantity: Integer, Not Null

### Example

```csv
RoomTypesToursId,Tours.ToursId,RoomTypes.RoomTypesId,CurrentQuantity
1,1,1,0
2,1,2,1
```

## BookingStates

### Overview

This table will contain the different states of a booking.

At this stage they will be `Booked`, `Refunded`, and `Cancelled`.

The `BookingStates` table will have a one-to-many relationship with the `Bookings` table. This is because, a booking state can belong to multiple bookings.

### Structure

- BookingStatesId: Integer, Primary Key, Not Null, Unique, Auto Increment
- BookingState: Text, Unique, Not Null

### Example

```csv
BookingStatesId,BookingState
1,Booked
2,Refunded
3,Cancelled
```

## Bookings

### Overview

This table will contain information about bookings.

The `Bookings` table will have a one-to-many relationship with the `Users` table. This is because, a user can have multiple bookings.

The `Bookings` table will have a one-to-many relationship with the `Tours` table. This is because, a tour can have multiple bookings.

The `Bookings` table will have a one-to-many relationship with the `RoomTypes` table. This is because, a room type can belong to multiple bookings.

The `Bookings` table will have a one-to-many relationship with the `BookingStates` table. This is because, a booking state can belong to multiple bookings.


### Structure

- BookingsId: Integer, Primary Key, Not Null, Unique, Auto Increment
- Users.UsersId: Integer, Foreign Key, Not Null
- Tours.ToursId: Integer, Foreign Key, Not Null
- RoomTypes.RoomTypesId: Integer, Foreign Key, Not Null
- BookingStates.BookingStatesId: Integer, Foreign Key, Not Null
- BookingPrice: Integer, Not Null

### Example

```csv
BookingsId,Users.UsersId,Tours.ToursId,RoomTypes.RoomTypesId,BookingStates.BookingStatesId
1,2,1,1,1,100
1,2,2,1,3,200
```

