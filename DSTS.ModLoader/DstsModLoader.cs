using System.Runtime.InteropServices;
// ReSharper disable InconsistentNaming

namespace DSTS.ModLoader;

public unsafe class DstsModLoader
{
    private delegate nint LoadFile(PackInfo* sourcePack, nint filePath, nint buffer, nint size, nint param_5, nint param_6, int param_7);
    private readonly SHFunction<LoadFile>? _LoadFile;
    
    private delegate nint GetFileSize(nint param_1, nint filePath, long* size, nint param_4, int param_5);
    private readonly SHFunction<GetFileSize>? _GetFileSize;
    
    private readonly DstsModRegistry _registry;
    
    public DstsModLoader(DstsModRegistry registry)
    {
        _registry = registry;
        _LoadFile = new(LoadFileImpl, "40 55 53 56 57 41 54 41 55 41 56 41 57 48 8D AC 24 ?? ?? ?? ?? 48 81 EC 58 05 00 00");
        _GetFileSize = new(GetFileSizeImpl, "48 89 5C 24 ?? 55 56 57 41 54 41 55 41 56 41 57 48 8D AC 24 ?? ?? ?? ?? 48 81 EC 20 06 00 00");
    }

    private nint LoadFileImpl(PackInfo* sourcePack, nint filePath, nint buffer, nint size, nint param_5, nint param_6, int param_7)
    {
        var filePathStr = Marshal.PtrToStringAnsi(filePath)!;
        if (Mod.Config.LogFilesEnabled)
        {
            Log.Information($"{nameof(LoadFile)} || File: {filePathStr}");
        }
        else
        {
            Log.Debug($"{nameof(LoadFile)} || File: {filePathStr}");
        }
        
        if (_registry.TryGetModFile(filePathStr, out var modFile))
        {
            Log.Debug($"{nameof(LoadFile)} || Replacing: {filePath}\nMod File: {modFile}");
            var temp = File.ReadAllBytes(modFile.Path);
            Marshal.Copy(temp, 0, buffer, temp.Length);
            return 1;
        }
        
        return _LoadFile!.Hook!.OriginalFunction(sourcePack, filePath, buffer, size, param_5, param_6, param_7);
    }

    private nint GetFileSizeImpl(nint param_1, nint filePath, long* size, nint param_4, int param_5)
    {
        var filePathStr = Marshal.PtrToStringAnsi(filePath)!;
        if (_registry.TryGetModFile(filePathStr, out var modFile))
        {
            *size = modFile.Size;
            return 1;
        }
        
        return _GetFileSize!.Hook!.OriginalFunction(param_1, filePath, size, param_4, param_5);;
    }
    
    [StructLayout(LayoutKind.Sequential)]
    struct PackInfo
    {
        public nint Name;
    }
}