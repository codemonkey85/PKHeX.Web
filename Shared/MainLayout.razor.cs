namespace PKHeX.Web.Shared;

public partial class MainLayout
{
    private bool isLoading = false;

    protected override void OnInitialized() => SE.OnChange += StateHasChanged;

    private readonly MudTheme MyCustomTheme = new()
    {
        Palette = new Palette()
        {
            Primary = Colors.Blue.Default,
            Secondary = Colors.Green.Accent4,
            AppbarBackground = Colors.Red.Default,
        }
    };

    private async void PromptFile() => await JS.InvokeVoidAsync("elementClick", "fileInput");

    private async void UploadSave(InputFileChangeEventArgs e)
    {
        isLoading = true;

        using var memoryStream = new MemoryStream();
        await e.File.OpenReadStream(e.File.Size).CopyToAsync(memoryStream);

        FileUtil.TryGetSAV(memoryStream.ToArray(), out var saveFile);

        OpenFile(memoryStream.ToArray(), e.File.Name, Path.GetExtension(e.File.Name));

        isLoading = false;
    }

    private void OpenFile(byte[] input, string path, string ext)
    {
        var obj = FileUtil.GetSupportedFile(input, ext, SE.SAV);
        if (obj != null && LoadFile(obj, path))
        {
            return;
        }

        Snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopCenter;
        //TODO: more specific message in case of pokémon
        Snackbar.Add("Could not open file", MudBlazor.Severity.Error);
    }

    private bool LoadFile(object? input, string path) => input != null && input switch
    {
        PKM pk => OpenPKM(pk),
        SaveFile s => OpenSAV(s, path),
        //IPokeGroup b => OpenGroup(b),
        //MysteryGift g => OpenMysteryGift(g, path),
        //IEnumerable<byte[]> pkms => OpenPCBoxBin(pkms),
        //IEncounterConvertible enc => OpenPKM(enc.ConvertToPKM(C_SAV.SAV)),
        SAV3GCMemoryCard gc => HandleGameCubeMemoryCardSave(gc, path),
        _ => false,
    };

    private bool HandleGameCubeMemoryCardSave(SAV3GCMemoryCard gc, string path) =>
        //if (!CheckGCMemoryCard(gc, path))
        //{
        //    return true;
        //}
        //var mcsav = SaveUtil.GetVariantSAV(gc);
        //return mcsav is not null && OpenSAV(mcsav, path);
        false;

    private bool OpenPKM(PKM pk) =>
        //var tmp = PKMConverter.ConvertToType(pk, SE.SAV.PKMType, out string c);
        //Console.WriteLine(c);
        //if (tmp == null)
        //    return false;
        //SE.SAV.AdaptPKM(tmp);
        //PE.PKM = tmp;
        //return true;
        false;

    private bool OpenSAV(SaveFile sav, string path)
    {
        sav.Metadata.SetExtraInfo(path);
        SE.SAV = sav;
        PE.PKM = sav.LoadTemplate();

        return true;
    }

    private async Task ExportSAV()
    {
        var saveFile = SE.SAV;
        var extension = saveFile.Metadata.GetSuggestedExtension();
        var flags = saveFile.Metadata.GetSuggestedFlags(extension);

        var fileStream = new MemoryStream(saveFile.Write(flags));
        var fileName = saveFile.Metadata.BAKName.Replace(".bak", extension).Remove(0, 1);

        using var streamRef = new DotNetStreamReference(stream: fileStream);

        await JS.InvokeVoidAsync("downloadFileFromStream", fileName, streamRef);
    }

    private async Task ExportPKM()
    {
        var pokemon = PE.PKM;

        var fileStream = new MemoryStream(pokemon.Data);
        var fileName = pokemon.FileName;

        using var streamRef = new DotNetStreamReference(stream: fileStream);

        await JS.InvokeVoidAsync("downloadFileFromStream", fileName, streamRef);
    }

    private void OpenSettings() => DialogService.Show<Settings>("Settings");

    private void OpenAbout() => DialogService.Show<About>("About", new DialogOptions { CloseButton = true });

    public void Dispose() => SE.OnChange -= StateHasChanged;
}
