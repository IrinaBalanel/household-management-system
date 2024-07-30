# Household Management System
The Household Management System contains 3 features
- Todo Items Management
- Household Finance Tracker
- Household Cleaning Pipeline

## `Todo Items Management`
Below are listed down all the Entities, Data Controllers related to the Todo Items Management

## Entities

### TodoItem
- `TodoItemId` - Primary key
- `TodoItemDescription` - Description of the todo item
- `StatusId` - Foreign key to the TodoItemStatus entity
- `AssignedToOwnerId` - Foreign key to the Owner entity representing the person assigned to the todo item
- `CreatedByOwnerId` - Foreign key to the Owner entity representing the person who created the todo item
- `CategoryId` - Foreign key to the Category entity
- `TransactionId` - Nullable foreign key to the Transaction entity

### TodoItemStatus
- `StatusId` - Primary key
- `StatusName` - Name of the status (e.g., Pending, Done)

## Data Controllers

### TodoItemStatusDataController

#### ListTodoItemStatus
- **URL**: `api/TodoItemStatusData/ListTodoItemStatus`
- **Method**: GET
- **Description**: Retrieves a list of all status names of the todo items.

### TodoItemDataController

#### ListAllTodoItems
- **URL**: `api/TodoItemData/ListAllTodoItems`
- **Method**: GET
- **Description**: Retrieves a list of all todo items. Optionally filters todo items by:
- **Parameters**:
  - `currentMonth` (optional, bool)
  - `status` (string)
  - `categoryName` (string)
  - `assignedToOwner` (string)
  - `createdByOwner` (string)
- **Examples**:
  - `/api/TodoItem/ListTodoItems?status=Pending`
  - `/api/TodoItem/ListTodoItems?categoryName=Rent`
  - `/api/TodoItem/ListTodoItems?assignedToOwner=Irina`
  - `/api/TodoItem/ListTodoItems?createdByOwner=Mom`

#### FindTodoItemById
- **URL**: `api/TodoItemData/FindTodoItemById/{todoItemId}`
- **Method**: GET
- **Description**: Retrieves a TodoItem by its ID.
- **Parameters**: `todoItemId` (int)
- **Example**: `GET api/TodoItemData/FindTodoItemById/1`

#### AddTodoItem
- **URL**: `api/TodoItemData/AddTodoItem`
- **Method**: POST
- **Description**: Adds a new TodoItem.
- **Parameters**: `todoItem` (TodoItem JSON object)

#### UpdateTodoItem
- **URL**: `api/TodoItemData/UpdateTodoItem/{id}`
- **Method**: PUT
- **Description**: Updates a particular TodoItem in the system with PUT data input.
- **Parameters**:
  - `id` (int)
  - `todoItem` (TodoItem JSON object)

#### DeleteTodoItem
- **URL**: `api/TodoItemData/DeleteTodoItem/{id}`
- **Method**: DELETE
- **Description**: Deletes a todo item from the system by its ID.
- **Parameters**: `id` (int)
- **Returns**: If the todo item has associated transaction, returns a `409 Conflict` status with a message indicating the todo item cannot be deleted.


## `Household Finance Tracker`
Below are listed down all the Entities, Data Controllers related to the HouseHold Finance Tracker

## Entities

### Transaction
- `TransactionId` - Primary key
- `Title` - Title of the transaction
- `Amount` - Amount of the transaction
- `TransactionDate` - Date of the transaction
- `CategoryId` - Foreign key to the Category entity

### Category
- `CategoryId` - Primary key
- `CategoryName` - Name of the category (e.g., Salary, Groceries)
- `TransactionTypeId` - Foreign key to the TransactionType entity

### TransactionType
- `TransactionTypeId` - Primary key
- `TransactionTypeName` - Type of the transaction (e.g., Expense, Income)

## Data Controllers

### TransactionDataController

#### ListAllTransactions
- **URL**: `api/TransactionData/ListAllTransactions`
- **Method**: GET
- **Description**: Retrieves a list of all transactions. Optionally filters transactions for the current month.
- **Parameters**:
  - `currentMonth` (optional, bool)
- **Example**: `GET api/TransactionData/ListAllTransactions?currentMonth=true`

