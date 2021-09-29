using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Controlador : MonoBehaviour
{
    public int populationSize = 50; // Tamaño de la población
    public GameObject creaturePrefab;
    public List<GameObject> population;
    public float populationLifetime = 5.0f; // Tiempo de vida de la población
    public float mutationRate = 0.1f; // Probabilidad de que ocurra una mutación, 0 - 100%

    public Text generationText;
    private int currentGeneration = 1;
    public Text timeText;
    private float lifetimeLeft;

    void Start()
    {
        // Inicializar una población random
        InitialisePopulation();

        // Por cada tiempo de vida de una población, criar una nueva generación. Esto se repetirá indefinidamente.
        InvokeRepeating("BreedPopulation", populationLifetime, populationLifetime);

        // Setear el valor de vida que queda de manera de que se pueda realizar la cuenta regresiva de cada población.
        lifetimeLeft = populationLifetime;
    }

    void Update()
    {
        // Actualizar el texto que muestra en que generación estamos actualmente
        generationText.text = "Generation " + currentGeneration;

        // Realizar la cuenta regresiva, mostrando el tiempo de vida de la población actual y reiniciar la crianza.
        lifetimeLeft -= Time.deltaTime;
        timeText.text = "Time " + (lifetimeLeft).ToString("0");
    }

    // FUNCIÓN QUE INICIA LA POBLACIÓN
    private void InitialisePopulation()
    {
        for (int i = 0; i < populationSize; i++)
        {
            // Elige una posición random para la aparición de la criatura
            Vector2 pos = new Vector2(Random.Range(-9, 9), Random.Range(-4.5f, 4.5f));

            // Instanciar una criatura
            GameObject creature = Instantiate(creaturePrefab, pos, Quaternion.identity);

            // Setear el color de la criatura
            ADN creatureDNA = creature.GetComponent<ADN>();
            creatureDNA.r = Random.Range(0.0f, 1.0f);
            creatureDNA.g = Random.Range(0.0f, 1.0f);
            creatureDNA.b = Random.Range(0.0f, 1.0f);

            creature.GetComponent<SpriteRenderer>().color = new Color(creatureDNA.r, creatureDNA.g, creatureDNA.b);

            // Agregar la criatura a la población
            population.Add(creature);
        }
    }

    private void BreedPopulation()
    {
        List<GameObject> newPopulation = new List<GameObject>();

        // Remover los indidividuos que menos se ajustan mediante el ordenamiento de la lista desde el mas rojo, en forma descendente.
        List<GameObject> sortedList = population.OrderByDescending(o => o.GetComponent<ADN>().r).ToList();

        population.Clear();

        // Luego, criar solo las criaturas mas rojas (mitad de la lista)
        int halfOfPopulation = (int)(sortedList.Count / 2.0f);
        for (int i = halfOfPopulation - 1; i < sortedList.Count - 1; i++)
        {
            // Breed two creatures
            population.Add(Breed(sortedList[i], sortedList[i + 1]));
            population.Add(Breed(sortedList[i + 1], sortedList[i]));

        }

        // Destruir todas las criaturas originales
        for (int i = 0; i < sortedList.Count; i++)
        {
            Destroy(sortedList[i]);
        }

        lifetimeLeft = populationLifetime;
        currentGeneration++;
    }

    // Criar una nueva criatura usando el ADN de dos padres
    private GameObject Breed(GameObject parent1, GameObject parent2)
    {
        Vector2 pos = new Vector2(Random.Range(-9, 9), Random.Range(-4.5f, 4.5f));

        // Crear una nueva criatura y obtener una referencia a su ADN.
        GameObject offspring = Instantiate(creaturePrefab, pos, Quaternion.identity);
        ADN offspringDNA = offspring.GetComponent<ADN>();

        // Obtener el ADN de los padres
        ADN dna1 = parent1.GetComponent<ADN>();
        ADN dna2 = parent2.GetComponent<ADN>();

        // Obtener una mezcla del ADN de los padres la mayoría del tiempo, dependiendo de la probabilidad de mutación
        if (mutationRate <= Random.Range(0, 100))
        {
            // Elegir un rango entre 0 - 10, si es menor que 5 entonces elegir el ADN del padre1, sino, el del padre2.
            offspringDNA.r = Random.Range(0, 10) < 5 ? dna1.r : dna2.r;
            offspringDNA.g = Random.Range(0, 10) < 5 ? dna1.g : dna2.g;
            offspringDNA.b = Random.Range(0, 10) < 5 ? dna1.b : dna2.b;
        }
        else
        {
            int random = Random.Range(0, 3);
            if (random == 0)
            {
                offspringDNA.r = Random.Range(0.0f, 1.0f);
                offspringDNA.g = Random.Range(0, 10) < 5 ? dna1.g : dna2.g;
                offspringDNA.b = Random.Range(0, 10) < 5 ? dna1.b : dna2.b;
            }
            else if (random == 1)
            {
                offspringDNA.r = Random.Range(0, 10) < 5 ? dna1.r : dna2.r;
                offspringDNA.g = Random.Range(0.0f, 1.0f);
                offspringDNA.b = Random.Range(0, 10) < 5 ? dna1.b : dna2.b;
            }
            else
            {
                offspringDNA.r = Random.Range(0, 10) < 5 ? dna1.r : dna2.r;
                offspringDNA.g = Random.Range(0, 10) < 5 ? dna1.g : dna2.g;
                offspringDNA.b = Random.Range(0.0f, 1.0f);
            }
        }

        offspring.GetComponent<SpriteRenderer>().color = new Color(offspringDNA.r, offspringDNA.g, offspringDNA.b);

        return offspring;
    }
}
