# VetClinic ASP.NET Project

This README provides instructions for setting up and running the VetClinic ASP.NET project.

## Prerequisites

- Docker
- .NET SDK
- Git

## Installation

1. Clone the repository:
   ```
   git clone [repository-url]
   cd VetClinic
   ```

2. Build the Docker Compose file:
   ```
   docker-compose build
   ```

3. Start the PostgreSQL container:
   ```
   docker-compose up postgres
   ```

4. Update the databases for the following services:
   
   Run the following commands from the root directory of the VetClinic project:

   ```
   dotnet ef database update --project IdentityServer/IdentityServer.csproj
   dotnet ef database update --project VetAid/VetAid.csproj
   dotnet ef database update --project DoctorProfile/DoctorProfile.csproj
   dotnet ef database update --project Appointment/Appointment.csproj
   ```

   These commands will update the databases for IdentityServer, VetAid, DoctorProfile, and Appointment services respectively.

5. Start the entire application:
   ```
   docker-compose up
   ```

## Project Structure

The project consists of the following main directories:

## IdentityServer Module

Handles user management. Key features include:


[AccountController](./IdentityServer/Controllers/AccountController.cs):

- **Register method**:
  - **POST**: /api/Account/Register
  - Accepts [`RegisterModel`](./IdentityServer/Model/RegisterModel.cs) with `Email`, `Password`
  - Registers a new user and returns a JWT token
  - Returns:
    - `200 OK`: Registration successful, with a token
    - `400 Bad Request`: Invalid model state or registration failed

- **Login method**:
  - **POST**: /api/Account/Login
  - Accepts [`LoginModel`](./IdentityServer/Model/LoginModel.cs) with `EmailOrUserName`, `Password`, and `RememberMe`
  - Returns a JWT token valid for 31 days
  - Uses HmacSha256 algorithm
  - Includes claims for user ID, username, email, and roles
  - Returns:
    - `200 OK`: Login successful, with a token
    - `400 Bad Request`: Invalid model state or login failed

- **Logout method**:
  - **POST**: /api/Account/Logout
  - Logs out the current user
  - Returns:
    - `200 OK`: Logout successful

#### Additional Information

This project uses `Entity Framework (EF)` and `ASP.NET Identity` with `IdentityUser` and `IdentityRole`. The `ApplicationDbContext` is based on `IdentityDbContext`.

#### DbInitializer

The [`DbInitializer`](./IdentityServer/Data/DbInitializer.cs) is used to set up initial roles and users in the system. It ensures that the database is created, and then creates roles and users.

#### Roles

The following roles are available in the system:
- `user` - pet owner
- `doctor` - doctor at the clinic
- `admin` - the owner of the system, has unlimited powers

#### Users

The following users are created in the system:

- **User1**:
  - **Id**: `f82ed7b4-6bd8-4a88-b5ff-62166e6480b8`
  - **UserName**: `user1`
  - **Email**: `user1@example.com`
  - **Role**: `user`

- **Doctor1**:
  - **Id**: `18403cdd-cb04-411e-afe5-fe8aa5ed30a3`
  - **UserName**: `doctor1`
  - **Email**: `doctor1@example.com`
  - **Role**: `doctor`

- **Admin1**:
  - **Id**: `8052ba8a-2b27-4d3e-83f5-c1db9f35f055`
  - **UserName**: `admin1`
  - **Email**: `admin1@example.com`
  - **Role**: `admin`

## Common Module

### ServiceResult<T>

