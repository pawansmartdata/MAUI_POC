#MAUI CRUD Application
 
##  Features
 
###  Authentication
- User Sign Up
- User Login
- Password visibility toggle with eye icon
- Logout clears stored credentials
 
###  Profile
- View & Edit profile
- Update name, email, phone number, and profile image
- Image picker with preview before upload
 
###  Items
- View all items in a two-column grid
- Add new item (name, description, image, location)
- Item detail view
- Edit and delete items
 
###  Search
- Dynamic filtering of item list as you type
 
###  Google Maps Integration
- Location displayed for items
 
###  UI/UX
- Validation and input error indicators

### 🤖 AI Assistant Integration (Test Purpose)
This application includes a basic AI assistant integrated for testing purposes in .NET MAUI. It provides simple, text-based guidance to help users during sign-up, profile editing, item management, and map usage. The responses are optimized for mobile display and avoid any special formatting.
Note: This AI is not fully accurate or production-ready — it is added to test how AI can be integrated into a .NET MAUI application.
 
---
 
##  How to Run
 
###  Prerequisites
- .NET 8 SDK
- Visual Studio 2022 or later with MAUI workload
- Android Emulator / Physical Device
- Ngrok (for backend tunneling)
 
###  Setup Steps
 
1. **Clone the repo**:
    git clone https://github.com/pawansmartdata/MAUI_POC

 
2. **Configure Backend API Base URL**:
    In your MAUIprogram.cs file set the base address of your Web API.
 
3. **Run the app**:
    - Select target platform (Android/iOS/Windows)
    - Hit `Run` in Visual Studio
 
---
 
##  API Integration
 
All API requests are handled via injected services:
- `IAuthService` → Login/Register
- `IUserService` → Update profile
- `IItemService` → CRUD operations
 
**Token-based authentication** and `HttpClient` are used to securely communicate with the backend.
 
---
 
##  State Management
 
- MVVM pattern with `INotifyPropertyChanged`
- Uses `Preferences` for storing user data locally
- `ObservableCollection<T>` to keep UI in sync
 
---
 
##  Security Notes
 
- Passwords are validated on both frontend and backend.
- Profile pictures are uploaded securely with `IFormFile` in multipart requests.
 
---
 
##  Screenshots
![Snapshot](Snapshots/Image1.jpeg)
![Snapshot](Snapshots/Image2.jpeg)
![Snapshot](Snapshots/Image3.jpeg)
![Snapshot](Snapshots/Image4.jpeg)
![Snapshot](Snapshots/Image5.jpeg)
![Snapshot](Snapshots/Image6.jpeg)
![Snapshot](Snapshots/Image7.jpeg)
![Snapshot](Snapshots/Image8.jpeg)

Backend
The backend is built with ASP.NET Core Web API and supports the following:

Features
JWT-based authentication and authorization
User management (register, login, profile update)
Item management (CRUD operations with image uploads and geolocation)
Secure endpoints (only authenticated users can perform protected actions)
Token sent in Authorization: Bearer {token} header

Technologies Used
ASP.NET Core 8 Web API
Entity Framework Core (Code First with SQL Server)
JWT Bearer Authentication
Swagger for API documentation
IFormFile support for image uploads

Running the Backend
1. Install dependencies
Make sure .NET 8 SDK and SQL Server are installed.
2. Run migrations
dotnet ef database update
3. Run the API
dotnet run
4. Expose API via Ngrok
ngrok http https://localhost:5001
ngrok http 5194(your backend port) --domain=wholly-rested-kid.ngrok-free.app(url that get from the swagger)
command ngrok http 5194 --domain=wholly-rested-kid.ngrok-free.app
Update the ngrok URL in your MAUI frontend’s MauiProgram.cs.