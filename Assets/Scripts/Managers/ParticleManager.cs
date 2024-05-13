using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager Instance;

    [SerializeField] private ParticleSystem explodeParticlesPrefab;
    [SerializeField] private ParticleSystem killedParticlesPrefab;
    [SerializeField] private ParticleSystem nukeParticlesPrefab;

    [SerializeField] private int explodeParticlesPoolSize = 1;
    [SerializeField] private int killedParticlesPoolSize = 1;
    [SerializeField] private int nukeParticlesPoolSize = 10;

    private Queue<ParticleSystem> explodeParticlesQueue = new Queue<ParticleSystem>();
    private Queue<ParticleSystem> killedParticlesQueue = new Queue<ParticleSystem>();
    private Queue<ParticleSystem> nukeParticlesQueue = new Queue<ParticleSystem>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        InitPools();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitPools()
    {
        for (int i = 0; i < explodeParticlesPoolSize; i++)
        {
            ParticleSystem particles = Instantiate(explodeParticlesPrefab);
            explodeParticlesQueue.Enqueue(particles);
            particles.gameObject.SetActive(false);
        }

        for (int i = 0; i < killedParticlesPoolSize; i++)
        {
            ParticleSystem particles = Instantiate(killedParticlesPrefab);
            killedParticlesQueue.Enqueue(particles);
            particles.gameObject.SetActive(false);
        }

        for (int i = 0; i < nukeParticlesPoolSize; i++)
        {
            ParticleSystem particles = Instantiate(nukeParticlesPrefab);
            nukeParticlesQueue.Enqueue(particles);
            particles.gameObject.SetActive(false);
        }
    }

    public void GetExplodeParticles(Vector3 pos)
    {
        ParticleSystem particles;
        if (explodeParticlesQueue.Count == 0)
        {
            particles = Instantiate(explodeParticlesPrefab);
            explodeParticlesQueue.Enqueue(particles);
            particles.gameObject.SetActive(false);
        }

        particles = explodeParticlesQueue.Dequeue();
        particles.transform.position = pos;
        particles.gameObject.SetActive(true);
        particles.Play();

        StartCoroutine(Co_ReturnParticles(explodeParticlesQueue, particles));
    }

    public void GetKilledParticles(Vector3 pos)
    {
        ParticleSystem particles;
        if (killedParticlesQueue.Count == 0)
        {
            particles = Instantiate(killedParticlesPrefab);
            killedParticlesQueue.Enqueue(particles);
            particles.gameObject.SetActive(false);
        }

        particles = killedParticlesQueue.Dequeue();
        particles.transform.position = pos;
        particles.gameObject.SetActive(true);
        particles.Play();

        StartCoroutine(Co_ReturnParticles(killedParticlesQueue, particles));
    }

    public void GetNukeParticles(Vector3 pos)
    {
        ParticleSystem particles;
        if (nukeParticlesQueue.Count == 0)
        {
            particles = Instantiate(nukeParticlesPrefab);
            nukeParticlesQueue.Enqueue(particles);
            particles.gameObject.SetActive(false);
        }

        particles = nukeParticlesQueue.Dequeue();
        particles.transform.position = pos;
        particles.gameObject.SetActive(true);
        particles.Play();

        StartCoroutine(Co_ReturnParticles(nukeParticlesQueue, particles));
    }

    IEnumerator Co_ReturnParticles(Queue<ParticleSystem> queue, ParticleSystem particles)
    {
        yield return new WaitForSeconds(particles.main.duration + particles.main.startLifetime.constantMax);
        particles.gameObject.SetActive(false);
        queue.Enqueue(particles);
    }
}
