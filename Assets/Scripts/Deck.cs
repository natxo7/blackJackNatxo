using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Deck : MonoBehaviour
{
    System.Random variableRandom = new System.Random();
    public Sprite[] faces;
    public GameObject dealer;
    public GameObject player;
    public Button hitButton;
    public Button stickButton;
    public Button playAgainButton;
    public Text finalMessage;
    public Text probMessage;
    //cartas para hacer shuffle
    public Sprite[] facesCopia;
    //PUNTOS DEL DEALER Y DEL JUGADOR
    public Text puntosPlayer;
    public Text puntosDealer;
    //PARA LAS APUESTAS
    public Button apuesta10Button;

    public Text apuestaMessage;
    public Text BancaMessage;
    int banca = 1000;
    int apuesta = 0;
    
    public int[] values = new int[52];
    int cardIndex = 0;    
      public int extraerValorSprite(Sprite sprite)
      {
        int valorAdevolver = 0;
        for(int i = 0; i < faces.Length; i++)
        {
            if (sprite == faces[i])
            {
                valorAdevolver = values[i];
            }
        }
        return valorAdevolver;
      } 
    private void Awake()
    {    
        InitCardValues();        

    }

    private void Start()
    {
        facesCopia = new  Sprite[52];
    ShuffleCards();
        StartGame();        
    }

    private void InitCardValues()
    {
        /*TODO:
         * Asignar un valor a cada una de las 52 cartas del atributo "values".
         * En principio, la posición de cada valor se deberá corresponder con la posición de faces. 
         * Por ejemplo, si en faces[1] hay un 2 de corazones, en values[1] debería haber un 2.
         */



        //CREAMOS EL ARRAY DE POSIBLES VALORES DE LAS CARTAS
        int[] valores = { 11, 2, 3, 4, 5, 6, 7, 8, 9, 10, 10, 10, 10, 11, 2, 3, 4, 5, 6, 7, 8, 9, 10, 10, 10, 10, 11, 2, 3, 4, 5, 6, 7, 8, 9, 10, 10, 10, 10, 11, 2, 3, 4, 5, 6, 7, 8, 9, 10, 10, 10, 10 };
        //RECORREMOS UN FOR PARA ASIGNARLE LOS VALORES A VALUES
        //VALUES ES UNA LISTA DE 52 ELEMENTOS IGUAL QUE VALORES
        for (int i = 0; i < values.Length; i++)
        {
            values[i] = valores[i];//ASSIGNAMOS
        }

    }
  
    public T[] barajar<T>(T[] array)
    {
        var random = variableRandom;
        for (int i = array.Length; i > 1; i--)
        {
            // Pick random element to swap.
            int j = random.Next(i); // 0 <= j <= i-1
                                    // Swap.
            T tmp = array[j];
            array[j] = array[i - 1];
            array[i - 1] = tmp;
        }
        return array;
    }
    private void ShuffleCards()
    {

        /*TODO:
         * Barajar las cartas aleatoriamente.
         * El método Random.Range(0,n), devuelve un valor entre 0 y n-1
         * Si lo necesitas, puedes definir nuevos arrays.
         */
        //declaramos una variable donde guardara un numero de 0  a 52
        for(int i = 0; i < faces.Length; i++)
        {
            facesCopia[i] = faces[i];
        }
        facesCopia = barajar(facesCopia);
    }

    void StartGame()
    {
        apuesta = 0;
        actualizarBanca();
        
        for (int i = 0; i < 2; i++)
        {
            PushPlayer();
            PushDealer();
        }
        //SIRVE PARA MOSTRAR LOS PUNTOS 
        //ES UNA COMPROVACIÓN

        puntosPlayer.text = player.GetComponent<CardHand>().points.ToString();
        puntosDealer.text = dealer.GetComponent<CardHand>().points.ToString();

        //Si alguno de los dos obtiene Blackjack, termina el juego y mostramos mensaje

        if (player.GetComponent<CardHand>().points == 21)
        {
            apuesta10Button.interactable = false;
            finalMessage.text = "HAS HECHO BLACKJACK A LA PRIMERA";
            hitButton.interactable = false;
            stickButton.interactable = false;
            banca += apuesta * 2;
    
            actualizarBanca();
           

        }
        if (dealer.GetComponent<CardHand>().points == 21)
        {
            apuesta10Button.interactable = false;
            finalMessage.text = "Blackjack!   HAS PERDIDO";
            hitButton.interactable = false;
            stickButton.interactable = false;
            dealer.GetComponent<CardHand>().cards[0].GetComponent<CardModel>().ToggleFace(true);
            banca += 0;
       
            actualizarBanca();
           
        }

    }

    private void CalculateProbabilities()
    {
        /*TODO:
         * Calcular las probabilidades de:
         * - Teniendo la carta oculta, probabilidad de que el dealer tenga más puntuación que el jugador
         * - Probabilidad de que el jugador obtenga entre un 17 y un 21 si pide una carta
         * - Probabilidad de que el jugador obtenga más de 21 si pide una carta          
         */
    }

    void PushDealer()
    {
        /*TODO:
         * Dependiendo de cómo se implemente ShuffleCards, es posible que haya que cambiar el índice.
         */
        dealer.GetComponent<CardHand>().Push(facesCopia[cardIndex],extraerValorSprite(facesCopia[cardIndex]));
        cardIndex++;        
    }

    void PushPlayer()
    {
        /*TODO:
         * Dependiendo de cómo se implemente ShuffleCards, es posible que haya que cambiar el índice.
         */
        player.GetComponent<CardHand>().Push(facesCopia[cardIndex], extraerValorSprite(facesCopia[cardIndex])/*,cardCopy*/);
        cardIndex++;
        CalculateProbabilities();
    }       

    public void Hit()
    {
        /*TODO: 
         * Si estamos en la mano inicial, debemos voltear la primera carta del dealer.
         */
 
        //Repartimos carta al jugador
        PushPlayer();

        /*TODO:
         * Comprobamos si el jugador ya ha perdido y mostramos mensaje
         */
        apuesta10Button.interactable = false;
        if (player.GetComponent<CardHand>().points > 21)
        {
            finalMessage.text = "Tu puntuación es mayor que 21";
            hitButton.interactable = false;
            stickButton.interactable = false;
            banca += 0;
           
            actualizarBanca();
           
        }

    }

    public void Stand()
    {
        


        /*TODO: 
         * Si estamos en la mano inicial, debemos voltear la primera carta del dealer.
         */

        /*TODO:
         * Repartimos cartas al dealer si tiene 16 puntos o menos
        
         * El dealer se planta al obtener 17 puntos o más
         * Mostramos el mensaje del que ha ganado
         */

        //Repartimos cartas al dealer si tiene 16 puntos o menos
      
        apuesta10Button.interactable = false;

        while (dealer.GetComponent<CardHand>().points <= 16)
        {
            PushDealer();
        }
        if (dealer.GetComponent<CardHand>().points > player.GetComponent<CardHand>().points)
        {
           
            finalMessage.text = "HAS PERDIDO";
            hitButton.interactable = false;
            stickButton.interactable = false;
            // dealer.GetComponent<CardModel>().ToggleFace(true); 
            dealer.GetComponent<CardHand>().cards[0].GetComponent<CardModel>().ToggleFace(true);
            banca += 0;
           
            actualizarBanca();
           

        }
        if (player.GetComponent<CardHand>().points > dealer.GetComponent<CardHand>().points)
        {
            
            finalMessage.text = "HAS GANADO";
            hitButton.interactable = false;
            stickButton.interactable = false;

            dealer.GetComponent<CardHand>().cards[0].GetComponent<CardModel>().ToggleFace(true);
            banca += apuesta * 2;
            
            actualizarBanca();
            

        }
        if (player.GetComponent<CardHand>().points == dealer.GetComponent<CardHand>().points)
        {
           
            finalMessage.text = "EMPATE";
            hitButton.interactable = false;
            stickButton.interactable = false;
            banca += apuesta;
            dealer.GetComponent<CardHand>().cards[0].GetComponent<CardModel>().ToggleFace(true);
            


        }
        if ( dealer.GetComponent<CardHand>().points>21)
        {

            finalMessage.text = "PIERDE EL DEALER SE HA PASADO DE 21";
            hitButton.interactable = false;
            stickButton.interactable = false;

            dealer.GetComponent<CardHand>().cards[0].GetComponent<CardModel>().ToggleFace(true);
            banca += apuesta * 2;
       
            actualizarBanca();
            

        }



    }

    public void PlayAgain()
    {
        

        finalMessage.text = "";
        player.GetComponent<CardHand>().Clear();
        dealer.GetComponent<CardHand>().Clear();
        hitButton.interactable = true;
        stickButton.interactable = true;
        apuesta10Button.interactable = true;
        cardIndex = 0;
        ShuffleCards();
        StartGame();
    }


    public void add10()
    {
        if (banca > 10)
        {

            apuesta += 10;
            banca -= 10;
            actualizarBanca();
            
        }
    }
    private void actualizarBanca()
    {
        
        apuestaMessage.text = apuesta.ToString();
        
        BancaMessage.text =banca.ToString();
    }

}
