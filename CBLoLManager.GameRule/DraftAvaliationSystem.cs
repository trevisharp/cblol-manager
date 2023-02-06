namespace CBLoLManager.GameRule;

using Model;

public class DrafAvaliationSystem
{
    public float BlueAdvantage => getDiff();

    private int rCount = 0;
    private int rAdDamage = 0;
    private int rApDamage = 0;
    private int rDefence = 0;
    private int rRange = 0;
    private int rMobility = 0;
    private int rControl = 0;
    private int rSupport = 0;
    private int rTanks = 0;
    private int rCcs = 0;

    private bool? rjgad = null;
    private bool? rtopad = null;
    private bool? rmidad = null;

    private int bCount = 0;
    private int bAdDamage = 0;
    private int bApDamage = 0;
    private int bDefence = 0;
    private int bRange = 0;
    private int bMobility = 0;
    private int bControl = 0;
    private int bSupport = 0;
    private int bTanks = 0;
    private int bCcs = 0;

    private bool? bjgad = null;
    private bool? btopad = null;
    private bool? bmidad = null;

    public void AddBluePick(Champion champion)
    {
        if (bCount >= 5)
            return;
        
        if (champion.AD)
            bAdDamage += 1 + 3 * champion.Damage;
        else bApDamage += 1 + 3 * champion.Damage;

        bDefence += 1 + 3 * champion.Defence;

        if (champion.Defence > 0)
            bTanks++;
        
        if (champion.Control > 0)
            bCcs++;

        bRange += champion.Range;
        bMobility += champion.Mobility;
        bSupport += champion.Support;
        bControl += champion.Control;

        if (champion.Role == Position.TopLaner)
            btopad = champion.AD;
        else if (champion.Role == Position.MidLaner)
            bmidad = champion.AD;
        else if (champion.Role == Position.Jungler)
            bjgad = champion.AD;
        
        bCount++;
    }
    
    public void AddRedPick(Champion champion)
    {
        if (rCount >= 5)
            return;
        
        if (champion.AD)
            rAdDamage += 1 + 3 * champion.Damage;
        else rApDamage += 1 + 3 * champion.Damage;

        rDefence += 1 + 3 * champion.Defence;
        
        if (champion.Defence > 0)
            rTanks++;
        
        if (champion.Control > 0)
            rCcs++;

        rRange += champion.Range;
        rMobility += champion.Mobility;
        rSupport += champion.Support;
        rControl += champion.Control;

        if (champion.Role == Position.TopLaner)
            rtopad = champion.AD;
        else if (champion.Role == Position.MidLaner)
            rmidad = champion.AD;
        else if (champion.Role == Position.Jungler)
            rjgad = champion.AD;
        
        rCount++;
    }

    private float getDiff()
    {
        float bluePower = 10;
        float redPower = 10;

        // Gank Empowered
        if (bjgad.HasValue && btopad.HasValue && bjgad != btopad)
            bluePower += 15;
            
        if (bjgad.HasValue && bmidad.HasValue && bjgad != bmidad)
            bluePower += 15;
            
        if (rjgad.HasValue && rtopad.HasValue && rjgad != rtopad)
            redPower += 15;
            
        if (rjgad.HasValue && rtopad.HasValue && rjgad != rtopad)
            redPower += 15;

        //  Power of Damage
        int worstRDam = rAdDamage < rApDamage ? rAdDamage : rApDamage;
        int bestRDam = rAdDamage > rApDamage ? rAdDamage : rApDamage;
        int worstBDam = bAdDamage < bApDamage ? bAdDamage : bApDamage;
        int bestBDam = bAdDamage > bApDamage ? bAdDamage : bApDamage;
        
        // Support Effect
        bluePower += (bSupport + 5) * bestBDam + 5 * worstBDam;
        redPower += (rSupport + 5) * bestRDam + 5 * worstRDam;

        // Facility to Tank
        if (bDefence < worstRDam) bluePower += 3 * bDefence;
        else bluePower += 3 * worstRDam + 7 * (bDefence - worstRDam);
            
        if (rDefence < worstBDam) redPower += 3 * rDefence;
        else redPower += 3 * worstBDam + 7 * (rDefence - worstBDam);
        
        // Out Range
        int dRange = bRange - rRange;
        if (dRange > 0)
            bluePower += 7 * (dRange * dRange - dRange);
        else redPower += 7 * (dRange * dRange + dRange);

        // Mobility Effect
        var bMob = bMobility > 3 ? 3 : bMobility;
        var rMob = rMobility > 3 ? 3 : rMobility;
        bluePower += 3 * (bMob * bMob + bMob);
        redPower += 3 * (rMob * rMob + rMob);

        // Control Effect
        int bConForce = 6 + bCcs - rTanks - rSupport;
        bluePower += bConForce * bControl;
        int rConForce = 6 + rCcs - bTanks - bSupport;
        redPower += rConForce * rControl;

        float diff = bluePower - redPower;
        diff = (100 + diff) / 200;
        diff = diff > 1f ? 1f : diff;
        diff = diff < 0f ? 0f : diff;
        return diff;
    }
}