using UnityEngine;
using System.Collections;

public class PuzzleControllerBathroom : PuzzleController
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float candleOnInterval = 1.0f;
    public float candleNextInterval = 1.0f;
    
    
    private GameObject socketParent;
    private int correctPlacements = 0;
    private GameObject candleParent;   
    
    private ParticleSystem[] flameParticleSystems = new ParticleSystem[4];
    private Light[] candleLights = new Light[4];
    private Color[] colors = {Color.red, Color.magenta, Color.yellow, Color.cyan};
    private Color originalColor;
    private bool isRunning = true; // To control the coroutine
    private bool stopExpansion = false;
    void Start()
    {
        puzzleComplete = false;
        correctPlacements = 0;
        
        socketParent = GameObject.FindGameObjectWithTag("Sockets");
        candleParent = GameObject.FindGameObjectWithTag("Candles");
        
        for (int i = 0; i < socketParent.transform.childCount; i++)
        {
            Transform child = socketParent.transform.GetChild(i);
            changeColor(child, Color.gray);

        }
        // Debug.Log(sockets[0].name)
        
        for (int i = 0; i < candleParent.transform.childCount; i++)
        {
            Transform child = candleParent.transform.GetChild(i);
            flameParticleSystems[i] = child.GetChild(0).GetChild(0).GetComponent<ParticleSystem>();
            candleLights[i] = child.GetChild(4).GetComponent<Light>();

        }
        
        originalColor = candleLights[0].color;
        StartCoroutine(ChangeCandleColorsRoutine());

    }

    // Update is called once per frame
    void Update()
    {
        if (puzzleComplete)
        {
            HandleCorrectPlacement();

        }
    }
    
    public void HandleCorrectPlacement()
    {
        correctPlacements++;
        
        if (correctPlacements >= 4) // All ducks placed
        {
            puzzleComplete = true;
            //Debug.Log("Puzzle Complete!");
            
            StopColorChange();
            
            for (int i = 0; i < socketParent.transform.childCount; i++)
            {
                Transform child = socketParent.transform.GetChild(i);
                
                child.GetChild(0).gameObject.SetActive(true);
                child.GetChild(1).gameObject.SetActive(false);
                
                changeCandleColor(i, colors[i]);
            }

        }
    }

    public void HandleRemoval()
    {
        correctPlacements--;
    }

    private void changeCandleColor(int i, Color color)
    {
        var particleSystem = flameParticleSystems[i];
    
        // Change color for new particles
        var main = particleSystem.main;
        main.startColor = color;
    
        // Change color of existing particles
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[particleSystem.particleCount];
        particleSystem.GetParticles(particles);
    
        for (int j = 0; j < particles.Length; j++)
        {
            particles[j].startColor = color;
        }
    
        particleSystem.SetParticles(particles, particles.Length);
        
        candleLights[i].color = color;
    }

    private void changeColor(Transform child, Color color)
    {
        
        child.GetComponent<MeshRenderer>().material.color = Color.gray;
        
    }
    
    private IEnumerator ChangeCandleColorsRoutine()
    {
        int currentIndex = 0;
    
        while (isRunning)
        {
            // Change color
            changeCandleColor(currentIndex, colors[currentIndex]);

            // Wait for a few seconds with the new color
            yield return new WaitForSeconds(candleOnInterval);

            // Revert to original color
            changeCandleColor(currentIndex, originalColor);

            // Move to next candle
            currentIndex = (currentIndex + 1) % flameParticleSystems.Length;

            // Optional: add a small delay before starting with the next candle
            if (currentIndex == 0)
            {
                // Wait for a few seconds before starting the next cycle
                yield return new WaitForSeconds(candleNextInterval);
            }
            yield return new WaitForSeconds(candleNextInterval);
        }
    }
    
    private void StopColorChange()
    {
        isRunning = false;
    }
}
