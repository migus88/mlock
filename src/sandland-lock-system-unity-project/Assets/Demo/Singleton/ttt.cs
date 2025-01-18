public class ZombieEffectsManager : MonoBehaviour
{
    public enum Effects
    {
        Poison,
        Fire,
        Water,
        Lightning
    }

    /**
     * 1p - cache the Zombie component directly to avoid get component at run-time
     * OR
     * 0.5p - if get component is done only once in awake
     * In addition:
     * 0.5p replacing List with array
     */
    [SerializeField] private List<Zombie> _zombies;

    /**
     * 1p - cache the Text component directly to avoid get component at run-time
     * OR
     * 0.5p - Get component is done only once in awake
     */
    [SerializeField] private Text _text;

    private string _poisonEffectName;

    // 1p - Replace List<Zombie> with a proper struct for fast look-ups
    private HashSet<Zombie> _zombiesEffected = new HashSet<Zombie>();
    private bool _poisonActive;
    private int _poisonDamage;

    private int _previousEffected;
    private int _previousDamage;
    private int _previousAVG;

    void Awake()
    {
        // 0.5p - get enum string value only once
        _poisonEffectName = Effects.Poison.ToString();
    }

    public void StartPoisonEffect(int damage)
    {
        _poisonActive = true;
        _poisonDamage = damage;
        // 0.5p Clear list instead of re-creating it every time
        _zombiesEffected.Clear();
    }

    public void StopPoisonEffect()
    {
        _poisonActive = false;
    }

    //
    // 1p - replace "FixedUpdate" with "Update" (no reason to use fixed update for something like this)
    // BONUS: 3p - disable compiler null checks: [Il2CppSetOption(Option.NullChecks, false)]
    void Update()
    {
        if (!_poisonActive)
            return;

        float totalHealth = 0f;
        int totalDamage = 0;

        // 1p replace "foreach" with "for" and cache the "count" field
        int count = _zombies.Count;
        for (int i = 0; i < count; i++)
        {
            Zombie zombie = _zombies[i];
            if (!zombie.ShieldActive)
            {
                if (!_zombiesEffected.Contains(zombie))
                {
                    _zombiesEffected.Add(zombie);
                }

                zombie.Health -= _poisonDamage;

                // 1p - remove this debug log, the code can't really run if we have thousands of zombies
                // Debug.Log($"Zombie {zombie.name} updated health: {zombie.Health}");

                totalHealth += zombie.Health;

                // 1p - Fix bug 
                if (zombie.Health <= 0)
                {
                    // 0.5p - Replace string concatenation with interpolation
                    zombie.ShowDeathMessage($"I died from: {_poisonEffectName}");
                }
            }
        }

        // 2p - Fixed AVG calculation 
        float avg = totalHealth / _zombiesEffected.Count;

        // 2p - Create string & update Text only if some values were actually changed
        if (_previousEffected != _zombiesEffected.Count || _previousDamage != totalDamage || _previousAVG != avg)
        {
            // 0.5p - Replace string format with interpolation
            string text =
                $"Total zombies effected: {_zombiesEffected.Count} Total Damage: {totalDamage} AVG health: {avg}";
            _text.text = text;
        }
    }
}