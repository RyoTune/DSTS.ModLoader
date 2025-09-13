using DSTS.ModLoader.Interfaces;

namespace DSTS.ModLoader;

public class DstsApi(DstsModRegistry registry) : IDstsApi
{
    public int AddFolder(string folder) => registry.AddFolder(folder);

    public void BindFile(string bindPath, string file) => registry.BindFile(bindPath, file);
}