public class UpdateSystem
{
    private UpdateSystem() { }

    private string instalationPath = "App";
    private Patch crr = null;

    private void createInstalationFolderIfNotExist()
    {
        try
        {
            if (Directory.Exists(instalationPath))
                return;
            
            Directory.CreateDirectory(instalationPath);    
        }
        catch
        {
            throw new Exception($"O sistema não teve permissão de criar/ler a pasta {instalationPath}.");
        }
    }

    private Patch getCurrentPatch()
    {
        createInstalationFolderIfNotExist();

        try
        {
            return Patch.Open(instalationPath);
        }
        catch
        {
            throw new Exception("O sistema não pode criar/ler o arquivo de informações de patch.");
        }
    }

    public static UpdateSystem Start()
    {
        UpdateSystem sys = new UpdateSystem();

        sys.crr = sys.getCurrentPatch();

        return sys;
    }
}