#### findTransactionById
- **URL**: `api/TransactionData/findTransactionById/{transactionId}`
- **Method**: GET
- **Description**: Finds a transaction by its ID.
- **Parameters**: `transactionId` (int)
- **Example**: `GET api/TransactionData/findTransactionById/1`

#### findTransactions
- **URL**: `api/TransactionData/findTransactions`
- **Method**: GET
- **Description**: Finds transactions based on optional filters such as category name, transaction type, current month, or last month.
- **Parameters**:
  - `id` (optional, int)
  - `categoryName` (optional, string)
  - `transactionType` (optional, string)
  - `currentMonth` (optional, bool)
  - `lastMonth` (optional, bool)
- **Examples**:
  - `GET api/TransactionData/findTransactions`
  - `GET api/TransactionData/findTransactions?id=1`
  - `GET api/TransactionData/findTransactions?categoryName=Groceries`
  - `GET api/TransactionData/findTransactions?transactionType=Income`
  - `GET api/TransactionData/findTransactions?currentMonth=true`
  - `GET api/TransactionData/findTransactions?lastMonth=true`
  - `GET api/TransactionData/findTransactions?categoryName=Groceries&transactionType=Expense&currentMonth=true`

#### AddTransaction
- **URL**: `api/TransactionData/AddTransaction`
- **Method**: POST
- **Description**: Adds a new transaction.
- **Parameters**: `transaction` (Transaction JSON object)

#### DeleteTransaction
- **URL**: `api/TransactionData/DeleteTransaction/{id}`
- **Method**: DELETE
- **Description**: Deletes a transaction from the system by its ID.
- **Parameters**: `id` (int)

#### UpdateTransaction
- **URL**: `api/TransactionData/UpdateTransaction/{id}`
- **Method**: PUT
- **Description**: Updates a particular transaction in the system with PUT data input.
- **Parameters**:
  - `id` (int)
  - `transaction` (Transaction JSON object)

### CategoryDataController

#### listCategoryByTransactionType
- **URL**: `api/CategoryData/listCategoryByTransactionType`
- **Method**: GET
- **Description**: Retrieves a list of categories based on the transaction type name.
- **Parameters**: `transactionTypeName` (string)
- **Example**: `GET api/CategoryData/listCategoryByTransactionType?transactionTypeName=Income`

#### AddCategory
- **URL**: `api/CategoryData/AddCategory`
- **Method**: POST
- **Description**: Adds a new category.
- **Parameters**: `category` (Category JSON object)

#### UpdateCategory
- **URL**: `api/CategoryData/UpdateCategory/{id}`
- **Method**: PUT
- **Description**: Updates a particular category in the system with PUT data input.
- **Parameters**:
  - `id` (int)
  - `category` (Category JSON object)

#### DeleteCategory
- **URL**: `api/CategoryData/DeleteCategory/{id}`
- **Method**: DELETE
- **Description**: Deletes a category from the system by its ID.
- **Parameters**: `id` (int)
- **Returns**: If the category has associated transactions, returns a `409 Conflict` status with a message indicating the category cannot be deleted.

## `Household Cleaning Pipeline`
Below are listed down all the Entities, Data Controllers related to the Household Cleaning Pipeline

## Entities

### Chore
- `ChoreId` - Primary key
- `ChoreDescription` - Description of the chore
- `ChoreFrequency` - How often the chore must be done (e.g., Daily, Weekly, etc.)
- `RoomId` - Foreign key to the Room entity
- `OwnerId` - Foreign key to the Owner entity

### Owner
- `OwnerId` - Primary key
- `OwnerName` - Name of the owner
- `OwnerAvailability` - Day of the week, available for doing chores (e.g., Daily, Weekly, etc.)

### Room
- `RoomId` - Primary key
- `Room` - Name of the area in the apartment/house

## Data Controllers

### ChoreDataController
#### ListChoresForOwner
- **URL**: `api/ChoreData/ListChoresForOwner/{id}`
- **Method**: GET
- **Description**: Reflects information about all chores related to a particular owner by its ID
- **Parameters**: `id` (int)
- **Example**: `api/ChoreData/ListChoresForOwner/1`

