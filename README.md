# Backend API Requirements for FileFlow Frontend

Based on the analysis of the frontend code, here's a comprehensive plan for the backend API functionality needed to support the FileFlow application.

## Core Functionality Requirements

### 1. User Management
- **User Authentication**: Support Auth0 integration for user authentication
- **Storage Quota Management**: Track and manage user storage limits and usage

### 2. File Management
- **File Upload**: Support single and multiple file uploads
- **File Download**: Allow downloading individual files
- **File Metadata**: Store and retrieve file metadata (name, type, size, creation/modification dates)
- **File Content**: Store and serve file content
- **File Renaming**: Allow renaming files
- **File Moving**: Support moving files between folders
- **File Deletion**: Move files to trash and permanent deletion

### 3. Folder Management
- **Folder Creation**: Create new folders
- **Folder Renaming**: Rename existing folders
- **Folder Moving**: Move folders to different locations
- **Folder Deletion**: Move folders to trash and permanent deletion

### 4. Organization Features
- **Starred Items**: Mark/unmark files and folders as starred
- **Recent Items**: Track and retrieve recently accessed files
- **Trash Management**: Store deleted items and support restoration or permanent deletion

## Data Models

### 1. UserStorage Model
```json
{
  "id": "string",
  "userId": "string",
  "maxSpace": "number", // In MB
  "usedSpace": "number", // In MB
  "storageBreakdown": {
    "documents": "number", // In MB
    "images": "number", // In MB
    "videos": "number", // In MB
    "other": "number" // In MB
  }
}
```

### 2. File/Folder Model
```json
{
  "id": "string",
  "name": "string", // Includes extension (for files)
  "type": "string", // "file" or "folder"
  "starred": "boolean",
  "parentId": "number", // ID of parent folder, null if in root
  "path": "string", // Full path to the item (includes name)
  "size": "number", // Size in MB (for files)
  "fileType": "string", // MIME type (for files)
  "inTrash": "boolean"
}
```

## API Endpoints Structure

### Authentication
- Integrate with Auth0 for authentication
- Implement necessary endpoints for user registration and profile management

### User Endpoints
- `GET /api/users/storage`: Get user storage

### File Endpoints
- `POST /api/files`: Upload new file(s)
- `GET /api/files/{id}`: Get file metadata
- `GET /api/files/{id}/content`: Download file content
- `PUT /api/files/{id}`: Update file metadata (rename)
- `DELETE /api/files/{id}`: Move file to trash
- `POST /api/files/{id}/restore`: Restore file from trash
- `DELETE /api/files/{id}/permanent`: Permanently delete file

### Folder Endpoints
- `POST /api/folders`: Create new folder
- `GET /api/folders/{id}`: Get folder metadata and contents
- `PUT /api/folders/{id}`: Update folder metadata (rename)
- `DELETE /api/folders/{id}`: Move folder to trash
- `POST /api/folders/{id}/restore`: Restore folder from trash
- `DELETE /api/folders/{id}/permanent`: Permanently delete folder

### Organization Endpoints
- `GET /api/items`: Get all files and folders (with optional filtering)
- `GET /api/items/starred`: Get starred files and folders
- `POST /api/items/{id}/star`: Star an item
- `DELETE /api/items/{id}/star`: Unstar an item
- `GET /api/items/recent`: Get recently accessed items
- `GET /api/items/trash`: Get items in trash
- `POST /api/trash/empty`: Empty trash (permanently delete all items in trash)
- `POST /api/items/{id}/move`: Move item to another folder

## Implementation Considerations

1. **File Storage**: Implement secure file storage with proper access controls
2. **Performance**: Optimize for handling large files and numerous small files
3. **Security**: Ensure proper authentication and authorization for all endpoints
4. **Scalability**: Design the API to handle growing user bases and storage requirements
5. **Error Handling**: Implement comprehensive error handling and status codes

This API structure will support all the functionality shown in the frontend code while providing a clean, RESTful interface for the application to interact with.
