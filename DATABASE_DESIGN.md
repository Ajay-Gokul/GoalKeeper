# Task Manager - Database Design Document (Simplified)

## Features Included

1. **Multi-User System** - Support for multiple users with separate task spaces
2. **User Management** - User accounts with authentication
3. **Task Creation** - Create new tasks with title and description
4. **Task Editing** - Update task details anytime
5. **Task Status Management** - Track tasks through states (To Do → In Progress → Done)
6. **Task Priority** - Set priority levels for tasks (Low, Medium, High)
7. **Task List Screen** - View user's own tasks with filtering and sorting capabilities
8. **Task Timestamps** - Track creation and update times
9. **Task Due Dates** - Set deadlines for tasks
10. **User-Specific Tasks** - Each user only sees their own tasks after login

---

## Database Schema Design

### Tables Overview

```
USERS
├── User Authentication & Profile
└── Owns Tasks

TASKS
├── Task Details (Title, Description)
├── Status Management (To Do, In Progress, Done)
├── Priority Levels (Low, Medium, High)
├── Owner (CreatedByUserUID)
└── Timestamps & Due Dates
```

---

## Detailed Table Schemas

### 1. **USERS** Table

```sql
CREATE TABLE USERS (
    UserUID UNIQUEIDENTIFIER PRIMARY KEY,
    Username VARCHAR(100) UNIQUE NOT NULL,
    Email VARCHAR(150) UNIQUE NOT NULL,
    PasswordHash VARCHAR(255) NOT NULL,
    FirstName VARCHAR(100),
    LastName VARCHAR(100),
    IsActive BIT NOT NULL,
    CreatedAt DATETIME2 NOT NULL,
    UpdatedAt DATETIME2 NOT NULL
);
```

### 2. **TASKS** Table

```sql
CREATE TABLE TASKS (
    TaskUID UNIQUEIDENTIFIER PRIMARY KEY,
    Title VARCHAR(300) NOT NULL,
    Description TEXT,
    Status VARCHAR(50) NOT NULL,
    Priority VARCHAR(50) NOT NULL,
    OwnerUserUID UNIQUEIDENTIFIER NOT NULL,
    DueDate DATE,
    CreatedAt DATETIME2 NOT NULL,
    UpdatedAt DATETIME2 NOT NULL,
    FOREIGN KEY (OwnerUserUID) REFERENCES USERS(UserUID)
);
```

---

## Entity Relationship Diagram

```
┌──────────────────────────────────────────────┐
│                    USERS                     │
│  UserUID, Username, Email, IsActive          │
└────┬──────────────────────────────────────────┘
     │
     │  (Owner)
     │
┌────▼────────────────────────────────────────┐
│                   TASKS                      │
│  TaskUID, Title, Description                 │
│  Status (To Do/In Progress/Done)             │
│  Priority (Low/Medium/High)                  │
│  OwnerUserUID (FK to USERS)                  │
│  DueDate, CreatedAt, UpdatedAt               │
└───────────────────────────────────────────────┘
```

---

## Key Relationships & Business Rules

| Relationship    | Description                                   |
| --------------- | --------------------------------------------- |
| User Owns Tasks | Each task belongs to the user who created it  |
| Status Workflow | To Do → In Progress → Done                    |
| Priority Levels | Low, Medium, High                             |
| Due Dates       | Optional deadline for task completion         |
| Task History    | CreatedAt and UpdatedAt timestamps track task |
| User-Specific   | Users only see their own tasks after login    |

---

## Indexes for Performance

```sql
CREATE INDEX idx_tasks_status ON TASKS(Status);
CREATE INDEX idx_tasks_priority ON TASKS(Priority);
CREATE INDEX idx_tasks_owner ON TASKS(OwnerUserUID);
CREATE INDEX idx_tasks_due_date ON TASKS(DueDate);
CREATE INDEX idx_users_active ON USERS(IsActive);
```

---

## Summary

This simplified database design supports:
✅ Multi-user system with user authentication
✅ Create, Read, Update operations for tasks
✅ Status management (To Do → In Progress → Done)
✅ Priority assignment (Low, Medium, High)
✅ User-specific task isolation (each user sees only their tasks)
✅ Task list view with filtering capabilities
✅ Due date tracking
✅ Timestamp tracking (created/updated)
✅ Performance optimization with proper indexes

---

## Normalization Rules Compliance

**First Normal Form (1NF):**

- All columns contain atomic (indivisible) values
- No repeating groups or arrays
- Each field contains only single-value data

**Second Normal Form (2NF):**

- Meets 1NF requirements
- All non-key columns are fully dependent on the primary key
- No partial dependencies exist

**Third Normal Form (3NF):**

- Meets 2NF requirements
- No transitive dependencies between non-key columns
- All non-key columns depend only on the primary key

**Design Notes:**

- **USERS table** - Minimal, focused on user identity and authentication - pure 3NF
- **TASKS table** - Each task is fully dependent on its primary key (TaskUID), with OwnerUserUID as the only foreign key reference
- **No redundant data** - All repeating information is properly normalized
- **Referential integrity** - FK constraint ensures data consistency between tables

---

## SQL Creation Script

```sql
-- Create USERS table
CREATE TABLE USERS (
    UserUID UNIQUEIDENTIFIER PRIMARY KEY,
    Username VARCHAR(100) UNIQUE NOT NULL,
    Email VARCHAR(150) UNIQUE NOT NULL,
    PasswordHash VARCHAR(255) NOT NULL,
    FirstName VARCHAR(100),
    LastName VARCHAR(100),
    IsActive BIT NOT NULL,
    CreatedAt DATETIME NOT NULL,
    UpdatedAt DATETIME NOT NULL
);

-- Create TASKS table
CREATE TABLE TASKS (
    TaskUID UNIQUEIDENTIFIER PRIMARY KEY,
    Title VARCHAR(300) NOT NULL,
    Description TEXT,
    Status VARCHAR(50) NOT NULL, -- 'To Do', 'In Progress', 'Done'
    Priority VARCHAR(50) NOT NULL, -- 'Low', 'Medium', 'High'
    OwnerUserUID UNIQUEIDENTIFIER NOT NULL,
    DueDate DATE,
    CreatedAt DATETIME NOT NULL,
    UpdatedAt DATETIME NOT NULL,
    FOREIGN KEY (OwnerUserUID) REFERENCES USERS(UserUID) ON DELETE CASCADE
);

-- Create Indexes for performance optimization
CREATE INDEX idx_tasks_owner ON TASKS(OwnerUserUID);
CREATE INDEX idx_tasks_status ON TASKS(Status);
CREATE INDEX idx_tasks_priority ON TASKS(Priority);
CREATE INDEX idx_tasks_due_date ON TASKS(DueDate);
CREATE INDEX idx_users_active ON USERS(IsActive);
```

---

## Feature Implementation Mapping

| Feature            | Table | Field(s)     | Notes                                         |
| ------------------ | ----- | ------------ | --------------------------------------------- |
| Create Task        | TASKS | All fields   | Insert new task with Title, Description       |
| Edit Task          | TASKS | Any field    | Update TaskUID record, UpdatedAt auto-refresh |
| Change Status      | TASKS | Status       | Update Status: To Do/In Progress/Done         |
| Assign Priority    | TASKS | Priority     | Set Priority: Low/Medium/High                 |
| Task List Screen   | TASKS | All fields   | SELECT where OwnerUserUID = current user      |
| Multi-User Support | USERS | OwnerUserUID | Each task owned by a specific user            |
