namespace FileFlow.Contracts.Requests;

public record CreateFolderRequest(string FolderName, string? TargetFolder = null);
