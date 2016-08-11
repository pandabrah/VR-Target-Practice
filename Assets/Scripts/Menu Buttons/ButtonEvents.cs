using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class ButtonEvents : MonoBehaviour {

    private Dictionary<string, UnityEvent> eventDictionary;

    private static ButtonEvents buttonEvents;

    public static ButtonEvents instance
    {
        get
        {
            if (!buttonEvents)
            {
                buttonEvents = FindObjectOfType(typeof(ButtonEvents)) as ButtonEvents;

                if (!buttonEvents)
                {
                    Debug.LogError("there needs to be one active ButtonEvent script on a GameObject in your scene");
                }

                else
                {
                    buttonEvents.Init();
                }
            }

            return buttonEvents;
        }
    }

    void Init()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<string, UnityEvent>)();
        }
    }

    public static void StartListening(string eventName, UnityAction listener)
    {
        UnityEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {

        }
    }
}
