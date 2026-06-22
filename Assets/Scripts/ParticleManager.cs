using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour {

    [SerializeField] private ParticlePoolEntry[] poolEntries;

    private Dictionary<string, ParticlePool> _poolLookup = new();

    [System.Serializable]
    private struct ParticlePoolEntry {
        public string id;
        public ParticlePool pool;
    }

    void Awake() {
        GameManager.particleManager = this;
        foreach (var entry in poolEntries)_poolLookup[entry.id] = entry.pool;
    }

    public ParticleSystem Get(string id) {
        if (_poolLookup.TryGetValue(id, out var pool)) return pool.Get();
        Debug.LogWarning($"PoolManager: No pool found for id '{id}'");
        return null;
    }

    public void Release(string id, ParticleSystem ps) {
        if (_poolLookup.TryGetValue(id, out var pool)) pool.Release(ps);
        else Debug.LogWarning($"PoolManager: No pool found for id '{id}'");
    }
}