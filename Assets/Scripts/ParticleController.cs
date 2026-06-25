using UnityEngine;

public class ParticleController : MonoBehaviour {
    [SerializeField] private ParticleSystem ps;

    private void OnEnable() { ps.Play(); }
}
