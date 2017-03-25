using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void ConsumerDelegate<T, U> (T sender, U eventArgs);

//who emits notifications to all subscribers
public interface INotificationEmitter<T, U> {
	event ConsumerDelegate<T, U> emitter;
}

//who consumes notifications from emitter
public interface INotificationConsumer<T, U> {
	void Consume(T sender, U eventArgs);
}

//who immediately provides own data without subscribtion (providing data only for request)
public interface IImmediateResultProvider<U> {
	U GetData ();
}

