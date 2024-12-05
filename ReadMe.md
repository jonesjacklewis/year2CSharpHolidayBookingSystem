# OOPA2

The code for the project is in the `02 Code` directory.

The default admin credentials are `DefaultAdmin` with password `DefaultAdminPass123`

## Features

- Logging In
    - Toggle Password Visibility
- Main Menu
    - Custom Welcome Message
- Create Another User (For Admins)
    - Username Validation
    - Password Validation
    - SHA256 Hashing
    - Password Confirmation
    - Toggle Password Visibility
- Create Booking
    - Optional Telephone Number
        - Telephone Number Validation
    - Email Validation
    - Optional Credit Card Number Validation
        - Luhn Algorithm
    - Dynamically Populating Combo Boxes based on User Input
    - Dynamic Content for Pricing
- View Tours
    - List of Tours
    - Show total generated revenue
         - Includes cancellations at 100% and refunds at 50%
    - Shows number of rooms remaining
    - Allows easy navigation to the View Bookings page, filtered by the selected tour
- View Bookings
    - List of Bookings
    - Shows N/a for null telephone numbers
    - Searchable by all fields
    - Able to clear search
    - Able to cancel bookings
        - If booking within 10 days, refund at 50%
        - If booking over 10 days, no refund
    - Able ot Amend Bookings
