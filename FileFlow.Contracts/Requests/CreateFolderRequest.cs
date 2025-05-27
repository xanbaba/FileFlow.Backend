namespace FileFlow.Contracts.Requests;

public record CreateFolderRequest(string FolderName, Guid? TargetFolderId = null);
