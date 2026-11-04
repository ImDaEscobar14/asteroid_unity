using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using extOSC;

public class OscProcess : MonoBehaviour
{
    /* c'est pour pointer vers le script AsteroidDodger et le asteroidDodger c'est la référence au script AsteroidDodger */
    public AsteroidDodger asteroidDodger;
    public OSCReceiver oscReceiver;

    public OSCTransmitter oscTransmitter;

    public float betaInMin = 0;
    public float betaInMax = 1023;
    public float betaOutMin = 0;
    public float betaOutMax = 1;

    // Start is called before the first frame update
    void Start()
    {
        /*c'est pour binder le récepteur OSC à l'adresse "/photo" c'est le message envoyé par le capteur */
        oscReceiver.Bind("/photo", TraiterMessagePhoto);

    }

    // Update is called once per frame
    void Update()
    {
        if ( asteroidDodger.GetShipIsVisible() ) {
            var oscMessage = new OSCMessage("/lumiere"); 
            oscMessage.AddValue( OSCValue.Int(1) ); // Ajoute l’entier 1
            oscTransmitter.Send( oscMessage ); // Envoyer le message 
        } else {
            var oscMessage = new OSCMessage("/lumiere"); 
            oscMessage.AddValue( OSCValue.Int(0) ); // Ajoute l’entier 0
            oscTransmitter.Send( oscMessage ); // Envoyer le message 
        }

    }

    /*c'est pour traiter le message OSC reçu à l'adresse "/photo" */
    void TraiterMessagePhoto(OSCMessage message)
    {
        // Traiter un entier et l’appliquer à une propriété (flux continu)

        // Récupérer la valeur du premier argument en taant qu’entier :
        int valeur = message.Values[0].IntValue;

        // Deboguer
        Debug.Log("Reçu : " + message.Address + " " + valeur);

        // Ajustement de la valeur
        float ajustee = (
            ((float)valeur - betaInMin) / (betaInMax - betaInMin) * (betaOutMax - betaOutMin) + betaOutMin
        );
        
        // DESTINATION

        asteroidDodger.SetShipY(ajustee);
    }
}
