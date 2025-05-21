namespace FileFlow.Contracts;

public static class Endpoints
{
    public const string ApiBase = "/api";
    public static class UserEndpoints
    {
        public const string Base = $"{ApiBase}/users";
        
        // Methods
        public const string GetStorage = $"{Base}/storage";
    }
    
    public static class FileEndpoints
    {
        public const string Base = $"{ApiBase}/files";
        
        // Methods
        public const string UploadFile = Base;
        public const string GetFile = $"{Base}/{{id}}";
        public const string GetFileContent = $"{Base}/{{id}}/content";
        public const string UpdateFile = $"{Base}/{{id}}";
        public const string DeleteFile = $"{Base}/{{id}}";
        public const string RestoreFile = $"{Base}/{{id}}/restore";
        public const string PermanentDeleteFile = $"{Base}/{{id}}/permanent";
    }
    
    public static class FolderEndpoints
    {
        public const string Base = $"{ApiBase}/folders";
        
        // Methods
        public const string CreateFolder = Base;
        public const string GetFolder = $"{Base}/{{id}}";
        public const string UpdateFolder = $"{Base}/{{id}}";
        public const string DeleteFolder = $"{Base}/{{id}}";
        public const string RestoreFolder = $"{Base}/{{id}}/restore";
        public const string PermanentDeleteFolder = $"{Base}/{{id}}/permanent";
    }
    
    public static class ItemsEndpoints
    {
        public const string Base = $"{ApiBase}/items";
        
        // Methods
        public const string GetAllItems = Base;
        public const string GetStarredItems = $"{Base}/starred";
        public const string StarItem = $"{Base}/{{id}}/star";
        public const string UnstarItem = $"{Base}/{{id}}/star";
        public const string GetRecentItems = $"{Base}/recent";
        public const string GetTrashItems = $"{Base}/trash";
        public const string MoveItem = $"{Base}/{{id}}/move";
    }
    
    public static class TrashEndpoints
    {
        public const string Base = $"{ApiBase}/trash";
        
        // Methods
        public const string EmptyTrash = $"{Base}/empty";
    }
}