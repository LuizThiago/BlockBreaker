using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] CanonController _canonController;
    [SerializeField] ProjectileController _projectileController;

    #region Properties

    public static CanonController CanonController => Instance._canonController;
    public static ProjectileController ProjectileController => Instance._projectileController;

    #endregion

    #region Monobehaviour

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
    }

    #endregion
}