#### ListChoresForRoom
- **URL**: `api/ChoreData/ListChoresForRoom/{id}`
- **Method**: GET
- **Description**: Reflects information about all chores related to a particular room by its ID
- **Parameters**: `id` (int)
- **Example**: `api/ChoreData/ListChoresForRoom/1`

#### AssignChoreToRoom
- **URL**: `api/ChoreData/AssignChoreToRoom/{ChoreId}/{RoomId}`
- **Method**: POST
- **Description**: Assigns a particular chore to a particular room by their IDs
- **Parameters**:
  - `ChoreId` (int)
  - `RoomId` (int)
- **Example**: `api/ChoreData/AssignChoreToRoom/20/3`

#### UnassignChoreFromRoom
- **URL**: `api/ChoreData/UnAssignChoreFromRoom/{ChoreId}/{RoomId}`
- **Method**: POST
- **Description**: Unassign a particular chore from a particular room by their IDs
- **Parameters**:
  - `ChoreId` (int)
  - `RoomId` (int)
- **Example**: `api/ChoreData/UnAssignChoreFromRoom/20/3`

### OwnerDataController
#### ListOwners
- **URL**: `api/OwnerData/ListOwners`
- **Method**: GET
- **Description**: Retrieves a list of all owners.

#### FindOwner
- **URL**: `api/OwnerData/FindOwner/{id}`
- **Method**: GET
- **Description**: Finds an owner by owner ID and outputs information associated with this specific owner
- **Parameters**: `id` (int)
- **Example**: `api/OwnerData/FindOwner/1`

#### AddOwner
- **URL**: `api/OwnerData/AddOwner`
- **Method**: POST
- **Description**: Adds an owner to the db
- **Example**: `api/OwnerData/AddOwner`
- **Parameters**: `owner` (Owner JSON object)

#### UpdateOwner
- **URL**: `api/OwnerData/UpdateOwner/{id}`
- **Method**: POST
- **Description**: Updates an owner by it's ID
- **Parameters**:
  - `id` (int)
  - `owner` (Owner JSON object)

#### DeleteOwner
- **URL**: `api/OwnerData/DeleteOwner/{id}`
- **Method**: POST
- **Description**: Deletes an owner from the system by its ID.
- **Parameters**: `id` (int)
- **Returns**: If the owner has associated todo items, returns a `409 Conflict` status with a message indicating the owner cannot be deleted.

### RoomDataController
#### ListRooms
- **URL**: `api/RoomData/ListRooms`
- **Method**: GET
- **Description**: Retrieves a list of all rooms.

#### ListRoomsForChore
- **URL**: `api/RoomData/ListRoomsForChore/{id}`
- **Method**: GET
- **Description**: Retrieves rooms for a particular chore by its ID
- **Parameters**: `id` (int)
- **Example**: `api/RoomData/ListRoomsForChore/1`

#### ListRoomsNotForChore
- **URL**: `api/RoomData/ListRoomsNotForChore/{id}`
- **Method**: GET
- **Description**: Lists rooms which are not associated with this particular chore by its ID
- **Parameters**: `id` (int)
- **Example**: `api/RoomData/ListRoomsNotForChore/1`

#### FindRoom
- **URL**: `api/RoomData/FindRoom/{id}`
- **Method**: GET
- **Description**: Finds a room by room ID and outputs information associated with this specific room
- **Parameters**: `id` (int)
- **Example**: `api/RoomData/FindRoom/1`

#### AddRoom
- **URL**: `api/RoomData/AddRoom`
- **Method**: POST
- **Description**: Adds a room to the db
- **Example**: `api/RoomData/AddRoom`
- **Parameters**: `room` (Room JSON object)

#### UpdateRoom
- **URL**: `api/RoomData/UpdateRoom/{id}`
- **Method**: POST
- **Description**: Updates a particular room by it's ID
- **Parameters**:
  - `id` (int)
  - `room` (Room JSON object)

#### DeleteRoom
- **URL**: `api/RoomData/DeleteRoom/{id}`
- **Method**: POST
- **Description**: Deletes a category from the system by its ID.
- **Parameters**: `id` (int)


## How to Use

### Prerequisites
- .NET Framework
- Visual Studio or any other IDE that supports ASP.NET

### Setup
1. Clone the repository
   ```bash
   git clone https://github.com/yourusername/PersonalFinanceTracker.git
