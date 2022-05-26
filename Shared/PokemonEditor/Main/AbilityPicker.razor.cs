namespace PKHeX.Web.Shared.PokemonEditor.Main;

public partial class AbilityPicker
{
    protected override void OnInitialized() => PE.OnChange += StateHasChanged;

    protected void SetAbility(int abilityIndex)
    {
        PE.PKM.SetAbilityIndex(abilityIndex);
        PE.NotifyDataChanged();
    }

    protected int GetIndex()
    {
        var abilityList = GameInfo.FilteredSources.GetAbilityList(PE.PKM);
        var abilityCount = abilityList.Count;
        return PE.PKM switch
        {
            G3PKM pk3 => pk3.AbilityBit && abilityCount > 1 ? 1 : 0,
            G4PKM pk4 => LoadAbility4(pk4, abilityCount),
            PK5 pk5 => pk5.HiddenAbility ? abilityCount - 1 : LoadAbility4(pk5, abilityCount),
            _ => GetAbilityIndex6(PE.PKM, abilityCount)
        };
    }

    private static int LoadAbility4(PKM pk, int abilityCount)
    {
        var index = GetAbilityIndex4(pk);
        return Math.Min(abilityCount, index);
    }

    private static int GetAbilityIndex4(PKM pk)
    {
        var pi = pk.PersonalInfo;
        var abilityIndex = pi.GetAbilityIndex(pk.Ability);
        if (abilityIndex < 0)
        {
            return 0;
        }

        if (abilityIndex >= 2)
        {
            return 2;
        }

        var abils = pi.Abilities;
        return abils[0] == abils[1] ? pk.PIDAbility : abilityIndex;
    }

    private static int GetAbilityIndex6(PKM pk, int abilityCount)
    {
        var bitNumber = pk.AbilityNumber;
        var abilityIndex = AbilityVerifier.IsValidAbilityBits(bitNumber) ? bitNumber >> 1 : 0;
        if (abilityIndex >= abilityCount)
        {
            abilityIndex = abilityCount - 1;
        }

        return abilityIndex;
    }
}
