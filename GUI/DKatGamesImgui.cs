using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if DEBUG_MENU
using ImGuiNET;
using UImGui;
#endif

public class DKatGamesImgui : SingletonMonoBehaviour<DKatGamesImgui>
{
#if DEBUG_MENU
	private bool isActive = false;

	private Dictionary<string, System.Action> callbacks;

	protected override void Awake()
	{
		base.Awake();

		UImGuiUtility.Layout += OnImguiLayout;
	}
#endif

	public static void Subscribe(string name, System.Action onDrawCallback)
	{
#if DEBUG_MENU
		Debug.Assert(HasInstance, $"No instance available yet for {typeof(DKatGamesImgui)}, please make it before trying to subscribe");

		if (Instance.callbacks == null)
		{
			Instance.callbacks = new Dictionary<string, System.Action>();
		}

		if (!Instance.callbacks.ContainsKey(name))
		{
			Instance.callbacks.Add(name, onDrawCallback);
		}
		else
		{
			Instance.callbacks[name] += onDrawCallback;
		}

		ReorderCallbacks();
#endif
	}

#if !DEBUG_MENU
	private void Update()
	{
		if (IsOpenCloseInputPressed())
		{
			Debug.LogError($"Trying to open or close ImGui, the scripting define symbol DEBUG_MENU has not been set");
		}
	}
#else
	private static void ReorderCallbacks()
	{
		Instance.callbacks.OrderBy(s => s.Key);
	}

	private void OnImguiLayout(UImGui.UImGui imguiObj)
	{
		if (IsOpenCloseInputPressed())
		{
			isActive = !isActive;

			if (isActive)
				WindowGetsOpened();
		}

		if (isActive)
		{
			Draw();
		}
	}

	private void Draw()
	{
		ImGui.Begin("MainWindow");

		if (callbacks != null)
		{
			ImGui.BeginTabBar("MainTabBar");

			foreach (var cb in callbacks)
			{
				// Use this flag to open the tab by default (useful to remember which tab I was in.)  //ImGuiTabItemFlags.SetSelected
				if (ImGui.BeginTabItem(cb.Key))
				{
					cb.Value.Invoke();

					ImGui.EndTabItem();
				}
			}

			ImGui.EndTabBar();
		}

		// Gotta call the event for layoutting here.
		// I think it's best if we have each separate class call the event subscription and unsubscription themselves
		// It may be best to make it a not so simple event sub?
		// We want a tab bar that's alphabetically sorted, so we subscribe a callback and add a name for the callback? yeah that sounds fine, that way the only downside is that we need to do individual callbacks + methods
		// per window you want to implement in the same class. Not a big caveat tbh. Yeah that sounds doable.
		// Then when we open the window we can more easily navigate back to the selected window, by going over the list of registered names?
		// Each class that subscribes the callback then simply needs to add a const string name for itself.

		ImGui.End();
	}


	private void WindowGetsOpened()
	{
		var io = ImGui.GetIO();
		io.FontGlobalScale = 1.8f;
	}
#endif

	private bool IsOpenCloseInputPressed()
	{
		return Input.GetKeyDown(KeyCode.F2);
	}
}
