using System;
using System.Collections.Generic;

public class Event {
	private Action action;

	public ListenerToken AddListener(Action listener) {
		action += listener;
		return new ListenerToken(this, listener);
	}

	public void Invoke() {
		action?.Invoke();
	}

	public void RemoveListener(Action listener) {
		action -= listener;
	}

	public void Clear() {
		action = delegate {};
	}
}

public class Event<T> {
	private Action<T> action;

	public ListenerToken<T> AddListener(Action<T> listener) {
		action += listener;
		return new ListenerToken<T>(this, listener);
	}

	public void Invoke(T value) {
		action?.Invoke(value);
	}

	public void RemoveListener(Action<T> listener) {
		action -= listener;
	}

	public void Clear() {
		action = delegate {};
	}
}

public class ListenerToken<T> : IListenerToken {
	private readonly Event<T> parentEvent;
	private readonly Action<T> listener;
	
	public ListenerToken(Event<T> parentEvent, Action<T> listener) {
		this.parentEvent = parentEvent;
		this.listener = listener;
	}

	public void Dispose() {
		parentEvent.RemoveListener(listener);
	}
}

public class ListenerToken : IListenerToken {
	private readonly Event parentEvent;
	private readonly Action listener;
	
	public ListenerToken(Event parentEvent, Action listener) {
		this.parentEvent = parentEvent;
		this.listener = listener;
	}

	public void Dispose() {
		parentEvent.RemoveListener(listener);
	}
}

public class MultiListenerToken : IListenerToken {
	private readonly List<IListenerToken> tokens = new List<IListenerToken>();

	public void Add(IListenerToken token) => tokens.Add(token);
	
	public void Dispose() {
		foreach (IListenerToken token in tokens) {
				token.Dispose();
		}
		tokens.Clear();
	}
}

public interface IListenerToken : IDisposable {}