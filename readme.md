
# Unity MonoBehaviour Execution Order (Quick Reference)

Docs: https://docs.unity3d.com/Manual/execution-order.html

This file explains the order Unity calls built-in MonoBehaviour methods.  
Use this as a guide to avoid confusion when working with object pooling, activation, and initialization.

---

## 🟣 Initialization
1. **`Awake()`**
   - Called immediately when the object is created (scene load or `Instantiate`).
   - Runs **once per lifetime**, even if disabled.
   - Use for: setting up references, creating data, internal state.

2. **`OnEnable()`**
   - Called every time the object is enabled (`gameObject.SetActive(true)` or `component.enabled = true`).
   - Runs after `Awake`.
   - Use for: event subscriptions, resetting values.

3. **`Start()`**
   - Called only **once**, the first frame the object is active.
   - Runs after all `Awake` + `OnEnable` calls have finished.
   - Use for: initialization that depends on other objects being ready.

---

## 🟢 Game Loop
- **`FixedUpdate()`**
  - Runs at a fixed interval (good for physics).
  - May run multiple times per frame or be skipped depending on framerate.

- **`Update()`**
  - Runs once per frame.
  - Use for: gameplay logic, input handling.

- **`LateUpdate()`**
  - Runs after all `Update()` calls.
  - Use for: camera follow, order-dependent updates.

---

## 🟠 Physics Events
- **`OnTriggerEnter/Stay/Exit(Collider other)`**
- **`OnCollisionEnter/Stay/Exit(Collision collision)`**
  - Called when physics detects trigger/collision events.

---

## 🔴 Rendering Events
- **`OnPreCull()`** – before the camera culls objects.  
- **`OnWillRenderObject()`** – before rendering this object.  
- **`OnRenderObject()`**, **`OnPostRender()`**, **`OnRenderImage()`** – for custom rendering effects.  
- **`OnBecameVisible()` / `OnBecameInvisible()`** – when renderer enters/exits camera view.

---

## ⚫ Disable & Destroy
- **`OnDisable()`**
  - Called when object is disabled.
  - Use for: unsubscribing from events, stopping coroutines.

- **`OnDestroy()`**
  - Called when object is destroyed or scene unloads.
  - Use for: cleanup, releasing resources.

---

## 🟡 Application Events
- **`OnApplicationPause(bool pause)`** – app pauses (mobile).  
- **`OnApplicationFocus(bool focus)`** – app focus gained/lost.  
- **`OnApplicationQuit()`** – app is quitting.

---

## ✅ Common Lifecycle Flow

```txt
Awake
  ↓
OnEnable
  ↓
Start
  ↓
Update / FixedUpdate / LateUpdate (loop)
  ↓
OnDisable (if deactivated)
  ↓
OnEnable (if re-activated)
  ↓
OnDestroy (when destroyed or scene unload)
