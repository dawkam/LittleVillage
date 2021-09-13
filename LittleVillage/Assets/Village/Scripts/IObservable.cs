using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObservable
{
    void Attach(IObserver observer);
    void Detach(IObserver observer);
    void NotifyObservers();
}
