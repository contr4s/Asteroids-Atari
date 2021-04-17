using UnityEngine;

public class ExplosionsPool: ObjectPool<ParticleSystem>
{
    public void InitExplosion(ParticleSystem explosion, Vector3 position, float size = 1)
    {
        explosion.transform.position = position;
        explosion.transform.localScale = Vector3.one * size;
        explosion.gameObject.SetActive(true);
        explosion.Play();

        GameManager.S.audioManager.PlayExplosionSound();
    }
}
