using System;

public class Observable<T> {
	private T value;

	public Event<T> OnChange = new Event<T>();

	public T Value {
		get => value;
		set {
			OnChange?.Invoke(value);
			this.value = value;
		}
	}
	
	public Observable(T defaultValue = default) {
		value = defaultValue;
	}

	public void Set(T value, bool silenceEvent = false) {
		if(!silenceEvent)
			OnChange.Invoke(value);
		this.value = value;
	}

	public ListenerToken<T> AddListener(Action<T> listener, bool invokeImmediately = false) {
		if (invokeImmediately)
			listener(value);
		return OnChange.AddListener(listener);
	}

	public void RemoveListener(Action<T> listener) {
		OnChange.RemoveListener(listener);
	}
}