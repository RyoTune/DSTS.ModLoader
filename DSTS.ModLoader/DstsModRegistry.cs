namespace DSTS.ModLoader;

public class DstsModRegistry
{
    private readonly Dictionary<string, ModFile> _modFiles = new(StringComparer.OrdinalIgnoreCase);

    public int AddFolder(string dir)
    {
        var numFiles = 0;
        foreach (var file in Directory.EnumerateFiles(dir, "*", SearchOption.AllDirectories))
        {
            var relPath = Path.GetRelativePath(dir, file).Replace('\\', '/');
            _modFiles[relPath] = new(file, new FileInfo(file).Length);
            Log.Debug($"Registered File: {file}\nPath: {relPath}");
            numFiles++;
        }

        return numFiles;
    }

    public bool TryGetModFile(string gameFile, out ModFile modFile)
    {
        if (_modFiles.TryGetValue(gameFile, out modFile)) return true;
        return false;
    }

    public readonly record struct ModFile(string Path, long Size);
}