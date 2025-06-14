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
        public const string GetFile = $"{Base}/{{id:guid}}";
        public const string GetFileContent = $"{Base}/{{id:guid}}/content";
        public const string RenameFile = $"{Base}/{{id:guid}}";
        public const string MoveFileToTrash = $"{Base}/{{id:guid}}";
        public const string RestoreFile = $"{Base}/{{id:guid}}/restore";
        public const string PermanentDeleteFile = $"{Base}/{{id:guid}}/permanent";
    }
    
    public static class FolderEndpoints
    {
        public const string Base = $"{ApiBase}/folders";
        
        // Methods
        public const string CreateFolder = Base;
        public const string GetFolder = $"{Base}/{{idOrPath}}";
        public const string GetChildren = $"{Base}/{{id}}/children";
        public const string UpdateFolder = $"{Base}/{{id:guid}}";
        public const string DeleteFolder = $"{Base}/{{id:guid}}";
        public const string RestoreFolder = $"{Base}/{{id:guid}}/restore";
        public const string PermanentDeleteFolder = $"{Base}/{{id:guid}}/permanent";
    }
    
    public static class ItemsEndpoints
    {
        public const string Base = $"{ApiBase}/items";
        
        // Methods
        public const string GetStarredItems = $"{Base}/starred";
        public const string StarItem = $"{Base}/{{id:guid}}/star";
        public const string UnstarItem = $"{Base}/{{id:guid}}/star";
        public const string GetRecentItems = $"{Base}/recent";
        public const string GetTrashItems = $"{Base}/trash";
        public const string MoveItem = $"{Base}/{{id:guid}}/move";
    }
    
    public static class TrashEndpoints
    {
        public const string Base = $"{ApiBase}/trash";
        
        // Methods
        public const string EmptyTrash = $"{Base}/empty";
        
        public const string RestoreTrash = $"{Base}/restore";
    }

    public static class SupportEndpoints
    {
        public const string Base = $"{ApiBase}/support";
        
        // Methods
        public const string SendSupportMessage = $"{Base}/messages";
    }
}