[`ServiceResult<T>`](./Common/Results/ServiceResult.cs) is a wrapper around [`CSharpFunctionalExtensions.Result<T>`](https://github.com/vkhorikov/CSharpFunctionalExtensions/blob/master/CSharpFunctionalExtensions/Result/ResultT.cs). It provides a standardized way to handle service results, including success and failure cases, along with associated errors.

Properties
- **IsSuccess**: Indicates if the operation was successful.
- **IsFailure**: Indicates if the operation failed.
- **Value**: The value of the result, available only if the operation was successful.
- **Error**: The associated error, available only if the operation failed.

#### [ServiceError](./Common/Results/ServiceError.cs)

Represents an error in a service operation.

#### [ServiceErrorType](./Common/Results/ServiceError.cs)

Enumerates possible error types.

#### Methods

- **Success(T value)**: Creates a successful `ServiceResult` with the specified value.
- **Failure(ServiceError error)**: Creates a failed `ServiceResult` with the specified error.
- **Failure(string errorMessage, ServiceErrorType errorType)**: Creates a failed `ServiceResult` with the specified error message and type.
- **Map<TK>(Func<T, TK> mapper)**: Transforms the result into another type using the provided mapper function.
- **Bind<TK>(Func<T, ServiceResult<TK>> f)**: Chains another `ServiceResult`-producing function.
- **Match<TK>(Func<T, TK> onSuccess, Func<ServiceError, TK> onFailure)**: Matches the success or failure case and returns the appropriate value.

### [Extensions](./Common/Results/ServiceResultExtensions.cs)

- **ToServiceResult<T>(this Result<T> result, ServiceErrorType errorType = ServiceErrorType.InternalError)**: Converts a `Result<T>` to a `ServiceResult<T>`, with an optional error type.
- **ToServiceResult<T>(this Maybe<T> maybe, string errorMessage = "Value not found", ServiceErrorType errorType = ServiceErrorType.NotFound)**: Converts a `Maybe<T>` to a `ServiceResult<T>`, with an optional error message and type.
- **ToServiceErrorType(this System.Net.HttpStatusCode statusCode)**: Converts an HTTP status code to a `ServiceErrorType`.
- **ToActionResult<T>(this ServiceResult<T> serviceResult)**: Converts a `ServiceResult<T>` to an `ActionResult<T>`.

### [Helper](./Common/Helpers/RepositoryHelper.cs)

Provides a method to safely execute service operations.

- **ExecuteSafeAsync<T>(Func<Task<ServiceResult<T>>> repositoryFunction)**: Executes the given asynchronous function and returns a `ServiceResult<T>`. If an exception occurs, it returns a failed `ServiceResult<T>` with an internal error.

### Authorization

[`BaseEntityAuthorizationHandler<TRequirement, TResource, TEntity, TService>`](./Common/Results/ServiceResultExtensions.cs) is used for more convenient authorization. It provides role-based authorization checks and entity ownership validation.

#### Properties and Methods
- **_service**: The service used for authorization.
- **HandleRequirementAsync**: Checks user roles (`admin`, `doctor`, `user`) and validates entity ownership.
- **HandleDoctorRoleAsync**: Handles authorization logic for users with the `doctor` role.
- **HandleUserRoleAsync**: Handles authorization logic for users with the `user` role.
- **GetEntityByIdAsync**: Retrieves an entity by its ID.
- **ValidateEntityOwnership**: Validates if the user owns the entity.

## VetAid Module
### Entities
[VetAidEntity](./VetAid/Data/Entities/VetAid.cs)
The VetAidEntity represents a veterinary aid service and includes properties such as:

- Id: Nullable integer representing the unique identifier.
- Name: Nullable string for the name of the vet aid.
- Description: Nullable string for the description of the service.
- ServiceType: Nullable string for the type of service.
- AnimalTypes: Collection of associated animal types.
- Duration: Nullable TimeSpan, defaults to 60 minutes.
- Price: Nullable decimal for the price of the service.

[AnimalTypeEntity](./VetAid/Data/Entities/AnimalType.cs)
The AnimalTypeEntity represents different types of animals and includes properties such as:

- Id: Nullable integer representing the unique identifier.
- Name: Nullable string for the name of the animal type.
- VetAids: Collection of associated vet aids.

### Configuration
Entities have configurations defined in the following files:

- [VetAidConfiguration](./VetAid/Data/EntityConfigurations/VetAidConfiguration.cs)
- [AnimalTypeConfiguration](./VetAid/Data/EntityConfigurations/AnimalTypeConfiguration.cs)

These configurations use Entity Framework to set business logic and set up the many-to-many relationships between VetAidEntity and AnimalTypeEntity.

### DTOs
Entities are mapped to DTOs using AutoMapper:

- [VetAidDto](./VetAid/DTOs/VetAidDto.cs)
- [AnimalTypeDto](./VetAid/DTOs/AnimalTypeDto.cs)

### Repositories

Repositories provide standard CRUD operations and use `ServiceResult` for handling exceptional situations.

[IVetAidRepository](./VetAid/Repositories/Interfaces/IVetAidRepository.cs)
- GetAllAsync(): Retrieves all VetAid entities.
- GetByIdAsync(int id): Retrieves a VetAid entity by its ID.
- AddAsync(VetAid entity): Adds a new VetAid entity.
- UpdateAsync(int id, VetAidEntity entity): Updates an existing VetAid entity by its ID.
- DeleteAsync(int id): Deletes a VetAid entity by its ID.

[IAnimalTypeRepository](./VetAid/Repositories/Interfaces/IAnimalTypeRepository.cs)
- GetAllAsync(): Retrieves all AnimalType entities.
- GetByIdAsync(int id): Retrieves an AnimalType entity by its ID.
- AddAsync(AnimalType entity): Adds a new AnimalType entity.
- UpdateAsync(int id, AnimalType entity): Updates an existing AnimalType entity by its ID.
- DeleteAsync(int id): Deletes an AnimalType entity by its ID.

Implementation for these interfaces
- [VetAidRepository](./VetAid/Repositories/VetAidRepository.cs)
- [AnimalTypeRepository](./VetAid/Repositories/AnimalTypeRepository.cs)

### Services
Services provide CRUD operations using DTOs and also use ServiceResult for handling exceptional situations.

[IVetAidService](./VetAid/Services/Interfaces/IVetAidService.cs)
- GetAllAsync(): Retrieves all VetAid DTOs.
- GetByIdAsync(int id): Retrieves a VetAid DTO by its ID.
- AddAsync(VetAidDto dto): Adds a new VetAid DTO.
- UpdateAsync(int id, VetAidDto dto): Updates an existing VetAid DTO by its ID.
- DeleteAsync(int id): Deletes a VetAid DTO by its ID.
- 
[IAnimalTypeService](./VetAid/Services/Interfaces/IAnimalTypeService.cs)
- GetAllAsync(): Retrieves all AnimalType DTOs.
- GetByIdAsync(int id): Retrieves an AnimalType DTO by its ID.
- AddAsync(AnimalTypeDto dto): Adds a new AnimalType DTO.
- UpdateAsync(int id, AnimalTypeDto dto): Updates an existing AnimalType DTO by its ID.
- DeleteAsync(int id): Deletes an AnimalType DTO by its ID.

Implementation for these interfaces
- [VetAidService](./VetAid/Services/VetAidService.cs)
- [AnimalTypeService](./VetAid/Services/AnimalTypeService.cs)

### Controllers
[AnimalTypeController](./VetAid/Controllers/AnimalTypeController.cs):

- **GetAllAsync method**:
  - **GET**: /api/AnimalType/All
  - Returns a list of all animal types.
  - Returns:
    - `200 OK`: List of `AnimalTypeDto` items
    - `500 Internal Server Error`: Error details if the operation fails

- **GetByIdAsync method**:
  - **GET**: /api/AnimalType/ById
  - **Query Parameter**: `id` (integer)
  - Returns a specific animal type by its ID.
  - Returns:
    - `200 OK`: `AnimalTypeDto` item
    - `404 Not Found`: Animal type with the specified ID not found
    - `500 Internal Server Error`: Error details if the operation fails

- **AddAsync method**:
  - **POST**: /api/AnimalType
  - Accepts [`AnimalTypeDto`](./Models/AnimalTypeDto.cs) with animal type details
  - Adds a new animal type.
  - **Requires**: `admin` role
  - Returns:
    - `200 OK`: Added `AnimalTypeDto` item
    - `400 Bad Request`: Invalid model state or addition failed
    - `401 Unauthorized`: Unauthorized access
    - `500 Internal Server Error`: Error details if the operation fails

- **UpdateAsync method**:
  - **PUT**: /api/AnimalType
  - **Query Parameter**: `id` (integer)
  - Accepts [`AnimalTypeDto`](./Models/AnimalTypeDto.cs) with updated animal type details
  - Updates an existing animal type.
  - **Requires**: `admin` role
  - Returns:
    - `200 OK`: Updated `AnimalTypeDto` item
    - `400 Bad Request`: Invalid model state or update failed
    - `401 Unauthorized`: Unauthorized access
    - `404 Not Found`: Animal type with the specified ID not found
    - `500 Internal Server Error`: Error details if the operation fails

- **DeleteAsync method**:
  - **DELETE**: /api/AnimalType
  - **Query Parameter**: `id` (integer)
  - Deletes an animal type by its ID.
  - **Requires**: `admin` role
  - Returns:
    - `204 No Content`: Deletion successful
    - `400 Bad Request`: Invalid model state or deletion failed
    - `401 Unauthorized`: Unauthorized access
    - `404 Not Found`: Animal type with the specified ID not found
    - `500 Internal Server Error`: Error details if the operation fails

[VetAidController](./VetAid/Controllers/VetAidController.cs):

- **GetAll method**:
  - **GET**: /api/VetAid
  - Returns a list of all vet aid items.
  - Returns:
    - `200 OK`: List of `VetAidDto` items
    - `500 Internal Server Error`: Error details if the operation fails

- **GetById method**:
  - **GET**: /api/VetAid/{id}
  - **Path Parameter**: `id` (integer)
  - Returns a specific vet aid item by its ID.
  - Returns:
    - `200 OK`: `VetAidDto` item
    - `404 Not Found`: Vet aid item with the specified ID not found
    - `500 Internal Server Error`: Error details if the operation fails

- **Add method**:
  - **POST**: /api/VetAid
  - Accepts [`VetAidDto`](./Models/VetAidDto.cs) with vet aid details
  - Adds a new vet aid item.
  - **Requires**: `admin` role
  - Returns:
    - `200 OK`: Added `VetAidDto` item
    - `400 Bad Request`: Invalid model state or addition failed
    - `401 Unauthorized`: Unauthorized access
    - `500 Internal Server Error`: Error details if the operation fails

- **Update method**:
  - **PUT**: /api/VetAid
  - **Query Parameter**: `id` (integer)
  - Accepts [`VetAidDto`](./Models/VetAidDto.cs) with updated vet aid details
  - Updates an existing vet aid item.
  - **Requires**: `admin` role
  - Returns:
    - `200 OK`: Updated `VetAidDto` item
    - `400 Bad Request`: Invalid model state or update failed
    - `401 Unauthorized`: Unauthorized access
    - `404 Not Found`: Vet aid item with the specified ID not found
    - `500 Internal Server Error`: Error details if the operation fails

- **Delete method**:
  - **DELETE**: /api/VetAid
  - **Query Parameter**: `id` (integer)
  - Deletes a vet aid item by its ID.
  - **Requires**: `admin` role
  - Returns:
    - `200 OK`: Deleted `VetAidDto` item
    - `400 Bad Request`: Invalid model state or deletion failed
    - `401 Unauthorized`: Unauthorized access
    - `404 Not Found`: Vet aid item with the specified ID not found
    - `500 Internal Server Error`: Error details if the operation fails

## DoctorProfile Module

### Entities

#### [DoctorInfo](./DoctorProfile/Data/Entities/DoctorInfo.cs)
The `DoctorInfo` entity represents a doctor's profile and includes properties such as:

- **Id**: Nullable integer representing the unique identifier.
- **UserId**: Nullable string for the user's ID.
- **FullName**: Nullable string for the doctor's full name.
- **Email**: Nullable string for the doctor's email.
- **Specialization**: Nullable string for the doctor's specialization.
- **Description**: Nullable string for the doctor's description.
- **DoctorTimetables**: Collection of associated `DoctorTimetable` items.

#### [DoctorTimetable](./DoctorProfile/Data/Entities/DoctorTimetable.cs)
The `DoctorTimetable` entity represents a doctor's availability schedule and includes properties such as:

- **Id**: Integer representing the unique identifier.
- **StartTime**: Nullable DateTimeOffset representing the start time of the schedule.
- **EndTime**: Nullable DateTimeOffset representing the end time of the schedule.
- **UserId**: Nullable string for the user's ID.
- **DoctorInfo**: Navigation property for the associated `DoctorInfo`.

#### [DoctorTimetableSegment](./DoctorProfile/Models/DoctorTimetableSegment.cs)
The `DoctorTimetableSegment` model represents a segment of the doctor's timetable and includes properties such as:

- **UserId**: String representing the user's ID.
- **StartTime**: DateTimeOffset representing the start time of the segment.
- **EndTime**: DateTimeOffset representing the end time of the segment.

### Configuration
Entities have configurations defined in the following files:

- [DoctorInfoConfiguration](./DoctorProfile/Data/EntityConfigurations/DoctorInfoConfiguration.cs)
- [DoctorTimetableConfiguration](./DoctorProfile/Data/EntityConfigurations/DoctorTimetableConfiguration.cs)

These configurations use Entity Framework to set business logic and set up relationships between `DoctorInfo` and `DoctorTimetable`.

### DTOs
Entities are mapped to DTOs using AutoMapper:

- [DoctorInfoDto](./DoctorProfile/DTOs/DoctorInfoDto.cs)
- [DoctorTimetableDto](./DoctorProfile/DTOs/DoctorTimetableDto.cs)
- [DoctorTimetableSegmentDto](./DoctorProfile/DTOs/DoctorTimetableSegmentDto.cs)

### Repositories

Repositories provide standard CRUD operations and use `ServiceResult` for handling exceptional situations.

#### [IDoctorInfoRepository](./DoctorProfile/Repositories/Interfaces/IDoctorInfoRepository.cs)
- **GetAllAsync()**: Retrieves all `DoctorInfo` entities.
- **GetByUserIdAsync(string userId)**: Retrieves a `DoctorInfo` entity by the user's ID.
- **GetByIdAsync(int id)**: Retrieves a `DoctorInfo` entity by its ID.
- **AddAsync(DoctorInfo profile)**: Adds a new `DoctorInfo` entity.
- **UpdateAsync(int id, DoctorInfo profile)**: Updates an existing `DoctorInfo` entity by its ID.
- **DeleteAsync(int id)**: Deletes a `DoctorInfo` entity by its ID.

#### [IDoctorTimetableRepository](./DoctorProfile/Repositories/Interfaces/IDoctorTimetableRepository.cs)
- **GetAllAsync()**: Retrieves all `DoctorTimetable` entities.
- **GetByIdAsync(int id)**: Retrieves a `DoctorTimetable` entity by its ID.
- **GetByUserIdAsync(string profileId)**: Retrieves `DoctorTimetable` entities by the user's ID.
- **GetSegmentsByDatesAsync(string userId, DateTimeOffset startDate, DateTimeOffset endDate)**: Retrieves `DoctorTimetableSegment` items within specified dates.
- **AddAsync(DoctorTimetable availability)**: Adds a new `DoctorTimetable` entity.
- **UpdateAsync(int id, DoctorTimetable availability)**: Updates an existing `DoctorTimetable` entity by its ID.
- **DeleteAsync(int id)**: Deletes a `DoctorTimetable` entity by its ID.

Implementation for these interfaces:

- [DoctorInfoRepository](./DoctorProfile/Repositories/DoctorInfoRepository.cs)
- [DoctorTimetableRepository](./DoctorProfile/Repositories/DoctorTimetableRepository.cs)

### Services

Services provide CRUD operations using DTOs and also use `ServiceResult` for handling exceptional situations.

#### [IDoctorInfoService](./DoctorProfile/Services/Interfaces/IDoctorInfoService.cs)
- **GetAllAsync()**: Retrieves all `DoctorInfoDto` items.
- **GetByUserIdAsync(string userId)**: Retrieves a `DoctorInfoDto` item by the user's ID.
- **GetByIdAsync(int id)**: Retrieves a `DoctorInfoDto` item by its ID.
- **AddAsync(DoctorInfoDto profile)**: Adds a new `DoctorInfoDto` item.
- **UpdateAsync(int id, DoctorInfoDto profile)**: Updates an existing `DoctorInfoDto` item by its ID.
- **DeleteAsync(int id)**: Deletes a `DoctorInfoDto` item by its ID.

#### [IDoctorTimetableService](./DoctorProfile/Services/Interfaces/IDoctorTimetableService.cs)
- **GetAllAsync()**: Retrieves all `DoctorTimetableDto` items.
- **GetByIdAsync(int id)**: Retrieves a `DoctorTimetableDto` item by its ID.
- **GetByUserIdAsync(string userId)**: Retrieves `DoctorTimetableDto` items by the user's ID.
- **GetSegmentsByDatesAsync(string userId, DateTimeOffset startDate, DateTimeOffset endDate)**: Retrieves `DoctorTimetableSegmentDto` items within specified dates.
- **AddAsync(DoctorTimetableDto availabilityDto)**: Adds a new `DoctorTimetableDto` item.
- **UpdateAsync(int id, DoctorTimetableDto availabilityDto)**: Updates an existing `DoctorTimetableDto` item by its ID.
- **DeleteAsync(int id)**: Deletes a `DoctorTimetableDto` item by its ID.

Implementation for these interfaces:

- [DoctorInfoService](./DoctorProfile/Services/DoctorInfoService.cs)
- [DoctorTimetableService](./DoctorProfile/Services/DoctorTimetableService.cs)

### Controllers

#### [DoctorInfoController](./DoctorProfile/Controllers/DoctorInfoController.cs):

- **GetAll method**:
  - **GET**: /api/DoctorInfo/All
  - Returns a list of all doctor profiles.
  - Returns:
    - `200 OK`: List of `DoctorInfoDto` items
    - `500 Internal Server Error`: Error details if the operation fails

- **GetById method**:
  - **GET**: /api/DoctorInfo/{id}
  - **Path Parameter**: `id` (integer)
  - Returns a specific doctor profile by its ID.
  - Returns:
    - `200 OK`: `DoctorInfoDto` item
    - `404 Not Found`: Doctor profile with the specified ID not found
    - `500 Internal Server Error`: Error details if the operation fails

- **GetByUserId method**:
  - **PATCH**: /api/DoctorInfo
  - **Query Parameter**: `userId` (string)
  - Returns a specific doctor profile by the user's ID.
  - Returns:
    - `200 OK`: `DoctorInfoDto` item
    - `404 Not Found`: Doctor profile with the specified ID not found
    - `500 Internal Server Error`: Error details if the operation fails

- **Add method**:
  - **POST**: /api/DoctorInfo
  - Accepts [`DoctorInfoDto`](./Models/DoctorInfoDto.cs) with `doctor` profile details.
  - Adds a new doctor profile. Accessible to `admin` or the `doctor` themselves.
  - **Requires**: `admin` role or the doctor to whom the profile belongs.
  - Returns:
    - `201 Created`: Added `DoctorInfoDto` item
    - `400 Bad Request`: Invalid model state or addition failed
    - `401 Unauthorized`: Unauthorized access
    - `500 Internal Server Error`: Error details if the operation fails

- **Update method**:
  - **PATCH**: /api/DoctorInfo
  - **Query Parameter**: `id` (integer)
  - Accepts [`DoctorInfoDto`](./Models/DoctorInfoDto.cs) with updated `doctor` profile details.
  - Updates an existing doctor profile. Accessible to `admin` or the `doctor` themselves.
  - **Requires**: `admin` role or the doctor to whom the profile belongs.
  - Returns:
    - `200 OK`: Updated `DoctorInfoDto` item
    - `400 Bad Request`: Invalid model state or update failed
    - `401 Unauthorized`: Unauthorized access
    - `404 Not Found`: Doctor profile with the specified ID not found
    - `500 Internal Server Error`: Error details if the operation fails

- **Delete method**:
  - **DELETE**: /api/DoctorInfo/{id}
  - **Path Parameter**: `id` (integer)
  - Deletes a doctor profile by its ID. Accessible to `admin` or the `doctor` themselves.
  - **Requires**: `admin` role or the `doctor` to whom the profile belongs.
  - Returns:
    - `204 No Content`: Deletion successful
    - `401 Unauthorized`: Unauthorized access
    - `404 Not Found`: Doctor profile with the specified ID not found
    - `500 Internal Server Error`: Error details if the operation fails
 
#### [DoctorTimetableController](./DoctorProfile/Controllers/DoctorTimetableController.cs):

- **GetAll method**:
  - **GET**: /api/DoctorTimetable/All
  - Returns a list of all doctor timetables.
  - Returns:
    - `200 OK`: List of `DoctorTimetableDto` items
    - `500 Internal Server Error`: Error details if the operation fails

- **GetById method**:
  - **GET**: /api/DoctorTimetable/{id}
  - **Path Parameter**: `id` (integer)
  - Returns a specific doctor timetable by its ID.
  - Returns:
    - `200 OK`: `DoctorTimetableDto` item
    - `404 Not Found`: Doctor timetable with the specified ID not found
    - `500 Internal Server Error`: Error details if the operation fails

- **GetByUserId method**:
  - **GET**: /api/DoctorTimetable/ByUserId
  - **Query Parameter**: `profileId` (string)
  - Returns a list of doctor timetables by the user's profile ID.
  - Returns:
    - `200 OK`: List of `DoctorTimetableDto` items
    - `404 Not Found`: Doctor timetables for the specified profile ID not found
    - `500 Internal Server Error`: Error details if the operation fails

- **GetSegmentsByDates method**:
  - **GET**: /api/DoctorTimetable/SegmentsByDates
  - **Query Parameters**: `userId` (string), `startDate` (DateTimeOffset), `endDate` (DateTimeOffset)
  - Returns a list of timetable segments by user ID and date range.
  - Returns:
    - `200 OK`: List of `DoctorTimetableSegmentDto` items
    - `404 Not Found`: No segments found for the specified criteria
    - `500 Internal Server Error`: Error details if the operation fails

- **Add method**:
  - **POST**: /api/DoctorTimetable
  - Accepts [`DoctorTimetableDto`](./Models/DoctorTimetableDto.cs) with timetable details.
  - Adds a new doctor timetable. Accessible to `admin` or the `doctor` themselves.
  - **Requires**: `admin` role or the `doctor` to whom the timetable belongs.
  - Returns:
    - `200 OK`: Added `DoctorTimetableDto` item
    - `400 Bad Request`: Invalid model state or addition failed
    - `401 Unauthorized`: Unauthorized access
    - `500 Internal Server Error`: Error details if the operation fails

- **Update method**:
  - **PATCH**: /api/DoctorTimetable/{id}
  - **Path Parameter**: `id` (integer)
  - Accepts [`DoctorTimetableDto`](./Models/DoctorTimetableDto.cs) with updated timetable details.
  - Updates an existing doctor timetable. Accessible to `admin` or the `doctor` themselves.
  - **Requires**: `admin` role or the `doctor` to whom the timetable belongs.
  - Returns:
    - `200 OK`: Updated `DoctorTimetableDto` item
    - `400 Bad Request`: Invalid model state or update failed
    - `401 Unauthorized`: Unauthorized access
    - `404 Not Found`: Doctor timetable with the specified ID not found
    - `500 Internal Server Error`: Error details if the operation fails

- **Delete method**:
  - **DELETE**: /api/DoctorTimetable/{id}
  - **Path Parameter**: `id` (integer)
  - Deletes a doctor timetable by its ID. Accessible to `admin` or the `doctor` themselves.
  - **Requires**: `admin` role or the `doctor` to whom the timetable belongs.
  - Returns:
    - `200 OK`: Deletion successful
    - `400 Bad Request`: Invalid model state or update failed
    - `401 Unauthorized`: Unauthorized access
    - `404 Not Found`: Doctor timetable with the specified ID not found
    - `500 Internal Server Error`: Error details if the operation fails

## Appointment Module

### Entities

#### [Booking](./Appointment/Data/Entities/Booking.cs)
The `Booking` entity represents a booking and includes properties such as:

- **Id**: Nullable integer representing the unique identifier.
- **StartDate**: Nullable DateTimeOffset representing the start date and time of the booking.
- **EndDate**: Nullable DateTimeOffset representing the end date and time of the booking.
- **ClientEmail**: Nullable string for the client's email.
- **DoctorId**: Nullable string for the doctor's ID.
- **VetAidId**: Nullable integer for the ID of the veterinary aid.
- **PetType**: Nullable integer for the type of the pet.
- **PetBreed**: Nullable string for the breed of the pet.
- **Status**: Nullable `AppointmentStatus` representing the status of the appointment.
- **BookingNode**: Nullable string for the booking node identifier.

### DTOs

Entities are mapped to DTOs using AutoMapper:

#### [BookingDto](./Appointment/DTOs/BookingDto.cs)
The `BookingDto` is a data transfer object for the `Booking` entity, containing necessary properties for API interaction.

#### [DoctorTimetableSegmentDto](./Appointment/DTOs/DoctorTimetableSegmentDto.cs)
The `DoctorTimetableSegmentDto` represents a segment of the doctor's timetable and includes properties such as:

- **UserId**: String representing the user's ID.
- **StartTime**: DateTimeOffset representing the start time of the segment.
- **EndTime**: DateTimeOffset representing the end time of the segment.

#### [VetAidDto](./Appointment/DTOs/VetAidDto.cs)
The `VetAidDto` represents veterinary aid details and includes properties such as:

- **Id**: Nullable integer for the ID of the vet aid.
- **Name**: Nullable string for the name of the vet aid.
- **Description**: Nullable string for the description of the vet aid.
- **ServiceType**: Nullable string for the type of service.
- **Duration**: Nullable TimeSpan for the duration of the service.
- **Price**: Nullable decimal for the price of the service.

### Repositories

Repositories provide standard CRUD operations and use `ServiceResult` for handling exceptional situations.

#### [IBookingRepository](./Appointment/Repositories/Interfaces/IBookingRepository.cs)
- **GetByIdAsync(int id)**: Retrieves a `Booking` entity by its ID.
- **GetAllAsync()**: Retrieves all `Booking` entities.
- **AddAsync(Booking booking)**: Adds a new `Booking` entity.
- **UpdateAsync(int id, Booking booking)**: Updates an existing `Booking` entity by its ID.
- **DeleteAsync(int id)**: Deletes a `Booking` entity by its ID.
- **GetFilteredBookingsAsync(string? clientEmail, string? doctorId, int? vetAidId, DateTimeOffset? startDate, DateTimeOffset? endDate)**: Retrieves bookings based on filter criteria.
- **IsFreeToBooking(string doctorId, DateTimeOffset startDate, DateTimeOffset endDate)**: Checks if the doctor is available for booking within the specified time.

Implementation for this interface:

- [BookingRepository](./Booking/Repositories/BookingRepository.cs)

### Services

Services provide CRUD operations using DTOs and also use `ServiceResult` for handling exceptional situations.

#### [IBookingService](./Appointment/Services/Interfaces/IBookingService.cs)
- **GetByIdAsync(int id)**: Retrieves a `BookingDto` item by its ID.
- **GetAllAsync()**: Retrieves all `BookingDto` items.
- **AddAsync(BookingDto booking)**: Adds a new `BookingDto` item.
- **UpdateAsync(int id, BookingDto booking)**: Updates an existing `BookingDto` item by its ID.
- **DeleteAsync(int id)**: Deletes a `BookingDto` item by its ID.
- **GetFilteredBookingsAsync(BookingFilter filter)**: Retrieves `BookingDto` items based on filter criteria.
- **GetFreeTimeAsync(string doctorId, DateTimeOffset startDate, DateTimeOffset endDate, int vetAidId)**: Retrieves available times for booking.
- **SetStatusAsync(int id, AppointmentStatus status)**: Sets the status of a booking.

Implementation for this interface:

- [BookingService](./Booking/Services/BookingService.cs)

### Clients

Clients interact with external services to retrieve data needed by the Booking module.

#### [IDoctorClient](./Appointment/Clients/Interfaces/IDoctorClient.cs)
- **GetTimetableByDatesAsync(string userId, DateTimeOffset startDate, DateTimeOffset endDate)**: Retrieves a list of `DoctorTimetableSegmentDto` for the specified doctor and date range.

#### [IVetAidClient](./Appointment/Clients/Interfaces/IVetAidClient.cs)
- **GetVetAidAsync(int id)**: Retrieves a `VetAidDto` by its ID.

Implementations for these interfaces:

- [DoctorClient](./Booking/Clients/DoctorClient.cs)
- [VetAidClient](./Booking/Clients/VetAidClient.cs)

### Controllers

#### [BookingController](./Booking/Controllers/BookingController.cs)

- **GetById method**:
  - **GET**: /api/Booking/{id}
  - **Path Parameter**: `id` (integer)
  - Retrieves a booking by its ID.
  - **Requires**: No authentication required.
  - Returns:
    - `200 OK`: Retrieved `BookingDto` item
    - `404 Not Found`: Booking with the specified ID not found
    - `500 Internal Server Error`: Error details if the operation fails

- **GetAll method**:
  - **GET**: /api/Booking
  - Retrieves all bookings.
  - **Requires**: No authentication required.
  - Returns:
    - `200 OK`: List of `BookingDto` items
    - `500 Internal Server Error`: Error details if the operation fails

- **Add method**:
  - **POST**: /api/Booking
  - Accepts [`BookingDto`](./Booking/DTOs/BookingDto.cs) with booking details.
  - Adds a new booking.
  - **Requires**: `admin` role or the `doctor` who owns the booking.
  - Returns:
    - `201 Created`: Successfully created `BookingDto` item
    - `400 Bad Request`: Invalid model state or creation failed
    - `401 Unauthorized`: Unauthorized access
    - `500 Internal Server Error`: Error details if the operation fails

- **Update method**:
  - **PUT**: /api/Booking/{id}
  - **Path Parameter**: `id` (integer)
  - Accepts [`BookingDto`](./Booking/DTOs/BookingDto.cs) with updated booking details.
  - Updates an existing booking.
  - **Requires**: `admin` role or the `doctor` who owns the booking.
  - Returns:
    - `200 OK`: Updated `BookingDto` item
    - `400 Bad Request`: Invalid model state or update failed
    - `401 Unauthorized`: Unauthorized access
    - `404 Not Found`: Booking with the specified ID not found
    - `500 Internal Server Error`: Error details if the operation fails

- **Delete method**:
  - **DELETE**: /api/Booking/{id}
  - **Path Parameter**: `id` (integer)
  - Deletes a booking by its ID.
  - **Requires**: `admin` role or the `doctor` who owns the booking.
  - Returns:
    - `204 No Content`: Successfully deleted the booking
    - `401 Unauthorized`: Unauthorized access
    - `404 Not Found`: Booking with the specified ID not found
    - `500 Internal Server Error`: Error details if the operation fails

- **GetFilteredBookings method**:
  - **GET**: /api/Booking/filtered
  - **Query Parameters**: `clientEmail` (string), `doctorId` (string), `vetAidId` (integer), `startDate` (DateTimeOffset), `endDate` (DateTimeOffset)
  - Retrieves bookings based on filter criteria.
  - **Requires**: No authentication required.
  - Returns:
    - `200 OK`: List of filtered `BookingDto` items
    - `400 Bad Request`: Invalid filter criteria
    - `500 Internal Server Error`: Error details if the operation fails

- **GetFreeTime method**:
  - **GET**: /api/Booking/free-time
  - **Query Parameters**: `doctorId` (string), `startDate` (DateTimeOffset), `endDate` (DateTimeOffset), `vetAidId` (integer)
  - Retrieves available time slots for booking.
  - **Requires**: No authentication required.
  - Returns:
    - `200 OK`: List of available time slots (`DateTimeOffset`)
    - `400 Bad Request`: Invalid parameters
    - `500 Internal Server Error`: Error details if the operation fails

- **SetStatus method**:
  - **PATCH**: /api/Booking/{id}/status
  - **Path Parameter**: `id` (integer)
  - **Body**: Accepts `AppointmentStatus` (enumeration)
  - Updates the status of a booking.
  - **Requires**: `admin` role or the `doctor` who owns the booking.
  - Returns:
    - `200 OK`: Status updated successfully
    - `400 Bad Request`: Invalid data
    - `401 Unauthorized`: Unauthorized access
    - `404 Not Found`: Booking with the specified ID not found
    - `500 Internal Server Error`: Error details if the operation fails