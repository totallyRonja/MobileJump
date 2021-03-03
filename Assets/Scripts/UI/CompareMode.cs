
using UnityEngine;

public enum CompareMode {
	[InspectorName("=")]
	Equals,
	[InspectorName("!=")]
	NotEqual,
	[InspectorName("<")]
	LT,
	[InspectorName("<=")]
	LTE,
	[InspectorName(">")]
	GT,
	[InspectorName(">=")]
	GTE,
}
