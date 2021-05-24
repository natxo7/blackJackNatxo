using System;
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
    public Button resta10Button;
    public Text apuestaMessage;
    public Text BancaMessage;
    int banca = 1000;
    int apuesta = 0;
    //PARA LA PROBABILIDAD
    public Text prob1;
   
   





    public int[] values = new int[52];
    int cardIndex = 0;    

    //PARA SACAR EL VALOR DE LA CARTA
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
        facesCopia = new  Sprite[52];//Sprite
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
  //metodo que baraja cartas 
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
        
        for(int i = 0; i < faces.Length; i++)
        {
            facesCopia[i] = faces[i];//asignamos los valores 
        }
        facesCopia = barajar(facesCopia);
    }

    void StartGame()
    {
        //PARA LAS APUESTAS
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

        //player tiene 21 
        if (player.GetComponent<CardHand>().points == 21)
        {
            apuesta10Button.interactable = false;
            finalMessage.text = "HAS HECHO BLACKJACK A LA PRIMERA";
            hitButton.interactable = false;
            stickButton.interactable = false;
            banca += apuesta * 2;
    
            actualizarBanca();
           

        }
        //dealer tiene 21
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
        /*  int carta0 = dealer.GetComponent<CardHand>().cards[0].GetComponent<CardHand>().points;

          //CASO 1
          float valorVisibleDealer =Math.Abs(  carta0-Convert.ToInt32(puntosDealer)) ;//puntos totales del dealer menos los de la carta oculta
          float casos_posibles = 13 - Convert.ToInt32(puntosPlayer) + valorVisibleDealer;
          float resultado = casos_posibles / 13f;
          if (resultado > 1)
          {
              resultado = 1;
          }
          else
          {
              resultado = 0;
          }

          prob1.text = resultado.ToString();*/
        //CASO 2
        int pPlayer = player.GetComponent<CardHand>().points;
        float probabilidad2;
        int casosPosibles2;
        casosPosibles2 = 13 - (21 - pPlayer);
        probabilidad2 = casosPosibles2 / 13f;
        if (probabilidad2 > 1)
        {
            probabilidad2 = 1;
        }
        else if (probabilidad2 < 0)
        {
            probabilidad2 = 0;
        }
        if (pPlayer < 12)
        {
            probabilidad2 = 0;
        }
        prob1.text = (probabilidad2 * 100).ToString() + " %";

    }

    void PushDealer()
    {
        /*TODO:
         * Dependiendo de cómo se implemente ShuffleCards, es posible que haya que cambiar el índice.
         */
        //he cambiado el parametro 2 y he añadido extarerValorSprite
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
        //una vez se pide carta ya no se puede apostar
        resta10Button.interactable = false;
        apuesta10Button.interactable = false;
        //si el player tiene mas de 21
       // CalculateProbabilities();
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

     //cuando nos plantamos ya no podemos apostar
        resta10Button.interactable = false;
        apuesta10Button.interactable = false;
        //Repartimos cartas al dealer si tiene 16 puntos o menos
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
        if (banca >= 10)
        {
            resta10Button.interactable = true;
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
        else
        {
            Debug.Log("no te quedan monedas");
        }
    }

    //metodo para apostaar 10 del boton 
    public void add10()
    {
        if (banca >= 10)
        {

            apuesta += 10;
            banca -= 10;
            actualizarBanca();
            
        }
    }
    //metodo para restar 10 del boton
    public void restar10()
    {
        if (apuesta >= 10)
        {
            apuesta -= 10;
            banca += 10;
            actualizarBanca();
        }
    }
    private void actualizarBanca()//para actualizar los carteles de dentro del juego
    {
        
        apuestaMessage.text = apuesta.ToString();
        
        BancaMessage.text =banca.ToString();
    }

}
