namespace DSTS.ModLoader.Interfaces;

public interface IDstsApi
{
    int AddFolder(string folder);

    void BindFile(string bindPath, string file);
}
